using System;
using System.Linq;

namespace ComputationalPracticum
{
    /* Abstract Class DifferentialEquation.
     * Used to represent First order differential equation y' = f(x, y).
     * Inherited from class Grid.
     */
    public abstract class DifferentialEquation : Grid
    {
        /* Constructor of an abstract class DifferentialEquation.
         * Checks whether solution of Differential Equation is defined on x in grid.
         * Calls method for calculating solution values.
         */
        public DifferentialEquation(double x0, double y0, double X, int N) : base(x0, y0, X, N)
        {
            if (IsContinuousOnInterval().Length != 0)
            {
                // Create message to display.
                String Message = "Specified interval contains point(s) of discontinuity:";
                foreach(double Point in IsContinuousOnInterval())
                    Message += " " + Point.ToString();
                Message += ".";
                throw new Exception(Message);
            }
            ComputeValues();
        }

        /* Default implementation of a function of slope of a tangential line f(x, y). 
         * Returns slope at a given point (x, y).
         */
        public virtual double Derivative(double x, double y) => 0;

        /* Default implementation of a method for calculating the solution values.
         * Returns array of y-coordinates of an exact solution.
         */
        public virtual double[] ComputeValues() => new double[0];

        /* Default implementation of a method checking whether solution of First order differential equation y' = f(x, y)
         * is continuous on specified interval.
         * Returns empty array if it is continuous, otherwise - array of points that belong to interval.
         */
        public virtual double[] IsContinuousOnInterval() => new double[0];
    }
}
