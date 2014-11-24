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
	}
				array<int>^ Robot_Arm_GPU::GPU::SegmentColors(array<float>^ Image, array<float,2>^ Colors) {
					return details::SegmentColors<float>(Image, Colors);
				}
				array<int>^ Robot_Arm_GPU::GPU::SegmentColors(array<int>^ Image, array<int, 2>^ Colors) {
					return details::SegmentColors<int>(Image, Colors);
				}

}
	
