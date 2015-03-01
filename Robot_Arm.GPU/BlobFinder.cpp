#include "NativeDeclarations.h"
#include "SegmentColors.h"
#include "BlobFinder.h"
#include <algorithm>


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
		std::vector<int> FixBlobImage(array<int,2>& BlobImage, ScanType ScanDirection) {
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
			array<int,2> DimCheck(ex);
			parallel_for_each(ex, [=, &BlobImage, &DimCheck] (index<2> idx) restrict (amp)
			{
				int PreviousVal = 0;
				int DiffCount = 0;
				auto old_idx = idx;
				for (int i = Settings.Start; i < Settings.End; i = i + Settings.Increment)
				{
					idx[Settings.Dimension] = i;
					int CurrentVal = BlobImage(idx);
					int NewVal = min(CurrentVal, PreviousVal);
					if (PreviousVal == 0)
					{
						NewVal = CurrentVal;
					}
					if (CurrentVal != NewVal)
					{
						BlobImage(idx) = NewVal;
						DiffCount++;
					}
					PreviousVal = CurrentVal;
				}
				DimCheck(old_idx) = DiffCount;
			});
			std::vector<int> CheckSum(ex.size());
			copy(DimCheck, CheckSum.begin());
			return CheckSum;
		}
		std::vector<Blob> GetBlobs(array<int,2>& BlobImage) {
			int Rows = BlobImage.extent[0];
			int Cols = BlobImage.extent[1];
			int NumElements = Rows * Cols;
			std::vector<int> BlobImageCPU(NumElements);
			std::vector<Blob> Blobs(NumElements);
			copy(BlobImage, BlobImageCPU.begin());
			
			for (int i = 0; i < NumElements; i++)
			{
				int BlobId = BlobImageCPU[i];
				Blobs[BlobId].addPixel(i % Cols, i/Cols);
			}
			// remove blobs with pixelcounts equal to 0 
			Blobs.erase(std::remove_if(Blobs.begin(), Blobs.end(),
				[] (Blob item) -> bool {return item.PixelCount == 0; }), Blobs.end());
			return Blobs;
		}
		std::vector<Blob> MakeBlobs(array<int,2>& BW_Image) {
			auto BlobImage = GetBlobImage(BW_Image);
			bool ImageFixed = false; // make sure all blobs that are spatially connected  
			while (ImageFixed == false)
			{
				FixBlobImage(BlobImage, ScanType::LeftRight);
				FixBlobImage(BlobImage, ScanType::UpDown);
				auto CheckSum_RL = FixBlobImage(BlobImage, ScanType::RightLeft);				
				auto CheckSum_DU = FixBlobImage(BlobImage, ScanType::DownUp);
				bool RL_Status = std::any_of(CheckSum_RL.begin(), CheckSum_RL.end(), [] (int Number) -> bool {return Number > 0;});
				bool DU_Status = std::any_of(CheckSum_DU.begin(), CheckSum_DU.end(), [] (int Number) -> bool {return Number > 0;});
				if (RL_Status == 0 && DU_Status == 0)
				{
					ImageFixed = true;
				}
			}
			return GetBlobs(BlobImage);
		}
		std::vector<Blob> BlobFinder(unsigned char* ImagePtr, int Rows, int Cols, unsigned char* ColorsPtr, const int* SelectionIndex,  int NumColors, int SelectedColor) {
			auto SelectionArray = SegmentColors(ImagePtr, Rows, Cols, ColorsPtr, NumColors, SelectionIndex); // Determine Which color each pixel corresponds to
			ConvertToBW(SelectionArray, SelectedColor); // convert to BW
			return MakeBlobs(SelectionArray); // Turn BW image into an vector of blobs
		}
	}
}