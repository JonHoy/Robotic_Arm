using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Robot_Arm.Video
{
    public class ColorObjectRecognizer
    {
        public BlobFinder FoundBlobs;
        public Image<Bgr, Byte> Photo;
        public string[] TargetColors;

        public ColorObjectRecognizer(string[] ColorsToLookFor, Image<Bgr, Byte> Frame)
        {
            ColorClassification ColorClassifier = new ColorClassification();
            
            this.Photo = Frame;
            this.TargetColors = ColorsToLookFor;

            // Perform Edge Detection
            Image<Gray, byte> grayFrame = Frame.Convert<Gray, byte>();
            
            Image<Gray, Byte> smallGrayFrame = grayFrame.PyrDown();
            Image<Gray, Byte> smoothedGrayFrame = smallGrayFrame.PyrUp();
            Image<Gray, Byte> cannyFrame = smoothedGrayFrame.Canny(100, 60);
            cannyFrame._Dilate(2); // use canny edge detection to determine object outlines
            BlobFinder Blobs_FromEdges = new BlobFinder(cannyFrame);
            bool[,] BW_2 = Blobs_FromEdges.BW;

            // Perform Color Detection
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
            bool[,] BW_Composite = BlobFinder.AND(ref BW_1, ref  BW_2);
            BlobFinder ImageBlobs = new BlobFinder(BW_Composite);
            bool[,] BW_Filled = ImageBlobs.FillBlobBoundingBox();
            BlobFinder ImageBlobsFilled = new BlobFinder(BW_Filled);
            ImageBlobsFilled.RemoveSmallBlobs(500);
            this.FoundBlobs = ImageBlobsFilled;
            ImageBlobsFilled.DrawBlobOutline(this.Photo.Bitmap);
        }


    }
}
