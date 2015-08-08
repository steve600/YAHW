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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using YAHW.Interfaces;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// Wrapper for the Open Hardware Monitor Library
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
    public class OpenHardwareMonitorManagementService : IOpenHardwareMonitorManagementService
    {
        #region Members and Constants

        private Computer observedComputer = null;

        private UpdateVisitor updateVisitor = new UpdateVisitor();

        #endregion Members and Constants

        #region Constructors

        /// <summary>
        /// CTOR
        /// </summary>
        public OpenHardwareMonitorManagementService()
        {
            this.observedComputer = new Computer();
            this.observedComputer.FanControllerEnabled = true;
            this.observedComputer.CPUEnabled = true;
            this.observedComputer.MainboardEnabled = true;
            this.observedComputer.RAMEnabled = true;
            this.observedComputer.GPUEnabled = true;
            this.observedComputer.HDDEnabled = true;

            this.observedComputer.Open();
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc />
        public void AcceptNewSettings()
        {
            this.observedComputer.Accept(this.updateVisitor);
        }

        /// <inheritdoc />
        public void Close()
        {
            if (this.observedComputer != null)
                this.observedComputer.Close();
        }
        /// <inheritdoc />
        public void UpdateMainboardSensors()
        {
            if (this.Mainboard != null)
            {
                this.Mainboard.Update();
                foreach (var h in this.Mainboard.SubHardware)
                    h.Update();
            }
        }

        /// <summary>
        /// Get mainboard sensor
        /// </summary>
        /// <param name="sensorType">The sensor type</param>
        /// <param name="sensorName">The sensor name</param>
        /// <returns></returns>
        private ISensor GetMainboardSensor(SensorType sensorType, string sensorName)
        {
            if (this.MainboardIOHardware != null)
            {
                return this.MainboardIOHardware.Sensors.Where(s => s.SensorType == sensorType && s.Name.Equals(sensorName)).FirstOrDefault();
            }

            return null;
        }

        #endregion Methods

        #region Mainboard-Properties

        /// <inheritdoc />
        public IHardware Mainboard
        {
            get
            {
                return this.observedComputer.Hardware.Where(m => m.HardwareType == HardwareType.Mainboard).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public ISensor Mainboard3VCC
        {
            get
            {
                return this.GetMainboardSensor(SensorType.Voltage, "3VCC");
            }
        }

        /// <inheritdoc />
        public ISensor Mainboard3VSB
        {
            get
            {
                return this.GetMainboardSensor(SensorType.Voltage, "3VSB");
            }
        }

        /// <inheritdoc />
        public ISensor MainboardAVCC
        {
            get
            {
                return this.GetMainboardSensor(SensorType.Voltage, "AVCC");
            }
        }

        /// <inheritdoc />
        public ISensor MainboardCPUCoreTemperature
        {
            get
            {
                return this.GetMainboardSensor(SensorType.Temperature, "CPU Core");
            }
        }

        /// <inheritdoc />
        public ISensor MainboardCPUVCore
        {
            get
            {
                return this.GetMainboardSensor(SensorType.Voltage, "CPU VCore");
            }
        }

        /// <inheritdoc />
        public IList<ISensor> MainboardFanControlSensors
        {
            get
            {
                if (this.MainboardIOHardware != null)
                {
                    return MainboardIOHardware.Sensors.Where(s => s.SensorType == SensorType.Control && s.Name.Contains("Fan")).ToList();
                }

                return null;
            }
        }

        /// <inheritdoc />
        public IHardware MainboardIOHardware
        {
            get
            {
                IHardware result = null;

                if (this.Mainboard != null && this.Mainboard.SubHardware != null)
                {
                    result = this.Mainboard.SubHardware.Where(i => i.HardwareType == HardwareType.SuperIO).FirstOrDefault();
                }

                return result;
            }
        }
        /// <inheritdoc />
        public IList<ISensor> MainboardTemperatureSensors
        {
            get
            {
                if (this.Mainboard != null && this.Mainboard.SubHardware != null)
                {
                    var io = this.Mainboard.SubHardware.Where(i => i.HardwareType == HardwareType.SuperIO).FirstOrDefault();

                    if (io != null)
                    {
                        io.Update();
                        return io.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();
                    }
                }

                return null;
            }
        }

        /// <inheritdoc />
        public ISensor MainboardVBAT
        {
            get
            {
                return this.GetMainboardSensor(SensorType.Voltage, "VBAT");
            }
        }

        /// <inheritdoc />
        public ISensor MainboardVTT
        {
            get
            {
                return this.GetMainboardSensor(SensorType.Voltage, "VTT");
            }
        }
        #endregion Mainboard-Properties

        #region CPU-Properties

        /// <inheritdoc />
        public IHardware CPU
        {
            get
            {
                return this.observedComputer.Hardware.Where(p => p.HardwareType == HardwareType.CPU).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public double CPUClockSpeed
        {
            get
            {
                var clockSpeed = this.CPUCoreClockSpeedSensors.Max(s => s.Value);

                return (clockSpeed != null) ? (double)clockSpeed : default(double);
            }
        }

        /// <inheritdoc />
        public ObservableCollection<ISensor> CPUCoreClockSpeedSensors
        {
            get
            {
                var sensors = this.CPU.Sensors.Where(s => s.SensorType == SensorType.Clock && s.Name.Contains("Core"));
                if (sensors != null)
                {
                    return new ObservableCollection<ISensor>(sensors);
                }

                return new ObservableCollection<ISensor>();
            }
        }

        /// <inheritdoc />
        public ISensor CPUCorePowerConsumptionSensor
        {
            get
            {
                return this.CPU.Sensors.Where(s => s.SensorType == SensorType.Power && s.Name.Equals("CPU Cores")).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public ObservableCollection<ISensor> CPUCoreTemperatureSensors
        {
            get
            {
                var sensors = this.CPU.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Name.Contains("Core"));
                if (sensors != null)
                {
                    return new ObservableCollection<ISensor>(sensors);
                }

                return new ObservableCollection<ISensor>();
            }
        }

        /// <inheritdoc />
        public ObservableCollection<ISensor> CPUCoreWorkloadSensors
        {
            get
            {
                var sensors = this.CPU.Sensors.Where(s => s.SensorType == SensorType.Load && s.Name.Contains("Core"));
                if (sensors != null)
                {
                    return new ObservableCollection<ISensor>(sensors);
                }

                return new ObservableCollection<ISensor>();
            }
        }

        /// <inheritdoc />
        public ISensor CPUPowerConsumptionSensor
        {
            get
            {
                return this.CPU.Sensors.Where(s => s.SensorType == SensorType.Power && s.Name.Equals("CPU Package")).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public ISensor CPUTemperatureSensor
        {
            get
            {
                return this.CPU.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Name.Equals("CPU Package")).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public ISensor CPUWorkloadSensor
        {
            get
            {
                return this.CPU.Sensors.Where(s => s.SensorType == SensorType.Load && s.Name.Equals("CPU Total")).FirstOrDefault();
            }
        }
        #endregion CPU-Properties

        #region GPU-Properties

        /// <inheritdoc />
        public IHardware GPU
        {
            get
            {
                return this.observedComputer.Hardware.Where(p => (p.HardwareType == HardwareType.GpuAti || p.HardwareType == HardwareType.GpuNvidia)).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public double GPUCoreClockSpeed
        {
            get
            {
                var clockSpeed = this.GPUCoreClockSpeedSensors.Max(s => s.Value);

                return (clockSpeed != null) ? (double)clockSpeed : default(double);
            }
        }

        /// <inheritdoc />
        public ObservableCollection<ISensor> GPUCoreClockSpeedSensors
        {
            get
            {
                var sensors = this.GPU.Sensors.Where(s => s.SensorType == SensorType.Clock && s.Name.Contains("Core"));
                if (sensors != null)
                {
                    return new ObservableCollection<ISensor>(sensors);
                }

                return new ObservableCollection<ISensor>();
            }
        }

        /// <inheritdoc />
        public ISensor GPUCorePowerConsumptionSensor
        {
            get
            {
                return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Power && s.Name.Equals("GPU Cores")).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public ObservableCollection<ISensor> GPUCoreTemperatureSensors
        {
            get
            {
                var sensors = this.GPU.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Name.Contains("Core"));
                if (sensors != null)
                {
                    return new ObservableCollection<ISensor>(sensors);
                }

                return new ObservableCollection<ISensor>();
            }
        }

        /// <inheritdoc />
        public ObservableCollection<ISensor> GPUCoreWorkloadSensors
        {
            get
            {
                var sensors = this.GPU.Sensors.Where(s => s.SensorType == SensorType.Load && s.Name.Contains("Core"));
                if (sensors != null)
                {
                    return new ObservableCollection<ISensor>(sensors);
                }

                return new ObservableCollection<ISensor>();
            }
        }

        /// <inheritdoc />
        public ObservableCollection<ISensor> GPUFanSpeedSensors
        {
            get
            {
                var sensors = this.GPU.Sensors.Where(s => s.SensorType == SensorType.Fan && s.Name.Contains("GPU"));
                if (sensors != null)
                {
                    return new ObservableCollection<ISensor>(sensors);
                }

                return new ObservableCollection<ISensor>();
            }
        }

        /// <inheritdoc />
        public string GPUManufactorName
        {
            get
            {
                return
                   (this.GPU.GetType().Equals(HardwareType.GpuAti)) ? "AMD" : (this.GPU.GetType().Equals(HardwareType.GpuNvidia) ? "Nvidia" : "");
            }
        }
        /// <inheritdoc />
        public double GPUMemoryClockSpeed
        {
            get
            {
                var clockSpeed = this.GPUMemoryClockSpeedSensors.Max(s => s.Value);

                return (clockSpeed != null) ? (double)clockSpeed : default(double);
            }
        }

        /// <inheritdoc />
        public ObservableCollection<ISensor> GPUMemoryClockSpeedSensors
        {
            get
            {
                var sensors = this.GPU.Sensors.Where(s => s.SensorType == SensorType.Clock && s.Name.Contains("Memory"));
                if (sensors != null)
                {
                    return new ObservableCollection<ISensor>(sensors);
                }

                return new ObservableCollection<ISensor>();
            }
        }

        /// <inheritdoc />
        public ISensor GPUTemperatureSensor
        {
            get
            {
                return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Name.Equals("GPU Package")).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public ISensor GPUWorkloadSensor
        {
            get
            {
                return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Load && s.Name.Equals("GPU Total")).FirstOrDefault();
            }
        }
        #endregion GPU-Properties
    }
}