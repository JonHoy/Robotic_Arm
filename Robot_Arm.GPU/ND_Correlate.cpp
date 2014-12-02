#include <amp.h>
#include <amp_math.h>
#include "NativeDeclarations.h"
#include <math.h>

using namespace concurrency;

namespace native_library {
	namespace details {
		template<typename T, int Dims>
		void ND_Correlate(const T* Y,
			const T* X,
			const int Y_Length,
			const T* Range,
			const int* Bins,
			T* Max,
			T* Min,
			T* Avg,
			int* Count)
		{
			array_view<const T> yView(Y_Length, &Y[0]);
			array_view<const T, 2> xView(Y_Length, Dims, &X[0]);
			array_view<const T, 2> rangeView(Dims, 2, &Range[0]);
			array_view<const int> binsView(Dims, &Bins[0]);

			int NumBins = 1;
			for (int iDim = 0; iDim < Dims; iDim++)
			{
				NumBins = NumBins * Bins[iDim];
			}

			static const int Y_TileSize = 1024;
			int tile_count = ceil((double) Y_Length / (double) Y_TileSize);

			array_view<T> maxView(NumBins, Max);
			array_view<T> minView(NumBins, Min);
			array_view<T> avgView(NumBins, Avg);
			array_view<int> countView(NumBins, Count);

			array<T, 2> Max_Matrix(NumBins, tile_count);
			array<T, 2> Min_Matrix(NumBins, tile_count);
			array<T, 2> Avg_Matrix(NumBins, tile_count);
			array<int, 2> Count_Matrix(NumBins, tile_count);
			array<int, 1> Index(Y_Length);
			
			parallel_for_each(Index.extent.tile<Y_TileSize>(),
				[=, &Index, &Max_Matrix, &Min_Matrix, &Count_Matrix, &Avg_Matrix](tiled_index<Y_TileSize> t_idx) restrict (amp)
			{
				tile_static T y_tile_data[Y_TileSize];
				tile_static T x_tile_data[Y_TileSize][Dims];
				tile_static T range_tile_data[Dims][2];
				tile_static int bins_tile_data[Dims];
				tile_static int cumprod_tile_data[Dims];

				int tile_number = t_idx.tile[0];
				int local_row = t_idx.local[0];
				int global_row = t_idx.global[0];
				if (global_row < Y_Length)
				{
					y_tile_data[local_row] = yView[global_row];
					for (int iDim = 0; iDim < Dims; iDim++)
					{
						x_tile_data[local_row][iDim] = xView(global_row, iDim);
					}
					if (local_row == 0)
					{
						cumprod_tile_data[0] = 1;
						for (int iDim = 0; iDim < Dims; iDim++)
						{
							range_tile_data[iDim][0] = rangeView(iDim, 0);
							range_tile_data[iDim][1] = rangeView(iDim, 1);
							bins_tile_data[iDim] = binsView(iDim);
							if (iDim > 0)
							{
								cumprod_tile_data[iDim] = bins_tile_data[iDim] * cumprod_tile_data[iDim - 1];
							}
						}
					}
				}
				t_idx.barrier.wait_with_tile_static_memory_fence();
				if (local_row == 0)
				{
					for (int iPt = 0; iPt < Y_TileSize; iPt++)
					{
						int idx = 0;
						for (int iDim = 0; iDim < Dims; iDim++)
						{
							T Nbins = (T) bins_tile_data[iDim];
							T RawIndex = Nbins * (x_tile_data[local_row][iDim] - range_tile_data[iDim][0])/(range_tile_data[iDim][1] - range_tile_data[iDim][0]);
							int LocalIndex = (int) concurrency::fast_math::floor((float)RawIndex);
							if (LocalIndex < 0)
								LocalIndex = 0;
							else if (LocalIndex >= bins_tile_data[iDim])
								LocalIndex = binsView[iDim] - 1;
							idx = idx + cumprod_tile_data[iDim] * LocalIndex;
						}
						Index[global_row] = idx;
						Count_Matrix(idx, tile_number) = Avg_Matrix(idx, tile_number);
						Avg_Matrix(idx, tile_number) = Avg_Matrix(idx, tile_number) + y_tile_data[local_row];
						if (local_row == 0 || Max_Matrix(idx, tile_number) < y_tile_data[local_row])
						{
							Max_Matrix(idx, tile_number) = y_tile_data[local_row];
						}
						if (local_row == 0 || Min_Matrix(idx, tile_number) > y_tile_data[local_row])
						{
							Min_Matrix(idx, tile_number) = y_tile_data[local_row];
						}
					}
				}
			});
			parallel_for_each(countView.extent, [=, &Max_Matrix, &Min_Matrix, &Count_Matrix, &Avg_Matrix](index<1> idx) restrict(amp)
			{
				const int BinNumber = idx[0];
				int totalcount = 0;
				int totalsum = 0;
				for (int i = 0; i < tile_count; i++) 
				{
					totalsum = totalsum + Avg_Matrix(BinNumber, i);
					totalcount = totalcount + Count_Matrix(BinNumber, i);
				}
				countView[BinNumber] = totalcount;
				if (totalcount > 0)
					avgView[BinNumber] = totalsum/ ((T) totalcount);
				T maxValue = avgView[BinNumber];
				T minValue = avgView[BinNumber];
				for (int i = 0; i < tile_count; i++)
				{
					if (maxValue < Max_Matrix(BinNumber, i))
						maxValue = Max_Matrix(BinNumber, i);
					if (minValue > Min_Matrix(BinNumber, i))
						minValue = Min_Matrix(BinNumber, i);
				}
				maxView[BinNumber] = maxValue;
				minView[BinNumber] = minValue;
			});
		}
	}

	void	CorrelateGPU_1(const double* Y, const double* X, const int Y_Length, const double* Range, const int* Bins, double* Max, double* Min, double* Avg, int* Count) {
		details::ND_Correlate<double, 1>(Y, X, Y_Length, Range, Bins, Max, Min, Avg, Count);
	}
	void	CorrelateGPU_1(const float* Y, const float* X, const int Y_Length, const float* Range, const int* Bins, float* Max, float* Min, float* Avg, int* Count) {
		details::ND_Correlate<float, 1>(Y, X, Y_Length, Range, Bins, Max, Min, Avg, Count);
	}
}