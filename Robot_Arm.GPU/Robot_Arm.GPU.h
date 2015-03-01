// Robot_Arm.GPU.h

#pragma once

using namespace System;

namespace Robot_Arm {
	public value class Blob {
		public:	
			int PixelCount;
			int RowMin;
			int RowMax;
			int ColMin;
			int ColMax;
			int RowSum;
			int ColSum;
		};	
	public ref class GPU{
		public:
			static array<Blob>^ SegmentColors(array<Byte,3>^ Image, array<Byte, 2>^ Colors, array<int,1>^ SelectionIndex, int SelectedColor);
		};
}
