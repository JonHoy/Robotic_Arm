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
    public partial class Base
    {
        // COM Port the arduino is connected to
        private string arduinoCOM = "COM3";
        // specific servo digital pin mappings on the arduino
        private int ServoCount = 4;
        private int xServoPin = 11, xServoStart = 90;
        private int y1ServoPin = 26, y1ServoMin = 120, y1ServoMax = 160, y1ServoStart = 120;
        private int y2ServoPin = 30, y2ServoMin = 60, y2ServoMax = 120, y2ServoStart = 80;
        private int gripServoPin = 8, gripServoMin = 30, gripServoMax = 80, gripServoStart = 80;
        // specific analog pin mappings on the arduino
        private int Distance_Analog_Pin = 0;
        private int Force_Analog_Pin = 1;

        public Servo xAxisServo;
        public Servo yAxisServo1;
        public Servo yAxisServo2;
        public Servo gripperServo;

        public Controller XboxController;
        public Capture Webcam;
        public Arduino Mega2560;
        public SharpIR DistanceSensor;
        public ResistiveForce ForceSensor;
        public SpeechRecognition.Base SpeechEngine;
        public SpeechRecognition.SpeechDictionary Dictionary;

        private string[] ColorsToLookFor;

        public Base()
        {
            var comports = SerialPort.GetPortNames();
            if (comports.Length == 0)
            {
                throw new Exception("Error: No COM ports found");
            }
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
            Webcam = new Capture(1);
            Dictionary = new SpeechDictionary();
            Webcam.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);
            Webcam.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 720); // set cam resolution to 720P
            Webcam.QueryFrame(); // take a test photo
            Webcam.ImageGrabbed += new EventHandler(Webcam_ImageGrabbed);
            
            XboxController = new Controller(UserIndex.One);
            
            xAxisServo = new Servo(ref Mega2560, xServoPin);
            yAxisServo1 = new Servo(ref Mega2560, y1ServoPin, y1ServoMax, y1ServoMin);
            yAxisServo2 = new Servo(ref Mega2560, y2ServoPin, y2ServoMax, y2ServoMin);
            gripperServo = new Servo(ref Mega2560, gripServoPin, gripServoMax, gripServoMin);
            xAxisServo.ServoAngleChange(xServoStart);
            yAxisServo1.ServoAngleChange(y1ServoStart);
            yAxisServo2.ServoAngleChange(y2ServoStart);
            gripperServo.ServoAngleChange(gripServoStart);

            SpeechEngine = new SpeechRecognition.Base();
        }

        private void Webcam_ImageGrabbed(object sender, EventArgs e)
        {
            RobotEventArgs args = new RobotEventArgs();
            Image<Bgr, Byte> Frame = this.Webcam.RetrieveBgrFrame().Clone();
            args.Photo = Frame.ToBitmap();
            args.Date = DateTime.Now.ToLongTimeString();
            args.ForceReading = ForceSensor.getForce();
            args.DistanceReading = DistanceSensor.getDistance();
            args.xAxisServoAngle = xAxisServo.Angle;
            args.y1AxisServoAngle = yAxisServo1.Angle;
            args.y2AxisServoAngle = yAxisServo2.Angle;
            args.gripperServoAngle = gripperServo.Angle;
            OnImageGrabbed(args);
        }

        public void Disconnect()
        {
            //Mega2560.Disconnect();
        }

        public class RobotEventArgs : EventArgs
        {
            public Bitmap Photo;
            public int xAxisServoAngle;
            public int y1AxisServoAngle;
            public int y2AxisServoAngle;
            public int gripperServoAngle;
            public double ForceReading;
            public double DistanceReading;
            public string Date;
        }
        private void OnImageGrabbed(RobotEventArgs e)
        {
            EventHandler<RobotEventArgs> handler = ImageTaken;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<RobotEventArgs> ImageTaken;

    }

}
