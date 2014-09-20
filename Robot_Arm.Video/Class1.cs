using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Robot_Arm.Video
{
    // class that contains the information about how to classify a RGB Color
    public class ColorClassification
    {
        public ColorClassification()
        {
            Array colorsArray = Enum.GetValues(typeof(KnownColor));
            KnownColor[] allColors = new KnownColor[colorsArray.Length];
            Array.Copy(colorsArray, allColors, colorsArray.Length);
            
        }
        private Byte[][] KnownColors; // N x 3 Array (Array of N RGB Values where N is the number of colors in the database)
        // array of array of booleans for each color (N x N boolean array where N is the number of colors tested)
        private bool[][] ColorClassifier; 
    }
}
