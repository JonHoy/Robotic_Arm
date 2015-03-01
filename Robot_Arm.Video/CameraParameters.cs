using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;

namespace Robot_Arm.Video
{
    public class CameraParameters
    {
        public Matrix<double> CameraMatrix;
        public Matrix<double> distCoeffs;
        public Matrix<double> RectificationTransform;
        public Matrix<double> ProjectionMatrix;
        public Matrix<double> Fc;
        public Matrix<double> Cc;
        public Matrix<float> mapx;
        public Matrix<float> mapy;
        public GpuMat<float> mapxGPU;
        public GpuMat<float> mapyGPU;
        public GpuImage<Gray, byte> ImageRawGPU;
        public GpuImage<Gray, byte> ImageRectifiedGPU;
        public Image<Gray, byte> ImageRaw;
        public Image<Gray, byte> ImageRectified;
        private int ImageWidth;
        private int ImageHeight;
        public System.Drawing.Rectangle ROI;
        
        public CameraParameters(double[,] A, double[,] kc, double[,] fc, double[,] cc, int Width, int Height) {
            Fc = new Matrix<double>(fc);
            Cc = new Matrix<double>(cc);
            CameraMatrix = new Matrix<double>(A);
            distCoeffs = new Matrix<double>(4, 1);
            for (int i = 0; i < distCoeffs.Rows; i++)
            {
                distCoeffs[i, 0] = kc[i, 0];
            }
            RectificationTransform = new Matrix<double>(3, 3);
            ProjectionMatrix = new Matrix<double>(3, 4);
            ImageWidth = Width;
            ImageHeight = Height;
            ImageRawGPU = new GpuImage<Gray, byte>(ImageHeight, ImageWidth);
            ImageRectifiedGPU = new GpuImage<Gray, byte>(ImageHeight, ImageWidth);
            ImageRectified = new Image<Gray, byte>(ImageWidth, ImageHeight);
        }
        public void UndistortRectifyMap() { 
            StereoSystem.UndistortRectifyMap(ImageHeight,
                ImageWidth,
                RectificationTransform.Data,
                CameraMatrix.Data,
                Fc.Data,
                Cc.Data,
                distCoeffs.Data,
                out mapx,
                out mapy);
            mapxGPU = new GpuMat<float>(mapx);
            mapyGPU = new GpuMat<float>(mapy);

        }
        public void RectifyImage(Image<Gray, byte> Raw) { 
            ImageRawGPU.Upload(Raw);
            GpuInvoke.Remap(ImageRawGPU.Ptr,
                ImageRectifiedGPU.Ptr,
                mapxGPU.Ptr,
                mapyGPU.Ptr,
                Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR,
                Emgu.CV.CvEnum.BORDER_TYPE.CONSTANT,
                new MCvScalar(0),
                IntPtr.Zero);
            ImageRectifiedGPU.Download(ImageRectified);

        }
        
    }
}
