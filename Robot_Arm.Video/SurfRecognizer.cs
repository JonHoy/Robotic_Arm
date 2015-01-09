using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.GPU;

namespace Robot_Arm.Video
{
    public class SurfRecognizer
    {
        Image<Gray, Byte> modelImage;
        HomographyMatrix homography;
        VectorOfKeyPoint modelKeyPoints;
        Matrix<float> modelDescriptors;
        VectorOfKeyPoint observedKeyPoints;
        // solver parameters
        double HessianThreshold = 500;
        double uniquenessThreshold = 0.8;
        float FeaturesRatio = 0.01f;
        bool extendedflag = false;
        int k = 2;
        int requiredNonZeroCount = 4;
        double scaleIncrement = 1.5;
        int RotationBins = 20;
        double ransacReprojThreshold = 2;

        Matrix<int> indices;
        Matrix<byte> mask;
        SURFDetector surfCPU;

        public SurfRecognizer(Image<Gray, Byte> modelImage) 
        {
            surfCPU = new SURFDetector(HessianThreshold, extendedflag);
            this.modelImage = modelImage;
            modelKeyPoints = new VectorOfKeyPoint();
            Matrix<float> modelDescriptors = surfCPU.DetectAndCompute(modelImage, null, modelKeyPoints); // extract information from the model image
        }
        public bool Recognize(Image<Gray, Byte> observedImage, out PointF[] Region)
        {
            // extract features from the observed image
            observedKeyPoints = new VectorOfKeyPoint();
            Matrix<float> observedDescriptors = surfCPU.DetectAndCompute(observedImage, null, observedKeyPoints);
            BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2);
            matcher.Add(modelDescriptors);
            indices = new Matrix<int>(observedDescriptors.Rows, k);
            using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
            {
                matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
                mask = new Matrix<byte>(dist.Rows, 1);
                mask.SetValue(255);
                Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
            }
            int nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount >= requiredNonZeroCount)
            {
                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, scaleIncrement, RotationBins);
                if (nonZeroCount >= requiredNonZeroCount)
                    homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, ransacReprojThreshold);
            }
            bool ObjectFound;
            if (homography != null)
            {  //draw a rectangle along the projected model
                Rectangle rect = modelImage.ROI;
                Region = new PointF[] { 
                new PointF(rect.Left, rect.Bottom),
                new PointF(rect.Right, rect.Bottom),
                new PointF(rect.Right, rect.Top),
                new PointF(rect.Left, rect.Top)};
                homography.ProjectPoints(Region);
                ObjectFound = true;
            }
            else
            {
                Region = null;
                ObjectFound = false;
            }
            return ObjectFound;
        }

    }
}
