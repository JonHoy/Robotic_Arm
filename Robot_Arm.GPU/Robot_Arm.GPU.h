// Robot_Arm.GPU.h

#pragma once

using namespace System;

namespace Robot_Arm_GPU {
		public ref class GPU{
		public:
			static array<int>^ SegmentColors(array<int,3>^ Image, array<int, 2>^ Colors);// TODO: Add your methods for this class here.
			static array<int>^ SegmentColors(array<float,3>^ Image, array<float, 2>^ Colors);// TODO: Add your methods for this class here.
			//static array<int>^ SegmentColors(array<Byte,3>^ Image, array<Byte, 2>^ Colors);// TODO: Add your methods for this class here.
			//static Statistics<float>^ ND_Correlate(array<float>^ Y, array<float,2>^ X, array<float,2>^ R, array<int>^ N);
			//static Statistics<double>^ ND_Correlate(array<double>^ Y, array<double,2>^ X, array<double,2>^ R, array<int>^ N);
		};

		//template <typename T>
		//public ref class Statistics {
		//public:	
		//	Statistics(int NumBins) {
		//		Avg = gcnew array<T>(NumBins);
		//		Count = gcnew array<int>(NumBins);
		//		Max = gcnew array<T>(NumBins);
		//		Min = gcnew array<T>(NumBins);
		//		Std = gcnew array<T>(NumBins);
		//	}
		//	
		//	array<T>^ Max;
		//	array<T>^ Min;
		//	array<T>^ Avg;
		//	array<T>^ Std;
		//	array<int>^ Count;
		//};

		//public ref class Bin {
		//public: 
		//	unsigned long Count;
		//	double Avg;
		//	double Std;
		//	double Max;
		//	double Min;
		//	double Median;
		//	double Skew;
		//	double Kurtosis;
		//};


}