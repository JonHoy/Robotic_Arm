#include <amp.h>
#include "NativeDeclarations.h"
#include "HSI_Pixel.h"
#include <assert.h>

using namespace concurrency;

namespace native_library {
	namespace details {	
	template <typename T>
		void SegmentColors(const T* ImagePtr, int PixelCount, const int Planes, const T* ColorsPtr, int NumColors, int* SelectedColorsPtr)
		{
			static const int TileSize = 256;
			extent<1> ex(PixelCount);
			// Create a view over the data on the CPU
			array_view<const T,2> imageView(PixelCount, Planes, ImagePtr);
			array_view<const T,2> colorsView(NumColors, Planes, ColorsPtr);

			array_view<int, 1> selectedcolorsView(ex, SelectedColorsPtr);
    
			// Run code on the GPU
			if (Planes == 3 && NumColors <= TileSize && PixelCount % TileSize == 0) // fast implementation for 3 planes and color count less than or equal to TileSize
			{
				std::vector<HSI_Pixel> HSIColors(NumColors);
				for (int i = 0; i < NumColors; i++)
				{
					float R = (float) colorsView(i, 0);
					float G = (float) colorsView(i, 1);
					float B = (float) colorsView(i, 2);
					HSIColors[i] = RGB2HSI(R, G, B);
					// If already in HSV
					//HSIColors[i].H = (float) colorsView(i, 0);
					//HSIColors[i].S = (float) colorsView(i, 1);
					//HSIColors[i].I = (float) colorsView(i, 2);
				}
				
				array_view<const HSI_Pixel> HSIColorsView(NumColors, &HSIColors[0]); // make an array view wrapper over the hsi

				parallel_for_each(selectedcolorsView.extent.tile<TileSize>(),  [=] (tiled_index<TileSize> t_idx) restrict(amp)
				{
					tile_static HSI_Pixel Color_HSI_Tile[TileSize];
					tile_static HSI_Pixel ImageTile[TileSize];
					tile_static int SelectedColorTile[TileSize];

					int iPixel = t_idx.global[0];
					int LocalIdx = t_idx.local[0];
					
					float R = (float) imageView(iPixel, 0);
					float G = (float) imageView(iPixel, 1);
					float B = (float) imageView(iPixel, 2);
					ImageTile[LocalIdx] = RGB2HSI(R, G, B);
					// If image is already converted to HSV;
					//ImageTile[LocalIdx].H = (float) imageView(iPixel, 0);
					//ImageTile[LocalIdx].S = (float) imageView(iPixel, 1);
					//ImageTile[LocalIdx].I = (float) imageView(iPixel, 2);

					if (LocalIdx < NumColors)
					{
						Color_HSI_Tile[LocalIdx] = HSIColorsView(LocalIdx);
					}

					t_idx.barrier.wait_with_tile_static_memory_fence();

					float min_distance = FLT_MAX;		
					for (int lColor = 0; lColor < NumColors; lColor++)
					{

						float distance = HSI_Distance(Color_HSI_Tile[lColor], ImageTile[LocalIdx]);
						if (min_distance > distance)
						{
							min_distance = distance;
							SelectedColorTile[LocalIdx] = lColor;
						}
					}
					selectedcolorsView(iPixel) = SelectedColorTile[LocalIdx];
				});
			}
			else // C++ AMP SIMPLE MODEL
			{
				parallel_for_each(selectedcolorsView.extent,  [=] (index<1> idx) restrict(amp)
				{
					int iPixel = idx[0];
					float min_distance = FLT_MAX;
					for (int lColor = 0; lColor < NumColors; lColor++)
					{
						float distance = 0;

						for (int kPlane = 0; kPlane < Planes; kPlane++)
						{
							float localdist;
							float localColor = (float) colorsView(lColor, kPlane);
							float localImage = (float) imageView(iPixel, kPlane);
							if (localImage > localColor)
							{
								localdist = localImage - localColor;
							}
							else
							{
								localdist = localColor - localImage;		
							}
							distance = distance + localdist;
						}
						if (min_distance > distance)
						{
							min_distance = distance;
							selectedcolorsView(iPixel) = lColor;
						}
					}
				});
			}
			// Copy data from GPU to CPU
			selectedcolorsView.synchronize();
		}
	}

	void SegmentColorsGPU(const float* Image, int PixelCount, int Planes, const float* Colors, int NumColors, int* SelectedColors) {
		details::SegmentColors<float>(Image, PixelCount, Planes, Colors, NumColors, SelectedColors);
	}
	void SegmentColorsGPU(const int* Image, int PixelCount, int Planes, const int* Colors, int NumColors, int* SelectedColors) {
		details::SegmentColors<int>(Image, PixelCount, Planes, Colors, NumColors, SelectedColors);
	}	
}