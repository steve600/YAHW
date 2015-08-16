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

using OpenHardwareMonitor.Hardware;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Windows.Threading;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.EventAggregator;
using YAHW.Events;
using YAHW.Interfaces;
using YAHW.Model;

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
    public class GPUInformationViewModel : ViewModelBase
    {
        #region Members and Constants

        private DateTime time;
        private IOpenHardwareMonitorManagementService openHardwareManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public GPUInformationViewModel()
        {
            // Setup the GPU-Plot
            this.SetupGPUCoreWorkloadPlot();
            this.SetupGPUCoreTemperaturePlot();

            // Get GPU-Information
            this.openHardwareManagementService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
            this.GPUInformations = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetGPUInformation();

            // Register for events
            DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<OpenHardwareMonitorManagementServiceTimerTickEvent>().Subscribe(this.OpenHardwareMonitorManagementServiceTimerTickEventHandler, ThreadOption.UIThread);
        }

        #endregion CTOR

        #region EventHandler

        /// <summary>
        /// Timer-Tick-Event of the OHM-Service
        /// </summary>
        /// <param name="args"></param>
        private void OpenHardwareMonitorManagementServiceTimerTickEventHandler(OpenHardwareMonitorManagementServiceTimerTickEventArgs args)
        {
            // Update values
            this.OnPropertyChanged(() => this.GPUCoreClockSpeed);
            this.OnPropertyChanged(() => this.GPUCoreTemperature);
            this.OnPropertyChanged(() => this.GPUCoreWorkload);
            this.OnPropertyChanged(() => this.GPUMemoryClockSpeed);

            this.UpdateGPUCoreWorkloadPlot();
            this.UpdateGPUCoreTemperaturePlot();
            time = time.AddSeconds(1);
        }

        #endregion EventHandler

        /// <summary>
        /// Setup GPU-Core-Workload-Plot
        /// </summary>
        private void SetupGPUCoreWorkloadPlot()
        {
            this.time = DateTime.Now;

            this.GPUCoreWorkloadPlot = new PlotModel();

            this.GPUCoreWorkloadPlot.Axes.Add(new LinearAxis()
            {
                IsZoomEnabled = false,
                Maximum = 102,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Left
            });
            
            this.GPUCoreWorkloadPlot.Axes.Add(new DateTimeAxis()
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

            this.GPUCoreWorkloadPlot.Series.Add(areaSeries);
        }

        /// <summary>
        /// Setup GPU-Core-Temperature-Plot
        /// </summary>
        private void SetupGPUCoreTemperaturePlot()
        {
            this.time = DateTime.Now;

            this.GPUCoreTemperaturePlot = new PlotModel();

            this.GPUCoreTemperaturePlot.Axes.Add(new LinearAxis()
            {
                IsZoomEnabled = false,
                Maximum = 80,
                Minimum = 0,
                MajorStep = 15,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Left
            });

            this.GPUCoreTemperaturePlot.Axes.Add(new DateTimeAxis()
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

            this.GPUCoreTemperaturePlot.Series.Add(areaSeries);
        }

        /// <summary>
        /// Update the GPU-Workload-Plot
        /// </summary>
        private void UpdateGPUCoreWorkloadPlot()
        {
            var areaSeries = (LineSeries)this.GPUCoreWorkloadPlot.Series[0];

            if (areaSeries.Points.Count > 60)
            {
                areaSeries.Points.RemoveAt(0);
            }

            // Update-Plot
            double percentage = default(double);
            if (this.openHardwareManagementService.GPUCoreWorkloadSensor.Value != null && this.openHardwareManagementService.GPUCoreWorkloadSensor.Value.HasValue)
                percentage = (double)this.openHardwareManagementService.GPUCoreWorkloadSensor.Value;

            areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(this.time), percentage));
           
            this.GPUCoreWorkloadPlot.InvalidatePlot(true);
        }

        private void UpdateGPUCoreTemperaturePlot()
        {
            var areaSeries = (LineSeries)this.GPUCoreTemperaturePlot.Series[0];

            if (areaSeries.Points.Count > 60)
            {
                areaSeries.Points.RemoveAt(0);
            }

            // Update-Plot
            double temperature = default(double);
            if (this.openHardwareManagementService.GPUCoreTemperatureSensor.Value != null && this.openHardwareManagementService.GPUCoreTemperatureSensor.Value.HasValue)
                temperature = (double)this.openHardwareManagementService.GPUCoreTemperatureSensor.Value;

            areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(this.time), temperature));

            this.GPUCoreTemperaturePlot.InvalidatePlot(true);
        }

        #region Properties

        private PlotModel gpuCoreWorkloadPlot;

        /// <summary>
        /// The GPU-Plot
        /// </summary>
        public PlotModel GPUCoreWorkloadPlot
        {
            get { return gpuCoreWorkloadPlot; }
            set { this.SetProperty<PlotModel>(ref this.gpuCoreWorkloadPlot, value); }
        }

        private PlotModel gpuCoreTemperaturePlot;

        /// <summary>
        /// The CPU-Core-Temperature-Plot
        /// </summary>
        public PlotModel GPUCoreTemperaturePlot
        {
            get { return gpuCoreTemperaturePlot; }
            set { this.SetProperty<PlotModel>(ref this.gpuCoreTemperaturePlot, value); }
        }
        
        private GPUInformation gpuInformations;

        /// <summary>
        /// GPU-Information
        /// </summary>
        public GPUInformation GPUInformations
        {
            get { return gpuInformations; }
            set { this.SetProperty<GPUInformation>(ref this.gpuInformations, value); }
        }

        /// <summary>
        /// GPU-Core-Workload
        /// </summary>
        public ISensor GPUCoreWorkload
        {
            get { return this.openHardwareManagementService.GPUCoreWorkloadSensor; }
        }

        /// <summary>
        /// GPU-Core-Temperature
        /// </summary>
        public ISensor GPUCoreTemperature
        {
            get { return this.openHardwareManagementService.GPUCoreTemperatureSensor; }
        }

        /// <summary>
        /// GPU-Core-ClockSpeed
        /// </summary>
        public ISensor GPUCoreClockSpeed
        {
            get { return this.openHardwareManagementService.GPUCoreClockSpeedSensor; }
        }

        /// <summary>
        /// GPU-Memory clock speed
        /// </summary>
        public ISensor GPUMemoryClockSpeed
        {
            get { return this.openHardwareManagementService.GPUMemoryClockSpeedSensor; }
        }

        #endregion Properties
    }
}
