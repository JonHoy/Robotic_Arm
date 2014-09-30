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
            // Now define each color with a string array of other colors that can be associated with the key 
            //"Red" for example can be {"Red", "DarkRed", "Crimson"}, "OrangeRed", etc 
            //
            Colors = new Dictionary<string, Color[]>();
            Actions = new Dictionary<string, byte>();
            Objects = new Dictionary<string, byte>();
            Colors.Add("Red", new Color[] {Color.Red, Color.DarkRed, Color.Maroon, Color.Crimson});
            Colors.Add("Green", new Color[] {Color.Green, Color.LightGreen, Color.DarkGreen, Color.LimeGreen});
            Colors.Add("Blue", new Color[] {Color.Blue, Color.LightBlue, Color.DarkBlue, Color.Aqua, Color.Turquoise});
            Colors.Add("Black", new Color[] {Color.Black});
            Colors.Add("White", new Color[] {Color.White});
            Colors.Add("Yellow", new Color[] {Color.Yellow, Color.LightYellow});
            Colors.Add("Purple", new Color[] {Color.Purple, Color.MediumPurple});
            Colors.Add("Orange", new Color[] {Color.Orange, Color.DarkOrange, Color.OrangeRed});
            Colors.Add("Brown", new Color[] {Color.Brown, Color.Tan, Color.Moccasin});
            Colors.Add("Gray", new Color[] {Color.Gray, Color.LightGray, Color.DarkGray});
            Colors.Add("Pink", new Color[] { Color.Pink, Color.HotPink, Color.LightPink, Color.DeepPink});
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

        public Dictionary<string, Color[]> Colors;
        public Dictionary<string, byte> Actions;
        public Dictionary<string, byte> Objects;

        public List<string[]> GetPhraseList()
        {
            List<string[]> PhraseList = new List<string[]>();
            PhraseList.Add(GetPhrases(Actions));
            PhraseList.Add(new string[] { "the", "a" });
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
        public string[] GetColorStrings(string KeyString)
        {
            Color[] myColors = null;
            Colors.TryGetValue(KeyString, out myColors);
            string[] OutStrings = new string[myColors.Length];
            for (int i = 0; i < OutStrings.Length; i++)
            {
                OutStrings[i] = myColors[i].ToKnownColor().ToString();
            }
            return OutStrings;
        }

    }
}
