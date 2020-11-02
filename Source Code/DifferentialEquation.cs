namespace ComputationalPracticum
{
    /* Abstract Class DifferentialEquation.
     * Used to represent First order differential equation y' = f(x, y).
     * Inherited from class Grid.
     */
    abstract class DifferentialEquation : Grid
    {
        /* Constructor of an abstract class DifferentialEquation.
         * Calls method for calculating solution values.
         */
        public DifferentialEquation(double x0, double y0, double X, int N) : base(x0, y0, X, N) => ComputeValues();

        /* Default implementation of a function of slope of a tangential line f(x, y). 
         * Returns slope at a given point (x, y).
         */
        public virtual double Derivative(double x, double y) => 0;

        /* Default implementation of a method for calculating the solution values.
         * Returns array of y-coordinates of an exact solution.
         */
        public virtual double[] ComputeValues() => new double[0];
    }
}
