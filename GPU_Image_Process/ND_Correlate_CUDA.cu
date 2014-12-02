#include "ND_Correlate_CUDA.h"


__global__ void ndCorrelateKernel(
	float* Y,
	float* X,
	float* Range,
	int* Bins,
	int* Cumprod,
	int Dims,
	int numPoints,
	float* Sum,
	float* Max,
	float* Min,
	int* Count,
	int* Index
	)
{
	int i = threadIdx.x + blockIdx.x * blockDim.x;
	int idx = 0;
	while (i < numPoints)
	{
		for (int iDim = 0; iDim < Dims; iDim++)
		{
			float Nbins = (float) Bins[iDim];
			float RawIndex = (X[i * Dims + iDim] - Range[iDim * Dims]) / (Range[iDim * Dims + 1] - Range[iDim * Dims]) * Nbins;
			//int LocalIndex = (int) floorf(RawIndex);
			int LocalIndex = (int) RawIndex;
			//			 if index is out of bounds clip the value
				if (LocalIndex < 0)
					LocalIndex = 0;
				else if (LocalIndex >= Bins[iDim])
					LocalIndex = Bins[iDim] - 1;
				idx = idx + Cumprod[iDim] * LocalIndex;
		}
		Index[i] = idx;
		atomicAdd(&Count[idx],1); // add to bin count
		atomicAdd(&Sum[idx],Y[i]); // keep a running sum
		if (Max[idx] < Y[i] || i == 0)
			atomicExch(&Max[idx], Y[i]);
		if (Min[idx] > Y[i] || i == 0)
			atomicExch(&Min[idx], Y[i]); 
	}
}

// Kernel to calculate the standard deviation of the dataset

__global__ void ndCorrelateKernel_STD(
	float* Y,
	int numPoints,
	float* Sum,
	float* Std,
	int* Count,
	int* Index
	)
{
	int i = threadIdx.x + blockIdx.x * blockDim.x;
	int stride = blockDim.x * gridDim.x;
	while (i < numPoints)
	{
		int idx = Index[i];
		float var = (Sum[idx]/Count[idx] - Y[i]) * (Sum[idx]/Count[idx] - Y[i]); // keep a running sum of the variance
		atomicAdd(&Std[idx],var); // add up the variance
		i += stride;
	}
	__syncthreads(); // now synchronize all threads so that we can now compute the standard deviation
}

extern "C" __declspec ( dllexport ) void _stdcall ND_Correlate_CUDA(
	const float* Y, 
	const float* X, 
	const int Y_length,
	const int Dims, 
	const float* Range, 
	const int *Bins,
	float* Max,
	float* Min,
	float* Std,
	float* Avg,
	int* Count)

{
	const int threadsPerBlock = 256;
	const int blocksPerGrid = max(32, Y_length/threadsPerBlock);

	int *Cumprod;
	Cumprod = (int*)malloc(Dims*sizeof(int));
	Cumprod[0] = 1;
	for (int iDim = 1; iDim < Dims; iDim++)
	{
		Cumprod[iDim] = Bins[iDim] * Cumprod[iDim - 1];
	}


	int *dev_Count, *dev_Cumprod, *dev_Bins, *dev_Index;
	float *dev_Max, *dev_Min, *dev_Var, *dev_Sum, *dev_Range;
	float *dev_X, *dev_Y;

	int NumBins = 1;
	for (int iDim = 0; iDim < Dims; iDim++)
	{
		NumBins = NumBins * Bins[iDim]; // count the total number of Bins needed for the Correlation operations
	}

	// allocate memory on the GPU
	cudaMalloc((void**)&dev_Bins, Dims * sizeof(int));
	cudaMalloc((void**)&dev_Range, Dims * 2 * sizeof(float));
	cudaMalloc((void**)&dev_Cumprod, Dims * sizeof(int));
	cudaMalloc((void**)&dev_Count, NumBins * sizeof(int));
	cudaMalloc((void**)&dev_Max, NumBins * sizeof(float));
	cudaMalloc((void**)&dev_Min, NumBins * sizeof(float));
	cudaMalloc((void**)&dev_Sum, NumBins * sizeof(float));
	cudaMalloc((void**)&dev_Var, NumBins * sizeof(float));
	cudaMalloc((void**)&dev_X, Y_length * Dims * sizeof(float));
	cudaMalloc((void**)&dev_Y, Y_length * sizeof(float));
	cudaMalloc((void**)&dev_Index, Y_length*sizeof(int));

	// copy memory from the cpu over to the gpu
	cudaMemcpy(dev_Bins, Bins, Dims * sizeof(int), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_Cumprod, Cumprod, Dims * sizeof(int), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_Count, Count, NumBins * sizeof(int), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_X, X, Dims * Y_length* sizeof(float), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_Y, Y, Y_length * sizeof(float), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_Max, Max, NumBins * sizeof(float), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_Min, Min, NumBins * sizeof(float), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_Var, Std, NumBins * sizeof(float), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_Sum, Avg, NumBins * sizeof(float), cudaMemcpyHostToDevice);
	
	ndCorrelateKernel<<<threadsPerBlock, blocksPerGrid>>>(dev_Y, dev_X, dev_Range, dev_Bins, dev_Cumprod, Dims, Y_length, dev_Sum, dev_Max, dev_Min, dev_Count, dev_Index);
	// calculate the variance from the Avg 
	ndCorrelateKernel_STD<<<threadsPerBlock, blocksPerGrid>>>(dev_Y, Y_length, dev_Sum, dev_Var, dev_Count, dev_Index);
	
	// copy memory from the gpu back over to the cpu
	cudaMemcpy(Max, dev_Max, NumBins * sizeof(float), cudaMemcpyDeviceToHost);
	cudaMemcpy(Min, dev_Min, NumBins * sizeof(float), cudaMemcpyDeviceToHost);
	cudaMemcpy(Avg, dev_Sum, NumBins * sizeof(float), cudaMemcpyDeviceToHost);
	cudaMemcpy(Std, dev_Var, NumBins * sizeof(float), cudaMemcpyDeviceToHost);
	cudaMemcpy(Count, dev_Count, NumBins * sizeof(float), cudaMemcpyDeviceToHost);

	// transform sum of variance and sum to avg and std
	for (int i = 0; i < NumBins; i++)
	{
		if (Count[i] > 0)
		{
			Std[i] = Std[i]/((float) Count[i]);
			Avg[i] = Avg[i]/((float) Count[i]);
		}
	}



	// free memory on the gpu
	cudaFree(dev_Range);
	cudaFree(dev_Index);
	cudaFree(dev_Count);
	cudaFree(dev_Bins);
	cudaFree(dev_Cumprod);
	cudaFree(dev_Sum);
	cudaFree(dev_Max);
	cudaFree(dev_Min);
	cudaFree(dev_Var);
	cudaFree(dev_X);
	cudaFree(dev_Y);

	free(Cumprod);
}