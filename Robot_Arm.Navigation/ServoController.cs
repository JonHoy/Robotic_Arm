using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoClass;

namespace Robot_Arm.Navigation
{
    public class ServoController // helper class
    {
        private Servo myServo; // servo that the control is mapped to
        private double Sensitivity; // sensitivity of the servo (how sensitive the servo is to controller) can be positive or negative
        private double DeadZone;
        private uint StickMapping;

        public ServoController(
            Servo myServo,
            double Sensitivity,
            double DeadZone,
            uint StickMapping
            )
        {
            this.myServo = myServo;
            this.Sensitivity = Sensitivity;
            this.DeadZone = DeadZone;
            this.StickMapping = StickMapping;
        }
        public void updateServoAngle(double StickValue)
        {
            if (StickValue > DeadZone)
            {
                int NewAngle = (int)((double)myServo.Angle + Sensitivity * StickValue);
                NewAngle = Math.Min(myServo.MaxAngle, NewAngle);
                NewAngle = Math.Max(myServo.MinAngle, NewAngle);
                if (NewAngle != myServo.Angle)
                {
                    myServo.ServoAngleChange(NewAngle);
                }
            }
        }
        public void updateServoAngle(double[] controllerValues)
        {
            updateServoAngle(controllerValues[StickMapping]);
        }
    }
}
