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

namespace Robot_Arm.Tests
{
    class Program
    {
        private static Image<Emgu.CV.Structure.Bgr, byte> Photo;
        private static Image<Emgu.CV.Structure.Bgr, byte> NewPhoto;
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
                ImageViewer viewer2 = new ImageViewer();
                //viewer.Visible = true; //show the image viewer
                //viewer2.Visible = true;
                Capture capture = new Capture(0); //create a camera captue
                Thread.Sleep(1000);
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, (double) 1280);
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, (double) 720);
                Thread.Sleep(1000);
                Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
                {  //run this until application closed (close button click on image viewer)
                    Photo = capture.QueryFrame(); //draw the image obtained from camera
                    viewer.Image = Photo;
                    short[,] SelectedColors = Colors.SegmentColors(Photo);
                    NewPhoto = Colors.ReColorPhoto(SelectedColors);
                    bool[,] BW = Colors.GenerateBW(ref SelectedColors, "Green");
                    BlobFinder ImageBlobs = new BlobFinder(BW);
                    viewer2.Image = NewPhoto;
                });
                viewer.ShowDialog();
                viewer2.ShowDialog();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            


        }
    }
}
