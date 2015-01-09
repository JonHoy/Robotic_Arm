using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot_Arm.Video;
using Emgu.CV;
using Emgu.CV.GPU;
using System.Drawing;
using DirectShow;

namespace ManualTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var CameraMatrix1 = new double[,]{ { 600.0, 0, 295.0 }, { 0, 600.0, 251.0 }, { 0, 0, 1 } };
            var CameraMatrix2 = new double[,] { { 600.0, 0, 303.5 }, { 0, 600.0, 251.0 }, { 0, 0, 1 } };
            var kc1 = new double[,] {{ 0.033}, {-0.0668}, {0.0025}, {0.002 }};
            var kc2 = new double[,] {{ 0.0347}, {-0.0765},{ 0.0036}, {0.0027}};
            var R = new double[,] {{0.9927, 0.1131, 0.0414}, {-0.1128, 0.9936, -0.0087}, {-0.0421, 0.0040, 0.9991}};
            var T = new double[,] { { -37.1726}, {3.0089}, {-0.2677 } };
            int ImageWidth = 640;
            int ImageHeight = 480;

            var Cam2 = new Webcam(0, "640 x 480");
            var Cam1 = new Webcam(1, "640 x 480");

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



                Cam1.Start();
                Cam2.Start();
                
                var Form = new Form1();
                Form.Visible = true;

                int count = 1;
                while (true)
                {
                    var Image1 = Cam1.GrabFrame();
                    var Image2 = Cam2.GrabFrame();
                    if (count == 100)
                    {
                        Form.pictureBox1.Image.Save("LeftImage.jpg");
                        Form.pictureBox2.Image.Save("RightImage.jpg");
                    }
                    var Image3 = StereoRig.ComputeDisparityMap(Image1, Image2);
                    Form.pictureBox1.Image = StereoRig.LeftCam.ImageRectifiedGPU.ToImage().Bitmap;
                    Form.pictureBox2.Image = StereoRig.RightCam.ImageRectifiedGPU.ToImage().Bitmap;
                    Form.pictureBox3.Image = Image3;
                    Form.Refresh();
                    Console.WriteLine("Count = {0}",count);
                    count++;
                }

                
            }
            catch (Exception ex)
            {
                Cam1.Stop();
                Cam2.Stop();
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
