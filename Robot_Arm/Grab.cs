using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Robot_Arm.SpeechRecognition;

namespace Robot_Arm
{
    public partial class Base
    {
        public bool Grab(string Color)
        {
            ColorsToLookFor = Dictionary.GetColorStrings(Color);
            ScanForBlobs();
            var Randomizer = new Random();
            double forceLevel = 200; // define a threshold value that the force sensor must go above to be considered grabbed
            double grabRange = 8;
            int Scan = 15;
            int iterationLimit = 30; // define max number of attempts to grab object before exiting
            int iterationCount = 1; // define the current iteration number
            bool objectGrabbed = false; // define success 

            var myAngleCalculator = new Robot_Arm.Navigation.AngleCalculator();

            while (iterationCount < iterationLimit) // continue trying to grab the object until success or iteration limit reached
            {
                var NewFrame = Webcam.QueryFrame(); // take image
                NewFrame = NewFrame.Clone(); // clone to remove null reference

                var TargetRegion = Robot_Arm.Video.ColorObjectRecognizer.GetRegion(ColorsToLookFor, NewFrame);
                if (TargetRegion.IsEmpty) // if there is no target in sight scan veritically until the target is aquired
                {
                    // randomly look around with the arm until the object is located or iteration count is upp
                    var Pos_or_Neg = Math.Round(Randomizer.NextDouble());
                    var X_or_Y = Math.Round(Randomizer.NextDouble());
                    if (Pos_or_Neg == 0)
                        Scan = -1 * Math.Abs(Scan);
                    else
                        Scan = Math.Abs(Scan);
                    if (X_or_Y == 0)
                        yAxisServo2.ServoAngleChange(yAxisServo2.Angle + Scan);
                    else
                        xAxisServo.ServoAngleChange(xAxisServo.Angle + Scan);
                    Thread.Sleep(300);
                    iterationCount++; // increase the iteration count
                    continue;
                }
                bool status = TrackBlobs();// center the object with the arm
                if (status == false)
                {
                    iterationCount++;
                    continue;
                }
                double objectDistance = DistanceSensor.getDistance() / 4; // take distance reading, then go halfway to estimated distance
                if (objectDistance < grabRange) // the object is within range of the gripper, try grabbing it
                {
                    gripperServo.ServoAngleChange(gripperServo.MinAngle); // engage gripper
                    Thread.Sleep(1000); // wait one second
                    if (ForceSensor.getSensorReading() > forceLevel) // take force reading
                    {
                        objectGrabbed = true; // if force reading above threshold it means the object has been grabbed and we can break out of the loop
                        break;
                    }
                    else
                    {
                        gripperServo.ServoAngleChange(gripperServo.MaxAngle); // if not gripped release the gripper and start the process over again
                    }

                }
                double theta1_New; double theta2_New;
                myAngleCalculator.getNewTheta(objectDistance, (double)yAxisServo1.Angle, (double)yAxisServo2.Angle, out theta1_New, out theta2_New); // estimate the new angles needed to pick up the object
                yAxisServo1.ServoAngleChange((int)theta1_New); // move the servos to the new estimated angles to pick up the object
                yAxisServo2.ServoAngleChange((int)theta2_New + 90);
                iterationCount++; // increase the iteration count
            }
            return objectGrabbed;
        }
    }
}
