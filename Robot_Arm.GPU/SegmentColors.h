
#include <amp.h>

using namespace concurrency;

namespace native_library {
	namespace details {	
		array<int,2> SegmentColors(unsigned char* ImagePtr, int Rows, int Cols, unsigned char* ColorsPtr, int NumColors, const int* SelectionIndex);
		void ConvertToBW(array<int,2>& SelectedColors, int SelectionNumber);
	}
}