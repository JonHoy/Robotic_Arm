using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Robot_Arm.SpeechRecognition;

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
                Color.Gold,
                Color.Beige,
                Color.DarkSlateBlue,
                Color.Khaki,
                Color.Silver,
                Color.Gainsboro,
                Color.SlateGray,
                Color.Navy,
                Color.SteelBlue,
                Color.Tan,
                Color.Peru,
                Color.Chocolate,
                Color.SaddleBrown,
                Color.Indigo,
                Color.Lime,
                Color.SeaGreen,
                Color.LightSeaGreen,
                Color.DarkOliveGreen
            };

            colornames = new string[allColors.Length];
            colorvalues_BGR = new float[allColors.Length, 3];
            for (int iColor = 0; iColor < allColors.Length; iColor++)
            {
                colornames[iColor] = allColors[iColor].ToKnownColor().ToString();
                Color CurrentColor = Color.FromName(colornames[iColor]);
                colorvalues_BGR[iColor, 0] = (float) CurrentColor.B + 1;
                colorvalues_BGR[iColor, 1] = (float)CurrentColor.G + 1;
                colorvalues_BGR[iColor, 2] = (float) CurrentColor.R +1;
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
                
                for (int j = 0; j < Cols; j++)
                {
                    float MinDistance = float.MaxValue;
                    float TempBluePhotoData = (float)PhotoData[i, j, 0] + 1;
                    float TempGreenPhotoData = (float)PhotoData[i, j, 1] + 1;
                    float TempRedPhotoData = (float)PhotoData[i, j, 2] + 1;

                    for (int k = 0; k < numcolors; k++)
                    {
                        float BlueDist = Math.Abs(TempBluePhotoData - colorvalues_BGR[k, 0]);
                        float GreenDist = Math.Abs(TempGreenPhotoData - colorvalues_BGR[k, 1]);
                        float RedDist = Math.Abs(TempRedPhotoData - colorvalues_BGR[k, 2]);
                        float distance = BlueDist + RedDist + GreenDist;
                       
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
        float[,] colorvalues_BGR; // value of colors [BGR]       
    }

}
