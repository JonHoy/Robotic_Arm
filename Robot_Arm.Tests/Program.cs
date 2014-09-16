using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot_Arm.SpeechRecognition;

namespace Robot_Arm.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Base RobotSpeech = new Base();
            Console.WriteLine("Start Talking...");
            string Phrase = RobotSpeech.GetPhrase();
            string[] ParsedString = RobotSpeech.DecodePhrase(Phrase);
            for (int iPhrase = 0; iPhrase < ParsedString.Length; iPhrase++)
            {
                Console.WriteLine(ParsedString[iPhrase]);
                RobotSpeech.Speak(ParsedString[iPhrase]);
            }
        }
    }
}
