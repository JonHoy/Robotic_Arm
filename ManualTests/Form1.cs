using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Robot_Arm.Video;
using Emgu.CV;
using Emgu.CV.GPU;
using DirectShow;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;
using AForge.Video.DirectShow;

namespace ManualTests
{
    public partial class Form1 : Form
    {
        Webcam Cam1, Cam2;
        StereoSystem StereoRig;
        GpuHOGDescriptor ObjectDetector;
        int count = 0;

        public Form1()
        {
            InitializeComponent();
            Cam2 = new Webcam(0, "1280 x 720");
            Cam1 = new Webcam(1, "1280 x 720");

            Cam2.setCameraProperty(CameraControlProperty.Focus, 0, CameraControlFlags.Manual);
            Cam1.setCameraProperty(CameraControlProperty.Focus, 0, CameraControlFlags.Manual);

            StereoRig = new StereoSystem("Calib_Results_stereo.mat","Calib_Results_stereo_rectified.mat");

            Cam1.Start();
            Cam2.Start();

            timer1.Interval = 100;

            timer1.Tick += timer1_Tick;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var Image1 = Cam1.GrabFrame();
                var Image2 = Cam2.GrabFrame();
                var Image3 = StereoRig.ComputeDisparityBitmap(Image1, Image2);
                count++;
                if (count == 50)
                    Image3.Save("Depth.jpg");
                pictureBox1.Image = Image1;
                pictureBox2.Image = Image2;
                pictureBox3.Image = Image3;
                Refresh();
            }
            catch (Exception)
            {
            
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
