using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot_Arm.SpeechRecognition;
using System.Speech.Recognition;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Robot_Arm.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Speech Tests...");
            Console.WriteLine("");
            Base RobotSpeech = new Base();
            Console.WriteLine("Start Talking...");
            RecognitionResult Result = RobotSpeech.GetPhrase();
            Int16[] Data = Base.GetAudioData(Result);
            Chart TestChart = new Chart();
            //TestChart.
            string Phrase = Result.Text;
            string[] ParsedString = RobotSpeech.DecodePhrase(Phrase);
            for (int iPhrase = 0; iPhrase < ParsedString.Length; iPhrase++)
            {
                Console.Write("SubPhrase #{0}", iPhrase + 1);
                Console.WriteLine(ParsedString[iPhrase]);
                RobotSpeech.Speak(ParsedString[iPhrase]);
            }

            Console.WriteLine("Video Tests... ");
        }
    }
}
