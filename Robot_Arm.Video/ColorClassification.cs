using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;


namespace Robot_Arm.Video
{
    // class that contains the information about how to classify a RGB Color
    public class ColorClassification
    {
        public ColorClassification()
        {
            KnownColor[] allColors = new KnownColor[]{
                KnownColor.Red,
                KnownColor.Blue,
                KnownColor.Green,
                KnownColor.Black,
                KnownColor.White,
                KnownColor.Yellow,
                KnownColor.Purple,
                KnownColor.Orange,
                KnownColor.Pink,
                KnownColor.Brown,
                KnownColor.Gray,
                KnownColor.DarkRed,
                KnownColor.DarkOrange,
                KnownColor.DarkBlue,
                KnownColor.DarkGreen,
                KnownColor.DarkGray,
            };
            colornames = new string[allColors.Length];
            colorvalues = new float[allColors.Length, 3];
            for (int iColor = 0; iColor < allColors.Length; iColor++)
            {
                colornames[iColor] = allColors[iColor].ToString();
                Color CurrentColor = Color.FromName(colornames[iColor]);
                colorvalues[iColor, 0] = CurrentColor.GetHue();
                colorvalues[iColor, 1] = CurrentColor.GetSaturation();
                colorvalues[iColor, 2] = CurrentColor.GetBrightness();
            }
            
        }

        public short[,] SegmentColors(Emgu.CV.Image<Emgu.CV.Structure.Hsv,float> Photo) 
        {
            int Rows = Photo.Rows;
            int Cols = Photo.Cols;
            short[,] SelectedColors = new short[Rows, Cols];
            int numcolors = colorvalues.GetLength(0);

            float[, ,] PhotoData = Photo.Data;

            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Cols; j++)
                {
                    //byte BlueVal = (float)Photo[i, j].Blue;
                    //byte RedVal = (byte)Photo[i, j].Red;
                    //byte GreenVal = (byte)Photo[i, j].Green;
                    float Hue = (float) PhotoData[i, j, 0];
                    float Sat = (float) PhotoData[i, j, 1];
                    float Int = (float) PhotoData[i, j, 2];
                    //int MinDistance = int.MaxValue;
                    float MinDistance = float.MaxValue;
                    for (int k = 0; k < numcolors; k++)
                    {
                        //int distance = Math.Abs(BlueVal - colorvalues[k, 2]) +
                        //               Math.Abs(GreenVal - colorvalues[k, 1]) +
                        //               Math.Abs(RedVal - colorvalues[k, 0]);
                        float distance = Math.Abs(Hue - colorvalues[k, 0]) / 360f +
                                       Math.Abs(Sat - colorvalues[k, 1]) +
                                       Math.Abs(Int - colorvalues[k, 2]);
                        if (MinDistance > distance)
                        {
                            MinDistance = distance;
                            SelectedColors[i, j] = (short)k;
                        }
                    }
                }
            }
            );
            return SelectedColors;
        }

        public Image<Emgu.CV.Structure.Hsv, float> ReColorPhoto(short[,] SelectedColor)
        { 
            int Rows = SelectedColor.GetLength(0);
            int Cols = SelectedColor.GetLength(1);

            float[,,] RawMatrix = new float[Rows, Cols, 3];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    int SelectedVal = SelectedColor[i, j];
                    RawMatrix[i, j, 2] = colorvalues[SelectedVal, 0];
                    RawMatrix[i, j, 1] = colorvalues[SelectedVal, 1];
                    RawMatrix[i, j, 0] = colorvalues[SelectedVal, 2];
                }
            }
            Image<Emgu.CV.Structure.Hsv, float> NewPhoto = new Image<Emgu.CV.Structure.Hsv, float>(RawMatrix);
            return NewPhoto;
        }

        public bool[,] GenerateBW(ref short[,] SegmentedImage, string ColorName)
        {
            bool[,] BW = new bool[SegmentedImage.GetLength(0), SegmentedImage.GetLength(1)];
            bool[] SelectionResult = new bool[colornames.Length];
            for (int i = 0; i < SelectionResult.Length; i++)
            {
                if (colornames[i].Contains(ColorName)) // if for example "blue" is in the colorname like "light blue" thats the same thing as blue
                {
                    SelectionResult[i] = true;
                }
            }
            for (int iRow = 0; iRow < BW.GetLength(0); iRow++)
            {
                for (int jCol = 0; jCol < BW.GetLength(1); jCol++)
                {
                    int ColorIndex = SegmentedImage[iRow,jCol];
                    BW[iRow, jCol] = SelectionResult[ColorIndex];
                }
            }
            return BW;
        }

        string[] colornames; // name of colors recognized by the computer
        float[,] colorvalues; // value of colors [R, G, B]  

    }

    

    public class Webcam
    {
        public Webcam() 
        {
            Device = new Capture();
            
        }

        private Capture Device;
    }

}
