namespace ComputationalPracticum
{
    /* Class ImprovedEuler.
     * Used to represent Improved Euler Method for solving ordinary differential equations with a given initial value.
     * Inherited from class NumericalMethod. Implements ComputeForPoint() method.
     * Idea behind: https://en.wikipedia.org/wiki/Heun%27s_method
     */
    public class ImprovedEuler : NumericalMethod
    {
        /* Constructor of a class ImprovedEuler.
         * Calls NumericalMethod's constructor.
         */
        public ImprovedEuler(double x0, double y0, double X, int N) : base(x0, y0, X, N) { }

        /* Method calculating point-wise approximation. Implementation of the Improved Euler Method.
         * Returns an approximation at a point with given index i.
         */
        public override double ComputeForPoint(double[] x, double[] y, int i, DifferentialEquation MyDifferentialEquation)
        {
            double h = GetStep();
            return y[i] + (h / 2 * (MyDifferentialEquation.Derivative(x[i], y[i]) +
                MyDifferentialEquation.Derivative(x[i + 1], y[i] + h * MyDifferentialEquation.Derivative(x[i], y[i]))));
        }

    }
}
