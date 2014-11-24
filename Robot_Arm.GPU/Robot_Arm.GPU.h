// Robot_Arm.GPU.h

#pragma once

using namespace System;

namespace Robot_Arm_GPU {
		public ref class GPU{
		public:
			static array<int>^ SegmentColors(array<int>^ Image, array<int, 2>^ Colors);// TODO: Add your methods for this class here.
			static array<int>^ SegmentColors(array<float>^ Image, array<float, 2>^ Colors);// TODO: Add your methods for this class here.
			};
}
