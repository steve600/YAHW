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

        private OpenHardwareMonitorManagementService openHardwareMonitorManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public MainboardInformationViewModel()
        {
            this.MainboardInformation = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetMainboardInformation();

            this.openHardwareMonitorManagementService = DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
            this.openHardwareMonitorManagementService.UpdateMainboardSensors();
        }

        #endregion CTOR

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

        /// <summary>
        /// CPU-VCore
        /// </summary>
        public ISensor MainboardCPUVCore
        {
            get
            {
                return this.openHardwareMonitorManagementService.MainboardCPUVCore;
            }
        }

        /// <summary>
        /// Mainboard AVCC
        /// </summary>
        public ISensor MainboardAVCC
        {
            get
            {
                return this.openHardwareMonitorManagementService.MainboardAVCC;
            }
        }

        /// <summary>
        /// Mainboard 3VCC
        /// </summary>
        public ISensor Mainboard3VCC
        {
            get
            {
                return this.openHardwareMonitorManagementService.Mainboard3VCC;
            }
        }

        /// <summary>
        /// Mainboard 3VSB
        /// </summary>
        public ISensor Mainboard3VSB
        {
            get
            {
                return this.openHardwareMonitorManagementService.Mainboard3VSB;
            }
        }

        /// <summary>
        /// Mainboard VBAT
        /// </summary>
        public ISensor MainboardVBAT
        {
            get
            {
                return this.openHardwareMonitorManagementService.MainboardVBAT;
            }
        }

        /// <summary>
        /// Mainboard VTT
        /// </summary>
        public ISensor MainboardVTT
        {
            get
            {
                return this.openHardwareMonitorManagementService.MainboardVTT;
            }
        }

        /// <summary>
        /// List with mainboard temperature sensors
        /// </summary>
        public IList<ISensor> MainboardTemperatureSensors
        {
            get
            {
                return this.openHardwareMonitorManagementService.MainboardTemperatureSensors;
            }
        }

        #endregion Properties
    }
}