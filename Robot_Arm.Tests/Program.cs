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
            Stopwatch Timer = new Stopwatch();

            // Navigation Tests

            var mySolver = new AngleCalculator();
            var Theta1 = double.NaN;
            var Theta2 = double.NaN;
            
            double x_Target = 8;
            double y_Target = -4;
            Timer.Start();
            mySolver.GetTheta(x_Target, y_Target, out Theta1, out Theta2);
            Timer.Stop();

            Console.WriteLine("Solve Time {0} sec", (double) Timer.ElapsedTicks / (double) Stopwatch.Frequency);
            Console.WriteLine("Theta 1: {0} deg", Theta1);
            Console.WriteLine("Theta 2: {0} deg", Theta2);

            Console.WriteLine("Error_X = {0}", 4.06 * Math.Cos(Theta1 * Math.PI/180.0) + 5.7 * Math.Cos((Theta2 + Theta1) * Math.PI/180.0) - x_Target);
            Console.WriteLine("Error_Y = {0}", 4.06 * Math.Sin(Theta1 * Math.PI/180.0) + 5.7 * Math.Sin((Theta2 + Theta1) * Math.PI/180.0) - y_Target);


            Console.WriteLine("Video Tests... ");

            ColorClassification Colors = new ColorClassification();
            SpeechDictionary myWords = new SpeechDictionary();
            try
            {
                ImageViewer viewer = new ImageViewer(); //create an image viewer
                Capture capture = new Capture(); //create a camera capture
                Thread.Sleep(2000);
                Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
                {  //run this until application closed (close button click on image viewer)
                    
                    Photo = capture.QueryFrame(); //draw the image obtained from camera
                    Timer.Start();
                    string[] MatchStrings = myWords.GetColorStrings("Orange");
                    ColorObjectRecognizer myColorObjectRecognizer = new ColorObjectRecognizer(MatchStrings, Photo.Convert<Bgr, Byte>());
                    //myColor
                    Timer.Stop();
                    //ImageBlobsFilled.DrawBlobOutline(Photo.Bitmap);
                    viewer.Image = myColorObjectRecognizer.Photo;
                    //Console.WriteLine("Time to Process Frame {0} ms, Blobs Found: {1}", Timer.ElapsedMilliseconds, ImageBlobsFilled.Blobs.Length);
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
