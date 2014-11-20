using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Robot_Arm
{
    partial class Base
    {
        public bool TrackBlobs(string Color)
        {
            ColorsToLookFor = Dictionary.GetColorStrings(Color);
            return TrackBlobs();
        }
        public bool TrackBlobs()
        {
            Rectangle Region = Robot_Arm.Video.ColorObjectRecognizer.GetRegion(ColorsToLookFor, Webcam.RetrieveBgrFrame().Clone());
            if (Region.IsEmpty)
            {
                return false;
            }
            double Xpoint = (double)((Region.Left + Region.Right) / 2);
            double Ypoint = (double)((Region.Top + Region.Bottom) / 2);
            double FOV = 50;
            double FrameWidth = (double)Webcam.Width;
            double FrameHeight = (double)Webcam.Height;
            int iterationCount = 0;
            int iterationLimit = 15;
            double tolerance = .10;
            double distancetoCenter = double.MaxValue;
            while (iterationCount < iterationLimit || distancetoCenter < tolerance)
            {
                // 0,0 corresponds to the top left
                var CenterX = FrameWidth / 2;
                var CenterY = FrameWidth / 2;

                var Y_FOV = (FrameHeight / Math.Sqrt(Math.Pow(FrameHeight, 2) + Math.Pow(FrameWidth, 2))) * FOV;
                var X_FOV = (FrameWidth / Math.Sqrt(Math.Pow(FrameHeight, 2) + Math.Pow(FrameWidth, 2))) * FOV;

                var dThetaY = (CenterY - Ypoint) / FrameHeight * Y_FOV;
                var dThetaX = (CenterX - Xpoint) / FrameWidth * X_FOV;

                int xAngle = xAxisServo.Angle;
                int yAngle = yAxisServo2.Angle;

                xAxisServo.ServoAngleChange(xAngle + (int)dThetaX);
                yAxisServo2.ServoAngleChange(yAngle + (int)dThetaY);
                iterationCount++;
                Region = Robot_Arm.Video.ColorObjectRecognizer.GetRegion(ColorsToLookFor, Webcam.RetrieveBgrFrame().Clone());
                if (Region.IsEmpty)
                    break;
                Xpoint = (double)((Region.Left + Region.Right) / 2);
                Ypoint = (double)((Region.Top + Region.Bottom) / 2);
                distancetoCenter = Math.Sqrt(Math.Pow(Math.Abs((FrameHeight / 2 - Ypoint))/FrameHeight,2) + Math.Pow(Math.Abs(FrameWidth / 2 - Xpoint)/FrameWidth,2));
            }
            if (distancetoCenter < tolerance && Region.IsEmpty == false)
                return true;
            else
                return false;

        }
    }
}
