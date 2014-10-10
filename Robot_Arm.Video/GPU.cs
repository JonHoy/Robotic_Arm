using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Robot_Arm.Video
{
    class GPU
    {
        [DllImport("GPU_Image_Process", CallingConvention = CallingConvention.StdCall)]
        extern unsafe static void SegmentColorsGPU(int* Image, int Rows, int Cols, int Planes, int* Colors, int NumColors, int* SelectedColors);
        
        public static unsafe int[,] SegmentColors(int[, ,] Image, int[,] Colors)
        { 
            int Rows = Image.GetLength(0);
            int Cols = Image.GetLength(1);
            int Planes = Image.GetLength(2);
            int NumColors = Colors.GetLength(0);
            if (Colors.GetLength(1) != Planes)
            {
                throw new Exception("The Number of columns in Colors must be equal to the number of columns in the image");
            }
            int[,] SelectedColors = new int[Rows, Cols];
            fixed (int* ImagePtr = &Image[0, 0, 0])
            fixed (int* ColorsPtr = &Colors[0, 0])
            fixed (int* SelectedColorsPtr = &SelectedColors[0, 0])
            SegmentColorsGPU(ImagePtr, Rows, Cols, Planes, ColorsPtr, NumColors, SelectedColorsPtr);
            return SelectedColors;
        }
    }
}
