using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.VideoSurveillance;
using System.Threading;
using Robot_Arm.SpeechRecognition;
using Robot_Arm.Video;
using Robot_Arm.Navigation;
using System.Windows.Forms;
using System.Diagnostics;
using Emgu.CV.Structure;

namespace Robot_Arm.Tests
{
    class Program
    {
        private static Image<Emgu.CV.Structure.Bgr, byte> Photo;
        static void Main(string[] args)
        {
            var Y = new double[1000000];
            var X = new double[1000000,1];
            Random rnd = new Random();
            for (int i = 0; i < Y.Length; i++)
            {
                Y[i] = rnd.NextDouble();
                X[i, 0] = rnd.NextDouble();
            }
            var N = new int[]{10000};
            var R = new double[,]{{0 , 1}};
            
            
            Stopwatch Timer = new Stopwatch();

            Console.WriteLine("Video Tests... ");

            SpeechDictionary myWords = new SpeechDictionary();
            try
            {
                ImageViewer viewer = new ImageViewer(); //create an image viewer
                //Capture capture = new Capture(); //create a camera capture
                //capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);
                //capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 720);
                
                //Thread.Sleep(2000);
                Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
                {  //run this until application closed (close button click on image viewer)

                    //Photo = capture.QueryFrame(); //draw the image obtained from camera
                    Timer.Start();
                    Statistics DataStats = Robot_Arm.Video.GPU.ND_Correlate(ref Y, ref X, N, R);
                    //string[] MatchStrings = myWords.GetColorStrings("Red");
                    //var NewPhoto = Photo.Clone();
                    //Rectangle myRect = Robot_Arm.Video.ColorObjectRecognizer.GetRegion(MatchStrings, NewPhoto);
                    Timer.Stop();
                    //viewer.Image = NewPhoto;
                    Console.WriteLine("Time to Process Frame {0} ms", Timer.ElapsedMilliseconds);
                    Timer.Reset();
                });
                viewer.ShowDialog();
                
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            


        }
    }
}
