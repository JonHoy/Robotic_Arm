using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using ArduinoClass;
using Robot_Arm.Video;
using Robot_Arm.Navigation;
using Robot_Arm.SpeechRecognition;
using SharpDX.XInput;
using System.IO.Ports;
using System.Windows.Forms;

namespace Robot_Arm
{
    public class Base
    {
        // COM Port the arduino is connected to
        private string arduinoCOM = "COM3";
        // specific servo digital pin mappings on the arduino
        private int ServoCount = 4;
        private int xServoPin = 11, xServoStart = 90;
        private int y1ServoPin = 26, y1ServoMin = 20, y1ServoMax = 160, y1ServoStart = 90;
        private int y2ServoPin = 30, y2ServoMin = 42, y2ServoMax = 160, y2ServoStart = 90;
        private int gripServoPin = 8, gripServoMin = 40, gripServoMax = 90, gripServoStart = 90;
        // specific analog pin mappings on the arduino
        private int Distance_Analog_Pin = 0;
        private int Force_Analog_Pin = 1;

        public Servo[] Servos;
        public Controller XboxController;
        public Capture Webcam;
        public Arduino Mega2560;
        public SharpIR DistanceSensor;
        public ResistiveForce ForceSensor;
        public SpeechRecognition.Base SpeechEngine;
        public PictureBox myViewer;

        public Base()
        {
            var comports = SerialPort.GetPortNames();
            for (int i = 0; i < comports.Length; i++)
            {
                try
                {
                    Mega2560 = new Arduino(comports[i]);
                    break;
                }
                catch (Exception) {}
            }
            ForceSensor = new ResistiveForce(Mega2560, Force_Analog_Pin);
            DistanceSensor = new SharpIR(Mega2560, Distance_Analog_Pin);
            Webcam = new Capture();
            Webcam.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);
            Webcam.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 720); // set cam resolution to 720P
            Webcam.ImageGrabbed += new EventHandler(Webcam_ImageGrabbed);
            
            XboxController = new Controller(UserIndex.One);
            Servos = new Servo[ServoCount];
            Servos[0] = new Servo(ref Mega2560, xServoPin);
            Servos[1] = new Servo(ref Mega2560, y1ServoPin, y1ServoMax, y1ServoMin);
            Servos[2] = new Servo(ref Mega2560, y2ServoPin, y2ServoMax, y2ServoMin);
            Servos[3] = new Servo(ref Mega2560, gripServoPin, gripServoMax, gripServoMin);
            Servos[0].ServoAngleChange(xServoStart);
            Servos[1].ServoAngleChange(y1ServoStart);
            Servos[2].ServoAngleChange(y2ServoStart);
            Servos[3].ServoAngleChange(gripServoStart);

            SpeechEngine = new SpeechRecognition.Base();
        }

        private void Webcam_ImageGrabbed(object sender, EventArgs e)
        {
            RobotEventArgs args = new RobotEventArgs();
            args.Photo = this.Webcam.RetrieveBgrFrame().Clone().ToBitmap();
            args.Date = DateTime.Now.ToLongTimeString();
            args.ForceReading = ForceSensor.getForce();
            args.DistanceReading = DistanceSensor.getDistance();
            args.ServoAngles = new int[Servos.Length];
            for (int i = 0; i < Servos.Length; i++)
            {
                args.ServoAngles[i] = Servos[i].Angle;
            }
            OnImageGrabbed(args);
        }
        
        public void Grab(string Color)
        { 
            string[] ColorStrings = SpeechEngine.mySpeechDictionary.GetColorStrings(Color);
            Robot_Arm.Navigation.Action.ScanForBlobs(ColorStrings, Webcam, Servos[0], Servos[2]);
            Robot_Arm.Navigation.Action.Grab(ForceSensor, DistanceSensor, Webcam, Servos[0], Servos[1], Servos[2], Servos[3], ColorStrings);
        }

        public class RobotEventArgs : EventArgs
        {
            public Bitmap Photo;
            public int[] ServoAngles;
            public double ForceReading;
            public double DistanceReading;
            public string Date;
        }
        private void OnImageGrabbed(RobotEventArgs e)
        {
            EventHandler<RobotEventArgs> handler = ImageTaken;
        }

        public event EventHandler<RobotEventArgs> ImageTaken;

    }

}
