//#include <cuda.h>
//#include <cuda_runtime.h>
//#include <math.h>
//#include "NativeDeclarations.h"
//#include <memory>
//
//#define PI 3.14159f
//#define BlockSize 16
//#define IterationLimit 10
//#define Planes 3
//
//template<typename T, int BLOCK_SIZE>
//__global__ void MeanFilterKernel(int Rows, int Cols, T* Image, int NumIterations)
//{
//    // Thread index
//    int tx = threadIdx.x;
//    int ty = threadIdx.y;
//
//	int x = threadIdx.x + blockIdx.x * blockDim.x;
//	int y = threadIdx.y + blockIdx.y * blockDim.y;
//
//	// put padding in for the edge (do all calcs in float and then convert back to native format)
//	__shared__ float OriginalImageTile[BLOCK_SIZE + 2][BLOCK_SIZE + 2][Planes];
//	
//	__shared__ float NewImageTile[BLOCK_SIZE][BLOCK_SIZE][Planes];
//
//	for (int iPlane = 0; iPlane < Planes; iPlane++)
//	{
//		int idx = (x + y * blockDim.x * gridDim.x) * Planes + iPlane;
//		for (int i = 0; i < NumIterations; i++)
//		{	
//			float val = (float) Image[idx];
//			if (idx < Rows * Cols * Planes)
//			{
//				if (tx == 0)
//					OriginalImageTile[0][ty + 1][iPlane] = val;	
//				if (ty == 0)
//					OriginalImageTile[tx + 1][0][iPlane] = val;
//				if (tx == blockDim.x)
//					OriginalImageTile[tx + 2][ty + 1][iPlane] = val;
//				if (ty == blockDim.y)
//					OriginalImageTile[tx + 1][ty + 2][iPlane] = val;
//
//				OriginalImageTile[tx + 1][ty + 1][iPlane] = val;
//			}
//			__syncthreads();
//			NewImageTile[tx][ty][iPlane] = (
//			OriginalImageTile[tx + 1][ty + 1][iPlane] +
//			OriginalImageTile[tx][ty + 1][iPlane] +
//			OriginalImageTile[tx + 2][ty + 1][iPlane] +
//			OriginalImageTile[tx + 1][ty][iPlane] +
//			OriginalImageTile[tx + 1][ty + 2][iPlane])/5;
//			__syncthreads();
//		}
//		Image[idx] = (T)NewImageTile[tx + 1][ty + 1][iPlane];
//	}
//			
//}
//
//// helper routine to convert colorspace from rgb to hsi
//__host__ __device__ void RGB2HSI(float R, float G, float B, float* H, float* S, float* I) {
//	*I = (R + G + B)/3;
//	float MinColor;
//	
//	if (R < G)
//		MinColor = R;
//	else
//		MinColor = G;
//	if (MinColor > B)
//		MinColor = B;
//	
//	*S = 1 - MinColor / *I;
//	
//	float Theta;
//	if (R == 0 && G == 0 && B == 0)
//		Theta = 0;
//	else
//		Theta = acosf(((R - G) + (R - B))/ 2 / sqrtf((R - G)*(R - G) + (R - B)*(G - B))); 
//	if (B <= G)
//		*H = Theta/(2 * PI);
//	else
//		*H = ((2 * PI) - Theta)/(2*PI);
//}
//
//template <typename T>
//__global__ void SegmentKernel(T* Image, int Rows, int Cols, float* Colors, int NumColors, int* SelectedColors) {
//	float R; // red value (0 to 1)
//	float G; // blue value (0 to 1)
//	float B; // green value (0 to 1)
//	int x = threadIdx.x + blockIdx.x * blockDim.x;
//	int y = threadIdx.y + blockIdx.y * blockDim.y;
//	int idx = (x + y * blockDim.x * gridDim.x);
//	R = ((float) Image[idx * Planes]) / 255.0f;
//	G = ((float) Image[idx * Planes + 1]) / 255.0f;
//	B = ((float) Image[idx * Planes + 2]) / 255.0f;
//	float H;
//	float S;
//	float I;
//	RGB2HSI(R, G, B, &H, &S, &I);
//	float MinDistance = 3;
//	float Distance;
//	int ClosestColor = 0;
//	for (int iColor = 0; iColor < NumColors; iColor++)
//	{
//		float DistH = H - Colors[iColor * Planes];
//		float DistS = S - Colors[iColor * Planes + 1];
//		float DistI = H - Colors[iColor * Planes + 2];
//		Distance = DistH * DistH + DistI * DistI + DistS * DistS;
//		if (Distance < MinDistance) {
//			MinDistance = Distance;
//			ClosestColor = iColor;
//		}		
//	}
//	SelectedColors[idx] = ClosestColor;
//}
//// RGB values are 0-1 and HSI outputs 0-1
//
//namespace native_library {
//	namespace details{
//		template<typename T>
//		void SegmentColorsCUDA(T* Image, int Rows, int Columns, T* Colors, int NumColors, int* SelectedColors)
//		{
//			cudaError Status;
//			int PixelCount = Rows * Columns;
//			//T* dev_Image;
//			//float* dev_Colors;
//			int* dev_SelectedColors;
//			float* Colors_float;
//			Colors_float = (float*) malloc(NumColors * Planes * sizeof(float));
//			for (int iColor = 0; iColor < NumColors; iColor++)
//			{
//				float R = ((float) Colors[iColor * Planes]) / 255.0f;
//				float G = ((float) Colors[iColor * Planes + 1]) / 255.0f;
//				float B = ((float) Colors[iColor * Planes + 2]) / 255.0f;
//				RGB2HSI(R, G, B, &Colors_float[iColor * Planes] , &Colors_float[iColor * Planes + 1], &Colors_float[iColor * Planes + 2]);
//				printf("H: %f",Colors_float[iColor * Planes]);
//				printf(" |S: %f",Colors_float[iColor * Planes + 1]);
//				printf(" |I: %f\n",Colors_float[iColor * Planes + 2]);
//			}
//			//Status = cudaMalloc((void **) &dev_Image, PixelCount * Planes * sizeof(T));
//			Status = cudaMalloc((void **) &dev_SelectedColors,  PixelCount * sizeof(int));
//			//Status = cudaMalloc((void **) &dev_Colors, NumColors * Planes * sizeof(float));
//			//Status = cudaMemcpy(dev_Image, Image, PixelCount * Planes * sizeof(T), cudaMemcpyHostToDevice);
//			//Status = cudaMemcpy(dev_Colors, Colors_float, NumColors * Planes * sizeof(float), cudaMemcpyHostToDevice);
//			dim3 TileSize(BlockSize, BlockSize);
//			dim3 GridSize(Rows/BlockSize, Columns/BlockSize);
////			 Filter image data on gpu side before processing
//			//MeanFilterKernel<T, BlockSize><<<TileSize, GridSize>>>(Rows, Columns, dev_Image, IterationLimit);
//			//SegmentKernel<T><<<TileSize, GridSize>>>(dev_Image, Rows, Columns, dev_Colors, NumColors, dev_SelectedColors);
//			//Status = cudaMemcpy(SelectedColors, dev_SelectedColors, PixelCount * sizeof(int), cudaMemcpyDeviceToHost);
//			free(Colors_float);
//			cudaFree(dev_SelectedColors);
//			//cudaFree(dev_Image);
////			cudaFree(dev_Colors);
//		}
//	}
//
//	void SegmentColorsCUDA(float* Image, int Rows, int Columns, float* Colors, int NumColors, int* SelectedColors) {
//		details::SegmentColorsCUDA<float>(Image, Rows, Columns, Colors, NumColors, SelectedColors);
//	}
//
//	void SegmentColorsCUDA(int* Image, int Rows, int Columns, int* Colors, int NumColors, int* SelectedColors) {
//		details::SegmentColorsCUDA<int>(Image, Rows, Columns, Colors, NumColors, SelectedColors);
//	}
//
//	void SegmentColorsCUDA(unsigned char* Image, int Rows, int Columns, unsigned char* Colors, int NumColors, int* SelectedColors) {
//		details::SegmentColorsCUDA<unsigned char>(Image, Rows, Columns, Colors, NumColors, SelectedColors);
//	}
//
//}
