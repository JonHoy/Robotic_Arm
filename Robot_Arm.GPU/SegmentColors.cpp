#include <amp.h>
#include "NativeDeclarations.h"

using namespace concurrency;

namespace native_library {
	namespace details {	
	template <typename T>
		void SegmentColors(const T* ImagePtr, int PixelCount, int Planes, const T* ColorsPtr, int NumColors, int* SelectedColorsPtr)
		{
			extent<1> ex(PixelCount);
			// Create a view over the data on the CPU
			array_view<const T,2> imageView(PixelCount, Planes, ImagePtr);
			array_view<const T,2> colorsView(NumColors, Planes, ColorsPtr);
			array_view<int, 1> selectedcolorsView(ex, SelectedColorsPtr);
    
			// Run code on the GPU
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
						T localColor = colorsView(lColor, kPlane);
						T localImage = imageView(iPixel, kPlane);
						if (localImage > localColor)
						{
							localdist = (float) (localImage - localColor);
						}
						else
						{
							localdist = (float) (localColor - localImage);		
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