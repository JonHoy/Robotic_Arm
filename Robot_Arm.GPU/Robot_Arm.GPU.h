// Robot_Arm.GPU.h

#pragma once

using namespace System;

namespace Robot_Arm_GPU {
		public ref class GPU{
		public:
			static array<int,2>^ SegmentColors(array<int,3>^ Image, array<int, 2>^ Colors);
			static array<int,2>^ SegmentColors(array<int,3>^ Image, array<int, 2>^ Colors, array<int>^ ParentIndices);
			static array<int,2>^ SegmentColors(array<float,3>^ Image, array<float, 2>^ Colors);
			static array<int,2>^ SegmentColors(array<float,3>^ Image, array<float, 2>^ Colors, array<int>^ ParentIndices);
			static array<float,2>^ InterpScattered(array<float,2>^ Scatter, int KernelSize, int IterationCount);
			static array<double,2>^ InterpScattered(array<double,2>^ Scatter, int KernelSize, int IterationCount);
		};


}
