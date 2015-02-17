
#include "NativeDeclarations.h"

namespace native_library {

	class GPUArray
	{
	public:
		GPUArray(long);
		~GPUArray();

	private:
		Concurrency::array<float,1> AMPArray;
	};

	GPUArray::GPUArray(long ArraySize)
	{
		AMPArray.
	}

	GPUArray::~GPUArray()
	{
	}

}