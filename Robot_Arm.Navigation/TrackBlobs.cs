using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoClass;
using System.Drawing;
using System.Threading;
using Emgu.CV;
using Robot_Arm.Video;
using Robot_Arm.SpeechRecognition;

namespace Robot_Arm.Navigation
{
    public partial class Action // class used to determine the
    {
        public static void TrackBlobs(double Xpoint, double Ypoint, Servo xAxisServo, Servo yAxisServo, double FOV, double FrameHeight, double FrameWidth)
        {
           // 0,0 corresponds to the top left
           var CenterX = FrameWidth/2;
           var CenterY = FrameWidth/2;

           var Y_FOV = (FrameHeight / Math.Sqrt(Math.Pow(FrameHeight, 2) + Math.Pow(FrameWidth, 2))) * FOV;
           var X_FOV = (FrameWidth / Math.Sqrt(Math.Pow(FrameHeight, 2) + Math.Pow(FrameWidth, 2))) * FOV;

            var dThetaY =  (CenterY - Ypoint) / FrameHeight * Y_FOV;  
            var dThetaX =  (Xpoint - CenterX) / FrameWidth * X_FOV;

            int xAngle = xAxisServo.Angle;
            int yAngle = yAxisServo.Angle;

            xAxisServo.ServoAngleChange(xAngle + (int)dThetaX);
            yAxisServo.ServoAngleChange(yAngle + (int)dThetaY); 
        }

        public static List<Rectangle> ScanForBlobs(
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
            var FoundBlobs = new List<Rectangle>();
            int yAngle;
            for (int i_x = 0; i_x < xPoints; i_x++)
            {
                for (int i_y = 0; i_y < yPoints; i_y++)
                {
                    if (i_x % 2 == 0)
                        yAngle = yStartPt + i_y * (yEndPt - yStartPt) / (yPoints - 1);
                    else
                        yAngle = yEndPt - i_y * (yEndPt - yStartPt) / (yPoints - 1);
                    yAxisServo.ServoAngleChange(yAngle);
                    Thread.Sleep(500);
                    var Photo = myCamera.QueryFrame();
                    FoundBlobs.Add(ColorObjectRecognizer.GetRegion(ColorsToLookFor, Photo.Clone()));
                }
                int xAngle = xStartPt + i_x * (xEndPt - xStartPt) / (xPoints - 1);
                xAxisServo.ServoAngleChange(xAngle);
                
            }
            return FoundBlobs;
        }
    }

}
