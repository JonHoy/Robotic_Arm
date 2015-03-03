using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPU_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int SelectedColor = 1;
            int[] SelectionIndex = {0, 1, 2};
            byte[,] Colors = {{255, 0, 0, 0}, {0, 255, 0, 0}, {0, 0, 255, 0}};
            var Image = new byte[720, 1280, 4];

            var Camera = new Emgu.CV.Capture();
            Camera.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 720);
            Camera.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);


            for (int i = 0; i < 1000; i++)
            {
                var Frame = Camera.RetrieveBgrFrame();
                var Frame2 = Frame.Convert<Emgu.CV.Structure.Bgra, Byte>();
                var Before = DateTime.Now;
                var Blobs = Robot_Arm.GPU.SegmentColors(Frame2.Data, Colors, SelectionIndex, SelectedColor);
                var After = DateTime.Now;
                var Time = After - Before;
                Console.WriteLine("Frame: {0}, Time: {1} sec", i, Time.TotalSeconds);
            }
        }
    }
}
