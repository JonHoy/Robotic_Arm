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


        public static Rectangle GetRegion(string[] ColorsToLookFor, Image<Bgr, Byte> Frame)
        {
            ColorClassification ColorClassifier = new ColorClassification();

            // Perform Edge Detection
            Image<Gray, byte> grayFrame = Frame.Convert<Gray, byte>();

            Task<bool[,]> detectEdges = new Task<bool[,]>(() =>
            {
                grayFrame._SmoothGaussian(3); // Smooth the frame
                Image<Gray, Byte> smallGrayFrame = grayFrame.PyrDown();
                Image<Gray, Byte> smoothedGrayFrame = smallGrayFrame.PyrUp();
                Image<Gray, Byte> cannyFrame = smoothedGrayFrame.Canny(100, 60);
                cannyFrame._Dilate(2); // use canny edge detection to determine object outlines
                BlobFinder Blobs_FromEdges = new BlobFinder(cannyFrame);
                bool[,] BW_2 = Blobs_FromEdges.BW;
                return BW_2;
            });

            detectEdges.Start();
            Frame._SmoothGaussian(5);
            short[,] SelectedColors = ColorClassifier.SegmentColors(Frame);
            bool[,] BW_FromColor = ColorClassifier.GenerateBW(ref SelectedColors, ColorsToLookFor);
            Image<Gray, byte> BW_GrayImg = BlobFinder.Gray_Converter(ref BW_FromColor);
            BW_GrayImg._Dilate(2);
            BW_FromColor = BlobFinder.BW_Converter(BW_GrayImg);
            BlobFinder Blobs_FromColor = new BlobFinder(BW_FromColor);
            bool[,] BW_1 = Blobs_FromColor.BW;


            // Combine objects found via color recognition and edge detection
            // If an object is found with edge and color keep it, otherwise discard it
            bool[,] EdgeBW = detectEdges.Result;
            bool[,] BW_Composite = BlobFinder.AND(ref BW_1, ref EdgeBW);
            BlobFinder ImageBlobs = new BlobFinder(BW_Composite);
            ImageBlobs.RemoveSmallBlobs(100);
            bool[,] BW_Filled = ImageBlobs.FillBlobBoundingBox();
            BlobFinder ImageBlobsFilled = new BlobFinder(BW_Filled);
            Blob bestBlob = ImageBlobsFilled.PickBestBlob();
            //ImageBlobsFilled.DrawBlobOutline(Frame.Bitmap);
            if (bestBlob == null)
            {
                return new Rectangle();
            }
            else
            {
                return new Rectangle(
                    bestBlob.Xmin,
                    bestBlob.Ymin,
                    bestBlob.Xmax - bestBlob.Xmin + 1,
                    bestBlob.Ymax - bestBlob.Ymin + 1);
            }
        }



    }
}
