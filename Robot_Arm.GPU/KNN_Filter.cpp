#include "NativeDeclarations.h"

#include <amp.h>

using namespace concurrency;

namespace native_library {
	namespace details {
		array<int,2> KNN_Filter(array<int,2>& SelectedColors, int NumColors, int KernelRadius) {
			int Rows = SelectedColors.extent[0];
			int Columns = SelectedColors.extent[1];	
			array<int, 2> NewSelectedColors(SelectedColors.extent);
			array<int, 3> KNNCount(Rows, Columns, NumColors);		
			int RowEnd = Rows - KernelRadius;
			int ColEnd = Columns - KernelRadius;
			parallel_for_each(NewSelectedColors.extent, [=, &KNNCount, &NewSelectedColors, &SelectedColors](index<2> idx) restrict (amp)
			{
				int i = idx[0];
				int j = idx[1];
				if (i >= KernelRadius &&  RowEnd > i)
				{
					if (j >= KernelRadius && ColEnd > j)
					{
						for (int ilocal = -KernelRadius; ilocal <= KernelRadius; ilocal++)
						{
							for (int jlocal = -KernelRadius; jlocal  <= KernelRadius; jlocal ++)
							{
								int SelectedValue = SelectedColors(i + ilocal,j + jlocal);
								KNNCount(i,j,SelectedValue)++;
							}
						}
						int MaxCount = 0;
						for (int iColor = 0; iColor < NumColors; iColor++)
						{
							int CurrentCount = KNNCount(i,j,iColor);
							if (CurrentCount > MaxCount)
							{
								NewSelectedColors(idx) = iColor;
								MaxCount = CurrentCount;
							}
						}
					}
				}
			});
			return NewSelectedColors;
		}
	}
}