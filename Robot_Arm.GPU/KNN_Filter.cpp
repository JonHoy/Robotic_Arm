#include "NativeDeclarations.h"

#include <amp.h>

using namespace concurrency;

namespace native_library {
	namespace details {
		void KNN_Filter(int* SelectedColors, int NumColors, int Rows, int Columns, int KernelRadius) {
			array_view<int, 2> SelectedColorsView(Rows, Columns, SelectedColors);
			array_view<int, 2> NewSelectedColors(Rows, Columns);
			array_view<int, 3> KNNCount(Rows, Columns, NumColors);
			int RowEnd = Rows - KernelRadius;
			int ColEnd = Columns - KernelRadius;
			parallel_for_each(NewSelectedColors.extent, [=](index<2> idx) restrict (amp)
			{
				int i = idx[0];
				int j = idx[1];
				if (i >= KernelRadius &&  RowEnd < i)
				{
					if (j >= KernelRadius && ColEnd < j)
					{
						for (int ilocal = -KernelRadius; ilocal <= KernelRadius; ilocal++)
						{
							for (int jlocal = -KernelRadius; jlocal  <= KernelRadius; jlocal ++)
							{
								int SelectedValue = SelectedColorsView(ilocal, jlocal);
								KNNCount(i,j,SelectedValue)++;
							}
						}
						int MaxCount = 0;
						for (int iColor = 0; iColor < NumColors; iColor++)
						{
							int CurrentCount = KNNCount(i,j,iColor);
							if (CurrentCount > MaxCount)
							{
								NewSelectedColors(i,j) = iColor;
								MaxCount = CurrentCount;
							}
						}
					}
				}
			});
			NewSelectedColors.copy_to(SelectedColorsView);
			SelectedColorsView.synchronize();
		}
	}

	void KNN_FilterGPU(int* SelectedColors, int NumColors, int Rows, int Columns, int KernelRadius) {
		details::KNN_Filter(SelectedColors, NumColors, Rows, Columns, KernelRadius);
	}

}