using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot_Arm.Video;
using System.Drawing;
using System.Threading;
using Emgu.CV.Structure;
using Emgu.CV;

namespace Robot_Arm
{
    public partial class Base
    {
        public void ScanForBlobs(
            int PhotoCount = 5,
            int yPoints = 2,
            int xPoints = 18,
            int yStartPt = 50,
            int yEndPt = 70
            )
        {
            int xStartPt = xAxisServo.MinAngle;
            int xEndPt = xAxisServo.MaxAngle;
            int yAngle;
            Rectangle newRect = new Rectangle();

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
                    yAxisServo2.ServoAngleChange(yAngle);
                    Thread.Sleep(500);
                    newRect = ColorObjectRecognizer.GetRegion(ColorsToLookFor, Webcam.QueryFrame().Clone());
                    if (newRect.IsEmpty == false)
                        break;
                }
                if (newRect.IsEmpty == false)
                    break;
            }
        }
    }
}
