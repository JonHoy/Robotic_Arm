using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.GPU;

using System.Drawing;

namespace Robot_Arm.Video
{
    public partial class StereoSystem
    {
        public CameraParameters LeftCam;
        public CameraParameters RightCam;
        private Matrix<double> RotationMatrix;
        private Matrix<double> TranslationVector;
        private Matrix<double> DisparityDepthMapMatrix;
        private GpuStereoConstantSpaceBP GPU_Block_Matcher;
        private StereoBM CPU_Block_Matcher;
        private GpuImage<Gray, Byte> DisparityMapGPU;
        private Image<Gray, Byte> DisparityMap;
        private Size ImageSize;

        public StereoSystem(double[,] A1, 
            double[,] A2, 
            double[,] kc1, 
            double[,] kc2, 
            int ImageWidth, 
            int ImageHeight, 
            double[,] R, 
            double[,] T)  {

            LeftCam = new CameraParameters(A1, kc1, ImageWidth, ImageHeight);
            RightCam = new CameraParameters(A2, kc2, ImageWidth, ImageHeight);
            TranslationVector = new Matrix<double>(T);
            RotationMatrix = new Matrix<double>(R);
            DisparityDepthMapMatrix = new Matrix<double>(4, 4);
            ImageSize = new Size(ImageWidth, ImageHeight);

            CvInvoke.cvStereoRectify(
                LeftCam.CameraMatrix.Ptr,
                RightCam.CameraMatrix.Ptr,
                LeftCam.distCoeffs.Ptr,
                RightCam.distCoeffs.Ptr,
                ImageSize,
                RotationMatrix.Ptr,
                TranslationVector.Ptr,
                LeftCam.RectificationTransform.Ptr,
                RightCam.RectificationTransform.Ptr,
                LeftCam.ProjectionMatrix.Ptr,
                RightCam.ProjectionMatrix.Ptr,
                DisparityDepthMapMatrix.Ptr,
                Emgu.CV.CvEnum.STEREO_RECTIFY_TYPE.DEFAULT,
                1,
                Size.Empty, 
                ref LeftCam.ROI, 
                ref RightCam.ROI);

            LeftCam.UndistortRectifyMap();
            RightCam.UndistortRectifyMap();
            DisparityMapGPU = new GpuImage<Gray, byte>(ImageHeight, ImageWidth);
            DisparityMap = new Image<Gray, byte>(ImageWidth, ImageHeight);
            GPU_Block_Matcher = new GpuStereoConstantSpaceBP(64, 4, 4, 4);
            CPU_Block_Matcher = new StereoBM(Emgu.CV.CvEnum.STEREO_BM_TYPE.BASIC, 30);
       }

        public void RectifyImagePair(Bitmap InputImage1, Bitmap InputImage2) {
            if (!InputImage1.Size.Equals(InputImage2.Size))
                throw new Exception("Input images must be the same size");
            if (!InputImage1.Size.Equals(ImageSize))
                throw new Exception("Image Resolution must be " + ImageSize.Width.ToString() + "  x " + ImageSize.Height.ToString());
            
            LeftCam.RectifyImage(new Image<Gray, byte>(InputImage1));
            RightCam.RectifyImage(new Image<Gray, byte>(InputImage2));
        }

        public Bitmap ComputeDisparityMap(Bitmap InputImage1, Bitmap InputImage2) {
            RectifyImagePair(InputImage1, InputImage2);
            var myStream = new Stream();
            //GPU_Block_Matcher.FindStereoCorrespondence(LeftCam.ImageRectifiedGPU, RightCam.ImageRectifiedGPU, DisparityMap, myStream);
            //CPU_Block_Matcher.FindStereoCorrespondence(LeftCam.ImageRectifiedGPU.ToImage(), RightCam.ImageRectifiedGPU.ToImage(), DisparityMap);
            var stateptr = CvInvoke.cvCreateStereoBMState(Emgu.CV.CvEnum.STEREO_BM_TYPE.BASIC, 10);
            CvInvoke.cvFindStereoCorrespondenceBM(LeftCam.ImageRectified.Ptr, RightCam.ImageRectified.Ptr, DisparityMap.Ptr, stateptr);
            //myStream.WaitForCompletion();
            //var Img = DisparityMapGPU.ToImage();
            return DisparityMap.Bitmap;
        }
        public void DrawROI(Bitmap Image, Rectangle ROI)
        {
            using (var g = Graphics.FromImage(Image))
            {
                SolidBrush Brush = new SolidBrush(Color.Red);
                g.FillRectangle(Brush, ROI);
            }
        }
    }
}

