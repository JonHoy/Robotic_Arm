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
            var Y = new double[10000000];
            var X = new double[10000000,3];
            Random rnd = new Random();
            for (int i = 0; i < Y.Length; i++)
            {
                Y[i] = rnd.NextDouble();
                X[i, 0] = rnd.NextDouble();
                X[i, 1] = rnd.NextDouble();
                X[i, 2] = rnd.NextDouble();
            }
            var N = new int[]{10, 10, 10};
            var R = new double[,]{{0, 1}, {0, 1}, {0, 1}};
            
            
            Stopwatch Timer = new Stopwatch();

            Console.WriteLine("Video Tests... ");

            SpeechDictionary myWords = new SpeechDictionary();

            while (true)
            {
                Timer.Start();
                Statistics DataStats = Robot_Arm.Video.GPU.ND_Correlate(ref Y, ref X, N, R);
                Timer.Stop();
                Console.WriteLine("Time to Process Frame {0} ms", Timer.ElapsedMilliseconds);
                Timer.Reset();
            }
            


        }
    }
}
