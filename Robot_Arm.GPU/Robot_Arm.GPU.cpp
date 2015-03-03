#include "Robot_Arm.GPU.h"
#include "NativeDeclarations.h"

using namespace System;

namespace Robot_Arm {
	array<Blob>^ GPU::SegmentColors(array<Byte,3>^ Image, array<Byte, 2>^ Colors, array<int,1>^ SelectionIndex, int SelectedColor)
	{
		if (Image == nullptr || Colors == nullptr)
			throw gcnew ArgumentNullException();
		if (Colors->GetLength(0) != SelectionIndex->Length)
			throw gcnew Exception("Colors Array must be the same length as SelectionIndex Array");
		int NumColors = Colors->GetLength(0);
		for (int i = 0; i < NumColors; i++)
		{
			if (SelectionIndex[i] > NumColors || SelectionIndex[i] < 0)
				throw gcnew Exception("Selection Index must contain valid array indices numbers");
		}
		if (Colors->GetLength(1) != Image->GetLength(2))
		{
			throw gcnew Exception("Colors and Image must be of RGBA data type");
		}
		auto Numel = Image->Length;
		if (Numel % 4 != 0)
			throw gcnew Exception("Colors and Image must be of RGBA data type");
		int Rows = Image->GetLength(0);
		int Cols = Image->GetLength(1);
		pin_ptr<Byte> ImagePtr = &Image[0,0,0];
		pin_ptr<Byte> ColorsPtr = &Colors[0,0];
		pin_ptr<int> SelectionIndexPtr = &SelectionIndex[0];
		auto Blobs = native_library::details::BlobFinder(ImagePtr, Rows, Cols, ColorsPtr, SelectionIndexPtr, NumColors, SelectedColor);
		auto Length = Blobs.size();
		auto ManagedBlobs = gcnew array<Blob>(Length);
		if (Length > 0) {
			pin_ptr<Blob> ManagedBlobPtr = &ManagedBlobs[0];
			memcpy((void*) ManagedBlobPtr, (void*)&Blobs[0], sizeof(Blob)*Length);
		}
		Blobs.~vector();
		return ManagedBlobs;
	}



}