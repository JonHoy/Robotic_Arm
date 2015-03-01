#pragma once

#ifndef max
#define max(a,b)            (((a) > (b)) ? (a) : (b))
#endif

#ifndef min
#define min(a,b)            (((a) < (b)) ? (a) : (b))
#endif

enum ScanType {
	LeftRight,
	UpDown,
	RightLeft,
	DownUp
};

struct ScanSettings
{
	int Start; // Starting subscript in the scan
	int End; // Ending subscript in the Scan
	int Increment; // amount added to the subscript on each loop iteration
	int Dimension; // Dimension in the array the scan belongs to
};

struct Blob {
	int PixelCount;
	int RowMin;
	int RowMax;
	int ColMin;
	int ColMax;
	int RowSum;
	int ColSum;

	void addPixel(int Row, int Col) {
		RowMin = min(Row, RowMin);
		RowMax = max(Row, RowMax);
		ColMin = min(Col, ColMin);
		ColMax = max(Col, ColMax);
		PixelCount++;
		RowSum += Row;
		ColSum += Col;
	}
	void operator+ (const Blob& a)
	{
		PixelCount += a.PixelCount;
		RowMin = min(RowMin, a.RowMin);
		RowMax = max(RowMax, a.RowMax);
		ColMin = min(ColMin, a.ColMin);
		ColMax = max(ColMax, a.ColMax);
		RowSum += a.RowSum;
		ColSum += a.ColSum;
	}

};