using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using ArduinoClass;
using System.Drawing;

namespace Robot_Arm.Navigation
{
    class Action
    {
        public static void Grab(
            Sensor forceSensor, // sensor used to determine if object is gripped or not
            Sensor distanceSensor, // sensor used to determine the distance between the gripper and the target
            Capture Camera, // Webcam used to locate object
            Servo xAxisServo, // servo used center the object between the gripper forks
            Servo yAxisServo1, // servo used to control planar vertical and horizontal position 
            Servo yAxisServo2, // servo used to control planar vertical and horizontal position 
            Servo gripperServo // servo used to grip object
            )
        {
 
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
