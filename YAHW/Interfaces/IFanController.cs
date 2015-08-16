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
using System.Windows.Controls;
using YAHW.Model;

namespace YAHW.Interfaces
{
    /// <summary>
    /// <para>
    /// Interface for a fan controller service
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
    /// <para>Date: 08.08.2015</para>
    /// </summary>
    public interface IFanController
    {
        /// <summary>
        /// The sensor
        /// TODO: Own interface for Sensors (this Sensor is from OHM-Lib)
        /// </summary>
        string FanSensorName { get; }

        /// <summary>
        /// Update values
        /// </summary>
        void UpdateValues();

        /// <summary>
        /// Reference to the fan controller service
        /// </summary>
        IFanControllerService FanControllerService { get; }

        /// <summary>
        /// The selected temperature sensor
        /// </summary>
        ISensor SelectedTemperatureSensor { get; set; }

        /// <summary>
        /// The fan controller template
        /// </summary>
        FanControllerTemplate SelectedFanControllerTemplate { get; set; }

        /// <summary>
        /// Current value of the temperature sensor
        /// </summary>
        float SelectedTemperatureSensorCurrentValue { get; }

        /// <summary>
        /// Flag for default mode
        /// </summary>
        bool IsDefaultModeEnabled { get; set; }

        /// <summary>
        /// Flag for advanced mode
        /// </summary>
        bool IsAdvancedModeEnabled { get; set; }

        /// <summary>
        /// Selected fan speed value
        /// </summary>
        float SelectedFanSpeedValue { get; set; }

        /// <summary>
        /// Current fan speed value
        /// </summary>
        float CurrentFanSpeedValue { get; }

        /// <summary>
        /// Min fan speed value
        /// </summary>
        float MinFanSpeedValue { get; }

        /// <summary>
        /// Max fan speed value
        /// </summary>
        float MaxFanSpeedValue { get; }

        /// <summary>
        /// Can set fan speed
        /// </summary>
        bool CanSetFanSpeed { get; set; }

        /// <summary>
        /// The user control for the UI
        /// </summary>
        UserControl FanControllerUserControl { get; }
    }
}
