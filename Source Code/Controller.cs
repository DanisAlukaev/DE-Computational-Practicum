using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ComputationalPracticum
{
    // Interconnect View and Model for the application.
    class Controller
    {
        // Model.
        Model NewModel;
        // View.
        View NewView;

        // Empty connstructor.
        public Controller() { }

        // Assign Model to Controller.
        public void SetModel(Model NewModel) => this.NewModel = NewModel;

        // Assign View to Controller.
        public void SetView(View NewView) => this.NewView = NewView;

        // Request running of a method for Tab 1 in Model.
        public void RequestTab1() => NewModel.Tab1();

        // Request running of a method for Tab 2 in Model.
        public void RequestTab2() => NewModel.Tab2();

        // Request a x-coordinate of leftmost grid point.
        public double RequestX0() => NewView.GetX0GUI();

        // Request a y-coordinate of leftmost grid point.
        public double RequestY0() => NewView.GetY0GUI();

        // Request a right boundary of x-coordinates interval.
        public double RequestX() => NewView.GetXGUI();

        // Request number of grid steps on Tab 1.
        public int RequestNTab1() => NewView.NTab1GUI;

        // Request number of grid steps on Tab 2.
        public int RequestNTab2() => NewView.GetNTab2GUI();

        // Request maximum number of grid steps. 
        public int RequestNMax() => NewView.GetNMaxGUI();

        // Request changing of N on Tab 2 to correspondent value on Tab 1.
        public void NTab2Tab1() => NewView.NTab2Tab1();

        // Request changing of N on Tab 1 to correspondent value on Tab 2.
        public void NTab1Tab2() => NewView.NTab1Tab2();

        // Request updating of N max.
        public void UpdateNMax() => NewView.UpdateNMax();

        // Requset updating of all charts.
        public void UpdateAllCharts() => NewModel.UpdateAllCharts();

        // Request replotting graphs on chart for approximations.
        public void RequestUpdateChartApproximation(DifferentialEquation MyDifferentialEquation,
                                                    Euler EulerApproximation,
                                                    ImprovedEuler ImprovedEulerApproximation,
                                                    RungeKutta RungeKuttaApproximation,
                                                    double X0,
                                                    double X) => NewView.UpdateChartApproximation(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, X0, X);

        // Request replotting graphs on chart for LTEs.
        public void RequestUpdateChartLTE(DifferentialEquation MyDifferentialEquation,
                                          Euler EulerApproximation,
                                          ImprovedEuler ImprovedEulerApproximation,
                                          RungeKutta RungeKuttaApproximation,
                                          double X0,
                                          double X) => NewView.UpdateChartLTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, X0, X);

        // Request replotting graphs on chart for GTEs.
        public void RequestUpdateChartGTE(DifferentialEquation MyDifferentialEquation,
                                          Euler EulerApproximation,
                                          ImprovedEuler ImprovedEulerApproximation,
                                          RungeKutta RungeKuttaApproximation,
                                          double X0,
                                          double X) => NewView.UpdateChartGTE(MyDifferentialEquation, EulerApproximation, ImprovedEulerApproximation, RungeKuttaApproximation, X0, X);

        // Request replotting graphs on chart for GTE Analysis.
        public void RequestUpdateChartGTEInterval(int[] IntervalGTE,
                                                  double[] EulerGTE,
                                                  double[] ImprovedEulerGTE,
                                                  double[] RungeKuttaGTE,
                                                  double N0,
                                                  double N) => NewView.UpdateChartGTEInterval(IntervalGTE, EulerGTE, ImprovedEulerGTE, RungeKuttaGTE, N0, N);
    }
}
