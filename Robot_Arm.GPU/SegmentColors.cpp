#include "SegmentColors.h"
#include "NativeDeclarations.h"
#include "HSI_Pixel.h"
#include <assert.h>

using namespace concurrency;

namespace native_library {
	namespace details {	
		array<int,2> SegmentColors(unsigned char* ImagePtr, int Rows, int Cols, unsigned char* ColorsPtr, int NumColors, const int* SelectionIndex)
		{
			static const int TileSize = 128;
			extent<2> ex(Rows, Cols);
			// Create a view over the data on the CPU
			array_view<const int,2> imageView(Rows, Cols, (const int*)ImagePtr);
			array_view<const int,1> colorsView(NumColors, (const int*)ColorsPtr);
			array_view<const int> selectionIndexView(NumColors, SelectionIndex);
			array<int, 2> SelectedColors(ex);
    
			// Run code on the GPU
			assert(NumColors <= TileSize); // fast implementation for 3 planes and color count less than or equal to TileSize
			std::vector<HSI_Pixel> HSIColors(NumColors);
			for (int i = 0; i < NumColors; i++)
			{
				RgbPixel Color;
				Color.UnpackPixel(colorsView(i));
				HSIColors[i] = RGB2HSI(Color);
			}
				
			array_view<const HSI_Pixel> HSIColorsView(NumColors, &HSIColors[0]); // make an array view wrapper over the hsi
			try 
			{ 	
				parallel_for_each(SelectedColors.extent.tile<TileSize,1>().pad(),  [=, &SelectedColors] (tiled_index<TileSize,1> t_idx) restrict(amp)
			{
				tile_static HSI_Pixel Color_HSI_Tile[TileSize];
				tile_static HSI_Pixel ImageTile[TileSize];
				tile_static int SelectedColorTile[TileSize];
				tile_static int SelectedColorIndexTile[TileSize];
				bool is_InBounds = t_idx.global[0] < Rows;
				int LocalIdx = t_idx.local[0];					
				int Color = 0;
				if (is_InBounds)
					Color = imageView(t_idx.global);
				RgbPixel Pixel;
				Pixel.UnpackPixel(Color);
				ImageTile[LocalIdx] = RGB2HSI(Pixel);

				if (LocalIdx < NumColors)
				{
					Color_HSI_Tile[LocalIdx] = HSIColorsView(LocalIdx);
					SelectedColorIndexTile[LocalIdx] = selectionIndexView(LocalIdx);
				}

				t_idx.barrier.wait_with_tile_static_memory_fence();

				float min_distance = FLT_MAX;		
				for (int lColor = 0; lColor < NumColors; lColor++)
				{

					float distance = HSI_Distance(Color_HSI_Tile[lColor], ImageTile[LocalIdx]);
					if (min_distance > distance)
					{
						min_distance = distance;
						SelectedColorTile[LocalIdx] = SelectedColorIndexTile[lColor];
					}
				}
				if (is_InBounds)
					SelectedColors(t_idx.global) = SelectedColorTile[LocalIdx];
			});
			}
			catch (runtime_exception& ex)
			{
				auto message = ex.what();
				throw ex;
			}

			return SelectedColors;
		}
		void ConvertToBW(array<int,2>& SelectedColors, int SelectionNumber) {
			parallel_for_each(SelectedColors.extent, [=, &SelectedColors] (index<2> idx) restrict (amp)
			{
				if (SelectedColors(idx) == SelectionNumber)
				{
					SelectedColors(idx) = 1;
				}
				else
				{
					SelectedColors(idx) = 0;	
				}
			});
		} 
	}
}