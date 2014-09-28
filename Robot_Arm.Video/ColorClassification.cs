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
                KnownColor.Silver,
                KnownColor.Gold,
                KnownColor.DarkRed,
                KnownColor.DarkOrange,
                KnownColor.Aqua,
                KnownColor.DarkBlue,
                KnownColor.DarkGreen,
                KnownColor.DarkGray,
                KnownColor.DarkCyan,
                KnownColor.DarkViolet,
                KnownColor.Navy,
                KnownColor.Moccasin,
            };
            colornames = new string[allColors.Length];
            colorvalues = new byte[allColors.Length, 3];
            for (int iColor = 0; iColor < allColors.Length; iColor++)
            {
                colornames[iColor] = allColors[iColor].ToString();
                Color CurrentColor = Color.FromName(colornames[iColor]);
                colorvalues[iColor, 0] = CurrentColor.R;
                colorvalues[iColor, 1] = CurrentColor.G;
                colorvalues[iColor, 2] = CurrentColor.B;
            }
            
        }

        public short[,] SegmentColors(Emgu.CV.Image<Emgu.CV.Structure.Bgr,Byte> Photo) 
        {
            int Rows = Photo.Rows;
            int Cols = Photo.Cols;
            short[,] SelectedColors = new short[Rows, Cols];
            int numcolors = colorvalues.GetLength(0);
            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Cols; j++)
                {
                    byte BlueVal = (byte)Photo[i, j].Blue;
                    byte RedVal = (byte)Photo[i, j].Red;
                    byte GreenVal = (byte)Photo[i, j].Green;
                    int MinDistance = int.MaxValue;
                    for (int k = 0; k < numcolors; k++)
                    {
                        int distance = Math.Abs(BlueVal - colorvalues[k, 2]) +
                                       Math.Abs(GreenVal - colorvalues[k, 1]) +
                                       Math.Abs(RedVal - colorvalues[k, 0]);
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

        public Image<Emgu.CV.Structure.Bgr, Byte> ReColorPhoto(short[,] SelectedColor)
        { 
            int Rows = SelectedColor.GetLength(0);
            int Cols = SelectedColor.GetLength(1);

            byte[,,] RawMatrix = new Byte[Rows, Cols, 3];
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
            Image<Emgu.CV.Structure.Bgr, Byte> NewPhoto = new Image<Emgu.CV.Structure.Bgr, byte>(RawMatrix);
            return NewPhoto;
        }

        public bool[,] GenerateBW(ref short[,] SegmentedImage, string ColorName)
        {
            bool[,] BW = new bool[SegmentedImage.GetLength(0), SegmentedImage.GetLength(1)];
            for (int iRow = 0; iRow < BW.GetLength(0); iRow++)
            {
                for (int jCol = 0; jCol < BW.GetLength(1); jCol++)
                {
                    int ColorIndex = SegmentedImage[iRow,jCol];
                    if (colornames[ColorIndex].Contains(ColorName)) // if for example "blue" is in the colorname like "light blue" thats the same thing as blue
                    {
                        BW[iRow,jCol] = true;
                    }
                }
            }
            return BW;
        }

        string[] colornames; // name of colors recognized by the computer
        byte[,] colorvalues; // value of colors [R, G, B]  

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
