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
        public static void TrackBlobs(Rectangle Region, Servo xAxisServo, Servo yAxisServo, double FOV, double FrameHeight, double FrameWidth)
        {
            double Xpoint = ((double)Region.Left + (double)Region.Right) / 2;
            double Ypoint = ((double)Region.Top + (double)Region.Bottom) / 2;
        }


    }

}
