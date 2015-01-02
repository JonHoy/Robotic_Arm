using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot_Arm_GPU;
using Robot_Arm.Video;
using Emgu.CV;
using System.Drawing;

namespace ManualTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var CameraMatrix1 = new double[,]{ { 600.7, 0, 335.6 }, { 0, 600.6, 242.2 }, { 0, 0, 1 } };
            var CameraMatrix2 = new double[,] { { 596.7, 0, 321.1 }, { 0, 597.7, 257.0 }, { 0, 0, 1 } };
            var kc1 = new double[,] {{ 0.033}, {-0.0668}, {0.0025}, {0.002 }};
            var kc2 = new double[,] {{ 0.0347}, {-0.0765},{ 0.0036}, {0.0027}};
            var R = new double[,] {{0.9927, 0.1131, 0.0414}, {-0.1128, 0.9936, -0.0087}, {-0.0421, 0.0040, 0.9991}};
            var T = new double[,] { { -37.1726}, {3.0089}, {-0.2677 } };
            int ImageWidth = 640;
            int ImageHeight = 480;
            try
            {
                StereoSystem StereoRig = new StereoSystem(
                CameraMatrix1,
                CameraMatrix2,
                kc1,
                kc2,
                ImageWidth,
                ImageHeight,
                R,
                T);

                var Cam2 = new Capture(1);
                var Cam1 = new Capture(2);
                Cam1.Start();
                Cam2.Start();

                Bitmap Im1Rect;
                Bitmap Im2Rect;
                Image<Emgu.CV.Structure.Bgr, Byte> Image1;
                Image<Emgu.CV.Structure.Bgr, Byte> Image2;

                var Form = new Form1();
                Form.Visible = true;

                while (true)
                {
                    Image1 = Cam1.QueryFrame();
                    Image2 = Cam2.QueryFrame();
                    StereoRig.RectifyImagePair(Image1.ToBitmap(), Image2.ToBitmap(), out Im1Rect, out Im2Rect);
                    Form.pictureBox1.Image = Im1Rect;
                    Form.pictureBox2.Image = Im2Rect;
                    Form.Refresh();
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
