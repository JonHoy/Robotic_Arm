#pragma once

#include <amp.h>

namespace native_library {
	namespace details {
		template <typename T>
			void SegmentColors(const T* Image, int PixelCount, int Planes, const T* Colors, int NumColors, int* SelectedColors, const int* MasterIndices);
			void KNN_Filter(int* SelectedColors, int NumColors, int Rows, int Columns, int KernelRadius);
		template <typename T>
			void InterpScattered(T* RawScatter, int Rows, int Columns, int KernelRadius, int Iterations);
	}

	void SegmentColorsGPU(const int* Image, int PixelCount, int Planes, const int* Colors, int NumColors, int* SelectedColors, const int* MasterIndicesPtr);	
	void SegmentColorsGPU(const float* Image, int PixelCount, int Planes, const float* Colors, int NumColors, int* SelectedColors, const int* MasterIndicesPtr);
	void KNN_FilterGPU(int* SelectedColors, int NumColors, int Rows, int Columns, int KernelRadius);
	void InterpScatteredGPU(float* RawScatter, int Rows, int Columns, int KernelRadius, int Iterations);
	void InterpScatteredGPU(double* RawScatter, int Rows, int Columns, int KernelRadius, int Iterations);

	public class GPUArray
	{
	public:
		Concurrency::array<float,1> AMPArray;
		GPUArray(long ArraySize);
		~GPUArray();
	};

}