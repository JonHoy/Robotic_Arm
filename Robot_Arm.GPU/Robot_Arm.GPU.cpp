#include "Robot_Arm.GPU.h"
#include "NativeDeclarations.h"

using namespace System;

namespace Robot_Arm_GPU {
	namespace details {
		template <typename T>
		array<int,2>^ SegmentColors(array<T,3>^ Image, array<T, 2>^ Colors)
		{
			if (Image == nullptr || Colors == nullptr)
				throw gcnew ArgumentNullException();
			int NumColors = Colors->GetLength(0);
			int Planes = Colors->GetLength(1);
			if (Planes != Image->GetLength(2) && Planes != 3)
				throw gcnew FormatException("The number of columns of Colors must be equal to the depth of the Image and equal to 3!");
			int Rows = Image->GetLength(0);
			int Cols = Image->GetLength(1);

			int NumElements = Image->Length;
			int PixelCount = NumElements/Planes;
			array<int,2>^ SelectedColors = gcnew array<int,2>(Rows, Cols);
			{
				pin_ptr<T> ImagePtr = &Image[0,0,0];
				pin_ptr<T> ColorsPtr = &Colors[0,0];
				pin_ptr<int> SelectedColorsPtr = &SelectedColors[0,0];
				native_library::SegmentColorsGPU(ImagePtr, PixelCount, Planes, ColorsPtr, NumColors, SelectedColorsPtr);
				native_library::KNN_FilterGPU(SelectedColorsPtr, NumColors, Rows, Cols, 12);
			}
			return SelectedColors;
		}
		
	}

	array<int,2>^ GPU::SegmentColors(array<float,3>^ Image, array<float,2>^ Colors) {
		return details::SegmentColors<float>(Image, Colors);
	}
	array<int,2>^ GPU::SegmentColors(array<int,3>^ Image, array<int, 2>^ Colors) {
		return details::SegmentColors<int>(Image, Colors);
	}

}