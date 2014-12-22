using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot_Arm_GPU;

namespace ManualTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            float[,] Colors = { 
                              { 255, 0, 0 }, 
                              { 0, 255, 0 }, 
                              { 0, 0, 255 },
                              };
            var Image = new float[1280, 720, 3];
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    for (int k = 0; k < Image.GetLength(2); k++)
                    {
                        Image[i, j, k] = (float)(rnd.NextDouble() * 255);
                    }
                }
            }
            int[] SelectedColors = GPU.SegmentColors(Image, Colors);
        }
    }
}
