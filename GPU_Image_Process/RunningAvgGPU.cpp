#include <amp.h>
#include <amp_math.h>
using namespace concurrency;

extern "C" __declspec ( dllexport ) void _stdcall RunningAvgGPU(float* Images, int PixelCount, int Frames, float* OutputImage)
{
	// Create a view over the data on the CPU
	
	array_view<const float, 2> ImagesView(Frames, PixelCount, Images);
	array_view<float, 1> OutputImageView(PixelCount, OutputImage);

	// Run code on the GPU
	parallel_for_each(OutputImageView.extent,  [=] (index<1> idx) restrict(amp)
	{
		int iPixel = idx[0];
		for (int iFrame = 0; iFrame < Frames; iFrame++)
		{
			OutputImageView(iPixel) += ImagesView(iFrame, iPixel);
		}
		OutputImageView(iPixel) = OutputImageView(iPixel) / Frames;
	});
	// Copy data from GPU to CPU
	OutputImageView.synchronize();
}
