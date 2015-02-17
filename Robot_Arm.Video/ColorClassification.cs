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
            
            var myColors = new Colors();
            var ColorInfo = myColors.getAllColors();
            var allColors = ColorInfo.ColorValues;
            ParentColorIdx = ColorInfo.MasterIndices;
            colornames = new string[allColors.Length];
            colorvalues_RGB = new float[allColors.Length, 3];
            for (int iColor = 0; iColor < allColors.Length; iColor++)
            {
                colornames[iColor] = allColors[iColor].ToKnownColor().ToString();
                Color CurrentColor = Color.FromName(colornames[iColor]);
                colorvalues_RGB[iColor, 0] = (float) CurrentColor.R;
                colorvalues_RGB[iColor, 1] = (float) CurrentColor.G;
                colorvalues_RGB[iColor, 2] = (float)  CurrentColor.B;
            }
        }

        public int[,] SegmentColors(float[,,] Image)
        {
            return Robot_Arm_GPU.GPU.SegmentColors(Image, colorvalues_RGB, ParentColorIdx);
        }

        public Image<Bgr, byte> ReColorPhoto(ref int[,] SelectedColor)
        {
            int Rows = SelectedColor.GetLength(0);
            int Cols = SelectedColor.GetLength(1);

            byte[, ,] RawMatrix = new byte[Rows, Cols, 3];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    int SelectedVal = SelectedColor[i, j];
                    RawMatrix[i, j, 0] = (byte) colorvalues_RGB[SelectedVal, 2];
                    RawMatrix[i, j, 1] = (byte) colorvalues_RGB[SelectedVal, 1];
                    RawMatrix[i, j, 2] = (byte) colorvalues_RGB[SelectedVal, 0];
                }
            }
            Image<Bgr, byte> NewPhoto = new Image<Bgr, byte>(RawMatrix);
            return NewPhoto;
        }

        public bool[,] GenerateBW(ref int[,] SegmentedImage, string[] ColorName)
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
        float[,] colorvalues_RGB; // value of colors [BGR]     
        int[] ParentColorIdx; // index of master color in the dictionary
    }

}
