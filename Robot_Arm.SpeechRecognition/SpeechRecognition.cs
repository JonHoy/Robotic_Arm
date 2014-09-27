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
        private SpeechDictionary mySpeechDictionary;
        private GrammarBuilder builder;
    }
    public class SpeechDictionary
    {
        public SpeechDictionary()
        { 
            // Now define each color with an  rgb value (This allows the translation of colors to physical numbers the computer understands
            Colors = new Dictionary<string, byte[]>();
            Actions = new Dictionary<string,byte>();
            Objects = new Dictionary<string, byte>();
            Colors.Add("Red", new byte[]{237, 28, 36});
            Colors.Add("Green", new byte[] { 34, 177, 36 });
            Colors.Add("Blue", new byte[] { 232, 28, 28 });
            Colors.Add("Black", new byte[] { 25, 25, 25 });
            Colors.Add("White", new byte[] { 220, 220, 220 });
            Colors.Add("Yellow", new byte[] { 255, 242, 36 });
            Colors.Add("Purple", new byte[] { 163, 73, 164 });
            Colors.Add("Orange", new byte[] { 255, 127, 39 });
            Colors.Add("Brown", new byte[] { 185, 122, 87 });
            Colors.Add("Gray", new byte[] { 127, 127, 127 });
            Colors.Add("Pink", new byte[] { 255, 174, 201 });
            // Now define Action phrases which correspond to command codes
            Actions.Add("Get", 1);
            Actions.Add("Fetch", 1);
            Actions.Add("Retreive", 1);
            Actions.Add("Pick up", 1);
            Actions.Add("Grab", 1);
            Actions.Add("Drop", 2);
            Actions.Add("Release", 2);
            Actions.Add("Let go of", 2);
            // Now Define phrases which are suitable for objects and have an object code 
            Objects.Add("Thing", 0);
            Objects.Add("Ball", 0);
            Objects.Add("Block", 0);
            Objects.Add("Cube", 0);
            Objects.Add("Object", 0);
            Objects.Add("Item", 0);

        }

        public Dictionary<string, byte[]> Colors;
        public Dictionary<string, byte> Actions;
        public Dictionary<string, byte> Objects;

        public List<string[]> GetPhraseList()
        {
            List<string[]> PhraseList = new List<string[]>();
            PhraseList.Add(GetPhrases(Actions));
            PhraseList.Add(new string[]{"the", "a"});
            PhraseList.Add(GetPhrases(Colors));
            PhraseList.Add(GetPhrases(Objects));
            return PhraseList;
        }

        private string[] GetPhrases<T>(Dictionary<string, T> Worddictionary)
        {
            string[] Key_Strings = new string[Worddictionary.Count];
            Dictionary<string, T>.KeyCollection KeyCol = Worddictionary.Keys;
            for (int iKey = 0; iKey < Key_Strings.Length; iKey++)
            {
                Key_Strings[iKey] = KeyCol.ElementAt(iKey);
            }
            return Key_Strings;
        }
        private T[] GetValues<T>(Dictionary<string, T> Worddictionary)
        {
            T[] ValueArray = new T[Worddictionary.Count];
            Dictionary<string, T>.ValueCollection ValCol = Worddictionary.Values;
            for (int iKey = 0; iKey < ValueArray.Length; iKey++)
            {
                ValueArray[iKey] = ValCol.ElementAt(iKey);
            }
            return ValueArray;
        }


    }
}
