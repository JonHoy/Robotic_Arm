using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace Robot_Arm.SpeechRecognition
{
    public class Base
    {
        public Base()
        {
            mySpeechEngine = new SpeechRecognitionEngine();
            mySpeechDictionary = new SpeechDictionary();
            mySpeaker = new SpeechSynthesizer();
            mySpeechEngine.UnloadAllGrammars();
            GrammarBuilder builder = new GrammarBuilder();
            List<string[]> WordList = mySpeechDictionary.GetPhraseList();
            for (int i = 0; i < WordList.Count; i++)
            {
                builder.Append(new Choices(WordList[i]));
            }
            myGrammar = new Grammar(builder);
            mySpeechEngine.LoadGrammar(myGrammar);
            mySpeechEngine.SetInputToDefaultAudioDevice();
        }
        // Main method used to turn speech into a string
        public RecognitionResult GetPhrase()
        {
            RecognitionResult Result = mySpeechEngine.Recognize();
            return Result;
        }

        public static Int16[] GetAudioData(RecognitionResult Result)
        {
            MemoryStream Stream = new MemoryStream();
            Result.Audio.WriteToAudioStream(Stream);
            Byte[] ByteData = Stream.ToArray();
            Int16[] AudioData = new Int16[ByteData.Length/2];
            Buffer.BlockCopy(ByteData, 0, AudioData, 0, ByteData.Length);
            return AudioData;
        }

        public void Speak(string SpeechString)
        {
            mySpeaker.Speak(SpeechString);
        }

        // take a large phrase and decompose it into a string array of sub phrases
        // ie Phrase = "Pick up the Red Ball"
        // PhraseList = {{"Get", "Find", "Pick up", "Grab"} , {"the", "a"} , {"Green", "Blue" , "Red"} , {"Ball", "Block", "Cube"}}
        // DecodedPhrase = {"Pick up", "the", "Red" , "Ball"}

        public string[] DecodePhrase(RecognitionResult Result)
        {
            return DecodePhrase(Result.Text);
        }

        public string[] DecodePhrase(string Phrase)
        {
            List<string[]> PhraseList = mySpeechDictionary.GetPhraseList();
            string[] DecodedPhrase = new string[4];
            string CompareString = "";
            for (int i = 0; i < DecodedPhrase.Length; i++)
            {
                string[] CurrentChoices = PhraseList[i];
                for (int j = 0; j < CurrentChoices.Length; j++)
                {
                    string TempCompareString = CompareString + CurrentChoices[j];
                    if (Phrase.Contains(TempCompareString))
                    {
                        CompareString = TempCompareString + " ";
                        DecodedPhrase[i] = CurrentChoices[j];
                        break;
                    }
                }
            }
            return DecodedPhrase;
        }
        public SpeechSynthesizer mySpeaker;
        private Grammar myGrammar;
        private SpeechRecognitionEngine mySpeechEngine;
        public SpeechDictionary mySpeechDictionary;
    }
    
}
