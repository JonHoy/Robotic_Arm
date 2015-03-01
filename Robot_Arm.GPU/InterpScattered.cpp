#include "NativeDeclarations.h"
#include <amp_math.h>
#include <amp.h>
#include <limits>

using namespace concurrency;
using namespace concurrency::fast_math;

namespace native_library {
	namespace details {
		template <typename T>
		void InterpScattered(T* RawScatter, int Rows, int Columns, int KernelRadius, int Iterations) {
			array_view<T,2> RawScatterView(Rows, Columns, RawScatter);
			array<T,2> NewScatter(Rows, Columns);
			int RowEnd = Rows - KernelRadius;
			int ColEnd = Columns - KernelRadius;
			RawScatterView.copy_to(NewScatter);
			for (int iIteration = 0; iIteration < Iterations; iIteration++)
			{
				parallel_for_each(RawScatterView.extent, [=, &NewScatter](index<2> idx) restrict (amp)
				{
					int i = idx[0];
					int j = idx[1];
					if (i >= KernelRadius &&  RowEnd > i)
					{
						if (j >= KernelRadius && ColEnd > j)
						{
							T WeightSum = 0;
							T FactorProduct = 0;
							int Count = 0;
							for (int ilocal = -KernelRadius; ilocal <= KernelRadius; ilocal++)
							{
								for (int jlocal = -KernelRadius; jlocal  <= KernelRadius; jlocal++)
								{
									T Value = (T) RawScatterView(i + ilocal,j + jlocal);
									if (isnan(Value) == false)
									{
										float WeightFactor = 1.0f/(fabsf((float)ilocal) + fabsf((float)jlocal) + 1.0f);
										FactorProduct = FactorProduct + ((T) WeightFactor) * Value;
										WeightSum = WeightSum + (T) WeightFactor;
										Count++;
									}
								}
							}
							if (Count > 0)
							{
								T InterpVal = FactorProduct / WeightSum;
								NewScatter(i,j) = InterpVal;
							}
						}
					}
				});
				NewScatter.copy_to(RawScatterView);
			}
			RawScatterView.synchronize();
		}
	}

	void InterpScatteredGPU(float* RawScatter, int Rows, int Columns, int KernelRadius, int Iterations) {
		return details::InterpScattered<float>(RawScatter, Rows, Columns, KernelRadius, Iterations);
	}
	void InterpScatteredGPU(double* RawScatter, int Rows, int Columns, int KernelRadius, int Iterations) {
		return details::InterpScattered<double>(RawScatter, Rows, Columns, KernelRadius, Iterations);
	}
}