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
    public class OpenHardwareMonitorManagementServiceDebug : OpenHardwareMonitorManagementService 
    {
        #region Members and Constants
        #endregion Members and Constants

        /// <summary>
        /// CTOR
        /// </summary>
        public OpenHardwareMonitorManagementServiceDebug()
        {
        }

        private ISensor getSimulatedSensor(Func<SensorType, string, ISensor> hardwareSensorSource, SensorType sensorType, string sensorName)
        {
            ISensor s = hardwareSensorSource(sensorType, sensorName);
            if (s != null)
                return s;

            SimulatedSensor sim = new SimulatedSensor();
            sim.SensorType = sensorType;
            sim.Name = sensorName;
            return sim;
        }

        #region Mainboard-Properties



        /// <summary>
        /// Mainboard IO-Hardware
        /// </summary>
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

        /// <summary>
        /// The CPU-VCore
        /// </summary>
        public ISensor MainboardCPUVCore
        {
            get
            {
                return getSimulatedSensor(base.GetMainboardSensor, SensorType.Voltage, "CPU VCore");
            }
        }

       

        /// <summary>
        /// Mainboard AVCC
        /// </summary>
        public ISensor MainboardAVCC
        {
            get
            {
                return getSimulatedSensor(base.GetMainboardSensor, SensorType.Voltage, "AVCC");
            }
        }

        /// <summary>
        /// Mainboard 3VCC
        /// </summary>
        public ISensor Mainboard3VCC
        {
            get
            {
                return getSimulatedSensor(base.GetMainboardSensor, SensorType.Voltage, "3VCC");
            }
        }

        /// <summary>
        /// Mainboard 3VSB
        /// </summary>
        public ISensor Mainboard3VSB
        {
            get
            {
                return getSimulatedSensor(base.GetMainboardSensor, SensorType.Voltage, "3VSB");
            }
        }

        /// <summary>
        /// Mainboard VBAT
        /// </summary>
        public ISensor MainboardVBAT
        {
            get
            {
                return getSimulatedSensor(base.GetMainboardSensor, SensorType.Voltage, "VBAT");
            }
        }

        /// <summary>
        /// Mainboard VTT
        /// </summary>
        public ISensor MainboardVTT
        {
            get
            {
                return getSimulatedSensor(base.GetMainboardSensor, SensorType.Voltage, "VTT");
            }
        }

        /// <summary>
        /// Mainboard CPU-Core Temperature
        /// </summary>
        public ISensor MainboardCPUCoreTemperature
        {
            get
            {
                return getSimulatedSensor(base.GetMainboardSensor, SensorType.Temperature, "CPU Core");
            }
        }

        /// <summary>
        /// Mainboard temperature sensors
        /// </summary>
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

        /// <summary>
        /// Fan control sensors
        /// </summary>
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

        #endregion Mainboard-Properties

        #region CPU-Properties

      
        /// <summary>
        /// CPU-Clock Speed
        /// </summary>
        public double CPUClockSpeed
        {
            get
            {
                var clockSpeed = this.CPUCoreClockSpeedSensors.Max(s => s.Value);

                return (clockSpeed != null) ? (double)clockSpeed : default(double);
            }
        }

        /// <summary>
        /// CPU-Workload Sensor (TOTAL)
        /// </summary>
        public ISensor CPUWorkloadSensor
        {
            get
            {
                return this.CPU.Sensors.Where(s => s.SensorType == SensorType.Load && s.Name.Equals("CPU Total")).FirstOrDefault();
            }
        }

        /// <summary>
        /// The CPU Power-Consumption Sensor
        /// </summary>
        public ISensor CPUPowerConsumptionSensor
        {
            get
            {
                return this.CPU.Sensors.Where(s => s.SensorType == SensorType.Power && s.Name.Equals("CPU Package")).FirstOrDefault();
            }
        }

        /// <summary>
        /// The CPU-Core Power-Consumption Sensor
        /// </summary>
        public ISensor CPUCorePowerConsumptionSensor
        {
            get
            {
                return this.CPU.Sensors.Where(s => s.SensorType == SensorType.Power && s.Name.Equals("CPU Cores")).FirstOrDefault();
            }
        }

        /// <summary>
        /// The CPU Temperature-Sensor
        /// </summary>
        public ISensor CPUTemperatureSensor
        {
            get
            {
                return this.CPU.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Name.Equals("CPU Package")).FirstOrDefault();
            }
        }

        /// <summary>
        /// CPU-Core Workload-Sensors
        /// </summary>
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

        /// <summary>
        /// CPU-Core Temperature-Sensors
        /// </summary>
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

        /// <summary>
        /// CPU-Core ClockSpeed-Sensors
        /// </summary>
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

        #endregion CPU-Properties

        #region GPU-Properties

        /// <summary>
        /// The GPU
        /// </summary>
        public IHardware GPU
        {
            get
            {
                int i = 5;
                if (base.GPU != default(IHardware))
                    return base.GPU;
                return new SimulatedGPU();
            }
        }

        /// <summary>
        /// GPU Manufactor Name
        /// </summary>
        public string GPUManufactorName
        {
            get
            {
                return
                   (this.GPU.GetType().Equals(HardwareType.GpuAti)) ? "AMD" : (this.GPU.GetType().Equals(HardwareType.GpuNvidia) ? "Nvidia" : "");
            }
        }

        /// <summary>
        /// GPU-Core Clock Speed
        /// </summary>
        public double GPUCoreClockSpeed
        {
            get
            {
                var clockSpeed = this.GPUCoreClockSpeedSensors.Max(s => s.Value);

                return (clockSpeed != null) ? (double)clockSpeed : default(double);
            }
        }

        /// <summary>
        /// GPU-Memory Clock Speed
        /// </summary>
        public double GPUMemoryClockSpeed
        {
            get
            {
                var clockSpeed = this.GPUMemoryClockSpeedSensors.Max(s => s.Value);

                return (clockSpeed != null) ? (double)clockSpeed : default(double);
            }
        }

        /// <summary>
        /// GPU-Workload Sensor (TOTAL)
        /// </summary>
        public ISensor GPUWorkloadSensor
        {
            get
            {
                return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Load && s.Name.Equals("GPU Total")).FirstOrDefault();
            }
        }

        /// <summary>
        /// The GPU-Core Power-Consumption Sensor
        /// </summary>
        public ISensor GPUCorePowerConsumptionSensor
        {
            get
            {
                return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Power && s.Name.Equals("GPU Cores")).FirstOrDefault();
            }
        }

        /// <summary>
        /// The GPU Temperature-Sensor
        /// </summary>
        public ISensor GPUTemperatureSensor
        {
            get
            {
                return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Name.Equals("GPU Package")).FirstOrDefault();
            }
        }

        /// <summary>
        /// GPU-Core Workload-Sensors
        /// </summary>
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

        /// <summary>
        /// GPU-Core Temperature-Sensors
        /// </summary>
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
                return new ObservableCollection<ISensor>( simulatedSensors);
            }
        }

        /// <summary>
        /// GPU-Core ClockSpeed-Sensors
        /// </summary>
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

        /// <summary>
        /// GPU-Memory ClockSpeed-Sensors
        /// </summary>
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

        /// <summary>
        /// GPU FanSpeed-Sensors
        /// </summary>
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

        #endregion GPU-Properties
    }
}