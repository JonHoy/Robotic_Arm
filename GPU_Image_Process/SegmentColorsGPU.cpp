#include <amp.h>
#include <amp_math.h>
using namespace concurrency;

extern "C" __declspec ( dllexport ) void _stdcall SegmentColorsGPU(int* Image, int Rows, int Cols, int Planes, int* Colors, int NumColors, int* SelectedColors)
{
	// Create a view over the data on the CPU
    array_view<const int,3> imageView(Rows, Cols, Planes, &Image[0]);
	array_view<const int,2> colorsView(NumColors, Planes, &Colors[0]);
	array_view<int,2> selectedcolorsView(Rows, Cols, &SelectedColors[0]);
    
	// Run code on the GPU
	parallel_for_each(selectedcolorsView.extent,  [=] (index<2> idx) restrict(amp)
	{
		int iRow = idx[0];
		int jCol = idx[1];
		int min_distance = INT32_MAX;
		
		for (int lColor = 0; lColor < NumColors; lColor++)
		{
			int distance = 0;

			for (int kPlane = 0; kPlane < Planes; kPlane++)
			{
				int localdist;
				int localColor = colorsView(lColor, kPlane);
				int localImage = imageView(iRow, jCol, kPlane);
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
				selectedcolorsView(iRow, jCol) = lColor;
			}
		}
	});
	// Copy data from GPU to CPU
    selectedcolorsView.synchronize();
}
