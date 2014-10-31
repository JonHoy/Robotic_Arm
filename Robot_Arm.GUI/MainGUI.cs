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
using System.Runtime.InteropServices;
using System.Speech;
using Robot_Arm.SpeechRecognition;
using Robot_Arm.Video;
using Robot_Arm.Navigation;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
//using Robot_Arm.

namespace Robot_Arm.GUI
{
    public partial class MainGUI : Form
    {
        public MainGUI()
        {
            InitializeComponent();
        }
        //private VideoCaptureDevice myCamera;
        private AngleCalculator myAngleCalculator;
        private Arduino myArduino;
        private Controller myController;
        private Capture myCamera;
        private List<Servo> myServos;
        private int RefreshRate_HZ = 10;
        private SpeechDictionary myWords;
        private string[] MatchStrings;
       

        private void Form1_Load(object sender, EventArgs e)
        {
            myWords = new SpeechDictionary();
            MatchStrings = myWords.GetColorStrings("Orange");
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
                myCamera = new Capture();
            }
            catch (Exception ex)
            {
                
                throw;
            }
            
            myAngleCalculator = new AngleCalculator((int)Servo2_Trackbar.Minimum, (int)Servo2_Trackbar.Maximum, (int)Servo3_Trackbar.Minimum - 180, (int)Servo3_Trackbar.Maximum - 180);

            myServos = new List<Servo>(4);
            myServos.Insert(0, new Servo(ref myArduino, (int)numericUpDown1.Value, (int)Servo1_Trackbar.Maximum, (int)Servo1_Trackbar.Minimum));
            myServos.Insert(1, new Servo(ref myArduino, (int)numericUpDown2.Value, (int)Servo2_Trackbar.Maximum, (int)Servo2_Trackbar.Minimum));
            myServos.Insert(2, new Servo(ref myArduino, (int)numericUpDown3.Value, (int)Servo3_Trackbar.Maximum, (int)Servo3_Trackbar.Minimum));
            myServos.Insert(3, new Servo(ref myArduino, (int)numericUpDown4.Value, (int)Servo4_Trackbar.Maximum, (int)Servo4_Trackbar.Minimum));

            myServos[3].Detach(); // detach the gripper servo until its needed

            JoyStickTimer.Interval = 1000 / RefreshRate_HZ;
            JoyStickTimer.Start();
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
            }
            catch (Exception){ }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            var Photo = myCamera.QueryFrame(); //draw the image obtained from camera
            this.imageBox1.Image = Photo;
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
            else if (auto_checkBox.Checked)
            {
                
                //var Region = Robot_Arm.Video.ColorObjectRecognizer.GetRegion(myWords.GetColorStrings("Orange"), Photo.Clone());
                //if (! Region.IsEmpty)
                //{
                    
                //    Robot_Arm.Navigation.Base.TrackBlobs(
                //        (Region.Left + Region.Right) / 2,
                //        (Region.Top + Region.Bottom) / 2,
                //        myServos[0],
                //        myServos[2],
                //        30);

                //}
                myServos[1].ServoAngleChange(115);
                myServos[2].ServoAngleChange(45);
                //Robot_Arm.Navigation.Action.ScanForBlobs(
                //    myWords.GetColorStrings("Orange"),
                //    myCamera,
                //    myServos[0]);
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

        private void AD_Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                ushort Val = myArduino.AnalogRead(i);
                string PinNumber = "Analog " + i.ToString();
                this.chart1.Series[PinNumber].Points.AddY((double)Val);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    string PinNumber = "Analog " + i.ToString();
                    this.chart1.Series.Add(PinNumber);
                    this.chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                }
                this.AD_Timer.Start();
            }
            catch (Exception)
            {
            }
           
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            this.AD_Timer.Stop();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            this.AD_Timer.Stop();
            this.chart1.Series.Clear();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void yTrackbar_ValueChanged(object sender, EventArgs e)
        {
            xTrackbar_ValueChanged(sender, e);
        }

        private void xTrackbar_ValueChanged(object sender, EventArgs e)
        {
            double y_Target = (double) yTrackbar.Value / 10.0;
            double x_Target = (double) xTrackbar.Value / 10.0;
            var theta1 = double.NaN;
            var theta2 = double.NaN;
            Console.WriteLine("X Target: {0}    Y Target: {1}", x_Target, y_Target); 
            myAngleCalculator.getTheta(x_Target, y_Target, out theta1, out theta2);
            if (!double.IsNaN(theta1) && !double.IsNaN(theta2))
            {
                int Servo2Val = (int) (170 - (double)theta1 * 8.0/9.0);
                int Servo3Val = (int) (1.2 * ((double) theta2 + 180) + 5.0);
                Servo2Val = Math.Min(Servo2Val, Servo2_Trackbar.Maximum);
                Servo2Val = Math.Max(Servo2Val, Servo2_Trackbar.Minimum); 
                Servo3Val = Math.Min(Servo3Val, Servo3_Trackbar.Maximum);
                Servo3Val = Math.Max(Servo3Val, Servo3_Trackbar.Minimum);
                Servo2_Trackbar.Value = Servo2Val;
                Servo3_Trackbar.Value = Servo3Val;
            }
        }

        private void auto_checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Controller_Checkbox_CheckedChanged(object sender, EventArgs e)
        {

        }



    }


}

