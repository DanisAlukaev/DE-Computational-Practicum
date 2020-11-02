using System;

namespace ComputationalPracticum
{
    /* Class ValuesHandler.
     * Used to check whether the sets of x and y coordinates contain values that System.Windows.Forms.DataVisualization.Charting is not able to work with.
     */
    class ValuesHandler
    {
        // Array to store x-coordinates of points.
        private readonly double[] x;
        // Array to store y-coordinates of points.
        private readonly double[] y;
        // Number of grid steps. 
        private readonly int N;
        // Absolute value of a number that System.Windows.Forms.DataVisualization.Charting is able to plot.
        private const double AbsMax = 1e20;

        /* Constructor of a class ValuesHandler.
         * Gets arrays of coordinates and number of grid steps.
         */
        public ValuesHandler(Grid grid)
        {
            x = grid.GetX();
            y = grid.GetY();
            N = grid.GetN();
        }

        // Method returns true if data in grid can be plotted, false - otherwise.
        public bool CheckPossibilityPlot() => CheckArray(x) & CheckArray(y);

        // Method returns false if array contains values that cannot be ploted, true - otherwise.
        private bool CheckArray(double[] array)
        {
            // Iterate through received array.
            for (int i = 0; i < N; i++)
                // Check whether the element is NaN, ±Infinity, or exceeds maximum possible number in absolute value. 
                if (Double.IsNaN(array[i]) || Double.IsInfinity(array[i]) || Math.Abs(array[i]) >= AbsMax)
                    return false;
            return true;
        }
    }
}
