#pragma once

namespace native_library {
	namespace details {
		template <typename T>
			void SegmentColors(const T* Image, int PixelCount, int Planes, const T* Colors, int NumColors, int* SelectedColors);
			void KNN_Filter(int* SelectedColors, int NumColors, int Rows, int Columns, int KernelRadius);
	}

	void SegmentColorsGPU(const int* Image, int PixelCount, int Planes, const int* Colors, int NumColors, int* SelectedColors);	
	void SegmentColorsGPU(const float* Image, int PixelCount, int Planes, const float* Colors, int NumColors, int* SelectedColors);
	void KNN_FilterGPU(int* SelectedColors, int NumColors, int Rows, int Columns, int KernelRadius);
}