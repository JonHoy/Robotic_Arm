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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.start_button = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.servo1_Label = new System.Windows.Forms.Label();
            this.servo2_Label = new System.Windows.Forms.Label();
            this.servo3_Label = new System.Windows.Forms.Label();
            this.servo4_Label = new System.Windows.Forms.Label();
            this.sensor1_Label = new System.Windows.Forms.Label();
            this.sensor2_Label = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(100, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1024, 550);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // start_button
            // 
            this.start_button.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_button.Location = new System.Drawing.Point(6, 12);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(82, 29);
            this.start_button.TabIndex = 1;
            this.start_button.Text = "Start";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Color Scan";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // servo1_Label
            // 
            this.servo1_Label.AutoSize = true;
            this.servo1_Label.BackColor = System.Drawing.SystemColors.Control;
            this.servo1_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servo1_Label.Location = new System.Drawing.Point(106, 19);
            this.servo1_Label.Name = "servo1_Label";
            this.servo1_Label.Size = new System.Drawing.Size(123, 21);
            this.servo1_Label.TabIndex = 3;
            this.servo1_Label.Text = "Servo 1 (X-Axis):";
            // 
            // servo2_Label
            // 
            this.servo2_Label.AutoSize = true;
            this.servo2_Label.BackColor = System.Drawing.Color.Transparent;
            this.servo2_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servo2_Label.Location = new System.Drawing.Point(106, 71);
            this.servo2_Label.Name = "servo2_Label";
            this.servo2_Label.Size = new System.Drawing.Size(123, 21);
            this.servo2_Label.TabIndex = 3;
            this.servo2_Label.Text = "Servo 2 (Y-Axis):";
            // 
            // servo3_Label
            // 
            this.servo3_Label.AutoSize = true;
            this.servo3_Label.BackColor = System.Drawing.Color.Transparent;
            this.servo3_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servo3_Label.Location = new System.Drawing.Point(106, 123);
            this.servo3_Label.Name = "servo3_Label";
            this.servo3_Label.Size = new System.Drawing.Size(123, 21);
            this.servo3_Label.TabIndex = 3;
            this.servo3_Label.Text = "Servo 3 (Y-Axis):";
            // 
            // servo4_Label
            // 
            this.servo4_Label.AutoSize = true;
            this.servo4_Label.BackColor = System.Drawing.Color.Transparent;
            this.servo4_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servo4_Label.Location = new System.Drawing.Point(106, 170);
            this.servo4_Label.Name = "servo4_Label";
            this.servo4_Label.Size = new System.Drawing.Size(133, 21);
            this.servo4_Label.TabIndex = 3;
            this.servo4_Label.Text = "Servo 4 (Gripper):";
            // 
            // sensor1_Label
            // 
            this.sensor1_Label.AutoSize = true;
            this.sensor1_Label.BackColor = System.Drawing.Color.Transparent;
            this.sensor1_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensor1_Label.Location = new System.Drawing.Point(106, 222);
            this.sensor1_Label.Name = "sensor1_Label";
            this.sensor1_Label.Size = new System.Drawing.Size(126, 21);
            this.sensor1_Label.TabIndex = 3;
            this.sensor1_Label.Text = "Sensor 1 (Force):";
            // 
            // sensor2_Label
            // 
            this.sensor2_Label.AutoSize = true;
            this.sensor2_Label.BackColor = System.Drawing.Color.Transparent;
            this.sensor2_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensor2_Label.Location = new System.Drawing.Point(106, 243);
            this.sensor2_Label.Name = "sensor2_Label";
            this.sensor2_Label.Size = new System.Drawing.Size(147, 21);
            this.sensor2_Label.TabIndex = 3;
            this.sensor2_Label.Text = "Sensor 2 (Distance):";
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(110, 43);
            this.trackBar1.Maximum = 180;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(188, 25);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // trackBar2
            // 
            this.trackBar2.AutoSize = false;
            this.trackBar2.Location = new System.Drawing.Point(110, 95);
            this.trackBar2.Maximum = 180;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(188, 25);
            this.trackBar2.TabIndex = 4;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar2.ValueChanged += new System.EventHandler(this.trackBar2_ValueChanged);
            // 
            // trackBar3
            // 
            this.trackBar3.AutoSize = false;
            this.trackBar3.Location = new System.Drawing.Point(110, 147);
            this.trackBar3.Maximum = 180;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(188, 25);
            this.trackBar3.TabIndex = 4;
            this.trackBar3.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar3.ValueChanged += new System.EventHandler(this.trackBar3_ValueChanged);
            // 
            // trackBar4
            // 
            this.trackBar4.AutoSize = false;
            this.trackBar4.Location = new System.Drawing.Point(110, 194);
            this.trackBar4.Maximum = 180;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new System.Drawing.Size(188, 25);
            this.trackBar4.TabIndex = 4;
            this.trackBar4.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar4.ValueChanged += new System.EventHandler(this.trackBar4_ValueChanged);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(110, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(143, 30);
            this.button2.TabIndex = 5;
            this.button2.Text = "Recognize Speech";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1136, 574);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.trackBar4);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.sensor2_Label);
            this.Controls.Add(this.sensor1_Label);
            this.Controls.Add(this.servo4_Label);
            this.Controls.Add(this.servo3_Label);
            this.Controls.Add(this.servo2_Label);
            this.Controls.Add(this.servo1_Label);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.start_button);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.Name = "MainGUI";
            this.ShowIcon = false;
            this.Text = "Robot Arm HUD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainGUI_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label servo1_Label;
        private System.Windows.Forms.Label servo2_Label;
        private System.Windows.Forms.Label servo3_Label;
        private System.Windows.Forms.Label servo4_Label;
        private System.Windows.Forms.Label sensor1_Label;
        private System.Windows.Forms.Label sensor2_Label;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.TrackBar trackBar4;
        private System.Windows.Forms.Button button2;
    }
}

