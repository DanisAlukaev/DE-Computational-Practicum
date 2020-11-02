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
            double c = (-2 - y[0] * x[0]) / (y[0] * Math.Pow(x[0], 5) - 2 * Math.Pow(x[0], 4));
            for (int i = 1; i < GetN(); i++)
                // Compute exact value of a solution.
                y[i] = (2 * (c * Math.Pow(x[i], 4) - 1)) / (x[i] * (c * Math.Pow(x[i], 4) + 1));
            // Set values of y-coordinates.
            SetY(y);
            return y;
        }

    }
}
