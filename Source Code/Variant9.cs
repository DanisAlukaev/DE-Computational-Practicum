using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ComputationalPracticum
{
    /* Class Variant9.
     * Used to represent ordinary differential equation y' = f(x, y) = 4 / x^2 - y / x - y^2 ,
     * given in Variant 9 of Computational Practicum.
     * Inherited from class DifferentialEquation. Implements Derivative() and ComputeValues() method.
     */
    class Variant9 : DifferentialEquation
    {
        /* Constructor of a class Variant9.
         * Calls DifferentialEquation's constructor.
         */
        public Variant9(double x0, double y0, double X, int N) : base(x0, y0, X, N) { }

        /* Method implementing function of slope of a tangential line f(x, y) = 4 / x^2 - y / x - y^2. 
         * Returns slope at a given point (x, y).
         */
        public override double Derivative(double x, double y) => 4 / Math.Pow(x, 2) - y / x - Math.Pow(y, 2);

        // Compute constant based on initial values.
        private double Constant => (-2 - (GetY0() * GetX0())) / (GetY0() * Math.Pow(GetX0(), 5) - 2 * Math.Pow(GetX0(), 4));

        /* Method for calculating the solution values.
         * Returns array of y-coordinates of an exact solution.
         * Exact solution for an ode given in Variant 9 defined as:
         * Const = ( -2 - y0 * x0 ) / ( y0 * x0^5 - 2 * x0 ^ 4 )
         * y = ( 2 * ( Const * x^4 - 1 ) ) / ( x * ( Const * x^4 + 1 ) ).
         */
        public override double[] ComputeValues() 
        {
            // Get x-coordinates.
            double[] x = GetX();
            // Initialize an array to store intermediate values of y-coordinates.
            double[] y = new double[GetN()];
            y[0] = GetY0();
            // Determine the value of a constant.
            double c = Constant;
            for (int i = 1; i < GetN(); i++)
                // Compute exact value of a solution.
                y[i] = (2 * (c * Math.Pow(x[i], 4) - 1)) / (x[i] * (c * Math.Pow(x[i], 4) + 1));
            // Set values of y-coordinates.
            SetY(y);
            return y;
        }

        /* Checks whether solution of First order differential equation  y' = f(x, y) = 4 / x^2 - y / x - y^2
         * is continuous on specified interval. There are 3 possible points x = 0 and for positive constant ± ( -1 / C )^(1/4).
         * See report in order to find more details.
         * Returns empty array if it is continuous, otherwise - array of points that belong to interval.
         */
        public override double[] IsContinuousOnInterval()
        {
            // Get boundaries of the interval.
            double x0 = GetX0();
            double X = GetXMax();
            // Create a list to store points that are in interval.
            List<double> Failed = new List<double>();
            // As shown in report there are 3 possible points of discontinuity.
            double Point1, Point2, Point3;
            // First is at x = 0.
            // Check it.
            Point1 = 0;
            if (x0 <= Point1 && Point1 <= X)
                Failed.Add(Point1);
            // Get the constant.
            double c = Constant;
            if (c < 0)
            {
                // For constants that are greater than 0, there exist points ± ( -1 / C )^(1/4).
                // Check them.
                Point2 = Math.Pow(-1 / c, 1 / 4);
                if (x0 <= Point2 && Point2 <= X)
                    Failed.Add(Point2);
                Point3 = -Math.Pow(-1 / c, 1 / 4);
                if (x0 <= Point3 && Point3 <= X)
                    Failed.Add(Point3);
            }
            return Failed.ToArray();
        }

    }
}
