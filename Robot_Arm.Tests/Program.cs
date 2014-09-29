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
using System.Windows.Forms;
using System.Diagnostics;
using Emgu.CV.Structure;

namespace Robot_Arm.Tests
{
    class Program
    {
        private static Image<Emgu.CV.Structure.Bgr, byte> Photo;
        private static Image<Emgu.CV.Structure.Hsv, float> NewPhoto;
        static void Main(string[] args)
        {
            //Console.WriteLine("Speech Tests...");
            //Console.WriteLine("");
            //Base RobotSpeech = new Base();
            //Console.WriteLine("Start Talking...");
            //RecognitionResult Result = RobotSpeech.GetPhrase();
            //Int16[] Data = Base.GetAudioData(Result);
            //Chart TestChart = new Chart();
            ////TestChart.
            //string Phrase = Result.Text;
            //string[] ParsedString = RobotSpeech.DecodePhrase(Phrase);
            //for (int iPhrase = 0; iPhrase < ParsedString.Length; iPhrase++)
            //{
            //    Console.Write("SubPhrase #{0}   ", iPhrase + 1);
            //    Console.WriteLine(ParsedString[iPhrase]);
            //    RobotSpeech.Speak(ParsedString[iPhrase]);
            //} 

            Console.WriteLine("Video Tests... ");

            ColorClassification Colors = new ColorClassification();
            try
            {
                ImageViewer viewer = new ImageViewer(); //create an image viewer
                //PictureBox viewer2 = new PictureBox();

                
                //viewer.Visible = true; //show the image viewer
                //viewer2.Visible = true;
                Capture capture = new Capture(); //create a camera captue
                Thread.Sleep(1000);

                //System.Drawing.Bitmap;
                Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
                {  //run this until application closed (close button click on image viewer)
                    Stopwatch Timer = new Stopwatch();
                    Photo = capture.QueryFrame(); //draw the image obtained from camera

                    Timer.Start();
                    Image<Hsv, float> Photo_HSV = Photo.Convert<Hsv, float>();
                    short[,] SelectedColors = Colors.SegmentColors(Photo_HSV);
                    bool[,] BW = Colors.GenerateBW(ref SelectedColors, "White");
                    BlobFinder ImageBlobs = new BlobFinder(BW);
                    Timer.Stop();
                    ImageBlobs.DrawBlobOutline(Photo.Bitmap);
                    viewer.Image = Photo;
                    Console.WriteLine("Time to Process Frame {0} ms, Blobs Found: {1}", Timer.ElapsedMilliseconds, ImageBlobs.Blobs.Length);
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
