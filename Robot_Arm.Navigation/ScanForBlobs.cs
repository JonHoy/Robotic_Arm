using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ArduinoClass;
using Emgu.CV;
using Robot_Arm.Video;
using System.Threading;

namespace Robot_Arm.Navigation
{
    public partial class Action // class used to determine the
    {
        public static void ScanForBlobs(
            string[] ColorsToLookFor,
            Capture myCamera,
            Servo xAxisServo,
            Servo yAxisServo,
            int yPoints = 2,
            int xPoints = 18,
            int yStartPt = 50,
            int yEndPt = 70)
        {
            int xStartPt = xAxisServo.MinAngle;
            int xEndPt = xAxisServo.MaxAngle;
            int yAngle;
            for (int i_x = 0; i_x < xPoints; i_x++)
            {
                int xAngle = xStartPt + i_x * (xEndPt - xStartPt) / (xPoints - 1);
                xAxisServo.ServoAngleChange(xAngle);
                for (int i_y = 0; i_y < yPoints; i_y++)
                {
                    if (i_x % 2 == 0)
                        yAngle = yStartPt + i_y * (yEndPt - yStartPt) / (yPoints - 1);
                    else
                        yAngle = yEndPt - i_y * (yEndPt - yStartPt) / (yPoints - 1);
                    yAxisServo.ServoAngleChange(yAngle);
                    Thread.Sleep(500);
                    var Photo = myCamera.QueryFrame();
                    var newRect = ColorObjectRecognizer.GetRegion(ColorsToLookFor, Photo.Clone());
                    if (newRect.IsEmpty == false)              
                        break;
                }
            }
        }


    }
}
