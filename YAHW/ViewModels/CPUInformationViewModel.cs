using System;
using YAHW.BaseClasses;
using YAHW.Model;
using YAHW.Interfaces;
using YAHW.Constants;
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OpenHardwareMonitor.Hardware;
using YAHW.Services;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the CPU-Information page
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
    public class CPUInformationViewModel : ViewModelBase
    {
        #region Members and Constants

        private DispatcherTimer timer = null;
        private PerformanceCounter cpuCounter = null;

        private DateTime time;

        private OpenHardwareMonitorManagementService openHardwareManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public CPUInformationViewModel()
        {
            this.SetupCpuPlot();

            this.timer = new DispatcherTimer();

            this.cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            this.openHardwareManagementService = DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
            this.CPUInformation = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetProcessorInformation();

            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        #endregion CTOR

        /// <summary>
        /// Setup CPU-Plot
        /// </summary>
        private void SetupCpuPlot()
        {
            this.time = DateTime.Now;

            this.CPUPlot = new PlotModel();

            this.CPUPlot.Axes.Add(new LinearAxis()
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

            this.CPUPlot.Axes.Add(new DateTimeAxis()
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

            this.CPUPlot.Series.Add(areaSeries);
        }

        #region EventHandler

        void timer_Tick(object sender, EventArgs e)
        {
            var areaSeries = (LineSeries)this.CPUPlot.Series[0];

            if (areaSeries.Points.Count > 60)
            {
                areaSeries.Points.RemoveAt(0);
            }

            if (this.openHardwareManagementService.CPU != null)
            {
                // Update CPU-Values
                this.openHardwareManagementService.CPU.Update();

                // Get CPU-Temperature
                this.CPUTemperature = (this.openHardwareManagementService.CPUTemperatureSensor.Value != null) ? (double)this.openHardwareManagementService.CPUTemperatureSensor.Value : default(double);

                // Update values
                this.OnPropertyChanged(() => this.CPUPowerConsumption);
                this.OnPropertyChanged(() => this.CPUCoreWorkloadSensors);
                this.OnPropertyChanged(() => this.CPUCoreClockSpeedSensors);
                this.OnPropertyChanged(() => this.CPUCoreTemperatureSensors);
                this.OnPropertyChanged(() => this.CPUCorePowerConsumption);
            }

            // Update-Plot
            //double x = areaSeries.Points.Count > 0 ? areaSeries.Points[areaSeries.Points.Count - 1].X + 1 : 0;
            double percentage = (this.openHardwareManagementService.CPUWorkloadSensor.Value != null) ? (double)this.openHardwareManagementService.CPUWorkloadSensor.Value : default(double);

            areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(this.time), percentage));
            time = time.AddSeconds(1);

            this.CPUUtilization = percentage / 100;

            this.CPUClockSpeed = this.openHardwareManagementService.CPUClockSpeed;

            this.CPUPlot.InvalidatePlot(true);
        }

        #endregion EventHandler

        #region Properties

        private PlotModel cpuPlot;

        /// <summary>
        /// The CPU-Plot
        /// </summary>
        public PlotModel CPUPlot
        {
            get { return cpuPlot; }
            set { this.SetProperty<PlotModel>(ref this.cpuPlot, value); }
        }

        private ProcessorInformation cpuInformation;

        /// <summary>
        /// CPU-Information
        /// </summary>
        public ProcessorInformation CPUInformation
        {
            get { return cpuInformation; }
            set { this.SetProperty<ProcessorInformation>(ref this.cpuInformation, value); }
        }

        private double cpuUtilization;

        /// <summary>
        /// CPU-Utilization
        /// </summary>
        public double CPUUtilization
        {
            get { return cpuUtilization; }
            private set { this.SetProperty<double>(ref this.cpuUtilization, value); }
        }

        private double cpuTemperature;

        /// <summary>
        /// CPU-Temperature
        /// </summary>
        public double CPUTemperature
        {
            get { return cpuTemperature; }
            private set { this.SetProperty<double>(ref this.cpuTemperature, value); }
        }

        private double cpuClockSpeed;

        /// <summary>
        /// CPU-Clock speed
        /// </summary>
        public double CPUClockSpeed
        {
            get { return cpuClockSpeed; }
            private set { this.SetProperty<double>(ref this.cpuClockSpeed, value); }
        }

        /// <summary>
        /// CPU-Power Consumption
        /// </summary>
        public ISensor CPUPowerConsumption
        {
            get
            {
                return this.openHardwareManagementService.CPUPowerConsumptionSensor;
            }
        }

        /// <summary>
        /// CPU-Core Power-Consumption
        /// </summary>
        public ISensor CPUCorePowerConsumption
        {
            get
            {
                return this.openHardwareManagementService.CPUCorePowerConsumptionSensor;
            }
        }

        /// <summary>
        /// List with CPU-Core temperature sensors
        /// </summary>
        public ObservableCollection<ISensor> CPUCoreTemperatureSensors
        {
            get
            {
                return this.openHardwareManagementService.CPUCoreTemperatureSensors;
            }
        }

        /// <summary>
        /// List with CPU-Core workload sensors
        /// </summary>
        public ObservableCollection<ISensor> CPUCoreWorkloadSensors
        {
            get
            {
                return this.openHardwareManagementService.CPUCoreWorkloadSensors;
            }
        }

        /// <summary>
        /// List with CPU-Core clock speed
        /// </summary>
        public ObservableCollection<ISensor> CPUCoreClockSpeedSensors
        {
            get
            {
                return this.openHardwareManagementService.CPUCoreClockSpeedSensors;
            }
        }

        #endregion
    }
}