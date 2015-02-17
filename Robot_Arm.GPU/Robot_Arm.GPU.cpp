#include "Robot_Arm.GPU.h"
#include "NativeDeclarations.h"

using namespace System;

namespace Robot_Arm_GPU {
	namespace details {
		template <typename T>
		array<int, 2>^ SegmentColors(array<T, 3>^ Image, array<T, 2>^ Colors)
		{
			array<int>^ MasterIndices = gcnew array<int>(Colors->GetLength(0));
			for (int i = 0; i < MasterIndices->Length; i++)
			{
				MasterIndices[i] = i;
			}
			return SegmentColors<T>(Image, Colors, MasterIndices);
		}
		template <typename T>
		array<int, 2>^ SegmentColors(array<T, 3>^ Image, array<T, 2>^ Colors, array<int>^ MasterIndices)
		{
			if (Image == nullptr || Colors == nullptr)
				throw gcnew ArgumentNullException();
			int NumColors = Colors->GetLength(0);
			if (MasterIndices->Length != NumColors) 
			{
				throw gcnew Exception("The MasterIndices array must be equal in size the number of rows of the Colors Array");
			}
			int Planes = Colors->GetLength(1);
			if (Planes != Image->GetLength(2) && Planes != 3)
				throw gcnew Exception("The number of columns of Colors must be equal to the depth of the Image and equal to 3!");
			int Rows = Image->GetLength(0);
			int Cols = Image->GetLength(1);

			int NumElements = Image->Length;
			int PixelCount = NumElements/Planes;
			array<int,2>^ SelectedColors = gcnew array<int,2>(Rows, Cols);
			{
				pin_ptr<T> ImagePtr = &Image[0,0,0];
				pin_ptr<T> ColorsPtr = &Colors[0,0];
				pin_ptr<int> SelectedColorsPtr = &SelectedColors[0,0];
				pin_ptr<int> MasterIndicesPtr = &MasterIndices[0];
				native_library::SegmentColorsGPU(ImagePtr, PixelCount, Planes, ColorsPtr, NumColors, SelectedColorsPtr, MasterIndicesPtr);
				native_library::KNN_FilterGPU(SelectedColorsPtr, NumColors, Rows, Cols, 12);
			}
			return SelectedColors;
		}
		
		template <typename T>
		array<T, 2>^ InterpScattered(array<T, 2>^ Scatter, int KernelRadius, int Iterations){
			if (Scatter == nullptr)
				throw gcnew ArgumentNullException("Scatter must not be null");
			if (KernelRadius < 1)
				throw gcnew ArgumentException("KernelSize must be greater than or equal to one");
			int Rows = Scatter->GetLength(0);
			int Cols = Scatter->GetLength(1);
			array<T, 2>^ ScatterNew = (array<T, 2>^ )Scatter->Clone();
			{
				pin_ptr<T> ScatterNewPtr = &ScatterNew[0,0];
				native_library::InterpScatteredGPU(ScatterNewPtr, Rows, Cols, KernelRadius, Iterations);
			}
			return ScatterNew;
		} 
	}

	array<int,2>^ GPU::SegmentColors(array<float,3>^ Image, array<float,2>^ Colors) {
		return details::SegmentColors<float>(Image, Colors);
	}
	array<int,2>^ GPU::SegmentColors(array<int,3>^ Image, array<int, 2>^ Colors) {
		return details::SegmentColors<int>(Image, Colors);
	}

	array<int,2>^ GPU::SegmentColors(array<float,3>^ Image, array<float,2>^ Colors, array<int,1>^ MasterIndices) {
		return details::SegmentColors<float>(Image, Colors, MasterIndices);
	}
	array<int,2>^ GPU::SegmentColors(array<int,3>^ Image, array<int, 2>^ Colors, array<int,1>^ MasterIndices) {
		return details::SegmentColors<int>(Image, Colors, MasterIndices);
	}

	array<float,2>^ GPU::InterpScattered(array<float,2>^ Scatter,  int KernelRadius, int Iterations) {
		return details::InterpScattered<float>(Scatter, KernelRadius, Iterations);
	}
	array<double,2>^ GPU::InterpScattered(array<double,2>^ Scatter,  int KernelRadius, int Iterations) {
		return details::InterpScattered<double>(Scatter, KernelRadius, Iterations);
	}
}