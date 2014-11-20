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
    public class Colors : Dictionary<System.String, Color[]>
    {
        public Colors()
        {
            Add("Red", new Color[] { Color.Red, Color.Firebrick, Color.Crimson});
            Add("Green", new Color[] { Color.Green, Color.DarkSlateGray, Color.SeaGreen});
            Add("Blue", new Color[] { Color.Blue, Color.MidnightBlue});
            Add("Black", new Color[] { Color.Black });
            Add("White", new Color[] { Color.White});
            Add("Yellow", new Color[] { Color.Yellow, Color.Gold, Color.DarkKhaki});
            Add("Purple", new Color[] { Color.Purple, Color.MediumPurple, Color.Indigo, Color.DarkOrchid, Color.BlueViolet, Color.DarkMagenta });
            Add("Orange", new Color[] { Color.Orange, Color.DarkOrange, Color.OrangeRed, Color.IndianRed, Color.Tomato, Color.Coral, Color.SaddleBrown});
            Add("Brown", new Color[] { Color.Brown});
            Add("Gray", new Color[] { Color.Gray, Color.LightGray, Color.DarkGray, Color.LightSlateGray, Color.Silver, Color.Gainsboro, Color.SlateGray });
            Add("Pink", new Color[] { Color.Pink});
        }

        public Color[] getAllColors()
        {
            var AllColors = new List<Color>();
            foreach (var Key in Keys)
            {
                Color[] TempColor = null;
                TryGetValue(Key, out TempColor);
                AllColors.AddRange(TempColor.ToList());
            }
            return AllColors.ToArray();
        }
    }
}
