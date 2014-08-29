using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using SharpDX.XInput; // api for the xbox controller
using ArduinoClass; // arduino api
using System.Timers;
using AForge.Video; // webcam api
using AForge.Video.DirectShow;
using System.Runtime.InteropServices;
using System.Speech;
using AForge.Imaging.Filters;

namespace Robot_Arm
{
    unsafe public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private VideoCaptureDevice myCamera;
        private Arduino myArduino;
        private Controller myController;
        private List<Servo> myServos;
        private int RefreshRate_HZ = 10;

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                try
                {
                    this.myArduino = new Arduino(port); // connect the arduino
                    break; // if connected break out of the loop
                }
                catch (Exception)
                {
                    continue;
                }
            }
            try
            {
                Debug.Assert(this.myArduino != null);
            }
            catch (Exception)
            {
                throw new Exception("Unable to Connect to the arduino exiting now");
            }
            try
            {
                myController = new Controller(UserIndex.One); // connect the xbox controller
            }
            catch (Exception)
            {
                throw new Exception("Unable to connect to the controller");
            }
            try
            {
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    throw new Exception("No Cameras detected");
                }

                myCamera = new VideoCaptureDevice(videoDevices[0].MonikerString);
                VideoCapabilities[] CameraCapabilities = myCamera.VideoCapabilities;
                myCamera.VideoResolution = CameraCapabilities[6];
                videoSourcePlayer1.VideoSource = myCamera;
                videoSourcePlayer1.Start();
                myCamera.NewFrame += new NewFrameEventHandler(camera1_NewFrame);
            }
            catch (Exception)
            {
                throw new Exception("Unable to connect to one or more of the cameras");
            }            
            
            myServos = new List<Servo>(4);
            myServos.Insert(0, new Servo(ref myArduino, (int)numericUpDown1.Value, (int)Servo1_Trackbar.Maximum, (int)Servo1_Trackbar.Minimum));
            myServos.Insert(1, new Servo(ref myArduino, (int)numericUpDown2.Value, (int)Servo2_Trackbar.Maximum, (int)Servo2_Trackbar.Minimum));
            myServos.Insert(2, new Servo(ref myArduino, (int)numericUpDown3.Value, (int)Servo3_Trackbar.Maximum, (int)Servo3_Trackbar.Minimum));
            myServos.Insert(3, new Servo(ref myArduino, (int)numericUpDown4.Value, (int)Servo4_Trackbar.Maximum, (int)Servo4_Trackbar.Minimum));
            JoyStickTimer.Interval = 1000 / RefreshRate_HZ;
            JoyStickTimer.Start();
        }


        private void Controller_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Servo1_Trackbar_ValueChanged(object sender, EventArgs e)
        {
            myServos[0].ServoAngleChange(Servo1_Trackbar.Value);
            UpdateLabel(ref Servo1_Trackbar, ref label1, 1);
        }

        private void Servo2_Trackbar_ValueChanged(object sender, EventArgs e)
        {
            myServos[1].ServoAngleChange(Servo2_Trackbar.Value);
            UpdateLabel(ref Servo2_Trackbar, ref label2, 2);
        }

        private void Servo3_TrackBar_ValueChanged(object sender, EventArgs e)
        {
            myServos[2].ServoAngleChange(Servo3_Trackbar.Value);
            UpdateLabel(ref Servo3_Trackbar, ref label3, 3);
        }

        private void Servo4_Trackbar_ValueChanged(object sender, EventArgs e)
        {
            myServos[3].ServoAngleChange(Servo4_Trackbar.Value);
            UpdateLabel(ref Servo4_Trackbar, ref label4, 4);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            myServos[0].DigitalPinChange((int) numericUpDown1.Value); 
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            myServos[1].DigitalPinChange((int)numericUpDown2.Value); 
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            myServos[2].DigitalPinChange((int)numericUpDown3.Value); 
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            myServos[3].DigitalPinChange((int)numericUpDown4.Value); 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (Servo Servo in myServos)
                {
                    Servo.Detach(); // detach each servo when exiting the program
                }
                videoSourcePlayer1.Stop();
            }
            catch (Exception){ }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Controller_Checkbox.Checked)
            {
                int myOldPacketNumber = 0;
                State CurrentState = myController.GetState();
                int myNewPacketNumber = CurrentState.PacketNumber;
                if (myNewPacketNumber != myOldPacketNumber) // only update if packet number has changed
                {
                    int LeftThumbX = CurrentState.Gamepad.LeftThumbX;
                    int RightThumbX = CurrentState.Gamepad.RightThumbX;
                    int LeftThumbY = CurrentState.Gamepad.LeftThumbY;
                    int RightThumbY = CurrentState.Gamepad.RightThumbY;
                    double Sensitivity = (((double)trackBar1.Value) / 5e5) * RefreshRate_HZ;
                    int DeadZone = 4000;
                    UpdateServoAngle(LeftThumbX, Sensitivity, myServos[0], ref Servo1_Trackbar, DeadZone);
                    UpdateServoAngle(LeftThumbY, Sensitivity, myServos[1], ref Servo2_Trackbar, DeadZone);
                    UpdateServoAngle(RightThumbY, Sensitivity, myServos[2], ref Servo3_Trackbar, DeadZone);
                    UpdateServoAngle(RightThumbX, Sensitivity, myServos[3], ref Servo4_Trackbar, DeadZone);
                    myOldPacketNumber = myNewPacketNumber;
                }
            }

        }
        private void UpdateLabel(ref TrackBar myTrackBar, ref Label myLabel, int Number)
        {
            myLabel.Text = "Servo " + Number.ToString();
            myLabel.Text = myLabel.Text + " : " + myTrackBar.Value.ToString();
            myLabel.Text = myLabel.Text + " deg";
        }

        private void UpdateServoAngle(int StickValue, double Sensitivity, Servo myServo, ref TrackBar myTrackbar, int DeadZone)
        {
            if (Math.Abs(StickValue) > DeadZone)
            {
                StickValue = StickValue - Math.Sign(StickValue) * DeadZone; // remove the deadzone offset
                int NewAngle = myServo.Angle + (int)(Sensitivity * (double)StickValue); // apply the sensitivity factor
                NewAngle = Math.Max(myTrackbar.Minimum, NewAngle);
                NewAngle = Math.Min(myTrackbar.Maximum, NewAngle); // bound the angle between the physical limits of the servo
                myTrackbar.Value = NewAngle;
            }
        }

        private void camera1_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap bitmap = eventArgs.Frame;
                ImageProcess ProcessObject = new ImageProcess((Bitmap)bitmap.Clone());
                ProcessObject.Segment_Colors();

                        
            
                pictureBox1.Image = (Bitmap)ProcessObject.myBitmap.Clone();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        //private void Servo4_Trackbar_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void Servo3_Trackbar_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void Servo1_Trackbar_Scroll(object sender, EventArgs e)
        //{

        //}
    }


}

