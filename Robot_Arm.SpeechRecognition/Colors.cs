using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

// To see what these colors look like go to the link below
//http://coloreminder.com


namespace Robot_Arm.SpeechRecognition
{
    public struct ColorIndex
    {
        public Color[] ColorValues {get; set;}
        public int[] MasterIndices { get; set;} // indices which tells robot which master color the child color belongs to  (FireBrick and DarkRed belong to Red) Family
    }

    public class Colors : Dictionary<System.String, Color[]>
    {
        public Colors()
        {
            Add("Red", new Color[] { Color.Red, Color.FromArgb(162, 30, 33), Color.DarkRed, Color.Firebrick});
            Add("Green", new Color[] { Color.Green, Color.FromArgb(11, 154, 72), Color.ForestGreen});
            Add("Blue", new Color[] { Color.Blue, Color.FromArgb(0, 93, 154), Color.Aqua});
            Add("Black", new Color[] { Color.Black, Color.FromArgb(34, 30, 31), Color.DarkBlue});
            Add("White", new Color[] { Color.White, Color.FromArgb(250, 244, 244), Color.Gray});
            Add("Yellow", new Color[] { Color.Yellow, Color.Gold, Color.FromArgb(254, 233, 0), Color.Goldenrod});
            Add("Orange", new Color[] { Color.Orange, Color.FromArgb(243, 121, 33), Color.DarkOrange});
            Add("Brown", new Color[] { Color.Brown, Color.FromArgb(85, 69, 55), Color.Sienna});
        }

        public ColorIndex getAllColors()
        {
            var AllColors = new List<Color>();
            var indices = new List<int>();
            int CurrentIndex = 0;
            foreach (var Key in Keys)
            {
                
                Color[] TempColor = null;
                TryGetValue(Key, out TempColor);
                AllColors.AddRange(TempColor.ToList());
                for (int i = 0; i < TempColor.Length; i++)
			    {
			        indices.Add(CurrentIndex);
			    }
                CurrentIndex++;
            }
            ColorIndex myColorIdx = new ColorIndex();
            myColorIdx.ColorValues = AllColors.ToArray();
            myColorIdx.MasterIndices = indices.ToArray();
            return myColorIdx;
        }
    }
}
