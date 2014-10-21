using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Robot_Arm.Video
{
    public class GPU
    {
        [DllImport("GPU_Image_Process", CallingConvention = CallingConvention.StdCall)]
        extern unsafe static void SegmentColorsGPU(int* Image, int Rows, int Cols, int Planes, int* Colors, int NumColors, int* SelectedColors);
        [DllImport("GPU_Image_Process", CallingConvention = CallingConvention.StdCall)]
        extern unsafe static void ND_Correlate_CPU(double* Y, double* X, int Y_length, int Dims, double* Range, int* Bins, 
            double* Max, double* Min, double* Std, double* Avg, double* Count);

        public static unsafe int[,] SegmentColors(int[, ,] Image, int[,] Colors)
        { 
            int Rows = Image.GetLength(0);
            int Cols = Image.GetLength(1);
            int Planes = Image.GetLength(2);
            int NumColors = Colors.GetLength(0);
            if (Colors.GetLength(1) != Planes)
                throw new Exception("The Number of columns in Colors must be equal to the number of columns in the image");
            int[,] SelectedColors = new int[Rows, Cols];
            fixed (int* ImagePtr = &Image[0, 0, 0])
            fixed (int* ColorsPtr = &Colors[0, 0])
            fixed (int* SelectedColorsPtr = &SelectedColors[0, 0])
            SegmentColorsGPU(ImagePtr, Rows, Cols, Planes, ColorsPtr, NumColors, SelectedColorsPtr);
            return SelectedColors;
        }
        public static unsafe Statistics ND_Correlate(ref double[] Y, ref double [,]X, int[] N, double[,] R)
        {
            if (N.Length != R.GetLength(0)) 
                throw new Exception("Number of Rows of R must the same as N");
            if (Y.Length != X.GetLength(0))
                throw new Exception("Number of Rows of X must the same as Y");
            if (X.GetLength(1) != N.GetLength(0))
                throw new Exception("Number of Columns of X must be the same as N");
            if (R.GetLength(1) != 2)
                throw new Exception("Number of Columns of R must be equal to 2");
            
            int NumBins = 1;
            for (int i = 0; i < N.Length; i++)
			{
			    NumBins = NumBins * N[i];
			}

            var Correlation = new Statistics();
            Correlation.Avg = new double[NumBins];
            Correlation.Count = new double[NumBins];
            Correlation.Max = new double[NumBins];
            Correlation.Min = new double[NumBins];
            Correlation.Std = new double[NumBins];

            // get the data pointers
            fixed (int* NPtr = &N[0])
            fixed (double* YPtr = &Y[0])
            fixed (double* XPtr = &X[0, 0])
            fixed (double* RPtr = &R[0, 0])
            fixed (double* StdPtr = &Correlation.Std[0])
            fixed (double* AvgPtr = &Correlation.Avg[0])
            fixed (double* MaxPtr = &Correlation.Max[0])
            fixed (double* MinPtr = &Correlation.Min[0])
            fixed (double* CountPtr = &Correlation.Count[0])
            // do the calculations
            ND_Correlate_CPU(YPtr, XPtr, Y.Length, N.Length, RPtr, NPtr, MaxPtr, MinPtr, StdPtr, AvgPtr, CountPtr);

            return Correlation;

        }
    }
    public class Statistics
    { 
        public double[] Max;
        public double[] Min;
        public double[] Avg;
        public double[] Std;
        public double[] Count;
    }
}
