// This file makes cuda c/c++ syntax look like c++ amp syntax
// Future todo: make c++ amp to cuda translater

#include "cuda.h"
#include "device_functions.h"
#include "device_launch_parameters.h"

#define UINT unsigned int

template <int Rank>
class Index
{
public:
	__device__ Index(&dim3 threadIdx, &dim3 blockIdx, &dim3 blockDim, &dim3 gridDim) {
		if (Rank == 3) {
			subscripts[0] = threadIdx.x + blockIdx.x * blockDim.x;
			subscripts[1] = threadIdx.y + blockIdx.y * blockDim.y;
			subscripts[2] = threadIdx.z + blockIdx.z * blockDim.z;
		}
	}
	__device__ UINT& operator[] (int Dim) 
	{
		return subscripts[Dim];
	}
	__device UINT globalID() {
		ID = subscripts[2] + subscripts[1] + gridDim.y * blockDim.y * subscripts[0];
		UINT Val = ID;
		return Val;
	}
private:
	UINT[Rank] subscripts;
	UINT ID;
};

template <int Rank> 
class Extent
{
public:
	__device__ Extent(int I0) {
		static_assert(Rank == 1,"Rank must be 1");
		Range[0] = I0;
	}
	__device__ Extent(int I0, int I1) {
		static_assert(Rank == 2,"Rank must be 2");
		Range[0] = I0;
		Range[1] = I1;
	}
	__device__ Extent(int I0, int I1, int I2) {
		static_assert(Rank == 3,"Rank must be 3");
		Range[0] = I0;
		Range[1] = I1;
		Range[2] = I2;
	}
	__device__ Extent(const UINT[Rank] _Range) {
		for (int i = 0; i < Rank; i++)
		{
			Range[i] = _Range[i];
		}
	}
	__device__ UINT operator[] (UINT Dim) {
		UINT Val = Range[Dim];
		return Val;
	}
private:
	UINT[Rank] Range;
};

// wrapper for device pointer with range checking
template <typename T, int Rank = 1>
class Array {
public:
	__device__ Array(T* Ptr, Extent<Rank> Dims) {
		_Ptr = Ptr;
		ex
	}
	__device__ T operator() (const Index<Rank>& Idx) {
		return _Ptr[Idx.globalID()];
	}
private:
	T* _Ptr;
	Extent<Rank> ex;
};