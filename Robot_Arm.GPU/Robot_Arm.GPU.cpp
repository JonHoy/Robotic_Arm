#include "Robot_Arm.GPU.h"
#include "NativeDeclarations.h"

using namespace System;

namespace Robot_Arm_GPU {
	namespace details {
		template <typename T>
		array<int>^ SegmentColors(array<T>^ Image, array<T, 2>^ Colors)
		{
			if (Image == nullptr || Colors == nullptr)
				throw gcnew ArgumentNullException();
			int NumColors = Colors->GetLength(0);
			int Planes = Colors->GetLength(1);
			int NumElements = Image->Length;
			int Remainder;
			System::Math::DivRem(NumElements, Planes, Remainder);
			if (Remainder != 0)
				throw gcnew ArgumentException("The number of array elements in the image must be divisible the number of color dimensions");
			int PixelCount = NumElements/NumColors;
			array<int>^ SelectedColors = gcnew array<int>(NumElements/Planes);
			{
				pin_ptr<const T> ImagePtr = &Image[0];
				pin_ptr<const T> ColorsPtr = &Colors[0,0];
				pin_ptr<int> SelectedColorsPtr = &SelectedColors[0];

				native_library::SegmentColorsGPU(ImagePtr, PixelCount, Planes, ColorsPtr, NumColors, SelectedColorsPtr);
			}
			return SelectedColors;
		}
		
		template <typename T>
		Statistics<T>^ ND_Correlate(array<T>^ Y, array<T,2>^ X, array<T,2>^ R, array<int>^ N)
		{
			if (Y == nullptr || X == nullptr || R == nullptr || N == nullptr)
				throw gcnew ArgumentNullException();
			if (X->GetLength(0) != Y->GetLength(0))
				throw gcnew ArgumentException("Number of Rows of X must be equal to number of elements of Y");
			if (X->GetLength(1) != R->GetLength(1))
				throw gcnew ArgumentException("Number of Columns of R must be equal to the number of columns of X");
			if (X->GetLength(1) != N->Length)
				throw gcnew ArgumentException("Number of elements of N must be equal to the number of columns of X");
			// range checking
			for (int iDim = 0; iDim < R->GetLength(1); iDim++)
			{
				if (R[0,iDim] >= R[1,iDim])
				{
					throw gcnew ArgumentException("Upper Range Value must be greater than or equal to Lower Range Value");
				}
			}
			int NumBins = 1;
			for (int i = 0; i < N->Length; i++)
			{
				NumBins = NumBins * N[i];
			}

			int Dims = R->GetLength(1);
			int Y_Length = Y->Length;

			Statistics<T>^ Correlation = gcnew Statistics<T>(NumBins);
			{
				pin_ptr<const T> YPtr = &Y[0];
				pin_ptr<const T> XPtr = &X[0,0];
				pin_ptr<const T> RPtr = &R[0,0];
				pin_ptr<int> NPtr = &N[0];
				pin_ptr<T> AvgPtr = &Correlation->Avg[0];
				pin_ptr<T> MaxPtr = &Correlation->Max[0];
				pin_ptr<T> MinPtr = &Correlation->Min[0];
				pin_ptr<int> CountPtr = &Correlation->Count[0];
				if (Dims == 1)
				{
					native_library::CorrelateGPU_1(YPtr, XPtr, Y_Length, RPtr, NPtr, MaxPtr, MinPtr, AvgPtr, CountPtr);
				}
			}
			return Correlation;
		}
	}
	array<int>^ Robot_Arm_GPU::GPU::SegmentColors(array<float>^ Image, array<float,2>^ Colors) {
		return details::SegmentColors<float>(Image, Colors);
	}
	array<int>^ Robot_Arm_GPU::GPU::SegmentColors(array<int>^ Image, array<int, 2>^ Colors) {
		return details::SegmentColors<int>(Image, Colors);
	}
	//Statistics<float>^ Robot_Arm_GPU::GPU::ND_Correlate(array<float>^ Y, array<float,2>^ X, array<float,2>^ R, array<int>^ N) {
	//	return details::ND_Correlate<float>(Y, X, R, N);
	//}
	//Statistics<double>^ Robot_Arm_GPU::GPU::ND_Correlate(array<double>^ Y, array<double,2>^ X, array<double,2>^ R, array<int>^ N) {
	//	return details::ND_Correlate<double>(Y, X, R, N);
	//}
}