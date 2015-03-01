#pragma once

#include <vector>
#include "BlobFinder.h"

namespace native_library {
	namespace details {
		std::vector<Blob> BlobFinder(unsigned char* ImagePtr, int Rows, int Cols, unsigned char* ColorsPtr, const int* SelectionIndex,  int NumColors, int SelectedColor);
	}
}