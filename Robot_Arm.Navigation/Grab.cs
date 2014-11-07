using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using ArduinoClass;
using System.Drawing;
using System.Threading;
using Robot_Arm.Video;

namespace Robot_Arm.Navigation
{
    public partial class Action
    {
        public static bool Grab(
            ResistiveForce forceSensor, // sensor used to determine if object is gripped or not
            SharpIR distanceSensor, // sensor used to determine the distance between the gripper and the target
            Capture Camera, // Webcam used to locate object
            Servo xAxisServo, // servo used center the object between the gripper forks
            Servo yAxisServo1, // servo used to control planar vertical and horizontal position 
            Servo yAxisServo2, // servo used to control planar vertical and horizontal position 
            Servo gripperServo, // servo used to grip object
            string[] ColorsToLookFor // colors to look for ie "Red", "Blue", "Yellow", etc
            )
        {
            double forceLevel = 200; // define a threshold value that the force sensor must go above to be considered grabbed
            double grabRange = 8;
            int iterationLimit = 30; // define max number of attempts to grab object before exiting
            int iterationCount = 1; // define the current iteration number
            bool objectGrabbed = false; // define success 

            var myAngleCalculator = new AngleCalculator();

            while (iterationCount < iterationLimit) // continue trying to grab the object until success or iteration limit reached
            {
                var NewFrame = Camera.QueryFrame(); // take image
                NewFrame = NewFrame.Clone(); // clone to remove null reference

                var TargetRegion = ColorObjectRecognizer.GetRegion(ColorsToLookFor, NewFrame.Clone());
                if (TargetRegion.IsEmpty) // if there is no target in sight break out of the loop
                {
                    break;
                }
                var Xpoint = ((double)TargetRegion.Left + (double)TargetRegion.Right) / 2;
                var Ypoint = ((double)TargetRegion.Top + (double)TargetRegion.Bottom) / 2;
                Robot_Arm.Navigation.Action.TrackBlobs(Xpoint, Ypoint, xAxisServo, yAxisServo2, 50, NewFrame.Height, NewFrame.Width); // center the object with the arm
                double objectDistance = distanceSensor.getDistance() / 2; // take distance reading, then go halfway to estimated distance
                if (objectDistance < grabRange) // the object is within range of the gripper, try grabbing it
                {
                    gripperServo.ServoAngleChange(gripperServo.MinAngle); // engage gripper
                    Thread.Sleep(1000); // wait one second
                    if (forceSensor.getSensorReading() > forceLevel) // take force reading
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
                yAxisServo1.ServoAngleChange((int) theta1_New); // move the servos to the new estimated angles to pick up the object
                yAxisServo2.ServoAngleChange((int) theta2_New + 90);
                iterationCount++; // increase the iteration count
            }
            return objectGrabbed;
        }

    }
}
