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
using System.Timers;
using YAHW.Constants;
using YAHW.EventAggregator;
using YAHW.Events;
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

        private Timer timer = null;

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
            this.observedComputer.GPUEnabled = true;
            this.observedComputer.HDDEnabled = true;

            this.observedComputer.Open();

            this.UpdateMainboardSensors();

            // Create timer
            this.timer = new Timer(1000);
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Update sensors
            if (this.CPU != null)
                this.CPU.Update();
            if (this.GPU != null)
                this.GPU.Update();

            this.UpdateMainboardSensors();

            // Fire event
            DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<OpenHardwareMonitorManagementServiceTimerTickEvent>().Publish(null);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Accept new settings for controllers, e.g. Fan-Controller
        /// </summary>
        public void AcceptNewSettings()
        {
            if (this.observedComputer != null)
                this.observedComputer.Accept(this.updateVisitor);
        }

        /// <summary>
        /// Close the observed computer
        /// </summary>
        public void Close()
        {
            if (this.observedComputer != null)
                this.observedComputer.Close();
        }

        /// <summary>
        /// Update mainboard sensors
        /// </summary>
        private void UpdateMainboardSensors()
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
        private ISensor GetMainboardSensor(string sensorName, string sensorType)
        {
            ISensor result = null;

            if (this.Mainboard != null)
            {
                var io = this.Mainboard.SubHardware.Where(sh => sh.HardwareType == HardwareType.SuperIO).FirstOrDefault();

                if (io.Sensors != null)
                {
                    result = io.Sensors.Where(s => s.Name.Equals(sensorName) && s.SensorType == this.GetSensorTypeByName(sensorType)).FirstOrDefault();
                }
            }

            return result;
        }

        /// <summary>
        /// Get a sensor
        /// </summary>
        /// <param name="sensorCategory">The sensor category, e.g. CPU</param>
        /// <param name="sensorName">The sensor name, e.g. CPU Package</param>
        /// <param name="sensorType">The sensor type, e.g. Load</param>
        /// <returns></returns>
        public ISensor GetSensor(string sensorCategory, string sensorName, string sensorType)
        {
            switch (sensorCategory)
            {
                case "CPU":
                    return this.GetCPUSensor(sensorName, sensorType);
                case "GPU":
                    return this.GetGPUSensor(sensorName, sensorType);
                case "Mainboard":
                    return this.GetMainboardSensor(sensorName, sensorType);
            }

            return null;
        }

        private SensorType GetSensorTypeByName(string sensorType)
        {
            switch (sensorType)
            {
                case "Load":
                    return SensorType.Load;
                case "Temperature":
                    return SensorType.Temperature;
                case "Clock":
                    return SensorType.Clock;
                case "Power":
                    return SensorType.Power;
                case "Voltage":
                    return SensorType.Voltage;
            }

            return SensorType.Load;
        }

        /// <summary>
        /// Get CPU-Sensor
        /// </summary>
        /// <param name="sensorName">The sensor name</param>
        /// <param name="sensorType">The sensor type</param>
        /// <returns></returns>
        private ISensor GetCPUSensor(string sensorName, string sensorType)
        {
            ISensor result = null;

            if (this.CPU != null)
            {
                if (CPU.Sensors != null)
                {
                    result = this.CPU.Sensors.Where(s => s.Name.Equals(sensorName) && s.SensorType == this.GetSensorTypeByName(sensorType)).FirstOrDefault();
                }
            }

            return result;
        }

        /// <summary>
        /// Get GPU-Sensor
        /// </summary>
        /// <param name="sensorName">The sensor name</param>
        /// <param name="sensorType">The sensor type</param>
        /// <returns></returns>
        private ISensor GetGPUSensor(string sensorName, string sensorType)
        {
            ISensor result = null;

            if (this.GPU != null)
            {
                if (GPU.Sensors != null)
                {
                    result = this.GPU.Sensors.Where(s => s.Name.Equals(sensorName) && s.SensorType == this.GetSensorTypeByName(sensorType)).FirstOrDefault();
                }
            }

            return result;
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

        /// <summary>
        /// Mainboard voltage sensors (ALL)
        /// </summary>
        public IList<ISensor> MainboardVoltageSensors
        {
            get
            {
                if (this.Mainboard != null && this.Mainboard.SubHardware != null)
                {
                    var io = this.Mainboard.SubHardware.Where(i => i.HardwareType == HardwareType.SuperIO).FirstOrDefault();

                    if (io != null)
                    {
                        return io.Sensors.Where(s => s.SensorType == SensorType.Voltage).ToList();
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Mainboard voltage sensors (ALL)
        /// </summary>
        public IList<ISensor> MainboardVoltageSensorsWithName
        {
            get
            {
                if (this.Mainboard != null && this.Mainboard.SubHardware != null)
                {
                    var io = this.Mainboard.SubHardware.Where(i => i.HardwareType == HardwareType.SuperIO).FirstOrDefault();

                    if (io != null)
                    {
                        return io.Sensors.Where(s => s.SensorType == SensorType.Voltage && !s.Name.Contains("#")).ToList();
                    }
                }

                return null;
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

        /// <summary>
        /// Mainboard sensors
        /// </summary>
        public IList<ISensor> MainboardSensors
        {
            get
            {
                if (this.Mainboard != null && this.Mainboard.SubHardware != null)
                {
                    var io = this.Mainboard.SubHardware.Where(i => i.HardwareType == HardwareType.SuperIO).FirstOrDefault();

                    if (io != null)
                    {
                        return io.Sensors.ToList();
                    }
                }

                return null;
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

        /// <summary>
        /// The installed GPU
        /// </summary>
        public IHardware GPU
        {
            get
            {
                return this.observedComputer.Hardware.Where(p => (p.HardwareType == HardwareType.GpuAti || p.HardwareType == HardwareType.GpuNvidia)).FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                var sensors = this.GPU.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Name.Contains("GPU Cores"));
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

        /// <summary>
        /// GPU-Core ClocSpeed
        /// </summary>
        public ISensor GPUCoreClockSpeedSensor
        {
            get
            {
                if (this.GPU != null && this.GPU.Sensors != null)
                    return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Clock && s.Name.Equals("GPU Core")).FirstOrDefault();

                return null;
            }
        }

        /// <inheritdoc />
        public ISensor GPUMemoryClockSpeedSensor
        {
            get
            {
                return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Clock && s.Name.Equals("GPU Memory")).FirstOrDefault();
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

        /// <summary>
        /// GPU-Core Temperature sensor
        /// </summary>
        public ISensor GPUCoreTemperatureSensor
        {
            get
            {
                if (this.GPU != null && this.GPU.Sensors != null)
                    return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Name.Equals("GPU Core")).FirstOrDefault();

                return null;
            }
        }

        /// <summary>
        /// GPU-Core-Worklaod sensor
        /// </summary>
        public ISensor GPUCoreWorkloadSensor
        {
            get
            {
                if (this.GPU != null && this.GPU.Sensors != null)
                    return this.GPU.Sensors.Where(s => s.SensorType == SensorType.Load && s.Name.Equals("GPU Core")).FirstOrDefault();

                return null;
            }
        }

        #endregion GPU-Properties
    }
}