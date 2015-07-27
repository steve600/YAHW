using System;
using System.Collections.Generic;
using System.Linq;
using YAHW.BaseClasses;
using YAHW.Model;
using YAHW.Constants;
using YAHW.Interfaces;
using System.Management;
using YAHW.SystemInformation;
using System.Windows.Threading;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using YAHW.ExtensionMethods;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the RAM-Information page
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
    public class RAMInformationViewModel : ViewModelBase
    {
        #region Members and Constants

        private PerfomanceInfoData performanceInfoData = null;

        private DispatcherTimer timer = null;
        private DateTime time;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public RAMInformationViewModel()
        {
            this.SetupRamPlot();

            this.timer = new DispatcherTimer();

            this.MemoryBanks = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetPhysicalMemoryInformation();

            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        #endregion CTOR

        #region Private methods

        /// <summary>
        /// Get physical memory information
        /// </summary>
        private void GetMemoryInformaton()
        {
            this.performanceInfoData = PsApiWrapper.GetPerformanceInfo();

            var AvailableGb = this.performanceInfoData.PhysicalAvailableBytes.ToPrettySize(1);
            var UsedGb = (this.performanceInfoData.PhysicalTotalBytes - this.performanceInfoData.PhysicalAvailableBytes).ToPrettySize(1);

            this.UsedPhysicalMemoryInPercent = (double)(this.performanceInfoData.PhysicalTotalBytes - this.performanceInfoData.PhysicalAvailableBytes) / this.performanceInfoData.PhysicalTotalBytes * 100;
            this.FreePhysicalMemoryInPercent = (double)100 - this.UsedPhysicalMemoryInPercent;

            this.OnPropertyChanged(() => this.UsedPhysicalMemory);
            this.OnPropertyChanged(() => this.FreePhysicalMemory);
        }

        /// <summary>
        /// Get physical memory information with WMI
        /// </summary>
        private void GetMemoryInformationWithWmi()
        {
            var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

            var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new
            {
                FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
            }).FirstOrDefault();

            if (memoryValues != null)
            {
                var percent = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
            }
        }

        /// <summary>
        /// Setup RAM-Plot
        /// </summary>
        private void SetupRamPlot()
        {
            this.time = DateTime.Now;

            this.RAMPlot = new PlotModel();

            this.RAMPlot.Axes.Add(new LinearAxis()
            {
                IsZoomEnabled = false,
                Maximum = 102,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Left
            });

            //this.CPUPlot.Axes.Add(new LinearAxis()
            //{
            //    IsZoomEnabled = false,
            //    Maximum = 102,
            //    Minimum = 0,
            //    MajorGridlineStyle = LineStyle.Solid,
            //    MinorGridlineStyle = LineStyle.Dot,
            //    Position = AxisPosition.Right
            //});

            this.RAMPlot.Axes.Add(new DateTimeAxis()
            {
                IsZoomEnabled = false,
                Position = AxisPosition.Bottom,
                IsAxisVisible = false
            });

            var areaSeries = new LineSeries()
            {
                StrokeThickness = 1,
                LineStyle = OxyPlot.LineStyle.Solid,
                Color = OxyColors.Blue,
                //Color2 = OxyColors.Transparent,
                //Fill = OxyColor.FromRgb(214, 231, 242),
                //DataFieldX2 = "X",
                //ConstantY2 = 0
            };

            // Fill series with initial values
            for (int i = 0; i < 60; i++)
            {
                areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time.Subtract(new TimeSpan(0, 0, 60 - i))), 0));
            }

            this.RAMPlot.Series.Add(areaSeries);
        }

        #endregion Private methods

        #region Event-Handler

        /// <summary>
        /// Timer-Tick Event-Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            var areaSeries = (LineSeries)this.RAMPlot.Series[0];

            if (areaSeries.Points.Count > 60)
            {
                areaSeries.Points.RemoveAt(0);
            }

            // Update-Plot
            this.GetMemoryInformaton();

            areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(this.time), this.UsedPhysicalMemoryInPercent));
            time = time.AddSeconds(1);

            this.RAMPlot.InvalidatePlot(true);
        }

        #endregion Event-Handler

        #region Properties

        private PlotModel ramPlot;

        /// <summary>
        /// The CPU-Plot
        /// </summary>
        public PlotModel RAMPlot
        {
            get { return ramPlot; }
            set { this.SetProperty<PlotModel>(ref this.ramPlot, value); }
        }

        private IList<RAMInformation> memoryBanks;

        /// <summary>
        /// RAM-Banks
        /// </summary>
        public IList<RAMInformation> MemoryBanks
        {
            get { return memoryBanks; }
            set { this.SetProperty<IList<RAMInformation>>(ref this.memoryBanks, value); }
        }

        /// <summary>
        /// Free physical memory as formatted string
        /// </summary>
        public string FreePhysicalMemory
        {
            get
            {
                if (this.performanceInfoData != null)
                    return this.performanceInfoData.PhysicalAvailableBytes.ToPrettySize(1);

                return "n.a.";
            }
        }
        
        private double freePhysicalMemoryInPercent;

        /// <summary>
        /// Free pyhsical memory in percent
        /// </summary>
        public double FreePhysicalMemoryInPercent
        {
            get { return freePhysicalMemoryInPercent; }
            set { this.SetProperty<double>(ref this.freePhysicalMemoryInPercent, value); }
        }

        /// <summary>
        /// Used physical memory as formatted string
        /// </summary>
        public string UsedPhysicalMemory
        {
            get
            {
                if (this.performanceInfoData != null)
                    return (this.performanceInfoData.PhysicalTotalBytes - this.performanceInfoData.PhysicalAvailableBytes).ToPrettySize(1);

                return "n.a.";
            }
        }

        private double usedPhysicalMemoryInPercent;

        /// <summary>
        /// Used memory in percent
        /// </summary>
        public double UsedPhysicalMemoryInPercent
        {
            get { return usedPhysicalMemoryInPercent; }
            set { this.SetProperty<double>(ref this.usedPhysicalMemoryInPercent, value); }
        }
        
        #endregion Properties
    }
}