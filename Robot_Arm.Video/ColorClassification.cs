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
            };
            colornames = new string[allColors.Length];
            colorvalues_HSV = new float[allColors.Length, 3];
            colorvalues_BGR = new byte[allColors.Length, 3];
            for (int iColor = 0; iColor < allColors.Length; iColor++)
            {
                colornames[iColor] = allColors[iColor].ToKnownColor().ToString();
                Color CurrentColor = Color.FromName(colornames[iColor]);
                colorvalues_HSV[iColor, 0] = CurrentColor.GetHue();
                colorvalues_HSV[iColor, 1] = CurrentColor.GetSaturation();
                colorvalues_HSV[iColor, 2] = CurrentColor.GetBrightness();
                colorvalues_BGR[iColor, 0] = CurrentColor.B;
                colorvalues_BGR[iColor, 1] = CurrentColor.G;
                colorvalues_BGR[iColor, 2] = CurrentColor.R;
            }
            
        }

        public short[,] SegmentColors(Image<Hsv,float> Photo) 
        {
            int Rows = Photo.Rows;
            int Cols = Photo.Cols;
            short[,] SelectedColors = new short[Rows, Cols];
            int numcolors = colorvalues_HSV.GetLength(0);

            float[, ,] PhotoData = Photo.Data;

            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Cols; j++)
                {
                    float Hue = (float) PhotoData[i, j, 0];
                    float Sat = (float) PhotoData[i, j, 1];
                    float Val = (float) PhotoData[i, j, 2];
                    float MinDistance = float.MaxValue;
                    for (int k = 0; k < numcolors; k++)
                    {
                        float HueDist = Math.Min(Math.Abs(Hue - colorvalues_HSV[k, 0]),
                            Math.Abs(360 - Hue - colorvalues_HSV[k, 0])) / 360f;
                        float SatDist = Math.Abs(Sat - colorvalues_HSV[k, 1]);
                        float ValDist = Math.Abs(Val - colorvalues_HSV[k, 2]);
                        float distance = SatDist + (ValDist * (float) 0.5) + HueDist;

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

        public short[,] SegmentColors(Image<Bgr, byte> Photo)
        {
            int Rows = Photo.Rows;
            int Cols = Photo.Cols;
            short[,] SelectedColors = new short[Rows, Cols];
            int numcolors = colorvalues_BGR.GetLength(0);

            byte[, ,] PhotoData = Photo.Data;

            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Cols; j++)
                {
                    int MinDistance = int.MaxValue;
                    for (int k = 0; k < numcolors; k++)
                    {
                        int TempBluePhotoData = (int) PhotoData[i, j, 0];
                        int TempGreenPhotoData = (int) PhotoData[i, j, 1];
                        int TempRedPhotoData = (int) PhotoData[i, j, 2];

                        int TempBlueVal = (int) colorvalues_BGR[k, 0];
                        int TempGreenVal = (int) colorvalues_BGR[k, 1];
                        int TempRedVal = (int) colorvalues_BGR[k, 2];

                        int BlueDist = Math.Abs(TempBlueVal - TempBluePhotoData);
                        int GreenDist = Math.Abs(TempGreenVal - TempGreenPhotoData);
                        int RedDist = Math.Abs(TempRedVal - TempRedPhotoData);
                        
                        int distance = BlueDist + GreenDist + RedDist;

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
                    RawMatrix[i, j, 0] = colorvalues_BGR[SelectedVal, 0];
                    RawMatrix[i, j, 1] = colorvalues_BGR[SelectedVal, 1];
                    RawMatrix[i, j, 2] = colorvalues_BGR[SelectedVal, 2];
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
        float[,] colorvalues_HSV; // value of colors [HSV]
        byte[,] colorvalues_BGR; // value of colors [BGR]
    }

}
