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
            SpeechDictionary myWords = new SpeechDictionary();
            try
            {
                ImageViewer viewer = new ImageViewer(); //create an image viewer
                Capture capture = new Capture(); //create a camera capture
                Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
                {  //run this until application closed (close button click on image viewer)
                    Stopwatch Timer = new Stopwatch();
                    Photo = capture.QueryFrame(); //draw the image obtained from camera
                    Timer.Start();
                    //Image<Bgr, byte> NewPhoto = Photo.Convert<Bgr, byte>();

                    //Image<Gray, byte> grayFrame = Photo.Convert<Gray, byte>();
                    //Image<Gray, Byte> smallGrayFrame = grayFrame.PyrDown();
                    //Image<Gray, Byte> smoothedGrayFrame = smallGrayFrame.PyrUp();
                    //Image<Gray, Byte> cannyFrame = smoothedGrayFrame.Canny(100, 60);
                    //cannyFrame._Dilate(3);

                    //short[,] SelectedColors = Colors.SegmentColors(NewPhoto);
                    //NewPhoto = Colors.ReColorPhoto(ref SelectedColors);
                    //string[] MatchStrings = myWords.GetColorStrings("Orange");
                    //bool[,] BW_FromColor = Colors.GenerateBW(ref SelectedColors, MatchStrings);

                    //Image<Gray, byte> BW_GrayImg = BlobFinder.Gray_Converter(ref BW_FromColor);
                    //BW_GrayImg._Dilate(5);
                    //BW_FromColor = BlobFinder.BW_Converter(BW_GrayImg);
                    //BlobFinder Blobs_FromColor = new BlobFinder(BW_FromColor);
                    //BlobFinder Blobs_FromEdges = new BlobFinder(cannyFrame);
                    //bool[,] BW_1 = Blobs_FromColor.FillBlobBoundingBox();
                    //bool[,] BW_2 = Blobs_FromEdges.FillBlobBoundingBox();


                    //bool[,] BW_Composite = BlobFinder.AND(ref BW_1, ref  BW_2);
                    //BlobFinder ImageBlobs = new BlobFinder(BW_Composite);
                    //bool[,] BW_Filled = ImageBlobs.FillBlobBoundingBox();
                    //BlobFinder ImageBlobsFilled = new BlobFinder(BW_Filled);
                    //ImageBlobsFilled.RemoveSmallBlobs(200);

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
