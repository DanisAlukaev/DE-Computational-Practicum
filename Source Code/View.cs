using System;
using System.Linq;
using System.Windows.Forms;

namespace ComputationalPracticum
{
    // Contains charts of approximations, LTEs, GTEs. Allows to analyze convergence of Numerical methods.
    // Allows to change initial conditions. 
    public partial class View : Form
    {
        // Create Controller.
        Controller NewController;
        // Create Model.
        Model NewModel;

        // Constructor of a class View.
        public View()
        {
            // Initialize GUI components.
            InitializeComponent();
            // Initialize Controller and Model.
            NewController = new Controller();
            NewModel = new Model(NewController);
            // Set Model and View.
            NewController.SetModel(NewModel);
            NewController.SetView(this);
        }

        // Retrieve a x-coordinate of leftmost grid point.
        public double GetX0GUI() => Double.Parse(Tab1TextBoxX0.Text);

        // Retrieve a y-coordinate of leftmost grid point.
        public double GetY0GUI() => Double.Parse(Tab1TextBoxY0.Text);

        // Retrieve a right boundary of x-coordinates interval.
        public double GetXGUI() => Double.Parse(Tab1TextBoxX.Text);

        // Retrieve number of grid steps on Tab 1. 
        public int NTab1GUI => Int32.Parse(Tab1TextBoxN.Text);

        // Retrieve number of grid steps on Tab 2.
        public int GetNTab2GUI() => Int32.Parse(Tab2TextBoxN0.Text);

        // Retrieve maximum number of grid steps. 
        public int GetNMaxGUI() => Int32.Parse(Tab2TextBoxN.Text);

        // Method executed when the button on the Tab 1 was clicked.
        private void Tab1ButtonClick(object sender, EventArgs e)
        {
            // In order to handle errors caused by incorrect data use try-catch operator.
            try
            {
                // Request updating of model from Tab 1.
                NewController.RequestTab1();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        // Method executed when the button on the Tab 2 was clicked.
        private void Tab2ButtonClick(object sender, EventArgs e)
        {
            // In order to handle errors caused by incorrect data use try-catch operator.
            try
            {
                // Request updating of model from Tab 2.
                NewController.RequestTab2();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        // Set value of Initial number of gris steps on Tab 2 to correspondent value of N on Tab 1.
        public void NTab2Tab1() => Tab2TextBoxN0.Text = Tab1TextBoxN.Text;

        // Set value of Initial number of gris steps on Tab 1 to correspondent value of N on Tab 2.
        public void NTab1Tab2() => Tab1TextBoxN.Text = Tab2TextBoxN0.Text;

        // Set value of maximum number of points on Tab 2 to value of initial number of points + 10.
        public void UpdateNMax() => Tab2TextBoxN.Text = (Int32.Parse(Tab1TextBoxN.Text) + 10).ToString();

        // Update chart of approximations.
        public void UpdateChartApproximation(DifferentialEquation MyDifferentialEquation, Euler EulerApproximation, ImprovedEuler ImprovedEulerApproximation, RungeKutta RungeKuttaApproximation, double X0, double X)
        {
            // Set minimum and maximum values of Tab1ChartApproximation.
            Tab1ChartApproximation.ChartAreas[0].AxisX.Minimum = X0;
            Tab1ChartApproximation.ChartAreas[0].AxisX.Maximum = X;
            // Plot the graphs of exact and numerical solutions.
            Tab1ChartApproximation.Series[0].Points.DataBindXY(MyDifferentialEquation.GetX(), MyDifferentialEquation.GetY());
            Tab1ChartApproximation.Series[1].Points.DataBindXY(EulerApproximation.GetX(), EulerApproximation.GetY());
            Tab1ChartApproximation.Series[2].Points.DataBindXY(ImprovedEulerApproximation.GetX(), ImprovedEulerApproximation.GetY());
            Tab1ChartApproximation.Series[3].Points.DataBindXY(RungeKuttaApproximation.GetX(), RungeKuttaApproximation.GetY());
        }

        // Update chart of LTEs.
        public void UpdateChartLTE(DifferentialEquation MyDifferentialEquation, Euler EulerApproximation, ImprovedEuler ImprovedEulerApproximation, RungeKutta RungeKuttaApproximation, double X0, double X)
        {
            // Set minimum and maximum values of Tab1ChartLTE.
            Tab1ChartLTE.ChartAreas[0].AxisX.Minimum = X0;
            Tab1ChartLTE.ChartAreas[0].AxisX.Maximum = X;
            // Plot the graphs of Local Truncation errors of numerical solutions.
            Tab1ChartLTE.Series[0].Points.DataBindXY(EulerApproximation.GetX(), EulerApproximation.ComputeLocalTruncationErrors(MyDifferentialEquation));
            Tab1ChartLTE.Series[1].Points.DataBindXY(ImprovedEulerApproximation.GetX(), ImprovedEulerApproximation.ComputeLocalTruncationErrors(MyDifferentialEquation));
            Tab1ChartLTE.Series[2].Points.DataBindXY(RungeKuttaApproximation.GetX(), RungeKuttaApproximation.ComputeLocalTruncationErrors(MyDifferentialEquation));
        }

        // Update chart of GTEs for N=N0.
        public void UpdateChartGTE(DifferentialEquation MyDifferentialEquation, Euler EulerApproximation, ImprovedEuler ImprovedEulerApproximation, RungeKutta RungeKuttaApproximation, double X0, double X)
        {
            // Set minimum and maximum values of Tab2ChartGTE.
            Tab2ChartGTE.ChartAreas[0].AxisX.Minimum = X0;
            Tab2ChartGTE.ChartAreas[0].AxisX.Maximum = X;
            // Plot the graphs of Global Truncation errors of numerical solutions.
            Tab2ChartGTE.Series[0].Points.DataBindXY(EulerApproximation.GetX(), EulerApproximation.ComputeGlobalTruncationErrors(MyDifferentialEquation));
            Tab2ChartGTE.Series[1].Points.DataBindXY(ImprovedEulerApproximation.GetX(), ImprovedEulerApproximation.ComputeGlobalTruncationErrors(MyDifferentialEquation));
            Tab2ChartGTE.Series[2].Points.DataBindXY(RungeKuttaApproximation.GetX(), RungeKuttaApproximation.ComputeGlobalTruncationErrors(MyDifferentialEquation));

        }

        // Update chart of GTEs for N on interval N0..NMax.
        public void UpdateChartGTEInterval(int[] IntervalGTE, double[] EulerGTE, double[] ImprovedEulerGTE, double[] RungeKuttaGTE, double N0, double N)
        {
            // Set minimum and maximum values of Tab2ChartGTEAnalysis.
            Tab2ChartGTEAnalysis.ChartAreas[0].AxisX.Minimum = N0;
            Tab2ChartGTEAnalysis.ChartAreas[0].AxisX.Maximum = N;
            // Plot the graphs of Global Truncation errors of numerical solutions within a given interval of grid steps.
            Tab2ChartGTEAnalysis.Series[0].Points.DataBindXY(IntervalGTE, EulerGTE);
            Tab2ChartGTEAnalysis.Series[1].Points.DataBindXY(IntervalGTE, ImprovedEulerGTE);
            Tab2ChartGTEAnalysis.Series[2].Points.DataBindXY(IntervalGTE, RungeKuttaGTE);
        }

        // Method executed when the state of chekbox for Exact solution on the Tab 1 changed.
        public void Tab1CheckBoxExactCheckedChanged(object sender, EventArgs e) => Tab1ChartApproximation.Series[0].Enabled = Tab1ChekBoxExact.Checked;

        // Method executed when the state of chekboxes (Numerical methods only) on the Tab 1 changed.
        public void Tab1CheckBoxesCheckedChanged(object sender, EventArgs e)
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
            NewController.UpdateAllCharts();
        }

        // Method executed when the state of chekboxes on the Tab 2 changed.
        public void Tab2CheckBoxesCheckedChanged(object sender, EventArgs e)
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
            NewController.UpdateAllCharts();
        }

        // Method executed when the program was started.
        public void DESolverShown(object sender, EventArgs e)
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
