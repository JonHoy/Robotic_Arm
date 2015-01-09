// Robot_Arm.GPU.h

#pragma once

using namespace System;

namespace Robot_Arm_GPU {
		public ref class GPU{
		public:
			static array<int,2>^ SegmentColors(array<int,3>^ Image, array<int, 2>^ Colors);// TODO: Add your methods for this class here.
			static array<int,2>^ SegmentColors(array<float,3>^ Image, array<float, 2>^ Colors);// TODO: Add your methods for this class here.
		};
}
