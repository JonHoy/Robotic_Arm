using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.GPU;
using csmatio.io;
using csmatio.types;

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
        private GpuStereoBM GPU_Block_Matcher;
        private GpuImage<Gray, Byte> DisparityMapGPU;
        private Image<Gray, Byte> DisparityMap;
        private Size ImageSize;
        private int NumDisparities = 256;
        private int BlockSize = 21;

        public StereoSystem(string CalibrationMatFile, string CalibrationMatFile_Rectified)
        {
            
            MatFileReader mfr = new MatFileReader(CalibrationMatFile);
            MatFileReader mfr2 = new MatFileReader(CalibrationMatFile_Rectified);
            double[,] A1 = MLDoubleParser("KK_left_new", mfr2);
            double[,] A2 = MLDoubleParser("KK_right_new", mfr2);
            double[,] kc1 = MLDoubleParser("kc_left", mfr);
            double[,] kc2 = MLDoubleParser("kc_right", mfr);
            double[,] fc1 = MLDoubleParser("fc_left", mfr);
            double[,] fc2 = MLDoubleParser("fc_right", mfr);
            double[,] cc1 = MLDoubleParser("cc_left", mfr);
            double[,] cc2 = MLDoubleParser("cc_right", mfr);
            double[,] R = MLDoubleParser("R", mfr);
            double[,] T = MLDoubleParser("T", mfr);
            double[,] nx = MLDoubleParser("nx", mfr);
            double[,] ny = MLDoubleParser("ny", mfr);
            int ImageWidth = (int)nx[0, 0];
            int ImageHeight = (int)ny[0, 0];

            LeftCam = new CameraParameters(A1, kc1, fc1, cc1, ImageWidth, ImageHeight);
            RightCam = new CameraParameters(A2, kc2, fc2, cc2, ImageWidth, ImageHeight);
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

            var Variables = new List<MLArray>();
            int[] Dims = {ImageHeight, ImageWidth};
            Variables.Add(MLVariableCreator("LeftMapX", LeftCam.mapx));
            Variables.Add(MLVariableCreator("RightMapX", RightCam.mapx));
            Variables.Add(MLVariableCreator("LeftMapY", LeftCam.mapy));
            Variables.Add(MLVariableCreator("RightMapY", RightCam.mapy));
            var Writer = new MatFileWriter("ImageMap.mat", Variables, false);
            DisparityMapGPU = new GpuImage<Gray, byte>(ImageHeight, ImageWidth);
            DisparityMap = new Image<Gray, byte>(ImageWidth, ImageHeight);
            GPU_Block_Matcher = new GpuStereoBM(NumDisparities, BlockSize);
        }

        private MLSingle MLVariableCreator(string Name, Matrix<float> Array) {
            MLSingle Variable = new MLSingle(Name, new int[] { Array.Rows, Array.Cols });
            Variable.RealByteBuffer.Put(Array.Transpose().Bytes);
            return Variable;
        }

        private double[,] MLDoubleParser(string VariableName, MatFileReader mfr) {
            MLDouble MLVariable = (mfr.Content[VariableName] as MLDouble);
            return MLDoubleParser(MLVariable);
        }

        private double[,] MLDoubleParser(MLDouble Variable) {
            var dims = Variable.Dimensions;
            var CS_Variable = new double[dims[0], dims[1]];
            if (dims.Length > 2)
            {
                throw new Exception("Variable being Parsed must be one or two dimensional");
            }
            int idx = 0;
            for (int j = 0; j < dims[1]; j++)
            {
                for (int i = 0; i < dims[0]; i++)
                {
                    CS_Variable[i, j] = Variable.Get(idx);
                    idx++;
                }
            }
            return CS_Variable;
        }

        public void RectifyImagePair(Bitmap InputImage1, Bitmap InputImage2) {
            if (!InputImage1.Size.Equals(InputImage2.Size))
                throw new Exception("Input images must be the same size");
            if (!InputImage1.Size.Equals(ImageSize))
                throw new Exception("Image Resolution must be " + ImageSize.Width.ToString() + "  x " + ImageSize.Height.ToString());
            
            LeftCam.RectifyImage(new Image<Gray, byte>(InputImage1));
            RightCam.RectifyImage(new Image<Gray, byte>(InputImage2));
        }

        public void ComputeDisparity(Bitmap InputImage1, Bitmap InputImage2) {
            RectifyImagePair(InputImage1, InputImage2);
            var myStream = new Stream();
            GPU_Block_Matcher.FindStereoCorrespondence(LeftCam.ImageRectifiedGPU, RightCam.ImageRectifiedGPU, DisparityMapGPU, myStream);
            DisparityMapGPU.Download(DisparityMap);
        }

        public Bitmap ComputeDisparityBitmap(Bitmap InputImage1, Bitmap InputImage2) {
            ComputeDisparity(InputImage1, InputImage2);
            return DisparityMap.Bitmap;
        }

        public void DepthMapCalculation()
        { 
            //ComputeDisparity()
            //var Points = PointCollection.ReprojectImageTo3D(DisparityMap, DisparityDepthMapMatrix);
            //var Dist = new float[DisparityMap.Rows * DisparityMap.Cols];
            //for (int i = 0; i < DisparityMap.Rows; i++)
            //{
            //    for (int j = 0; j < DisparityMap.Cols; j++)
            //    {
            //        byte blockdist = DisparityMap.Data[i, j, 0];
            //        if (blockdist > 0)
            //            Dist[i * DisparityMap.Cols + j] = 1.0f / ((float)blockdist);
            //        else
            //            Dist[i * DisparityMap.Cols + j] = 0;
            //    }
            //}
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

