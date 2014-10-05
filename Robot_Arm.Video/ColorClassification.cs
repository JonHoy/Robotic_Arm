using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Robot_Arm.Video
{
    // class that contains the information about how to classify a RGB Color
    public class ColorClassification
    {
        public ColorClassification()
        {
            Color[] allColors = new Color[]{
                Color.Red,
                Color.Blue,
                Color.Green,
                Color.Black,
                Color.White,
                Color.Yellow,
                Color.Purple,
                Color.Orange,
                Color.Pink,
                Color.Brown,
                Color.DarkRed,
                Color.DarkOrange,
                Color.DarkBlue,
                Color.DarkGreen,
                Color.OrangeRed,
                Color.LightGreen,
                Color.LimeGreen,
                Color.LightBlue,
                Color.LightYellow,
                Color.MediumPurple,
                Color.Maroon,
                Color.Tan,
                Color.LightGray,
                Color.Moccasin,
                Color.PeachPuff,
                Color.HotPink,
                Color.DeepPink,
                Color.LightPink,
                Color.DarkKhaki,
                Color.RoyalBlue,
                Color.DodgerBlue,
                Color.Goldenrod,
                Color.DarkSlateBlue,
            };
            colornames = new string[allColors.Length];
            colorvalues_BGR = new int[allColors.Length, 3];
            for (int iColor = 0; iColor < allColors.Length; iColor++)
            {
                colornames[iColor] = allColors[iColor].ToKnownColor().ToString();
                Color CurrentColor = Color.FromName(colornames[iColor]);
                colorvalues_BGR[iColor, 0] = (int) CurrentColor.B;
                colorvalues_BGR[iColor, 1] = (int)CurrentColor.G;
                colorvalues_BGR[iColor, 2] = (int) CurrentColor.R;
            }
            
        }


        public short[,] SegmentColors(Image<Bgr, byte> Photo)
        {
            int Rows = Photo.Rows;
            int Cols = Photo.Cols;
            short[,] SelectedColors = new short[Rows, Cols];
            int numcolors = colorvalues_BGR.GetLength(0);

            byte[, ,] PhotoData = Photo.Data;

            Parallel.For(0, Rows, i =>
            {
                int TempBluePhotoData, TempGreenPhotoData, TempRedPhotoData;
                int TempBlueVal, TempGreenVal, TempRedVal;
                int BlueDist, GreenDist, RedDist;
                int distance;
                for (int j = 0; j < Cols; j++)
                {
                    int MinDistance = int.MaxValue;
                    for (int k = 0; k < numcolors; k++)
                    {
                        TempBluePhotoData = (int) PhotoData[i, j, 0];
                        TempGreenPhotoData = (int) PhotoData[i, j, 1];
                        TempRedPhotoData = (int) PhotoData[i, j, 2];

                        TempBlueVal = colorvalues_BGR[k, 0];
                        TempGreenVal = colorvalues_BGR[k, 1];
                        TempRedVal = colorvalues_BGR[k, 2];

                        BlueDist = Math.Abs(TempBlueVal - TempBluePhotoData);
                        GreenDist = Math.Abs(TempGreenVal - TempGreenPhotoData);
                        RedDist = Math.Abs(TempRedVal - TempRedPhotoData);
                        
                        distance = BlueDist + GreenDist + RedDist;

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

        public Image<Bgr, byte> ReColorPhoto(ref short[,] SelectedColor)
        {
            int Rows = SelectedColor.GetLength(0);
            int Cols = SelectedColor.GetLength(1);

            byte[, ,] RawMatrix = new byte[Rows, Cols, 3];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    int SelectedVal = SelectedColor[i, j];
                    RawMatrix[i, j, 0] = (byte) colorvalues_BGR[SelectedVal, 0];
                    RawMatrix[i, j, 1] = (byte) colorvalues_BGR[SelectedVal, 1];
                    RawMatrix[i, j, 2] = (byte) colorvalues_BGR[SelectedVal, 2];
                }
            }
            Image<Bgr, byte> NewPhoto = new Image<Bgr, byte>(RawMatrix);
            return NewPhoto;
        }

        public bool[,] GenerateBW(ref short[,] SegmentedImage, string[] ColorName)
        {
            bool[,] BW = new bool[SegmentedImage.GetLength(0), SegmentedImage.GetLength(1)];
            bool[] SelectionResult = new bool[colornames.Length];
            for (int i = 0; i < SelectionResult.Length; i++)
            {
                for (int j = 0; j < ColorName.Length; j++)
		        {
                if (colornames[i].Equals(ColorName[j])) // if for example "blue" is in the colorname like "light blue" thats the same thing as blue
                {
                    SelectionResult[i] = true;
                    break;
                }			 
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

        public Image<Gray, Byte> Generate_GrayCVImage(ref bool[,] BW)
        {
            int Rows = BW.GetLength(0);
            int Cols = BW.GetLength(1);
            byte[,,] ByteData = new byte[Rows, Cols, 1];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; i++)
                {
                    if (BW[i,j] == true)
                    {
                        ByteData[i, j, 1] = 255;
                    }
                }
            }
            Image<Gray, byte> GrayCV_Image = new Image<Gray, byte>(ByteData);
            return GrayCV_Image;
            
        }

        string[] colornames; // name of colors recognized by the computer
        int[,] colorvalues_BGR; // value of colors [BGR]
    }

}
