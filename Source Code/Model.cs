using System;
using System.Linq;

namespace ComputationalPracticum
{
    // Component that directly manage the data, logic and rules of the application.
    class Model
    {
        // A x-coordinate of leftmost grid point. 
        public double x0;
        // A y-coordinate of leftmost grid point. 
        public double y0;
        // A right boundary of x-coordinates interval. 
        public double X;
        // Number of grid steps. 
        public int N;
        // Number of grid steps on second tab. 
        public int Tab2N;
        // Maximum number of grid steps. 
        public int NMax;

        // Ordinary differential equation Variant 9 for computaions.
        public Variant9 MyDifferentialEquation;
        // Ordinary differential equation Variant 9 for ploting.
        public Variant9 DisplayDE;

        // Numerical methods.
        public Euler EulerApproximation;
        public ImprovedEuler ImprovedEulerApproximation;
        public RungeKutta RungeKuttaApproximation;

        // Values handlers for each plot.
        public ValuesHandler ExactVH;
        public ValuesHandler EulerVH;
        public ValuesHandler ImprovedEulerVH;
        public ValuesHandler RungeKuttaVH;

        // Array to store numbers of grid steps.
        public int[] IntervalGTE;
        // Arrays to store GTE for Numerical methods.
        public double[] EulerGTE;
        public double[] ImprovedEulerGTE;
        public double[] RungeKuttaGTE;

        // Controller.
        Controller NewController;

        // Assign Controller to Model.
        public Model(Controller NewController) => this.NewController = NewController;

        // Assign new instances of correspondent classes to Differential equation and Numerical methods.
        public void InitializeSolutions()
        {
            // Create an instance of Differential Equation given in Variant 9.
            MyDifferentialEquation = new Variant9(x0, y0, X, N);
            DisplayDE = new Variant9(x0, y0, X, (int)(X - x0) * 15);
            // Create instances of Numerical methods, i.e., Euler, Improved Euler, and Runge-Kutta.
            EulerApproximation = new Euler(x0, y0, X, N);
            ImprovedEulerApproximation = new ImprovedEuler(x0, y0, X, N);
            RungeKuttaApproximation = new RungeKutta(x0, y0, X, N);
        }

        // Apply Numerical methods to a given Differential Equation.
        public void ApplyNumericalMethods()
        {
            EulerApproximation.Apply(MyDifferentialEquation);
            ImprovedEulerApproximation.Apply(MyDifferentialEquation);
            RungeKuttaApproximation.Apply(MyDifferentialEquation);
        }

        // Initialize values handlers for all methods to keep track of values in grid.
        public void InitializeValuesHandlers()
        {
            ExactVH = new ValuesHandler(DisplayDE);
            EulerVH = new ValuesHandler(EulerApproximation);
            ImprovedEulerVH = new ValuesHandler(ImprovedEulerApproximation);
            RungeKuttaVH = new ValuesHandler(RungeKuttaApproximation);
        }

        // Initialize auxiliary arrays for computing GTEs on an interval.
        public void InitializeArraysGTE()
        {
            // Create an array to store values of grid step.
            IntervalGTE = new int[NMax - N + 1];
            // Create arrays to store GTE for Numerical methods.
            EulerGTE = new double[NMax - N + 1];
            ImprovedEulerGTE = new double[NMax - N + 1];
            RungeKuttaGTE = new double[NMax - N + 1];
        }

        // Updates arrays of GTEs for Numerical methods.
        public void ComputeIntervalGTE()
        {
            // Iteratively find GTE.
            for (int i = 0; i <= NMax - N; i++)
            {
                // Set number of grid steps.
                IntervalGTE[i] = i + N;
                // Create an instance of Differential Equation given in Variant 9.
                Variant9 MyDifferentialEquationTemp = new Variant9(x0, y0, X, IntervalGTE[i]);
                // Create instances of Numerical methods, i.e., Euler, Improved Euler, and Runge-Kutta.
                Euler EulerApproximationTemp = new Euler(x0, y0, X, IntervalGTE[i]);
                ImprovedEuler ImprovedEulerApproximationTemp = new ImprovedEuler(x0, y0, X, IntervalGTE[i]);
                RungeKutta RungeKuttaApproximationTemp = new RungeKutta(x0, y0, X, IntervalGTE[i]);

                // Apply Numerical methods to a given Differential Equation.
                EulerApproximationTemp.Apply(MyDifferentialEquationTemp);
                ImprovedEulerApproximationTemp.Apply(MyDifferentialEquationTemp);
                RungeKuttaApproximationTemp.Apply(MyDifferentialEquationTemp);

                // Compute GTE for each method.
                EulerGTE[i] = EulerApproximationTemp.ComputeGlobalTruncationErrors(MyDifferentialEquationTemp).Max();
                ImprovedEulerGTE[i] = ImprovedEulerApproximationTemp.ComputeGlobalTruncationErrors(MyDifferentialEquationTemp).Max();
                RungeKuttaGTE[i] = RungeKuttaApproximationTemp.ComputeGlobalTruncationErrors(MyDifferentialEquationTemp).Max();
            }
        }

        // Checks whether data can be plotted.
        public bool IsPossiblePlot()
        {
            // Return false if data contains elements that cannot be ploted, true - otherwise.
            if (!ExactVH.CheckPossibilityPlot()
                    || !EulerVH.CheckPossibilityPlot()
                    || !ImprovedEulerVH.CheckPossibilityPlot()
                    || !RungeKuttaVH.CheckPossibilityPlot())
                return false;
            return true;
        }

        // Set values retrieved from GUI on Tab 1.
        public void UpdateValuesTab1GUI()
        {
            // Get IVP.
            x0 = NewController.RequestX0();
            y0 = NewController.RequestY0();
            // Get number of grid steps. 
            X = NewController.RequestX();
            // Check whether X is greater than x0.
            if (x0 >= X)
                throw new Exception("X should be greater than x0.");
            // Get number of grid steps on Tab 1. 
            N = NewController.RequestNTab1();
            // Check whether N is positive.
            if (N <= 0)
                throw new Exception("N should be greater than 0.");
        }

        // Set values retrieved from GUI on Tab 2.
        public void UpdateValuesTab2GUI()
        {
            // Get number of grid steps on Tab 2. 
            Tab2N = NewController.RequestNTab2();
            // Check whether Tab2N is positive.
            if (Tab2N <= 0)
                throw new Exception("N0 should be greater than 0.");
            // Get maximum number of grid steps on Tab 2. 
            NMax = NewController.RequestNMax();
            // Check whether NMax is positive.
            if (NMax <= 0)
                throw new Exception("N should be greater than 0.");
        }

        // Method executed when button Plot on Tab 1 clicked.
        public void Tab1()
        {
            // Get Values from GUI of Tab 1.
            UpdateValuesTab1GUI();

            // Initialize Numerical methods.
            InitializeSolutions();
            // Apply Numerical methods.
            ApplyNumericalMethods();
            // Initialize ValuesHandlers.
            InitializeValuesHandlers();

            // Check whether approximations can be plotted.
            if (!IsPossiblePlot())
            {
                // Change of N on Tab 1 to correspondent value on Tab 2.
                NewController.NTab1Tab2();
                throw new Exception("Due to an inaccuracy of numerical methods overflow occured.");
            }
            // Change of N on Tab 2 to correspondent value on Tab 1.
            NewController.NTab2Tab1();
            // Update N max.
            NewController.UpdateNMax();
            // Get Values from GUI of Tab 2.
            UpdateValuesTab2GUI();

            // Tab 1. 
            // Plot graphs of approximation and LTEs.
            NewController.RequestUpdateChartApproximation(DisplayDE, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
            NewController.RequestUpdateChartLTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
            // Tab 2.
            // Plot graph of GTEs.
            NewController.RequestUpdateChartGTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
            // Compute Global Truncation Errors for a given interval of grid steps.
            InitializeArraysGTE();
            ComputeIntervalGTE();
            // Plot graph of GTEs on the interval.
            NewController.RequestUpdateChartGTEInterval(IntervalGTE, EulerGTE, ImprovedEulerGTE, RungeKuttaGTE, N, NMax);
        }

        // Method executed when button Plot on Tab 2 clicked.
        public void Tab2()
        {
            // Get Values from GUI of Tab 2.
            UpdateValuesTab2GUI();
            // Compare N0 and N on second tab.
            if (Tab2N >= NMax)
            {
                // Change of N on Tab 2 to correspondent value on Tab 1.
                NewController.NTab2Tab1();
                throw new Exception("N0 should be greater than N.");
            }
            // Change of N on Tab 1 to correspondent value on Tab 2.
            NewController.NTab1Tab2();
            // Get Values from GUI of Tab 1.
            UpdateValuesTab1GUI();
            // Initialize Numerical methods.
            InitializeSolutions();
            // Apply Numerical methods.
            ApplyNumericalMethods();
            // Initialize ValuesHandlers.
            InitializeValuesHandlers();

            // Tab2.
            // Check whether approximations can be plotted.
            if (!IsPossiblePlot())
                throw new Exception("Due to an inaccuracy of numerical methods overflow occured.");
            // Plot graph of GTEs.
            NewController.RequestUpdateChartGTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
            // Compute Global Truncation Errors for a given interval of grid steps.
            InitializeArraysGTE();
            ComputeIntervalGTE();
            // Plot graph of GTEs on the interval.
            NewController.RequestUpdateChartGTEInterval(IntervalGTE, EulerGTE, ImprovedEulerGTE, RungeKuttaGTE, N, NMax);

            // Tab 1. 
            // Plot graphs of approximation and LTEs.
            NewController.RequestUpdateChartApproximation(DisplayDE, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
            NewController.RequestUpdateChartLTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
        }

        // Method passing entities to be plotted.
        public void UpdateAllCharts()
        {
            // Request replotting graphs on chart for approximations.
            NewController.RequestUpdateChartApproximation(DisplayDE, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
            // Request replotting graphs on chart for GTEs.
            NewController.RequestUpdateChartLTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
            // Request replotting graphs on chart for LTEs.
            NewController.RequestUpdateChartGTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, x0, X);
            // Request replotting graphs on chart for GTE Analysis.
            NewController.RequestUpdateChartGTEInterval(IntervalGTE, EulerGTE, ImprovedEulerGTE, RungeKuttaGTE, N, NMax);
        }
    }
}
