using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using YAHW.Model;
using YAHW.SystemInformation;
using YAHW.ExtensionMethods;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for RAMInformationUserControl.xaml
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
    public partial class RAMInformationUserControl : UserControl
    {
        #region Members and Constants

        private DispatcherTimer timer = null;
        private PerfomanceInfoData performanceInfoData = null;

        #endregion Members and Constants

        public RAMInformationUserControl()
        {
            InitializeComponent();
            this.timer = new DispatcherTimer();

            this.GetPhysicalMemoryInformation();

            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        #region Event-Handler

        /// <summary>
        /// Timer-Tick Event-Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            this.GetPhysicalMemoryInformation();
        }

        #endregion Event-Handler

        #region Private methods

        /// <summary>
        /// Get physical memory information
        /// </summary>
        private void GetPhysicalMemoryInformation()
        {
            this.performanceInfoData = PsApiWrapper.GetPerformanceInfo();

            var AvailableGb = this.performanceInfoData.PhysicalAvailableBytes.ToPrettySize(1);
            var UsedGb = (this.performanceInfoData.PhysicalTotalBytes - this.performanceInfoData.PhysicalAvailableBytes).ToPrettySize(1);

            this.UsedPhysicalMemoryInPercent = (double)(this.performanceInfoData.PhysicalTotalBytes - this.performanceInfoData.PhysicalAvailableBytes) / this.performanceInfoData.PhysicalTotalBytes * 100;
            this.UsedPhysicalMemory = (this.performanceInfoData.PhysicalTotalBytes - this.performanceInfoData.PhysicalAvailableBytes).ToPrettySize(1);
            this.FreePhysicalMemoryInPercent = (double)100 - this.UsedPhysicalMemoryInPercent;
            this.FreePhysicalMemory = this.performanceInfoData.PhysicalAvailableBytes.ToPrettySize(1);

            if (this.CurrentRAMWorkloadSeries != null && this.CurrentRAMWorkloadSeries.Count == 0)
            {
                this.CurrentRAMWorkloadSeries.Add(new ChartDataPoint() { Name = "Used Physical Memory", Value = this.UsedPhysicalMemoryInPercent });
            }
            else
            {
                this.CurrentRAMWorkloadSeries.FirstOrDefault().Value = this.UsedPhysicalMemoryInPercent;
            }
        }

        #endregion Private methods

        #region Dependency Properties
        
        /// <summary>
        /// Title visibility
        /// </summary>
        public Visibility TitleVisiblility
        {
            get { return (Visibility)GetValue(TitleVisiblilityProperty); }
            set { SetValue(TitleVisiblilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleVisiblility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleVisiblilityProperty =
            DependencyProperty.Register("TitleVisiblility", typeof(Visibility), typeof(RAMInformationUserControl), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Details button visiblility
        /// </summary>
        public Visibility DetailsButtonVisibility
        {
            get { return (Visibility)GetValue(DetailsButtonVisibilityProperty); }
            set { SetValue(DetailsButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DetailsButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetailsButtonVisibilityProperty =
            DependencyProperty.Register("DetailsButtonVisibility", typeof(Visibility), typeof(RAMInformationUserControl), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Used physical memory
        /// </summary>
        public double UsedPhysicalMemoryInPercent
        {
            get { return (double)GetValue(UsedPhysicalMemoryInPercentProperty); }
            set { SetValue(UsedPhysicalMemoryInPercentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UsedPhysicalMemoryInPercent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsedPhysicalMemoryInPercentProperty =
            DependencyProperty.Register("UsedPhysicalMemoryInPercent", typeof(double), typeof(RAMInformationUserControl), new PropertyMetadata(default(double)));

        /// <summary>
        /// Free pyhsical memory
        /// </summary>
        public double FreePhysicalMemoryInPercent
        {
            get { return (double)GetValue(FreePhysicalMemoryInPercentProperty); }
            set { SetValue(FreePhysicalMemoryInPercentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FreePhysicalMemoryInPercent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FreePhysicalMemoryInPercentProperty =
            DependencyProperty.Register("FreePhysicalMemoryInPercent", typeof(double), typeof(RAMInformationUserControl), new PropertyMetadata(default(double)));

        /// <summary>
        /// Free physical memory
        /// </summary>
        public string FreePhysicalMemory
        {
            get { return (string)GetValue(FreePhysicalMemoryProperty); }
            set { SetValue(FreePhysicalMemoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FreePhysicalMemory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FreePhysicalMemoryProperty =
            DependencyProperty.Register("FreePhysicalMemory", typeof(string), typeof(RAMInformationUserControl), new PropertyMetadata("n.a."));
        
        /// <summary>
        /// Used physical memory in percent
        /// </summary>
        public string UsedPhysicalMemory
        {
            get { return (string)GetValue(UsedPhysicalMemoryProperty); }
            set { SetValue(UsedPhysicalMemoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UsedPhysicalMemory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsedPhysicalMemoryProperty =
            DependencyProperty.Register("UsedPhysicalMemory", typeof(string), typeof(RAMInformationUserControl), new PropertyMetadata("n.a."));
        
        /// <summary>
        /// Current RAM workload
        /// </summary>
        public ObservableCollection<ChartDataPoint> CurrentRAMWorkloadSeries
        {
            get { return (ObservableCollection<ChartDataPoint>)GetValue(CurrentRAMWorkloadSeriesProperty); }
            set { SetValue(CurrentRAMWorkloadSeriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentRAMWorkloadSeries.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentRAMWorkloadSeriesProperty =
            DependencyProperty.Register("CurrentRAMWorkloadSeries", typeof(ObservableCollection<ChartDataPoint>), typeof(RAMInformationUserControl), new PropertyMetadata(new ObservableCollection<ChartDataPoint>()));

        #endregion Dependecy Properties
    }
}