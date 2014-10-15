#include <amp.h>
#include <omp.h>

using namespace concurrency;

extern "C" __declspec ( dllexport ) void _stdcall ND_Correlate(
	const double* Y, 
	const double* X, 
	const int Y_length,
	const int Dims, 
	const double* Range, 
	const int *Bins,
	double* Max,
	double* Min,
	double* Std,
	double* Avg,
	double* Count)
{
	array_view<const double> yView(Y_length, Y);
	array_view<const double, 2> xView(Y_length, Dims, X);
	array_view<const double, 2> rangeView(Dims, 2, Range);

	int NumLocks = 1;
	for (int iDim = 0; iDim < Dims; iDim++)
	{
		NumLocks = NumLocks * Bins[iDim]; // count the total number of locks needed for the Correlation operations
	}

	std::vector<omp_lock_t> BinLocks(NumLocks);
	// calculate the index number of the data point
	std::vector<int> Index(Y_length);
	
# pragma omp parallel for
	for (int iBin = 0; iBin < NumLocks; iBin++)
	{
		omp_init_lock(&BinLocks[iBin]);
	}
	
std::vector<int> cumprod(Dims); 
cumprod[0] = 1;
for (int iDim = 1; iDim < Dims; iDim++)
{
	cumprod[iDim] = Bins[iDim] * cumprod[iDim - 1];
}

// calculate the linear index of the bin

# pragma omp parallel for
	for (int i = 0; i < Y_length; i++)
	{
		array<int> LocalIndex(Dims);
		for (int iDim = 0; iDim < Dims; iDim++)
		{
			double Nbins = (double) Bins[iDim];
			LocalIndex[iDim] = (int) floor((xView(i, iDim) - rangeView(iDim, 1)) / (rangeView(iDim, 2) - rangeView(iDim, 1)) * Nbins);
			// if index is out of bounds clip the value
			if (LocalIndex[iDim] < 0)
				LocalIndex[iDim] = 0;
			else if (LocalIndex[iDim] >= Bins[iDim])
				LocalIndex[iDim] = Bins[iDim] - 1;
		}
		// now use this information to calculate the bin index
		int idx = 0;
		for (int iDim = 0; iDim < Dims; iDim++)
		{
			idx = idx + cumprod[iDim] * LocalIndex[iDim];
		}
		Index[i] = idx;
		omp_set_lock(&BinLocks[idx]); // put the lock on
		Count[idx] = Count[idx] + 1;
		Avg[idx] = Avg[idx] + yView(i); // sum everything up then divide by bincount at the end
		if (i == 0 || Max[idx] < yView(i))
		{
			Max[idx] = yView(i);
		}
		if (i == 0 || Min[idx] > yView(i))
		{
			Min[idx] = yView(i);
		}
		omp_unset_lock(&BinLocks[idx]); // turn the lock off
	}

	for (int i = 0; i < NumLocks; i++)
	{
		if (Count[i] > 0)
			Avg[i] = Avg[i] / Count[i]; // now divide the sums by the bincount to get the average
	}

# pragma omp parallel for	// now we can calculate std
	for (int i = 0; i < Y_length; i++)
	{
		int idx = Index[i];
		omp_set_lock(&BinLocks[idx]); // put the lock on
		Std[idx] = Std[idx] + abs(Avg[idx] - yView(i)); // add up the variance
		omp_unset_lock(&BinLocks[idx]); // turn the lock off
	}

# pragma omp parallel for
	for (int iBin = 0; iBin < NumLocks; iBin++)
	{
		if (Count[iBin] > 0)
		{
			Std[iBin] = Std[iBin] / Count[iBin]; // now divide the variance by the bincount to get the Std
		}
		omp_destroy_lock(&BinLocks[iBin]); // finally destroy the lock
	}
}
