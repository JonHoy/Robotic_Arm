using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot_Arm.Video;
using Emgu.CV;
using Emgu.CV.GPU;
using System.Drawing;
using DirectShow;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;

namespace ManualTests
{
    class Program
    {
        static void Main(string[] args)
        {

            var Form = new Form1();
            Form.ShowDialog();
        }
    }
}
