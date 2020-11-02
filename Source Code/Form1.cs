using System;
using System.Linq;
using System.Windows.Forms;

namespace ComputationalPracticum
{
    public partial class DESolver : Form
    {
        // A x-coordinate of leftmost grid point. 
        double x0;
        // A y-coordinate of leftmost grid point. 
        double y0;
        // A right boundary of x-coordinates interval. 
        double X;
        // Number of grid steps. 
        int N;
        // Number of grid steps on second tab. 
        int Tab2N;
        // Maximum number of grid steps. 
        int NMax;

        // Ordinary differential equation Variant 9 for computaions.
        Variant9 MyDifferentialEquation;
        // Ordinary differential equation Variant 9 for ploting.
        Variant9 DisplayDE;
        
        // Numerical methods.
        Euler EulerApproximation;
        ImprovedEuler ImprovedEulerApproximation;
        RungeKutta RungeKuttaApproximation;

        // Values handlers for each plot.
        ValuesHandler ExactVH;
        ValuesHandler EulerVH;
        ValuesHandler ImprovedEulerVH;
        ValuesHandler RungeKuttaVH;

        // Array to store numbers of grid steps.
        int[] IntervalGTE;
        // Arrays to store GTE for Numerical methods.
        double[] EulerGTE;
        double[] ImprovedEulerGTE;
        double[] RungeKuttaGTE;

        // Application.
        public DESolver()
        {
            // Initialize GUI components.
            InitializeComponent();
        }

        // Retrieve values from GUI on first tab.
        private void Tab1GetValuesGUI()
        {
            // IVP: y0 = y(x0).
            x0 = Double.Parse(Tab1TextBoxX0.Text);
            y0 = Double.Parse(Tab1TextBoxY0.Text);
            // A x-coordinate of a point program needs to approximate.
            X = Double.Parse(Tab1TextBoxX.Text);
            // Check whether 0 belongs to set of x's
            if (x0 == 0 || X == 0 || x0 < 0 && X > 0)
                throw new Exception("Solution is not defined in 0.");
            // Check whether X is greater than x0.
            if (x0 >= X)
                throw new Exception("X should be greater than x0.");
            // Initial number of grid steps.
            N = Int32.Parse(Tab1TextBoxN.Text);
        }

        // Retrieve values from GUI on second tab.
        private void Tab2GetValuesGUI()
        {
            // Number of grid steps. 
            Tab2N = Int32.Parse(Tab2TextBoxN0.Text);
            // Maximum number of grid steps.
            NMax = Int32.Parse(Tab2TextBoxN.Text);
        }

        // Assign new instances of correspondent classes to Differential equation and Numerical methods.
        private void InitializeSolutions()
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
        private void ApplyNumericalMethods()
        {
            EulerApproximation.Apply(MyDifferentialEquation);
            ImprovedEulerApproximation.Apply(MyDifferentialEquation);
            RungeKuttaApproximation.Apply(MyDifferentialEquation);
        }

        // Initialize values handlers for all methods to keep track of values in grid.
        private void InitializeValuesHandlers()
        {
            ExactVH = new ValuesHandler(DisplayDE);
            EulerVH = new ValuesHandler(EulerApproximation);
            ImprovedEulerVH = new ValuesHandler(ImprovedEulerApproximation);
            RungeKuttaVH = new ValuesHandler(RungeKuttaApproximation);
        }

        // Initialize auxiliary arrays for computing GTEs on an interval.
        private void InitializeArraysGTE()
        {
            // Create an array to store values of grid step.
            IntervalGTE = new int[NMax - N + 1];
            // Create arrays to store GTE for Numerical methods.
            EulerGTE = new double[NMax - N + 1];
            ImprovedEulerGTE = new double[NMax - N + 1];
            RungeKuttaGTE = new double[NMax - N + 1];
        }

        // Updates arrays of GTEs for Numerical methods.
        private void ComputeIntervalGTE()
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

        // Method that runs when the button on the first tab was clicked.
        private void Tab1ButtonClick(object sender, EventArgs e)
        {
            // In order to handle errors caused by incorrect data use try-catch operator.
            try
            {
                // Get Values from GUI of first tab.
                Tab1GetValuesGUI();
                // Initialize Numerical methods.
                InitializeSolutions();
                // Apply Numerical methods.
                ApplyNumericalMethods();
                // Initialize ValuesHandlers.
                InitializeValuesHandlers();

                // Tab 1.
                // Check whether approximations can be plotted.
                if (!ExactVH.CheckPossibilityPlot()
                    || !EulerVH.CheckPossibilityPlot()
                    || !ImprovedEulerVH.CheckPossibilityPlot()
                    || !RungeKuttaVH.CheckPossibilityPlot())
                {
                    Tab1TextBoxN.Text = Tab2TextBoxN0.Text;
                    throw new Exception("Due to an inaccuracy of numerical methods overflow occured.");
                }
                // Set value of Initial number of gris steps on Tab2 to correspondent value of N on Tab 1.
                Tab2TextBoxN0.Text = Tab1TextBoxN.Text;
                // Set value of maximum number of points on Tab2 to value of initial number of points + 5.
                Tab2TextBoxN.Text = (Int32.Parse(Tab1TextBoxN.Text) + 5).ToString();
                // Get Values from GUI of second tab.
                Tab2GetValuesGUI();
                // Plot graphs of approximation and LTEs.
                UpdateChartApproximation(DisplayDE, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
                UpdateChartLTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);

                // Tab2.
                // Plot graph of GTEs.
                UpdateChartGTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
                // Compute Global Truncation Errors for a given interval of grid steps.
                InitializeArraysGTE();
                ComputeIntervalGTE();
                // Plot graph of GTEs on the interval.
                UpdateChartGTEInterval();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Tab2ButtonClick(object sender, EventArgs e)
        {
            // In order to handle errors caused by incorrect data use try-catch operator.
            try
            {
                // Get Values from GUI of second tab.
                Tab2GetValuesGUI();
                if (Tab2N >= NMax)
                {
                    Tab2TextBoxN0.Text = Tab1TextBoxN.Text;
                    throw new Exception("N0 should be greater than N.");
                }
                Tab1TextBoxN.Text = Tab2TextBoxN0.Text;
                // Get Values from GUI of first tab.
                Tab1GetValuesGUI();
                // Initialize Numerical methods.
                InitializeSolutions();
                // Apply Numerical methods.
                ApplyNumericalMethods();
                // Initialize ValuesHandlers.
                InitializeValuesHandlers();

                // Tab2.
                // Check whether approximations can be plotted.
                if (!ExactVH.CheckPossibilityPlot()
                    || !EulerVH.CheckPossibilityPlot()
                    || !ImprovedEulerVH.CheckPossibilityPlot()
                    || !RungeKuttaVH.CheckPossibilityPlot())
                    throw new Exception("Due to an inaccuracy of numerical methods overflow occured.");
                // Plot graph of GTEs.
                UpdateChartGTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
                // Compute Global Truncation Errors for a given interval of grid steps.
                InitializeArraysGTE();
                ComputeIntervalGTE();
                // Plot graph of GTEs on the interval.
                UpdateChartGTEInterval();

                // Tab 1.
                // Plot graphs of approximation and LTEs.
                UpdateChartApproximation(DisplayDE, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
                UpdateChartLTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        // Update chart of approximations.
        private void UpdateChartApproximation(DifferentialEquation MyDifferentialEquation, Euler EulerApproximation, ImprovedEuler ImprovedEulerApproximation, RungeKutta RungeKuttaApproximation)
        {
            // Set minimum and maximum values of Tab1ChartApproximation.
            Tab1ChartApproximation.ChartAreas[0].AxisX.Minimum = x0;
            Tab1ChartApproximation.ChartAreas[0].AxisX.Maximum = X;
            // Plot the graphs of exact and numerical solutions.
            Tab1ChartApproximation.Series[0].Points.DataBindXY(MyDifferentialEquation.GetX(), MyDifferentialEquation.GetY());
            Tab1ChartApproximation.Series[1].Points.DataBindXY(EulerApproximation.GetX(), EulerApproximation.GetY());
            Tab1ChartApproximation.Series[2].Points.DataBindXY(ImprovedEulerApproximation.GetX(), ImprovedEulerApproximation.GetY());
            Tab1ChartApproximation.Series[3].Points.DataBindXY(RungeKuttaApproximation.GetX(), RungeKuttaApproximation.GetY());
        }

        // Update chart of LTEs.
        private void UpdateChartLTE(DifferentialEquation MyDifferentialEquation, Euler EulerApproximation, ImprovedEuler ImprovedEulerApproximation, RungeKutta RungeKuttaApproximation)
        {
            // Set minimum and maximum values of Tab1ChartLTE.
            Tab1ChartLTE.ChartAreas[0].AxisX.Minimum = x0;
            Tab1ChartLTE.ChartAreas[0].AxisX.Maximum = X;
            // Plot the graphs of Local Truncation errors of numerical solutions.
            Tab1ChartLTE.Series[0].Points.DataBindXY(EulerApproximation.GetX(), EulerApproximation.ComputeLocalTruncationErrors(MyDifferentialEquation));
            Tab1ChartLTE.Series[1].Points.DataBindXY(ImprovedEulerApproximation.GetX(), ImprovedEulerApproximation.ComputeLocalTruncationErrors(MyDifferentialEquation));
            Tab1ChartLTE.Series[2].Points.DataBindXY(RungeKuttaApproximation.GetX(), RungeKuttaApproximation.ComputeLocalTruncationErrors(MyDifferentialEquation));
        }

        // Update chart of GTEs for N=N0.
        private void UpdateChartGTE(DifferentialEquation MyDifferentialEquation, Euler EulerApproximation, ImprovedEuler ImprovedEulerApproximation, RungeKutta RungeKuttaApproximation)
        {
            // Set minimum and maximum values of Tab2ChartGTE.
            Tab2ChartGTE.ChartAreas[0].AxisX.Minimum = x0;
            Tab2ChartGTE.ChartAreas[0].AxisX.Maximum = X;
            // Plot the graphs of Global Truncation errors of numerical solutions.
            Tab2ChartGTE.Series[0].Points.DataBindXY(EulerApproximation.GetX(), EulerApproximation.ComputeGlobalTruncationErrors(MyDifferentialEquation));
            Tab2ChartGTE.Series[1].Points.DataBindXY(ImprovedEulerApproximation.GetX(), ImprovedEulerApproximation.ComputeGlobalTruncationErrors(MyDifferentialEquation));
            Tab2ChartGTE.Series[2].Points.DataBindXY(RungeKuttaApproximation.GetX(), RungeKuttaApproximation.ComputeGlobalTruncationErrors(MyDifferentialEquation));

        }

        // Update chart of GTEs for N on interval N0..NMax.
        private void UpdateChartGTEInterval()
        {
            // Set minimum and maximum values of Tab2ChartGTEAnalysis.
            Tab2ChartGTEAnalysis.ChartAreas[0].AxisX.Minimum = N;
            Tab2ChartGTEAnalysis.ChartAreas[0].AxisX.Maximum = NMax;
            // Plot the graphs of Global Truncation errors of numerical solutions within a given interval of grid steps.
            Tab2ChartGTEAnalysis.Series[0].Points.DataBindXY(IntervalGTE, EulerGTE);
            Tab2ChartGTEAnalysis.Series[1].Points.DataBindXY(IntervalGTE, ImprovedEulerGTE);
            Tab2ChartGTEAnalysis.Series[2].Points.DataBindXY(IntervalGTE, RungeKuttaGTE);

        }

        // Method that runs when the state of chekbox for Exact solution on the first tab changed.
        private void Tab1CheckBoxExactCheckedChanged(object sender, EventArgs e) => Tab1ChartApproximation.Series[0].Enabled = Tab1ChekBoxExact.Checked;

        // Method that runs when the state of chekboxes (Numerical methods only) on the first tab changed.
        private void Tab1CheckBoxesCheckedChanged(object sender, EventArgs e)
        {
            // Enable/Disable Euler plot on both tabs.
            Tab1ChartApproximation.Series[1].Enabled = Tab1CheckBoxEuler.Checked;
            Tab1ChartLTE.Series[0].Enabled = Tab1CheckBoxEuler.Checked;
            Tab2CheckBoxEuler.Checked = Tab1CheckBoxEuler.Checked;

            // Enable/Disable Improved Euler plot on both tabs.
            Tab1ChartApproximation.Series[2].Enabled = Tab1CheckBoxImprovedEuler.Checked;
            Tab1ChartLTE.Series[1].Enabled = Tab1CheckBoxImprovedEuler.Checked;
            Tab2CheckBoxImprovedEuler.Checked = Tab1CheckBoxImprovedEuler.Checked;

            // Enable/Disable Runge-Kutta plot on both tabs.
            Tab1ChartApproximation.Series[3].Enabled = Tab1CheckBoxRungeKutta.Checked;
            Tab1ChartLTE.Series[2].Enabled = Tab1CheckBoxRungeKutta.Checked;
            Tab2CheckBoxRungeKutta.Checked = Tab1CheckBoxRungeKutta.Checked;

            // Update charts taking into account hidden plots.
            UpdateChartApproximation(DisplayDE, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
            UpdateChartLTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
            UpdateChartGTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
            UpdateChartGTEInterval();
        }

        // Method that runs when the state of chekboxes on the second tab changed.
        private void Tab2CheckBoxesCheckedChanged(object sender, EventArgs e)
        {
            // Enable/Disable Euler plot on both tabs.
            Tab2ChartGTE.Series[0].Enabled = Tab2CheckBoxEuler.Checked;
            Tab2ChartGTEAnalysis.Series[0].Enabled = Tab2CheckBoxEuler.Checked;
            Tab1CheckBoxEuler.Checked = Tab2CheckBoxEuler.Checked;

            // Enable/Disable Improved Euler plot on both tabs.
            Tab2ChartGTE.Series[1].Enabled = Tab2CheckBoxImprovedEuler.Checked;
            Tab2ChartGTEAnalysis.Series[1].Enabled = Tab2CheckBoxImprovedEuler.Checked;
            Tab1CheckBoxImprovedEuler.Checked = Tab2CheckBoxImprovedEuler.Checked;

            // Enable/Disable Runge-Kutta plot on both tabs.
            Tab2ChartGTE.Series[2].Enabled = Tab2CheckBoxRungeKutta.Checked;
            Tab2ChartGTEAnalysis.Series[2].Enabled = Tab2CheckBoxRungeKutta.Checked;
            Tab1CheckBoxRungeKutta.Checked = Tab2CheckBoxRungeKutta.Checked;

            // Update charts taking into account hidden plots.
            UpdateChartGTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
            UpdateChartGTEInterval();
            UpdateChartApproximation(DisplayDE, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
            UpdateChartLTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation);
        }

        // Method that runs when the program was started.
        private void DESolverShown(object sender, EventArgs e)
        {
            // In order to display plot area when all charts are hidden, add invisible points to each plot.
            Tab1ChartApproximation.Series[4].Points.Add();
            Tab1ChartApproximation.Series[4].Points[0].IsEmpty = true;
            Tab1ChartLTE.Series[3].Points.Add();
            Tab1ChartLTE.Series[3].Points[0].IsEmpty = true;
            Tab2ChartGTE.Series[3].Points.Add();
            Tab2ChartGTE.Series[3].Points[0].IsEmpty = true;
            Tab2ChartGTEAnalysis.Series[3].Points.Add();
            Tab2ChartGTEAnalysis.Series[3].Points[0].IsEmpty = true;

            // Plot charts on both tabs.
            Tab1ButtonClick(sender, e);
            Tab2ButtonClick(sender, e);
        }
    }
}
