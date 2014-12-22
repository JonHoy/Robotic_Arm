using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Robot_Arm;
using Robot_Arm.Video;
using Robot_Arm.Navigation;
using Robot_Arm.SpeechRecognition;
using Emgu.CV;
using Emgu.CV.Structure;
using ArduinoClass;

namespace Robot_Arm.GUI
{
    public partial class MainGUI : Form
    {
        public MainGUI()
        {
            InitializeComponent();
            //TransparentHelper(pictureBox1, new Label[]{
            //    servo1_Label, 
            //    servo2_Label, 
            //    servo3_Label, 
            //    servo4_Label, 
            //    sensor1_Label, 
            //    sensor2_Label});
            this.Arm = new Robot_Arm.Base();
            this.Arm.ImageTaken += new EventHandler<Base.RobotEventArgs>(GUI_Refresh);
            trackBar1.Maximum = Arm.xAxisServo.MaxAngle;
            trackBar2.Maximum = Arm.yAxisServo1.MaxAngle;
            trackBar3.Maximum = Arm.yAxisServo2.MaxAngle;
            trackBar4.Maximum = Arm.gripperServo.MaxAngle;
            trackBar1.Minimum = Arm.xAxisServo.MinAngle;
            trackBar2.Minimum = Arm.yAxisServo1.MinAngle;
            trackBar3.Minimum = Arm.yAxisServo2.MinAngle;
            trackBar4.Minimum = Arm.gripperServo.MinAngle;
        }

        public Robot_Arm.Base Arm;

        private void start_button_Click(object sender, EventArgs e)
        {
            this.Arm.Grab("Orange");
        }

        private void GUI_Refresh(object sender, Base.RobotEventArgs e)
        {
            this.pictureBox1.Image = e.Photo;
            servo1_Label.Text = "Servo 1 (X-Axis) " + e.xAxisServoAngle.ToString() + " deg";
            servo2_Label.Text = "Servo 2 (Y-Axis) " + e.y1AxisServoAngle.ToString() + " deg";
            servo3_Label.Text = "Servo 3 (Y-Axis) " + e.y2AxisServoAngle.ToString() + " deg";
            servo4_Label.Text = "Servo 4 (Gripper) " + e.gripperServoAngle.ToString() + " deg";
            
            if (double.IsInfinity(e.ForceReading))
                sensor1_Label.Text = "Sensor 1 (Force) " + "\u221E" + " k\u2126"; // add kOhm to reading
            else
                sensor1_Label.Text = "Sensor 1 (Force) " + (e.ForceReading / 1000).ToString("F1") + " k\u2126"; // add kOhm to reading
            
            sensor2_Label.Text = "Sensor 2 (Distance) " + e.DistanceReading.ToString("F1") + " cm";
            sensor3_Label.Text = "Sensor 3 (Light) " + (e.LightReading / 1000).ToString("F1") + " k\u2126"; // add kOhm to reading
            progressBar1.Value = (int) (Arm.ForceSensor.getSensorReading()*1023.0/5.0);
            progressBar2.Value = (int) (Arm.DistanceSensor.getSensorReading()*1023.0/5.0);
            progressBar3.Value = (int)(Arm.LightSensor.getSensorReading() * 1023.0 / 5.0);
            trackBar1.Value = e.xAxisServoAngle;
            trackBar2.Value = e.y1AxisServoAngle;
            trackBar3.Value = e.y2AxisServoAngle;
            trackBar4.Value = e.gripperServoAngle;
            Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Photo = this.Arm.Webcam.QueryFrame().Clone();
            var Dictionary = new SpeechRecognition.SpeechDictionary();
            ColorObjectRecognizer.GetRegion(Dictionary.GetColorStrings("Orange"), Photo);
            this.pictureBox1.Image = Photo.ToBitmap();
            Refresh();
        }

        private void TransparentHelper(Control parent_control, Control label)
        {
            var pos = this.PointToScreen(label.Location);
            pos = pictureBox1.PointToClient(pos);
            label.Parent = parent_control;
            label.Location = pos;
            label.BackColor = Color.Transparent;
        }
        private void TransparentHelper(Control parent_control, Control[] labels)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                TransparentHelper(parent_control, labels[i]);
            }
        }

        private void MainGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Arm.Disconnect();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            Arm.xAxisServo.ServoAngleChange(trackBar1.Value);
            Arm.Webcam.QueryFrame();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            Arm.yAxisServo1.ServoAngleChange(trackBar2.Value);
            Arm.Webcam.QueryFrame();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            Arm.yAxisServo2.ServoAngleChange(trackBar3.Value);
            Arm.Webcam.QueryFrame();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            Arm.gripperServo.ServoAngleChange(trackBar4.Value);
            Arm.Webcam.QueryFrame();
        }

        private void MainGUI_Load(object sender, EventArgs e)
        {

        }

        private void auto_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (auto_radioButton.Checked)
            {
                this.Arm.Grab("Orange");
            }

        }

        private void manual_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (manual_radioButton.Checked)
            {

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }

}
