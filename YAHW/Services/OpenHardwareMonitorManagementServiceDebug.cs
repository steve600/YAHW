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
using System.Collections.ObjectModel;
using System.Linq;
using YAHW.Services.Simulated;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// Wrapper for the Open Hardware Monitor Library in Debug functionality
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
    /// <para>Author: No3x</para>
    /// <para>Date: 07.08.2015</para>
    /// </summary>
    public class OpenHardwareMonitorManagementServiceDebug : IOpenHardwareMonitorManagementService
    {
        #region Members and Constants

        private IHardware cpu;
        private IHardware gpu;
        private IHardware mainboard;
        #endregion Members and Constants

        #region Constructors

        /// <summary>
        /// CTOR
        /// </summary>
        public OpenHardwareMonitorManagementServiceDebug()
        {
            this.mainboard = new SimulatedMainboard();
            this.cpu = new SimulatedCPU();
            this.gpu = new SimulatedGPU();
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc />
        public void AcceptNewSettings()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Close()
        {
            this.mainboard = null;
            this.cpu = null;
            this.gpu = null;
        }

        /// <inheritdoc />
        public void UpdateMainboardSensors()
        {
            this.mainboard.Update();
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
                return this.mainboard;
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
                return this.cpu;
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
                return this.gpu;
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

                SimulatedSensor sim = new SimulatedSensor();
                sim.SensorType = SensorType.Temperature;
                sim.Name = "GPU Core";
                List<ISensor> simulatedSensors = new List<ISensor>(5);
                simulatedSensors.Add(sim);
                return new ObservableCollection<ISensor>(simulatedSensors);
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