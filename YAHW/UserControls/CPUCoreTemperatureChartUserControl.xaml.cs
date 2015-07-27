using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using YAHW.Model;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for CPUCoreTemperatureChartUserControl.xaml
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
    public partial class CPUCoreTemperatureChartUserControl : UserControl
    {
        public CPUCoreTemperatureChartUserControl()
        {
            InitializeComponent();

            this.CPUCoreTemperaturePlot = new PlotModel();

            this.CPUCoreTemperaturePlot.Axes.Add(new LinearAxis()
            {
                IsZoomEnabled = false,
                Maximum = 85,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Left
            });

            this.CPUCoreTemperaturePlot.Axes.Add(new LinearAxis()
            {
                IsZoomEnabled = false,
                Maximum = 85,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Right
            });

            this.CPUCoreTemperaturePlot.Axes.Add(new LinearAxis()
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

            this.CPUCoreTemperaturePlot.Series.Add(areaSeries);
        }

        #region Callbacks

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCurrentCoreTemperatureChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CPUCoreTemperatureChartUserControl instance = (CPUCoreTemperatureChartUserControl)sender;
            instance.CurrentCoreTemperatureChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void CurrentCoreTemperatureChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            var areaSeries = (LineSeries)this.CPUCoreTemperaturePlot.Series[0];

            if (areaSeries.Points.Count > 60)
                areaSeries.Points.RemoveAt(0);

            double x = areaSeries.Points.Count > 0 ? areaSeries.Points[areaSeries.Points.Count - 1].X + 1 : 0;

            var dp = new DataPoint(x, Convert.ToDouble(args.NewValue));
            areaSeries.Points.Add(dp);

            //if (this.CoreTemperaturesSeries != null && this.CoreTemperaturesSeries.Count == 0)
            //{
            //    this.CoreTemperaturesSeries.Add(new ChartDataPoint() { Name = "Temperature", Value = Convert.ToDouble(args.NewValue) });
            //}
            //else
            //{
            //    this.CoreTemperaturesSeries.FirstOrDefault().Value = Convert.ToDouble(args.NewValue);
            //}

            this.CPUCoreTemperaturePlot.InvalidatePlot(true);
        }

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCoreNameChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CPUCoreTemperatureChartUserControl instance = (CPUCoreTemperatureChartUserControl)sender;
            instance.CoreNameChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void CoreNameChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            this.CPUCoreTemperaturePlot.Title = args.NewValue.ToString();
            this.CPUCoreTemperaturePlot.InvalidatePlot(true);
        }

        #endregion Callbacks

        #region Dependency Properties

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
            DependencyProperty.Register("CoreName", typeof(string), typeof(CPUCoreTemperatureChartUserControl), new PropertyMetadata(OnCoreNameChangedPropertyCallback));

        /// <summary>
        /// CPU-Core-Temperature Chart
        /// </summary>
        public PlotModel CPUCoreTemperaturePlot
        {
            get { return (PlotModel)GetValue(CPUCoreTemperaturePlotProperty); }
            set { SetValue(CPUCoreTemperaturePlotProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CPUCoreTemperaturePlot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CPUCoreTemperaturePlotProperty =
            DependencyProperty.Register("CPUCoreTemperaturePlot", typeof(PlotModel), typeof(CPUCoreTemperatureChartUserControl), new PropertyMetadata(null));

        /// <summary>
        /// Current core temperature
        /// </summary>
        public double CurrentCoreTemperature
        {
            get { return (double)GetValue(CurrentCoreTemperatureProperty); }
            set { SetValue(CurrentCoreTemperatureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentCoreTemperature.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentCoreTemperatureProperty =
            DependencyProperty.Register("CurrentCoreTemperature", typeof(double), typeof(CPUCoreTemperatureChartUserControl), new PropertyMetadata(OnCurrentCoreTemperatureChangedPropertyCallback));


        /// <summary>
        /// Max. Temperatures
        /// </summary>
        public ObservableCollection<ChartDataPoint> CoreTemperaturesSeries
        {
            get { return (ObservableCollection<ChartDataPoint>)GetValue(MaxTemperaturesProperty); }
            set { SetValue(MaxTemperaturesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CoreTemperaturesSeries.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxTemperaturesProperty =
            DependencyProperty.Register("CoreTemperaturesSeries", typeof(ObservableCollection<ChartDataPoint>), typeof(CPUCoreTemperatureChartUserControl), new PropertyMetadata(new ObservableCollection<ChartDataPoint>()));

        #endregion Dependency Properties
    }
}