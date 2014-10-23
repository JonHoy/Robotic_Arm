using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Robot_Arm.Navigation
{
    public class AngleCalculator
    {
        private double L1 = 4.06;
        private double L2 = 5.7;
        private double[] xTableLookup; // lookup tables for x, y, theta1, and theta2
        private double[] yTableLookup;
        private double[] theta1TableLookup;
        private double[] theta2TableLookup;

        private double Tolerance = .15;

        public AngleCalculator(int theta1_Min = 0, int theta1_Max = 180, int theta2_Min = -135, int theta2_Max = 0)
        {
            int Rows, Cols;
            Rows = (int)(theta1_Max - theta1_Min + 1);
            Cols = (int)(theta2_Max - theta2_Min + 1);
            xTableLookup = new double[Rows * Cols];
            yTableLookup = new double[xTableLookup.Length];
            var xKeys = new int[xTableLookup.Length];
            var temptheta1Table = new double[Rows * Cols];
            var temptheta2Table = new double[Rows * Cols];

            Parallel.For(0, Rows, i =>
            {
                
                double theta1 = theta1_Min + (double) i;
                for (int j = 0; j < Cols; j++)
                {
                    double theta2 = theta2_Min + (double)j;
                    int CurrentIndex = i * Cols + j;
                    xKeys[CurrentIndex] = CurrentIndex;
                    temptheta1Table[CurrentIndex] = theta1;
                    temptheta2Table[CurrentIndex] = theta2;
                    yTableLookup[CurrentIndex] = L1 * Math.Sin(theta1 * Math.PI / 180) + L2 * Math.Sin((theta1 + theta2) * Math.PI / 180.0);
                    xTableLookup[CurrentIndex] = L1 * Math.Cos(theta1 * Math.PI / 180) + L2 * Math.Cos((theta1 + theta2) * Math.PI / 180.0);
                }
            });
            Array.Sort(xTableLookup, xKeys); // sort the xlookup table so we can do a binary search for fast calcs
            theta1TableLookup = new double[temptheta1Table.Length];
            theta2TableLookup = new double[temptheta2Table.Length];
            var tempyTableLookup = new double[yTableLookup.Length];
            for (int i = 0; i < Rows*Cols; i++)
            {
                int CurrentIndex = xKeys[i];
                theta1TableLookup[i] = temptheta1Table[CurrentIndex];
                theta2TableLookup[i] = temptheta2Table[CurrentIndex];
                tempyTableLookup[i] = yTableLookup[CurrentIndex];
            }
            yTableLookup = tempyTableLookup;

        }
        // Gets theta1 and theta2 given a target x and y value
        public void getTheta(double x_Target, double y_Target, out double theta1, out double theta2)
        {
            theta1 = double.NaN;
            theta2 = double.NaN;
            
            // do a binary search of the sorted list within the tolerance band
            // locate the start and endpt of the array subset within that tolerance

            int startPt = ~Array.BinarySearch(xTableLookup, x_Target - Tolerance);
            int endPt = ~Array.BinarySearch(xTableLookup, x_Target + Tolerance);

            // if criteria are not met then return NaNs for theta1 and theta2
            if (startPt >= xTableLookup.Length || endPt >= xTableLookup.Length)
            {
                return;
            }

            // within that band find the theta1 and theta2 value combination that is the shortest distance to target 

            double MinDistance = double.MaxValue;
            int MinIndex = 0;
            for (int i = startPt; i <= endPt; i++)
            {
                double LocalDistanceX = Math.Abs(xTableLookup[i] - x_Target);
                double LocalDistanceY = Math.Abs(yTableLookup[i] - y_Target);
                double TotalDistance = LocalDistanceX * LocalDistanceX + LocalDistanceY * LocalDistanceY;
                if (MinDistance > TotalDistance)
                {
                    MinDistance = TotalDistance;
                    MinIndex = i;
                }
            }
            theta1 = theta1TableLookup[MinIndex];
            theta2 = theta2TableLookup[MinIndex];



        }

        public void get_xy_Target(double targetDistance,double theta1_Current, double theta2_Current, out double x_Target, out double y_Target)
        {
            var x_Current = Math.Cos(theta1_Current) * L1 + Math.Cos(theta2_Current) * L2;
            var y_Current = Math.Sin(theta1_Current) * L1 + Math.Sin(theta2_Current) * L2;
            x_Target = x_Current + Math.Cos(theta2_Current) * targetDistance;
            y_Target = y_Current + Math.Sin(theta1_Current) * targetDistance;
        }

        public void getNewTheta(double targetDistance, double theta1_Current, double theta2_Current, out double theta1_New, out double theta2_New)
        { 
            double x_Target; double y_Target;
            get_xy_Target(targetDistance, theta1_Current, theta2_Current, out x_Target, out y_Target);
            getTheta(x_Target, y_Target, out theta1_New, out theta2_New);
        }

    }
}
