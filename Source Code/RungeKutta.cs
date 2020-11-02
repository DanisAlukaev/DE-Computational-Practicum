using System;

namespace ComputationalPracticum
{
    /* Class RungeKutta.
     * Used to represent Runge-Kutta Method for solving ordinary differential equations with a given initial value.
     * Inherited from class NumericalMethod. Implements ComputeForPoint() method.
     * Idea behind: https://en.wikipedia.org/wiki/Runge–Kutta_methods
     */
    class RungeKutta : NumericalMethod
    {
        /* Constructor of a class RungeKutta.
         * Calls NumericalMethod's constructor.
         */
        public RungeKutta(double x0, double y0, double X, int N) : base(x0, y0, X, N) { }

        /* Method calculating point-wise approximation. Implementation of the Runge-Kutta Method.
         * Returns an approximation at a point with given index i.
         */
        public override double ComputeForPoint(double[] x, double[] y, int i, DifferentialEquation MyDifferentialEquation)
        {
            // Initialize temporary variables.
            double h = GetStep(), K1, K2, K3, K4;
            // By default use RK4.
            K1 = MyDifferentialEquation.Derivative(x[i], y[i]);
            K2 = MyDifferentialEquation.Derivative(x[i] + h / 2, y[i] + h / 2 * K1);
            K3 = MyDifferentialEquation.Derivative(x[i] + h / 2, y[i] + h / 2 * K2);
            K4 = MyDifferentialEquation.Derivative(x[i] + h, y[i] + h * K3);
            return y[i] + h / 6 * (K1 + 2 * K2 + 2 * K3 + K4);
        }
    }
}
