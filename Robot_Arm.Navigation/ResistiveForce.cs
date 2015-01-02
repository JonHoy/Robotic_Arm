using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoClass;

namespace Robot_Arm.Navigation
{
    public class ResistiveForce : Sensor // Class for resistive force sensors in a voltage divider circuit
    {
        private double rConst = 10000; // resistance of the constant value resistor in the voltage divider circuit 
        private double voltageMultiplier = 2.979e11; // constants based on curve fit
        private double voltagePower = -1.388;

        public ResistiveForce(Arduino Board, int Pin) : base (Board, Pin) { }

        public double getForce() // gets the force reading in grams
        {
            var voltage = getSensorReading();
            var v_Ratio = (voltage - minVoltage) / (maxVoltage - minVoltage);
            var r_Sensor = rConst / v_Ratio - rConst;
            var Force = voltageMultiplier * Math.Pow(r_Sensor, voltagePower);
            return Force;
        }
    }
}
