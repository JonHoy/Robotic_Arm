using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoClass;

namespace Robot_Arm.Navigation
{
    class SharpIR : Sensor // Class for Sharp IR distance sensors
    {
        private double voltageMultiplier = 11.58;
        private double voltagePower = -1.058;
        private double maxDistance = 40;
        private double cm_to_inch = .394;

        public SharpIR(Arduino Board, int Pin) : base(Board, Pin) { }

        public double getDistance() // return distance reading in inches
        {
            var voltage = getSensorReading();
            var distance = voltageMultiplier * Math.Pow(voltage, voltagePower);
            distance = cm_to_inch * Math.Min(distance, maxDistance);
            return distance;
        }
    }
}
