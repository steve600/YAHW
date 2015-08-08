﻿// YAHW - Yet Another Hardware Monitor
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
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Services;
using YAHW.UserControls;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the CPU-Core workload page
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
    public class CPUCoreTemperaturesViewModel : ViewModelBase
    {
        #region Members and Constants

        private DispatcherTimer timer = null;
        private IOpenHardwareMonitorManagementService openHardwareManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public CPUCoreTemperaturesViewModel()
        {
            this.timer = new DispatcherTimer();

            this.openHardwareManagementService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);

            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        #endregion CTOR

        #region Event-Handler

        /// <summary>
        /// Timer-Tick Event-Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            if (this.openHardwareManagementService.CPU != null)
            {
                // Update hardware item
                this.openHardwareManagementService.CPU.Update();

                // Get core workload
                foreach (var sensor in this.openHardwareManagementService.CPUCoreTemperatureSensors)
                {
                    var chart = (from r in this.MainContent.Children.OfType<CPUCoreTemperatureChartUserControl>()
                                 where r.CoreName == sensor.Name
                                 select r).FirstOrDefault();

                    if (chart == null)
                    {
                        var newChart = new CPUCoreTemperatureChartUserControl();
                        newChart.CoreName = sensor.Name;
                        newChart.CurrentCoreTemperature = (sensor.Value != null) ? (double)sensor.Value.Value : default(double);
                        this.MainContent.Children.Add(newChart);
                    }
                    else
                    {
                        chart.CurrentCoreTemperature = (sensor.Value != null) ? (double)sensor.Value.Value : default(double);
                    }
                }
            }
        }

        #endregion Event-Handler

        #region Properties

        private StackPanel mainContent = new StackPanel();

        /// <summary>
        /// Main-Content
        /// </summary>
        public StackPanel MainContent
        {
            get { return mainContent; }
            set { this.SetProperty<StackPanel>(ref this.mainContent, value); }
        }

        #endregion Properties
    }
}