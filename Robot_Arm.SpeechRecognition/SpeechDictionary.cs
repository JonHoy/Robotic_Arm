using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Robot_Arm.SpeechRecognition
{
    public class SpeechDictionary
    {
        public SpeechDictionary()
        {

            myColors = new Colors();
            Actions = new Dictionary<string, byte>();
            Objects = new Dictionary<string, byte>();
            
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

        public Colors myColors;
        public Dictionary<string, byte> Actions;
        public Dictionary<string, byte> Objects;

        public List<string[]> GetPhraseList()
        {
            List<string[]> PhraseList = new List<string[]>();
            PhraseList.Add(GetPhrases(Actions));
            PhraseList.Add(new string[] { "the", "a" });
            PhraseList.Add(GetPhrases(myColors));
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
        public string[] GetColorStrings(string KeyString)
        {
            Color[] myColorList = null;
            myColors.TryGetValue(KeyString, out myColorList);
            string[] OutStrings = new string[myColorList.Length];
            for (int i = 0; i < OutStrings.Length; i++)
            {
                OutStrings[i] = myColorList[i].ToKnownColor().ToString();
            }
            return OutStrings;
        }

    }
}
