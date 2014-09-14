namespace Robot_Arm
{
    partial class Form1
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Video = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Servo1_Trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo2_Trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo3_Trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Servo4_Trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.Video.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // Servo1_Trackbar
            // 
            this.Servo1_Trackbar.BackColor = System.Drawing.SystemColors.Window;
            this.Servo1_Trackbar.Location = new System.Drawing.Point(30, 152);
            this.Servo1_Trackbar.Maximum = 180;
            this.Servo1_Trackbar.Name = "Servo1_Trackbar";
            this.Servo1_Trackbar.Size = new System.Drawing.Size(302, 45);
            this.Servo1_Trackbar.TabIndex = 0;
            this.Servo1_Trackbar.Tag = "";
            this.Servo1_Trackbar.TickFrequency = 5;
            this.Servo1_Trackbar.Value = 90;
            this.Servo1_Trackbar.ValueChanged += new System.EventHandler(this.Servo1_Trackbar_ValueChanged);
            // 
            // Servo2_Trackbar
            // 
            this.Servo2_Trackbar.BackColor = System.Drawing.SystemColors.Window;
            this.Servo2_Trackbar.Location = new System.Drawing.Point(30, 223);
            this.Servo2_Trackbar.Maximum = 160;
            this.Servo2_Trackbar.Minimum = 20;
            this.Servo2_Trackbar.Name = "Servo2_Trackbar";
            this.Servo2_Trackbar.Size = new System.Drawing.Size(302, 45);
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
            this.Controller_Checkbox.Location = new System.Drawing.Point(561, 1);
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
            this.label1.Location = new System.Drawing.Point(27, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Servo 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Servo 2";
            // 
            // Servo3_Trackbar
            // 
            this.Servo3_Trackbar.BackColor = System.Drawing.SystemColors.Window;
            this.Servo3_Trackbar.Location = new System.Drawing.Point(29, 299);
            this.Servo3_Trackbar.Maximum = 160;
            this.Servo3_Trackbar.Minimum = 42;
            this.Servo3_Trackbar.Name = "Servo3_Trackbar";
            this.Servo3_Trackbar.Size = new System.Drawing.Size(303, 45);
            this.Servo3_Trackbar.TabIndex = 1;
            this.Servo3_Trackbar.Tag = "";
            this.Servo3_Trackbar.TickFrequency = 5;
            this.Servo3_Trackbar.Value = 90;
            this.Servo3_Trackbar.ValueChanged += new System.EventHandler(this.Servo3_TrackBar_ValueChanged);
            // 
            // Servo4_Trackbar
            // 
            this.Servo4_Trackbar.BackColor = System.Drawing.SystemColors.Window;
            this.Servo4_Trackbar.Location = new System.Drawing.Point(29, 373);
            this.Servo4_Trackbar.Maximum = 90;
            this.Servo4_Trackbar.Minimum = 30;
            this.Servo4_Trackbar.Name = "Servo4_Trackbar";
            this.Servo4_Trackbar.Size = new System.Drawing.Size(303, 45);
            this.Servo4_Trackbar.TabIndex = 1;
            this.Servo4_Trackbar.Tag = "";
            this.Servo4_Trackbar.TickFrequency = 5;
            this.Servo4_Trackbar.Value = 70;
            this.Servo4_Trackbar.ValueChanged += new System.EventHandler(this.Servo4_Trackbar_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 277);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Servo 3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(27, 351);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "Servo 4";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(280, 132);
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
            this.numericUpDown2.Location = new System.Drawing.Point(280, 203);
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
            this.numericUpDown3.Location = new System.Drawing.Point(280, 279);
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
            this.numericUpDown4.Location = new System.Drawing.Point(280, 353);
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
            this.label5.Location = new System.Drawing.Point(28, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 19);
            this.label5.TabIndex = 3;
            this.label5.Text = "Stick Sensitivity";
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.SystemColors.Window;
            this.trackBar1.Location = new System.Drawing.Point(29, 79);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 20;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(303, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Tag = "";
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Value = 60;
            this.trackBar1.ValueChanged += new System.EventHandler(this.Servo4_Trackbar_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(371, 1);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(163, 23);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Autonomous Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.Controller_Checkbox_CheckedChanged);
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.BackColor = System.Drawing.SystemColors.Window;
            this.videoSourcePlayer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.videoSourcePlayer1.Location = new System.Drawing.Point(22, 81);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(430, 365);
            this.videoSourcePlayer1.TabIndex = 5;
            this.videoSourcePlayer1.Text = "videoSourcePlayer1";
            this.videoSourcePlayer1.VideoSource = null;
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
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(481, 80);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(430, 366);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Video);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(1, -2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(981, 540);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Gray;
            this.tabPage1.Controls.Add(this.pictureBox2);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.numericUpDown4);
            this.tabPage1.Controls.Add(this.Servo1_Trackbar);
            this.tabPage1.Controls.Add(this.numericUpDown3);
            this.tabPage1.Controls.Add(this.Servo2_Trackbar);
            this.tabPage1.Controls.Add(this.numericUpDown2);
            this.tabPage1.Controls.Add(this.Servo3_Trackbar);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.Servo4_Trackbar);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.trackBar1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(973, 508);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Servos & Joystick";
            // 
            // Video
            // 
            this.Video.BackColor = System.Drawing.Color.Gray;
            this.Video.Controls.Add(this.pictureBox1);
            this.Video.Controls.Add(this.videoSourcePlayer1);
            this.Video.Controls.Add(this.label6);
            this.Video.Location = new System.Drawing.Point(4, 28);
            this.Video.Name = "Video";
            this.Video.Padding = new System.Windows.Forms.Padding(3);
            this.Video.Size = new System.Drawing.Size(973, 508);
            this.Video.TabIndex = 1;
            this.Video.Text = "Video";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gray;
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(973, 508);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Speech";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::Robot_Arm.Properties.Resources.gm_xboxcw_wht_ci;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(366, 76);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(459, 342);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(984, 538);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.Controller_Checkbox);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.Video.ResumeLayout(false);
            this.Video.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
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
        private System.Windows.Forms.CheckBox checkBox1;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TabPage Video;
        private System.Windows.Forms.TabPage tabPage2;
    }
}

