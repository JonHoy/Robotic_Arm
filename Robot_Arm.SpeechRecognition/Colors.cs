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
            Add("Red", new Color[] { Color.Red, Color.FromArgb(162, 30, 33)});
            Add("Green", new Color[] { Color.Green, Color.FromArgb(11, 154, 72)});
            Add("Blue", new Color[] { Color.Blue, Color.FromArgb(0, 93, 154)});
            Add("Black", new Color[] { Color.Black, Color.FromArgb(34, 30, 31)});
            Add("White", new Color[] { Color.White, Color.FromArgb(250, 244, 244)});
            Add("Yellow", new Color[] { Color.Yellow, Color.Gold, Color.FromArgb(254, 233, 0)});
            //Add("Purple", new Color[] { Color.Purple, Color.MediumPurple, Color.Indigo, Color.DarkOrchid, Color.BlueViolet, Color.DarkMagenta });
            Add("Orange", new Color[] { Color.Orange, Color.FromArgb(243, 121, 33)});
            Add("Brown", new Color[] { Color.Brown, Color.FromArgb(85, 69, 55)});
            //Add("Gray", new Color[] { Color.Gray, Color.LightGray, Color.DarkGray, Color.LightSlateGray, Color.Silver, Color.Gainsboro, Color.SlateGray });
            //Add("Pink", new Color[] { Color.Pink});
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
