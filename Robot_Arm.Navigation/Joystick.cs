using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoClass;
using System.Timers;
using SharpDX.XInput; // api for the xbox controller

namespace Robot_Arm.Navigation
{
    public class Joystick
    {
        private ServoController[] myServos;
        private Timer myTimer;
        private Controller myController;
        private int oldPacketNumber;
        private double[] controllerValues;
        private uint[] ServoMappings; // maps a servo to a stick
        public Joystick(
            ServoController[] myServos,
            double Fs // sampling frequency of the timer
            ) 
        {
            this.myServos = myServos;
            controllerValues = new double[6];
            ServoMappings = new uint[myServos.Length];
            myController = new Controller(UserIndex.One);
            oldPacketNumber = 0;
            myTimer = new Timer(1000.0 / Fs);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(updatePosition);
        }
        public void updatePosition(object sender, ElapsedEventArgs e)
        {
            State myState = myController.GetState();
            if (myState.PacketNumber != oldPacketNumber) 
            {
                controllerValues[0] = (double) myState.Gamepad.LeftThumbX;
                controllerValues[1] = (double) myState.Gamepad.RightThumbX;
                controllerValues[2] = (double) myState.Gamepad.LeftThumbY;
                controllerValues[3] = (double) myState.Gamepad.RightThumbY;
                controllerValues[4] = (double) myState.Gamepad.LeftTrigger;
                controllerValues[5] = (double) myState.Gamepad.RightTrigger;
                for (int i = 0; i < myServos.Length; i++)
                {
                    myServos[i].updateServoAngle(controllerValues);
                }
            }
            oldPacketNumber = myState.PacketNumber;
        }
        public void Start() {
            myTimer.Start();
        }
        public void Stop() {
            myTimer.Stop();
        }
    }
}
