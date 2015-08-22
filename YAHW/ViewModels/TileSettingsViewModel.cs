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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using XAHW.Interfaces;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Helper;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the tile settings page
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
    /// <para>Date: 14.08.2015</para>
    /// </summary>
    public class TileSettingsViewModel : ViewModelBase
    {
        #region Members and Constants

        private IOpenHardwareMonitorManagementService openHardwareMonitorManagementDebugService = null;

        private IConfigurationFile configFile = null;

        #endregion Members and Constants

        /// <summary>
        /// CTOR
        /// </summary>
        public TileSettingsViewModel()
        {
            this.configFile = DependencyFactory.Resolve<IConfigurationFile>(ConfigFileNames.ApplicationConfig);
            this.openHardwareMonitorManagementDebugService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementDebugService);

            if (this.configFile != null)
            {
                this.SelectedColorForCpuTiles = ColorHelper.GetColorFromString(configFile.Sections["TileSettings"].Settings["CpuTilesColor"].Value);
                this.SelectedColorForGpuTiles = ColorHelper.GetColorFromString(configFile.Sections["TileSettings"].Settings["GpuTilesColor"].Value);
                this.SelectedColorForMainboardTiles = ColorHelper.GetColorFromString(configFile.Sections["TileSettings"].Settings["MainboardTilesColor"].Value);
            }
        }

        private Color selectedColorForCpuTiles;

        /// <summary>
        /// Sected color for CPU-Tiles
        /// </summary>
        public Color SelectedColorForCpuTiles
        {
            get { return selectedColorForCpuTiles; }
            set
            {
                if (this.SetProperty<Color>(ref this.selectedColorForCpuTiles, value))
                {
                    this.configFile.Sections["TileSettings"].Settings["CpuTilesColor"].Value = value.ToString();
                    this.configFile.Save();
                }
            }
        }

        private Color selectedColorForGpuTiles;

        /// <summary>
        /// Selected color for GPU-Tiles
        /// </summary>
        public Color SelectedColorForGpuTiles
        {
            get { return selectedColorForGpuTiles; }
            set
            {
                if (this.SetProperty<Color>(ref this.selectedColorForGpuTiles, value))
                {
                    this.configFile.Sections["TileSettings"].Settings["GpuTilesColor"].Value = value.ToString();
                    this.configFile.Save();
                }
            }
        }

        private Color selectedColorForMainboardTiles;

        /// <summary>
        /// Selected color for mainboard tiles
        /// </summary>
        public Color SelectedColorForMainboardTiles
        {
            get { return selectedColorForMainboardTiles; }
            set
            {
                if (this.SetProperty<Color>(ref this.selectedColorForMainboardTiles, value))
                {
                    this.configFile.Sections["TileSettings"].Settings["MainboardTilesColor"].Value = value.ToString();
                    this.configFile.Save();
                }
            }
        }
        
        /// <summary>
        /// CPU-Temperature Sensor
        /// </summary>
        public ISensor CPUTemperatureSensor
        {
            get
            {
                return this.openHardwareMonitorManagementDebugService.CPUTemperatureSensor;
            }
        }

    }
}
