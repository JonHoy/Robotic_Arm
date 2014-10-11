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

namespace Robot_Arm.Navigation
{
    class Action
    {
        public static bool Grab(
            Sensor forceSensor, // sensor used to determine if object is gripped or not
            Sensor distanceSensor, // sensor used to determine the distance between the gripper and the target
            Capture Camera, // Webcam used to locate object
            Servo xAxisServo, // servo used center the object between the gripper forks
            Servo yAxisServo1, // servo used to control planar vertical and horizontal position 
            Servo yAxisServo2, // servo used to control planar vertical and horizontal position 
            Servo gripperServo // servo used to grip object
            )
        {
            double forceLevel = 200; // define a threshold value that the force sensor must go above to be considered grabbed
            int iterationLimit = 30; // define max number of attempts to grab object before exiting
            int iterationCount = 1; // define the current iteration number
            bool objectGrabbed = true; // define success 

            var myAngleCalculator = new AngleCalculator();

            while (forceSensor.getSensorReading() < forceLevel)
            {
                
                
                
                
                if (iterationCount > iterationLimit)
                {
                    objectGrabbed = false; // iteration limit reached exit out and report as failure
                    break;
                }
                iterationCount++;
            }
            return objectGrabbed;
        }
    }
    class SharpIR : Sensor // Class for Sharp IR distance sensors
    {
        private double voltageMultiplier = 11.58;
        private double voltagePower = -1.058;
        private double maxDistance = 40;
        private double cm_to_inch = .394;

        public SharpIR(Arduino Board, int Pin) : base(Board, Pin){ }

        double getDistance() // return distance reading in inches
        {
            var voltage = getSensorReading();
            var distance = voltageMultiplier * Math.Pow(voltage, voltagePower);
            distance = cm_to_inch * Math.Min(distance, maxDistance);
            return distance;
        }
    }
    class ResistiveForce : Sensor // Class for resistive force sensors
    {
        private double rConst = 10000; // resistance of the constant value resistor in the voltage divider circuit 
        private double voltageMultiplier = 2.979e11; // constants based on curve fit
        private double voltagePower = -1.388;

        public ResistiveForce(Arduino Board, int Pin) : base(Board, Pin){ }

        double getForce() // gets the force reading in grams
        {
            var voltage = getSensorReading();
            var v_Ratio = (voltage - minVoltage) / (maxVoltage - minVoltage);
            var r_Sensor = rConst / v_Ratio - rConst;
            var Force = voltageMultiplier * Math.Pow(r_Sensor, voltagePower);
            return Force;
        }
    }
}
