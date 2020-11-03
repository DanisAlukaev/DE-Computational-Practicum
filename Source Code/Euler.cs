using System;

namespace ComputationalPracticum
{
    /* Class Euler.
     * Used to represent Euler Method for solving ordinary differential equations with a given initial value.
     * Inherited from class NumericalMethod. Implements ComputeForPoint() method.
     * Idea behind: https://en.wikipedia.org/wiki/Euler_method
     */
    public class Euler : NumericalMethod
    {
        /* Constructor of a class Euler.
         * Calls NumericalMethod's constructor.
         */
        public Euler(double x0, double y0, double X, int N) : base(x0, y0, X, N) { }

        /* Method calculating point-wise approximation. Implementation of the Euler Method.
         * Returns an approximation at a point with given index i.
         */
        public override double ComputeForPoint(double[] x, double[] y, int i, DifferentialEquation MyDifferentialEquation) => y[i] + (GetStep() * MyDifferentialEquation.Derivative(x[i], y[i]));
    }
}
