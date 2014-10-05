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
    public class Base // class used to determine the
    {
        //public Base(Lis myServos, Arduino myArduino)
        //{ 
            

        //}
        //// Servos on the arm
        //private Servo[] myServos;
        //// the Arduino
        //private Arduino myArduino;
        //// Rectangle which defines the location of the discovered object
        //private Rectangle myTarget;
        //// measured distance from object
        //private double TargetDistance;
        //// Force on gripper
        //private double GripperForce;
        //// status telling whether the object has been gripped or not
        //private bool GripStatus;


        public static void TrackBlobs(double Xpoint, double Ypoint, Servo xAxisServo, Servo yAxisServo, double FOV, double FrameHeight = 480, double FrameWidth = 640)
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
            int yPoints = 0,
            int xPoints = 18)
        {
            int StartPt = xAxisServo.MinAngle;
            int EndPt = xAxisServo.MaxAngle;
            var FoundBlobs = new List<Rectangle>();
            for (int iPhoto = 0; iPhoto < xPoints; iPhoto++)
            {
                int NewAngle = StartPt + iPhoto * (EndPt - StartPt) / (xPoints - 1);
                xAxisServo.ServoAngleChange(NewAngle);
                Thread.Sleep(500);
                var Photo = myCamera.QueryFrame();
                FoundBlobs.Insert(iPhoto, ColorObjectRecognizer.GetRegion(ColorsToLookFor, Photo.Clone()));
            }
            return FoundBlobs;
        }
    }

}
