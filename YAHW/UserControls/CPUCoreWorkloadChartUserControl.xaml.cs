using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using YAHW.Model;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for CPUCoreWorkloadChartUserControl.xaml
    /// </para>
    /// 
    /// <para>
    /// Class history:
    /// <list type="bullet">
    ///     <item>
    ///         <description>1.0: First release, working (Steffen Steinbrecher).</description>
    ///     </item>
    /// </list>
    /// </para>
    /// 
    /// <para>Author: Steffen Steinbrecher</para>
    /// <para>Date: 12.07.2015</para>
    /// </summary>
    public partial class CPUCoreWorkloadChartUserControl : UserControl
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public CPUCoreWorkloadChartUserControl()
        {
            InitializeComponent();

            this.CPUCoreWorkloadPlot = new PlotModel();

            this.CPUCoreWorkloadPlot.Axes.Add(new LinearAxis()
            {
                IsZoomEnabled = false,
                Maximum = 102,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Left
            });

            this.CPUCoreWorkloadPlot.Axes.Add(new LinearAxis()
            {
                IsZoomEnabled = false,
                Maximum = 102,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Right
            });

            this.CPUCoreWorkloadPlot.Axes.Add(new LinearAxis()
            {
                IsZoomEnabled = false,
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.None,
                IsAxisVisible = false
            });

            var areaSeries = new LineSeries()
            {
                StrokeThickness = 1,
                LineStyle = OxyPlot.LineStyle.Solid,
                Color = OxyColors.Blue,
            //    Color2 = OxyColors.Transparent,
            //    Fill = OxyColor.FromRgb(214, 231, 242),
            //    DataFieldX = "X",
            //    DataFieldY = "Y",
            //    DataFieldX2 = "X"
            };

            for (int i = 0; i < 60; i++)
            {
                var dp = new DataPoint(i, 0);
                areaSeries.Points.Add(dp);
            }

            this.CPUCoreWorkloadPlot.Series.Add(areaSeries);
        }
        
        #region Callbacks

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCurrentCoreWorkloadChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CPUCoreWorkloadChartUserControl instance = (CPUCoreWorkloadChartUserControl)sender;
            instance.CurrentCoreWorkloadChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void CurrentCoreWorkloadChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            var areaSeries = (LineSeries)this.CPUCoreWorkloadPlot.Series[0];

            if (areaSeries.Points.Count > 60)
                areaSeries.Points.RemoveAt(0);

            double x = areaSeries.Points.Count > 0 ? areaSeries.Points[areaSeries.Points.Count - 1].X + 1 : 0;

            var dp = new DataPoint(x, Convert.ToDouble(args.NewValue));
            areaSeries.Points.Add(dp);

            if (this.CurrentCpuWorkloadSeries != null && this.CurrentCpuWorkloadSeries.Count == 0)
            {
                this.CurrentCpuWorkloadSeries.Add(new ChartDataPoint() { Name = "Workload", Value = Convert.ToDouble(args.NewValue) });
            }
            else
            {
                this.CurrentCpuWorkloadSeries.FirstOrDefault().Value = Convert.ToDouble(args.NewValue);
            }

            this.CPUCoreWorkloadPlot.InvalidatePlot(true);
        }

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCoreNameChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CPUCoreWorkloadChartUserControl instance = (CPUCoreWorkloadChartUserControl)sender;
            instance.CoreNameChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void CoreNameChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            this.CPUCoreWorkloadPlot.Title = args.NewValue.ToString();
            this.CPUCoreWorkloadPlot.InvalidatePlot(true);
        }

        #endregion Callbacks

        /// <summary>
        /// Core-Name
        /// </summary>
        public string CoreName
        {
            get { return (string)GetValue(CoreNameProperty); }
            set { SetValue(CoreNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CoreName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoreNameProperty =
            DependencyProperty.Register("CoreName", typeof(string), typeof(CPUCoreWorkloadChartUserControl), new PropertyMetadata(OnCoreNameChangedPropertyCallback));

        /// <summary>
        /// The CPU-Core Workload
        /// </summary>
        public PlotModel CPUCoreWorkloadPlot
        {
            get { return (PlotModel)GetValue(CPUCoreWorkloadPlotProperty); }
            set { SetValue(CPUCoreWorkloadPlotProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CPUCoreWorkloads.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CPUCoreWorkloadPlotProperty =
            DependencyProperty.Register("CPUCoreWorkloadPlot", typeof(PlotModel), typeof(CPUCoreWorkloadChartUserControl), new PropertyMetadata(null));

        /// <summary>
        /// The current core workload
        /// </summary>
        public double CurrentCoreWorkload
        {
            get { return (double)GetValue(CurrentCoreWorkloadProperty); }
            set { SetValue(CurrentCoreWorkloadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentCoreWorkload.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentCoreWorkloadProperty =
            DependencyProperty.Register("CurrentCoreWorkload", typeof(double), typeof(CPUCoreWorkloadChartUserControl), new PropertyMetadata(OnCurrentCoreWorkloadChangedPropertyCallback));

        /// <summary>
        /// Series for the gauge chart
        /// </summary>
        public ObservableCollection<ChartDataPoint> CurrentCpuWorkloadSeries
        {
            get { return (ObservableCollection<ChartDataPoint>)GetValue(CurrentCpuWorkloadSeriesProperty); }
            set { SetValue(CurrentCpuWorkloadSeriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentCpuWorkloadSeries.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentCpuWorkloadSeriesProperty =
            DependencyProperty.Register("CurrentCpuWorkloadSeries", typeof(ObservableCollection<ChartDataPoint>), typeof(CPUCoreWorkloadChartUserControl), new PropertyMetadata(new ObservableCollection<ChartDataPoint>()));
    }
}