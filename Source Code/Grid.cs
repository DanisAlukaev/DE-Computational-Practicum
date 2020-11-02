namespace ComputationalPracticum
{
    /* Class Grid.
     * Used to represent set of points on coordinate plane.
     */
    public class Grid
    {
        // A x-coordinate of leftmost grid point. 
        private readonly double x0;
        // A y-coordinate of leftmost grid point. 
        private readonly double y0;
        // A right boundary of x-coordinates interval. 
        private readonly double X;
        // Number of grid steps. 
        private readonly int N;
        // Array to store x-coordinates of points.
        private double[] x;
        // Array to store y-coordinates of points. 
        private double[] y;
        // Step size.
        private double h;

        /* Constructor of a class Grid.
         * Sets coordinates of the first point, right boundary of x-coordinates interval and number of grid steps. 
         */
        public Grid(double x0, double y0, double X, int N)
        {
            // Set initial conditions.
            this.x0 = x0;
            this.y0 = y0;
            this.X = X;
            // N in GUI stands for number of approximation steps.
            // Since the first point is not included in this number, add 1 to it.
            this.N = N + 1;
            // Initialize arrays of points' coordinates.
            InitializeDomain();
            InitializeCodomain();
        }

        // Method initializes set of x-coordinates based of step size h.
        private void InitializeDomain()
        {
            // Initialize array of x-coordinates.
            x = new double[N];
            // Compute step size.
            h = (X - x0) / (N - 1);
            // Filling the array with values.
            for (int i = 0; i < N; i++)
            {
                if (i == 0)
                    // Leftmost point.
                    x[i] = x0;
                else
                    // Increase previous x-coordinate by step size.
                    x[i] = x[i - 1] + h;
            }
        }

        // Method initializes set of y-coordinates.
        private void InitializeCodomain()
        {
            // Initialize array of y-coordinates.
            y = new double[N];
            // Set value for the leftmost point.
            y[0] = y0;
        }

        // Returns a x-coordinate of leftmost point. 
        public double GetX0() => x0;

        // Returns a y-coordinate of leftmost point.
        public double GetY0() => y0;

        // Returns a right boundary of x-coordinates interval. 
        public double GetXMax() => X;

        // Returns a number of grid steps. 
        public int GetN() => N;

        // Returns an array of points' x-coordinates.
        public double[] GetX() => x;

        // Returns an array of points' y-coordinates.
        public double[] GetY() => y;

        // Returns step size.
        public double GetStep() => h;

        // Sets values of y-coordinates.
        public void SetY(double[] Values) => y = Values;
    }
}
