// YAHW - Yet Another Hardware Monitor
// Copyright (c) 2015 Steffen Steinbrecher
// Contact and Information: http://csharp-blog.de/category/yahw/
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE

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
using YAHW.Events;
using Prism.Events;

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

        private DateTime time;
        private IOpenHardwareMonitorManagementService openHardwareManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public CPUInformationViewModel()
        {
            // Setup CPU-Plot
            this.SetupCpuPlot();

            // Get services
            this.openHardwareManagementService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
            this.CPUInformation = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetProcessorInformation();

            // Register for events
            DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<OpenHardwareMonitorManagementServiceTimerTickEvent>().Subscribe(this.OpenHardwareMonitorManagementServiceTimerTickEventHandler, ThreadOption.UIThread);
        }

        #endregion CTOR

        /// <summary>
        /// Setup CPU-Plot
        /// </summary>
        private void SetupCpuPlot()
        {
            this.time = DateTime.Now;

            // Create instance
            this.CPUPlot = new PlotModel();

            // Add Y-axis
            this.CPUPlot.Axes.Add(new LinearAxis()
                                    {
                                        IsZoomEnabled = false,
                                        Maximum = 102,
                                        Minimum = 0,
                                        MajorGridlineStyle = LineStyle.Solid,
                                        MinorGridlineStyle = LineStyle.Dot,
                                        Position = AxisPosition.Left
                                    });

            // Add X-axis
            this.CPUPlot.Axes.Add(new DateTimeAxis()
                                    {
                                        IsZoomEnabled = false,
                                        Position = AxisPosition.Bottom,
                                        IsAxisVisible = false
                                    });

            // Create line series to visualize the values
            var areaSeries = new AreaSeries()
            {
                StrokeThickness = 1,
                LineStyle = OxyPlot.LineStyle.Solid,
                Color = OxyColors.Blue,
            };

            // Fill series with initial values
            for (int i = 0; i < 60; i++)
            {
                areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time.Subtract(new TimeSpan(0, 0, 60 - i))), 0));
            }

            // Add to plot
            this.CPUPlot.Series.Add(areaSeries);
        }

        #region EventHandler

        /// <summary>
        /// Timer-Tick-Event of the OHM-Service
        /// </summary>
        /// <param name="args"></param>
        private void OpenHardwareMonitorManagementServiceTimerTickEventHandler(OpenHardwareMonitorManagementServiceTimerTickEventArgs args)
        {
            var areaSeries = (AreaSeries)this.CPUPlot.Series[0];

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
            double percentage = default(double);
            if (this.openHardwareManagementService.CPUWorkloadSensor.Value != null && this.openHardwareManagementService.CPUWorkloadSensor.Value.HasValue)
                percentage = (double)this.openHardwareManagementService.CPUWorkloadSensor.Value;

            areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(this.time), percentage));
            areaSeries.Points2.Add(DateTimeAxis.CreateDataPoint(this.time, 0));

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