#include "NativeDeclarations.h"
#include "SegmentColors.h"
#include "BlobFinder.h"
#include <algorithm>
#include <ppl.h>

using namespace concurrency;

namespace native_library {
	namespace details {
		array<int,2> GetBlobImage(array<int,2>& BW_Image) {
			array<int,2> BlobImage(BW_Image.extent);
			int Rows = BW_Image.extent[0];
			int Cols = BW_Image.extent[1];
			extent<1> ColExtent(Cols);
			parallel_for_each(ColExtent, [=, &BlobImage, &BW_Image] (index<1> idx) restrict (amp)
			{
				int jCol = idx[0];
				int PreviousVal = 0;
				int BlobNumber = jCol * Rows;
				for (int iRow = 1; iRow < Rows; iRow++)
				{
					int CurrentVal = BW_Image(iRow, jCol);
					if (CurrentVal != 0)
					{
						if (CurrentVal != PreviousVal)
						{
							BlobNumber++;
						}
						BlobImage(iRow, jCol) = BlobNumber;
					}
					else 
					{
						BlobImage(iRow, jCol) = 0;
					}
					PreviousVal = CurrentVal;

				}
			});
			return BlobImage;
		}
		std::vector<int> FixBlobImage(array_view<int,2>& BlobImage, ScanType ScanDirection) {
			ScanSettings Settings;
			switch (ScanDirection)
			{								
				case LeftRight:
					Settings.Dimension = 1;
					Settings.Increment = 1;
					break;
				case UpDown:
					Settings.Dimension = 0;
					Settings.Increment = 1;
					break;
				case RightLeft:
					Settings.Dimension = 1;
					Settings.Increment = -1;
					break;
				case DownUp:
					Settings.Dimension = 0;
					Settings.Increment = -1;
					break;
			}
			Settings.Start = 0;
			Settings.End = BlobImage.extent[Settings.Dimension];
			if (Settings.Increment == -1)
				std::swap(Settings.Start, Settings.End);

			extent<2> ex(BlobImage.extent);
			ex[Settings.Dimension] = 1;
			
			auto Start = Settings.Start;
			auto End = Settings.End;
			auto Dimension = Settings.Dimension;
			auto Increment = Settings.Increment;

			int Length = ex[0] * ex[1];

			std::vector<int> DimCheck(ex[Dimension]);
			array_view<int,2> DimCheckView(ex[0], ex[1], &DimCheck[0]);

			parallel_for_each(ex, [&BlobImage, &DimCheckView, Start, End, Dimension, Increment] (index<2> idx) restrict (cpu) // determine which blobs are connected and fix them
			{
				int PreviousVal = 0;
				int DiffCount = 0;
				auto old_idx = idx;
				for (int i = Start; i < End; i = i + Increment)
				{
					idx[Dimension] = i;
					int CurrentVal = BlobImage(idx);
					if (PreviousVal != 0 && CurrentVal != 0)
					{
						BlobImage(idx) = min(CurrentVal, PreviousVal);
					}
					PreviousVal = CurrentVal;
				}
				DimCheckView(old_idx) = DiffCount;
			});
		}
		std::vector<Blob> GetBlobs(std::vector<int> BlobImageCPU, int Cols, int Rows) {
			int NumElements = Rows * Cols;
			std::vector<Blob> Blobs(NumElements);			
			for (int i = 0; i < NumElements; i++)
			{
				int BlobId = BlobImageCPU[i];
				if (BlobId > 0 && BlobId < Blobs.size())
					Blobs[BlobId].addPixel(i % Cols, i/Cols);
			}
			// remove blobs with pixelcounts equal to 0 
			Blobs.erase(std::remove_if(Blobs.begin(), Blobs.end(),
				[] (Blob item) -> bool {return item.PixelCount == 0; }), Blobs.end());
			return Blobs;
		}
		std::vector<Blob> MakeBlobs(array<int,2>& SelectedColors, int SelectedColor) {
			int Length = SelectedColors.extent[0] * SelectedColors.extent[1];
			std::vector<int> BlobImageCPU(Length);
			copy(SelectedColors, BlobImageCPU.begin());
			int Rows = SelectedColors.extent[0];
			int Cols = SelectedColors.extent[1];
			parallel_for(0, Rows,
				[&BlobImageCPU, SelectedColor, Cols](int i) 
				{
					int Index = Cols * i;
					int BlobId = Index;
					int PreviousVal = 0;
					for (int idx = Index; idx < Cols; idx++)
					{
						// first compute whether its a one or zero
						if (BlobImageCPU[idx] == SelectedColor)
							BlobImageCPU[idx] = 1;
						else
							BlobImageCPU[idx] = 0;
						// now compute a one dimensional blob number
						int CurrentVal = BlobImageCPU[idx];
						if (CurrentVal == 1 && PreviousVal != 0)
						{
							BlobImageCPU[idx] = PreviousVal;
						}
						else if (PreviousVal == 0 && CurrentVal == 1) 
						{
							BlobId++; // start a new blob
							BlobImageCPU[idx] = BlobId;
						}
						PreviousVal = CurrentVal;
					}					
				});


			return GetBlobs(SelectedColors, Cols, Rows);
		}
		std::vector<Blob> BlobFinder(unsigned char* ImagePtr, int Rows, int Cols, unsigned char* ColorsPtr, const int* SelectionIndex,  int NumColors, int SelectedColor) {
			auto SelectionArray = SegmentColors(ImagePtr, Rows, Cols, ColorsPtr, NumColors, SelectionIndex); // Determine Which color each pixel corresponds to
			return MakeBlobs(SelectionArray, SelectedColor); // Turn integer image into an vector of blobs
		}
	}
}