using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

namespace Robot_Arm.Video
{
    public class StereoSystem
    {
        private Matrix<double> CameraMatrix1;
        private Matrix<double> CameraMatrix2;
        private Matrix<double> CameraMatrix1New;
        private Matrix<double> CameraMatrix2New;        
        private Matrix<double> distCoeffs1;
        private Matrix<double> distCoeffs2;
        private Matrix<double> RotationMatrix;
        private Matrix<double> TranslationVector;
        private Matrix<double> RectificationTransform1;
        private Matrix<double> RectificationTransform2;
        private Matrix<double> ProjectionMatrix1;
        private Matrix<double> ProjectionMatrix2;
        private Matrix<double> DisparityDepthMapMatrix;
        private Matrix<float> map1x;
        private Matrix<float> map1y;
        private Matrix<float> map2x;
        private Matrix<float> map2y;
        private Rectangle ROI1;
        private Rectangle ROI2;

        private IntrinsicCameraParameters Camera1_Intrinsic;
        private IntrinsicCameraParameters Camera2_Intrinsic;

        private Size ImageSize;

        public StereoSystem(double[,] A1, 
            double[,] A2, 
            double[,] kc1, 
            double[,] kc2, 
            int ImageWidth, 
            int ImageHeight, 
            double[,] R, 
            double[,] T)  {
            ImageSize = new Size(ImageWidth, ImageHeight);

            CameraMatrix1 = new Matrix<double>(A1);
            CameraMatrix2 = new Matrix<double>(A2);
            distCoeffs1 = new Matrix<double>(kc1);
            distCoeffs2 = new Matrix<double>(kc2);
            TranslationVector = new Matrix<double>(T);
            RotationMatrix = new Matrix<double>(R);
            
            RectificationTransform1 = new Matrix<double>(3, 3);
            RectificationTransform2 = new Matrix<double>(3, 3);
            ProjectionMatrix1 = new Matrix<double>(3, 4);
            ProjectionMatrix2 = new Matrix<double>(3, 4);
            DisparityDepthMapMatrix = new Matrix<double>(4, 4);
            
            var ROI = new Rectangle(0, 0, ImageSize.Width, ImageSize.Height);
            ROI1 = new Rectangle();
            ROI2 = new Rectangle();
            CvInvoke.cvStereoRectify(
                CameraMatrix1.Ptr,
                CameraMatrix2.Ptr,
                distCoeffs1.Ptr,
                distCoeffs2.Ptr,
                ImageSize,
                RotationMatrix.Ptr,
                TranslationVector.Ptr,
                RectificationTransform1.Ptr,
                RectificationTransform2.Ptr,
                ProjectionMatrix1.Ptr,
                ProjectionMatrix2.Ptr,
                DisparityDepthMapMatrix.Ptr,
                Emgu.CV.CvEnum.STEREO_RECTIFY_TYPE.DEFAULT,
                1,
                Size.Empty, 
                ref ROI1, 
                ref ROI2);

            Camera1_Intrinsic = new IntrinsicCameraParameters();
            Camera2_Intrinsic = new IntrinsicCameraParameters();

            Camera1_Intrinsic.DistortionCoeffs = distCoeffs1;
            Camera2_Intrinsic.DistortionCoeffs = distCoeffs2;
            Camera1_Intrinsic.IntrinsicMatrix = CameraMatrix1;
            Camera2_Intrinsic.IntrinsicMatrix = CameraMatrix2;

            Camera1_Intrinsic.InitUndistortMap(ImageWidth, ImageHeight, out map1x, out map1y);
            Camera2_Intrinsic.InitUndistortMap(ImageWidth, ImageHeight, out map2x, out map2y);
        }

        public void RectifyImagePair(Bitmap InputImage1, Bitmap InputImage2, out Bitmap OutputImage1, out Bitmap OutputImage2) {
            if (!InputImage1.Size.Equals(InputImage2.Size))
                throw new Exception("Input images must be the same size");
            if (!InputImage1.Size.Equals(ImageSize))
                throw new Exception("Image Resolution must be " + ImageSize.Width.ToString() + "  x " + ImageSize.Height.ToString());
            
            var InputImage1_CV = new Image<Gray, float>(InputImage1);
            var InputImage2_CV = new Image<Gray, float>(InputImage2);
            var OutputImage1_CV = new Image<Gray, float>(ImageSize);
            var OutputImage2_CV = new Image<Gray, float>(ImageSize);

            var ScaleVal = new MCvScalar(0);

            CvInvoke.cvRemap(InputImage1_CV.Ptr, OutputImage1_CV.Ptr, map1x.Ptr, map1y.Ptr, 0, ScaleVal);
            CvInvoke.cvRemap(InputImage2_CV.Ptr, OutputImage2_CV.Ptr, map2x.Ptr, map2y.Ptr, 0, ScaleVal);

            OutputImage1 = OutputImage1_CV.Bitmap;
            OutputImage2 = OutputImage2_CV.Bitmap;
            return;
        }

        //public Bitmap ComputeDisparityMap(Bitmap InputImage1, Bitmap InputImage2) {
        //    Bitmap Im1Rect;
        //    Bitmap Im2Rect;
        //    RectifyImagePair(InputImage1, InputImage2, out Im1Rect, out  Im2Rect);
        //    var Im1Rect_CV = new Image<Gray, byte>(Im1Rect);
        //    var Im2Rect_CV = new Image<Gray, byte>(Im2Rect);
        //    var Disparity = new Image<Gray, Int16>(ImageSize);
        //    var BlockMatcher = new StereoGC(500, 10);
        //    BlockMatcher.State = State;
        //    BlockMatcher.FindStereoCorrespondence(

        //}

    }
}

