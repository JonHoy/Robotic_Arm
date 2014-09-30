using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoClass;
using System.Drawing;
using Emgu.CV;
using Robot_Arm.Video;
using Robot_Arm.SpeechRecognition;

namespace Robot_Arm.Navigation
{
    public class Base // class used to determine the
    {
        public Base(Servo[] myServos, Arduino myArduino, Capture myCamera)
        { 
            

        }
        // Servos on the arm
        private Servo[] myServos;
        // the Arduino
        private Arduino myArduino;
        // Rectangle which defines the location of the discovered object
        private Rectangle myTarget;
        // measured distance from object
        private double TargetDistance;
        // Force on gripper
        private double GripperForce;
        // status telling whether the object has been gripped or not
        private bool GripStatus;
    }

}
