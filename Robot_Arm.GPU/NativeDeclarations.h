#pragma once

namespace native_library {
	namespace details {
		template <typename T>
			void SegmentColors(const T* Image, int PixelCount, int Planes, const T* Colors, int NumColors, int* SelectedColors);
		template <typename T, int Dims>
			void ND_Correlate(const T* Y, const T* X, const int Y_Length, const T* Range, const int* Bins, T* Max, 
				 T* Min, T* Avg, int* Count);
	}

	void SegmentColorsGPU(const int* Image, int PixelCount, int Planes, const int* Colors, int NumColors, int* SelectedColors);	
	void SegmentColorsGPU(const float* Image, int PixelCount, int Planes, const float* Colors, int NumColors, int* SelectedColors);	
	void CorrelateGPU_1(const double* Y, const double* X, const int Y_Length, const double* Range, const int* Bins, double* Max, double* Min, double* Avg, int* Count);
	void CorrelateGPU_1(const float* Y, const float* X, const int Y_Length, const float* Range, const int* Bins, float* Max, float* Min, float* Avg, int* Count);
	
}