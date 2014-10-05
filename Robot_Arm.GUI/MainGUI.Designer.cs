namespace Robot_Arm.GUI
{
    partial class MainGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.Servo1_Trackbar = new System.Windows.Forms.TrackBar();
            this.Servo2_Trackbar = new System.Windows.Forms.TrackBar();
            this.Controller_Checkbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Servo3_Trackbar = new System.Windows.Forms.TrackBar();
            this.Servo4_Trackbar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.JoyStickTimer = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.auto_checkBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Video = new System.Windows.Forms.TabPage();
            this.Speech = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.Servo_Joystick = new System.Windows.Forms.TabPage();
            this.yTrackbar = new System.Windows.Forms.TrackBar();
            this.xTrackbar = new System.Windows.Forms.TrackBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Navigation = new System.Windows.Forms.TabPage();
            this.clearButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.AD_Timer = new System.Windows.Forms.Timer(this.components);
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.Servo1_Trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo2_Trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo3_Trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo4_Trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.Video.SuspendLayout();
            this.Speech.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.Servo_Joystick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.Navigation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Servo1_Trackbar
            // 
            this.Servo1_Trackbar.BackColor = System.Drawing.SystemColors.Window;
            this.Servo1_Trackbar.Location = new System.Drawing.Point(7, 154);
            this.Servo1_Trackbar.Maximum = 180;
            this.Servo1_Trackbar.Name = "Servo1_Trackbar";
            this.Servo1_Trackbar.Size = new System.Drawing.Size(265, 45);
            this.Servo1_Trackbar.TabIndex = 0;
            this.Servo1_Trackbar.Tag = "";
            this.Servo1_Trackbar.TickFrequency = 5;
            this.Servo1_Trackbar.Value = 90;
            this.Servo1_Trackbar.ValueChanged += new System.EventHandler(this.Servo1_Trackbar_ValueChanged);
            // 
            // Servo2_Trackbar
            // 
            this.Servo2_Trackbar.BackColor = System.Drawing.SystemColors.Window;
            this.Servo2_Trackbar.Location = new System.Drawing.Point(7, 225);
            this.Servo2_Trackbar.Maximum = 160;
            this.Servo2_Trackbar.Minimum = 20;
            this.Servo2_Trackbar.Name = "Servo2_Trackbar";
            this.Servo2_Trackbar.Size = new System.Drawing.Size(265, 45);
            this.Servo2_Trackbar.TabIndex = 1;
            this.Servo2_Trackbar.Tag = "";
            this.Servo2_Trackbar.TickFrequency = 5;
            this.Servo2_Trackbar.Value = 90;
            this.Servo2_Trackbar.ValueChanged += new System.EventHandler(this.Servo2_Trackbar_ValueChanged);
            // 
            // Controller_Checkbox
            // 
            this.Controller_Checkbox.AutoSize = true;
            this.Controller_Checkbox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Controller_Checkbox.Location = new System.Drawing.Point(644, 1);
            this.Controller_Checkbox.Name = "Controller_Checkbox";
            this.Controller_Checkbox.Size = new System.Drawing.Size(145, 23);
            this.Controller_Checkbox.TabIndex = 2;
            this.Controller_Checkbox.Text = "Joystick Mode";
            this.Controller_Checkbox.UseVisualStyleBackColor = true;
            this.Controller_Checkbox.CheckedChanged += new System.EventHandler(this.Controller_Checkbox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Servo 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Servo 2";
            // 
            // Servo3_Trackbar
            // 
            this.Servo3_Trackbar.BackColor = System.Drawing.SystemColors.Window;
            this.Servo3_Trackbar.Location = new System.Drawing.Point(7, 301);
            this.Servo3_Trackbar.Maximum = 160;
            this.Servo3_Trackbar.Minimum = 42;
            this.Servo3_Trackbar.Name = "Servo3_Trackbar";
            this.Servo3_Trackbar.Size = new System.Drawing.Size(265, 45);
            this.Servo3_Trackbar.TabIndex = 1;
            this.Servo3_Trackbar.Tag = "";
            this.Servo3_Trackbar.TickFrequency = 5;
            this.Servo3_Trackbar.Value = 90;
            this.Servo3_Trackbar.ValueChanged += new System.EventHandler(this.Servo3_TrackBar_ValueChanged);
            // 
            // Servo4_Trackbar
            // 
            this.Servo4_Trackbar.BackColor = System.Drawing.SystemColors.Window;
            this.Servo4_Trackbar.Location = new System.Drawing.Point(7, 375);
            this.Servo4_Trackbar.Maximum = 90;
            this.Servo4_Trackbar.Minimum = 30;
            this.Servo4_Trackbar.Name = "Servo4_Trackbar";
            this.Servo4_Trackbar.Size = new System.Drawing.Size(265, 45);
            this.Servo4_Trackbar.TabIndex = 1;
            this.Servo4_Trackbar.Tag = "";
            this.Servo4_Trackbar.TickFrequency = 5;
            this.Servo4_Trackbar.Value = 80;
            this.Servo4_Trackbar.ValueChanged += new System.EventHandler(this.Servo4_Trackbar_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 276);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Servo 3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 350);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "Servo 4";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(343, 154);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            69,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(52, 26);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(343, 225);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            69,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(52, 26);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.Value = new decimal(new int[] {
            26,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(343, 300);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            69,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(52, 26);
            this.numericUpDown3.TabIndex = 4;
            this.numericUpDown3.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(343, 375);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            69,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(52, 26);
            this.numericUpDown4.TabIndex = 4;
            this.numericUpDown4.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // JoyStickTimer
            // 
            this.JoyStickTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(189, 19);
            this.label5.TabIndex = 3;
            this.label5.Text = "Joystick Sensitivity";
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.SystemColors.Window;
            this.trackBar1.Location = new System.Drawing.Point(7, 81);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 20;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(265, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Tag = "";
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Value = 60;
            this.trackBar1.ValueChanged += new System.EventHandler(this.Servo4_Trackbar_ValueChanged);
            // 
            // auto_checkBox
            // 
            this.auto_checkBox.AutoSize = true;
            this.auto_checkBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.auto_checkBox.Location = new System.Drawing.Point(454, 1);
            this.auto_checkBox.Name = "auto_checkBox";
            this.auto_checkBox.Size = new System.Drawing.Size(163, 23);
            this.auto_checkBox.TabIndex = 2;
            this.auto_checkBox.Text = "Autonomous Mode";
            this.auto_checkBox.UseVisualStyleBackColor = true;
            this.auto_checkBox.CheckedChanged += new System.EventHandler(this.auto_checkBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 19);
            this.label6.TabIndex = 3;
            this.label6.Text = "Camera 1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Video);
            this.tabControl1.Controls.Add(this.Speech);
            this.tabControl1.Controls.Add(this.Servo_Joystick);
            this.tabControl1.Controls.Add(this.Navigation);
            this.tabControl1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(1, -2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(981, 540);
            this.tabControl1.TabIndex = 7;
            // 
            // Video
            // 
            this.Video.BackColor = System.Drawing.Color.Gray;
            this.Video.Controls.Add(this.imageBox1);
            this.Video.Controls.Add(this.label6);
            this.Video.Location = new System.Drawing.Point(4, 28);
            this.Video.Name = "Video";
            this.Video.Padding = new System.Windows.Forms.Padding(3);
            this.Video.Size = new System.Drawing.Size(973, 508);
            this.Video.TabIndex = 1;
            this.Video.Text = "Video";
            // 
            // Speech
            // 
            this.Speech.BackColor = System.Drawing.Color.Gray;
            this.Speech.Controls.Add(this.pictureBox3);
            this.Speech.Location = new System.Drawing.Point(4, 28);
            this.Speech.Name = "Speech";
            this.Speech.Padding = new System.Windows.Forms.Padding(3);
            this.Speech.Size = new System.Drawing.Size(973, 508);
            this.Speech.TabIndex = 2;
            this.Speech.Text = "Speech";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::Robot_Arm.GUI.Properties.Resources.MeteorMic_xlarge;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(659, 68);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(273, 375);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // Servo_Joystick
            // 
            this.Servo_Joystick.BackColor = System.Drawing.Color.Gray;
            this.Servo_Joystick.Controls.Add(this.yTrackbar);
            this.Servo_Joystick.Controls.Add(this.xTrackbar);
            this.Servo_Joystick.Controls.Add(this.pictureBox2);
            this.Servo_Joystick.Controls.Add(this.label5);
            this.Servo_Joystick.Controls.Add(this.numericUpDown4);
            this.Servo_Joystick.Controls.Add(this.Servo1_Trackbar);
            this.Servo_Joystick.Controls.Add(this.numericUpDown3);
            this.Servo_Joystick.Controls.Add(this.Servo2_Trackbar);
            this.Servo_Joystick.Controls.Add(this.numericUpDown2);
            this.Servo_Joystick.Controls.Add(this.Servo3_Trackbar);
            this.Servo_Joystick.Controls.Add(this.numericUpDown1);
            this.Servo_Joystick.Controls.Add(this.Servo4_Trackbar);
            this.Servo_Joystick.Controls.Add(this.label4);
            this.Servo_Joystick.Controls.Add(this.trackBar1);
            this.Servo_Joystick.Controls.Add(this.label3);
            this.Servo_Joystick.Controls.Add(this.label10);
            this.Servo_Joystick.Controls.Add(this.label9);
            this.Servo_Joystick.Controls.Add(this.label8);
            this.Servo_Joystick.Controls.Add(this.label7);
            this.Servo_Joystick.Controls.Add(this.label1);
            this.Servo_Joystick.Controls.Add(this.label2);
            this.Servo_Joystick.Location = new System.Drawing.Point(4, 28);
            this.Servo_Joystick.Name = "Servo_Joystick";
            this.Servo_Joystick.Padding = new System.Windows.Forms.Padding(3);
            this.Servo_Joystick.Size = new System.Drawing.Size(973, 508);
            this.Servo_Joystick.TabIndex = 0;
            this.Servo_Joystick.Text = "Servos & Joystick";
            // 
            // yTrackbar
            // 
            this.yTrackbar.Location = new System.Drawing.Point(449, 81);
            this.yTrackbar.Maximum = 60;
            this.yTrackbar.Minimum = -40;
            this.yTrackbar.Name = "yTrackbar";
            this.yTrackbar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.yTrackbar.Size = new System.Drawing.Size(45, 339);
            this.yTrackbar.TabIndex = 8;
            this.yTrackbar.Value = 20;
            this.yTrackbar.ValueChanged += new System.EventHandler(this.yTrackbar_ValueChanged);
            // 
            // xTrackbar
            // 
            this.xTrackbar.Location = new System.Drawing.Point(507, 442);
            this.xTrackbar.Maximum = 100;
            this.xTrackbar.Minimum = 40;
            this.xTrackbar.Name = "xTrackbar";
            this.xTrackbar.Size = new System.Drawing.Size(449, 45);
            this.xTrackbar.TabIndex = 7;
            this.xTrackbar.Value = 60;
            this.xTrackbar.ValueChanged += new System.EventHandler(this.xTrackbar_ValueChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::Robot_Arm.GUI.Properties.Resources.Controller_Modified;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(507, 81);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(449, 339);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(283, 377);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 19);
            this.label10.TabIndex = 3;
            this.label10.Text = "Pin #";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(283, 302);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 19);
            this.label9.TabIndex = 3;
            this.label9.Text = "Pin #";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(283, 227);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 19);
            this.label8.TabIndex = 3;
            this.label8.Text = "Pin #";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(283, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 19);
            this.label7.TabIndex = 3;
            this.label7.Text = "Pin #";
            // 
            // Navigation
            // 
            this.Navigation.BackColor = System.Drawing.Color.Gray;
            this.Navigation.Controls.Add(this.clearButton);
            this.Navigation.Controls.Add(this.stopButton);
            this.Navigation.Controls.Add(this.startButton);
            this.Navigation.Controls.Add(this.chart1);
            this.Navigation.Location = new System.Drawing.Point(4, 28);
            this.Navigation.Name = "Navigation";
            this.Navigation.Padding = new System.Windows.Forms.Padding(3);
            this.Navigation.Size = new System.Drawing.Size(973, 508);
            this.Navigation.TabIndex = 3;
            this.Navigation.Text = "Navigation";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(487, 120);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 29);
            this.clearButton.TabIndex = 1;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(487, 67);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 26);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(487, 16);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 34);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // chart1
            // 
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(23, 16);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(412, 382);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // AD_Timer
            // 
            this.AD_Timer.Interval = 200;
            this.AD_Timer.Tick += new System.EventHandler(this.AD_Timer_Tick);
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(22, 67);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(425, 412);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(984, 538);
            this.Controls.Add(this.auto_checkBox);
            this.Controls.Add(this.Controller_Checkbox);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainGUI";
            this.Text = "Arduino Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Servo1_Trackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo2_Trackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo3_Trackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo4_Trackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.Video.ResumeLayout(false);
            this.Video.PerformLayout();
            this.Speech.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.Servo_Joystick.ResumeLayout(false);
            this.Servo_Joystick.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.Navigation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar Servo1_Trackbar;
        private System.Windows.Forms.TrackBar Servo2_Trackbar;
        private System.Windows.Forms.CheckBox Controller_Checkbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar Servo3_Trackbar;
        private System.Windows.Forms.TrackBar Servo4_Trackbar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Timer JoyStickTimer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.CheckBox auto_checkBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Servo_Joystick;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TabPage Video;
        private System.Windows.Forms.TabPage Speech;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage Navigation;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Timer AD_Timer;
        private System.Windows.Forms.TrackBar yTrackbar;
        private System.Windows.Forms.TrackBar xTrackbar;
        private Emgu.CV.UI.ImageBox imageBox1;
    }
}

