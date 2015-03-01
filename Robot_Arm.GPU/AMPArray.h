#include <amp.h>

using namespace concurrency;

template<typename T, int Rank = 1>
class AMPArray
{
public:
	AMPArray(int Length);
	~AMPArray();
private:
	array<T>* ArrayPtr;
	accelerator* GPUPtr;
};

template class AMPArray<float>;
template class AMPArray<double>;
template class AMPArray<int>;
template class AMPArray<unsigned int>;