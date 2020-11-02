using System;

namespace ComputationalPracticum
{
    /* Abstract Class NumericalMethod.
     * Used to represent Numerical Methods for solving ordinary differential equations with a given initial value.
     * Inherited from class Grid.
     */
    abstract class NumericalMethod : Grid
    {
        /* Constructor of an abstract class NumericalMethod.
         * Calls Grid's constructor.
         */
        public NumericalMethod(double x0, double y0, double X, int N) : base(x0, y0, X, N) { }

        /* Method solving given ordinary differential equation MyDifferentialEquation. 
         * Returns y-coordinates of an solution approximation.
         */
        public double[] Apply(DifferentialEquation MyDifferentialEquation)
        {
            // Get number of grid steps.
            int N = GetN();
            // Get values of x-coordinates.
            double[] x = GetX();
            // Get values of y-coordinates.
            double[] y = GetY();
            // Iteratively compute an approximation for each grid step.
            for (int i = 1; i < N; i++)
            {
                y[i] = ComputeForPoint(x, y, i - 1, MyDifferentialEquation);
                // Debug information.
                // System.Diagnostics.Debug.WriteLine("y[i]={0:F} | x[i - 1]={1:F} y[i - 1]={2:F} h={3:F} Derivative={4:f}", y[i], x[i - 1], y[i - 1], GetStep(), MyDifferentialEquation.Derivative(x[i - 1], y[i - 1]));
            }
            // Set y-coordinates.
            SetY(y);
            return y;
        }

        /* Default implementation of a method calculating point-wise approximation. 
         * Unique for each Numerical method.
         * Returns approximation at a point with given index i.
         */
        public virtual double ComputeForPoint(double[] x, double[] y, int i, DifferentialEquation MyDifferentialEquation) => 0;

        /* Method computing Local Truncation Errors of Numerical Method.
         * Apply() should be called before utilizing this method.
         * Returns array of LTE's at every point of grid.  
         */
        public double[] ComputeLocalTruncationErrors(DifferentialEquation MyDifferentialEquation)
        {
            // Get values of x-coordinates.
            double[] x = GetX();
            // Get exact values of y-coordinates.
            double[] y = MyDifferentialEquation.GetY();
            // Initialize an array to store Local Truncation errors.
            double[] LTE = new double[GetN()];
            // Local Truncation error for a first point is 0 as it given by IVP.
            LTE[0] = 0;
            // Iteratively compute Local Truncation error for each grid step.
            for (int i = 1; i < GetN(); i++)
                LTE[i] = Math.Abs(ComputeForPoint(x, y, i - 1, MyDifferentialEquation) - y[i]);
            return LTE;
        }

        /* Method computing Global Truncation Errors of Numerical Method.
         * Apply() should be called before utilizing this method.
         * Returns array of GTE's at every point of grid.  
         */
        public double[] ComputeGlobalTruncationErrors(DifferentialEquation MyDifferentialEquation)
        {
            // Get exact values of y-coordinates.
            double[] y = GetY();
            // Initialize an array to store Global Truncation errors.
            double[] GTE = new double[GetN()];
            // Iteratively compute Global Truncation error for each grid step.
            for (int i = 0; i < GetN(); i++)
                GTE[i] = Math.Abs(y[i] - MyDifferentialEquation.GetY()[i]);
            return GTE;
        }
    }
}
