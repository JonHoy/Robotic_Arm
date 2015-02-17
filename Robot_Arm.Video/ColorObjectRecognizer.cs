using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace Robot_Arm.Video
{
    public class ColorObjectRecognizer
    {
        public static Image<Bgr, Byte> ReColorPhoto(string[] ColorsToLookFor, Image<Bgr, Byte> Frame)
        {
            var ColorClassifier = new ColorClassification();
            var SelectedColors = ColorClassifier.SegmentColors(Frame.Convert<Rgb, float>().Data);
            return ColorClassifier.ReColorPhoto(ref SelectedColors);
        }
        public static Rectangle GetRegion(string[] ColorsToLookFor, Image<Bgr, Byte> Frame)
        {
            ColorClassification ColorClassifier = new ColorClassification();

            // Perform Edge Detection
            Image<Gray, byte> grayFrame = Frame.Convert<Gray, byte>();

            Task<bool[,]> detectEdges = new Task<bool[,]>(() =>
            {
                Image<Gray, Byte> cannyFrame = grayFrame.Canny(100, 60);
                cannyFrame._Dilate(3); // use canny edge detection to determine object outlines
                bool[,] BW_2 = BlobFinder.BW_Converter(cannyFrame); 
                return BW_2;
            });

            detectEdges.Start();
            Frame.SmoothGaussian(25);
            var ColorFrame = Frame.Convert<Rgb, float>();
            int[,] SelectedColors = ColorClassifier.SegmentColors(ColorFrame.Data);
            bool[,] BW_FromColor = ColorClassifier.GenerateBW(ref SelectedColors, ColorsToLookFor);
            Image<Gray, byte> BW_GrayImg = BlobFinder.Gray_Converter(ref BW_FromColor);
            BW_GrayImg._Dilate(3);
            BW_FromColor = BlobFinder.BW_Converter(BW_GrayImg);

            // Combine objects found via color recognition and edge detection
            // If an object is found with edge and color keep it, otherwise discard it
            bool[,] EdgeBW = detectEdges.Result;
            bool[,] BW_Composite = BlobFinder.AND(ref BW_FromColor, ref EdgeBW);

            BlobFinder ImageBlobs = new BlobFinder(BW_Composite);
            ImageBlobs.RemoveSmallBlobs(3000);
            Blob bestBlob = ImageBlobs.PickBestBlob();
            Rectangle myRect = new Rectangle();
            if (bestBlob != null)
            {
                myRect = new Rectangle(
                    bestBlob.Xmin,
                    bestBlob.Ymin,
                    bestBlob.Xmax - bestBlob.Xmin + 1,
                    bestBlob.Ymax - bestBlob.Ymin + 1);
            }
            BlobFinder.DrawBlobOutline(Frame.Bitmap, myRect);
            return myRect;
        }



    }
}
