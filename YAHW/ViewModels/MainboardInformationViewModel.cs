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

using System.Collections.Generic;
using YAHW.BaseClasses;
using YAHW.Services;
using YAHW.Constants;
using OpenHardwareMonitor.Hardware;
using YAHW.Model;
using YAHW.Interfaces;
using YAHW.UserControls;
using YAHW.EventAggregator;
using YAHW.Events;

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
    public class MainboardInformationViewModel : ViewModelBase
    {
        #region Members and Constants

        private IOpenHardwareMonitorManagementService openHardwareMonitorManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public MainboardInformationViewModel()
        {
            this.MainboardInformation = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetMainboardInformation();

            this.openHardwareMonitorManagementService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);

            if (this.openHardwareMonitorManagementService != null)
            {
                if (this.openHardwareMonitorManagementService.MainboardVoltageSensorsWithName != null)
                {
                    foreach (var vs in this.openHardwareMonitorManagementService.MainboardVoltageSensorsWithName)
                    {
                        SensorTile st = new SensorTile();
                        st.HardwareSensor = vs;
                        this.MainboardVoltageSensors.Add(st);
                    }
                }

                if (this.openHardwareMonitorManagementService.MainboardTemperatureSensors != null)
                {
                    foreach (var ts in this.openHardwareMonitorManagementService.MainboardTemperatureSensors)
                    {
                        SensorTile st = new SensorTile();
                        st.HardwareSensor = ts;
                        this.MainboardTemperatureSensors.Add(st);
                    }
                }
            }

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
            foreach (var st in this.MainboardVoltageSensors)
                st.UpdateValues();

            foreach (var st in this.MainboardTemperatureSensors)
                st.UpdateValues();
        }

        #endregion EventHandler

        #region Properties

        private MainboardInformation mainboardInformation;

        /// <summary>
        /// Mainboard information
        /// </summary>
        public MainboardInformation MainboardInformation
        {
            get { return mainboardInformation; }
            private set { this.SetProperty<MainboardInformation>(ref this.mainboardInformation, value); }
        }

        /// <summary>
        /// Mainboard IO-Hardware
        /// </summary>
        public IHardware MainboardIOHardware
        {
            get
            {
                return this.openHardwareMonitorManagementService.MainboardIOHardware;
            }
        }

        private IList<SensorTile> mainboardTemperatureSensors = new List<SensorTile>();

        /// <summary>
        /// List with mainboard temperature sensors
        /// </summary>
        public IList<SensorTile> MainboardTemperatureSensors
        {
            get
            {
                return this.mainboardTemperatureSensors;
            }
        }

        private IList<SensorTile> mainboardVoltagedSensors = new List<SensorTile>();

        /// <summary>
        /// List with mainobard voltage sensors
        /// </summary>
        public IList<SensorTile> MainboardVoltageSensors
        {
            get
            {
                return mainboardVoltagedSensors;
            }
        }

        #endregion Properties
    }
}