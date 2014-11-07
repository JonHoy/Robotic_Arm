using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;

// To see what these colors look like go to the link below
//http://coloreminder.com

namespace Robot_Arm.SpeechRecognition
{
    public class Colors : Dictionary<System.String, Color[]>
    {
        public Colors()
        {
            Add("Red", new Color[] { Color.Red, Color.DarkRed, Color.Maroon, Color.Crimson, Color.Firebrick, Color.IndianRed, Color.Tomato });
            Add("Green", new Color[] { Color.Green, Color.LightGreen, Color.DarkGreen, Color.Lime, Color.Aqua, Color.LightSeaGreen });
            Add("Blue", new Color[] { Color.Blue, Color.DarkBlue, Color.RoyalBlue, Color.Navy });
            Add("Black", new Color[] { Color.Black });
            Add("White", new Color[] { Color.White, Color.OldLace });
            Add("Yellow", new Color[] { Color.Yellow, Color.LightYellow, Color.Gold, Color.Beige, Color.Khaki });
            Add("Purple", new Color[] { Color.Purple, Color.MediumPurple, Color.Indigo, Color.DarkOrchid, Color.BlueViolet, Color.DarkMagenta });
            Add("Orange", new Color[] { Color.Orange, Color.DarkOrange, Color.OrangeRed, Color.PeachPuff });
            Add("Brown", new Color[] { Color.Tan, Color.Peru, Color.SaddleBrown, Color.Chocolate });
            Add("Gray", new Color[] { Color.Gray, Color.LightGray, Color.DarkGray, Color.LightSlateGray, Color.Silver, Color.Gainsboro, Color.SlateGray });
            Add("Pink", new Color[] { Color.Pink, Color.HotPink, Color.LightPink, Color.DeepPink, Color.Violet, Color.Magenta });
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
