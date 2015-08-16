using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YAHW.Interfaces
{
    /// <summary>
    /// <para>
    /// Interface for Open Hardware Monitor Library
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
    public interface IOpenHardwareMonitorManagementService
    {
        #region Properties

        /// <summary>
        /// Get CPU component
        /// </summary>
        /// <returns></returns>
        IHardware CPU { get; }

        /// <summary>
        /// CPU-Clock Speed
        /// </summary>
        double CPUClockSpeed { get; }

        /// <summary>
        /// CPU-Core ClockSpeed-Sensors
        /// </summary>
        ObservableCollection<ISensor> CPUCoreClockSpeedSensors { get; }

        /// <summary>
        /// The CPU-Core Power-Consumption Sensor
        /// </summary>
        ISensor CPUCorePowerConsumptionSensor { get; }

        /// <summary>
        /// CPU-Core Temperature-Sensors
        /// </summary>
        ObservableCollection<ISensor> CPUCoreTemperatureSensors { get; }

        /// <summary>
        /// CPU-Core Workload-Sensors
        /// </summary>
        ObservableCollection<ISensor> CPUCoreWorkloadSensors { get; }

        /// <summary>
        /// The CPU Power-Consumption Sensor
        /// </summary>
        ISensor CPUPowerConsumptionSensor { get; }

        /// <summary>
        /// The CPU Temperature-Sensor
        /// </summary>
        ISensor CPUTemperatureSensor { get; }

        /// <summary>
        /// CPU-Workload Sensor (TOTAL)
        /// </summary>
        ISensor CPUWorkloadSensor { get; }

        /// <summary>
        /// The GPU
        /// </summary>
        IHardware GPU { get; }

        /// <summary>
        /// The GPU-Core Power-Consumption Sensor
        /// </summary>
        ISensor GPUCorePowerConsumptionSensor { get; }

        /// <summary>
        /// GPU-Core Temperature-Sensors
        /// </summary>
        ObservableCollection<ISensor> GPUCoreTemperatureSensors { get; }

        /// <summary>
        /// GPU-Core Workload-Sensors
        /// </summary>
        ObservableCollection<ISensor> GPUCoreWorkloadSensors { get; }

        /// <summary>
        /// GPU FanSpeed-Sensors
        /// </summary>
        ObservableCollection<ISensor> GPUFanSpeedSensors { get; }

        /// <summary>
        /// GPU Manufactor Name
        /// </summary>
        string GPUManufactorName { get; }

        /// <summary>
        /// GPU-Core clock speed sensor
        /// </summary>
        ISensor GPUCoreClockSpeedSensor { get; }

        /// <summary>
        /// The GPU Temperature-Sensor
        /// </summary>
        ISensor GPUCoreTemperatureSensor { get; }

        /// <summary>
        /// GPU-Workload Sensor (TOTAL)
        /// </summary>
        ISensor GPUCoreWorkloadSensor { get; }

        /// <summary>
        /// GPU-Memory clock speed sensor
        /// </summary>
        ISensor GPUMemoryClockSpeedSensor { get; }

        /// <summary>
        /// Mainboard IO-Hardware
        /// </summary>
        IHardware Mainboard { get; }

        /// <summary>
        /// Mainboard voltage sensors
        /// </summary>
        IList<ISensor> MainboardVoltageSensors { get; }

        /// <summary>
        /// Mainboard voltages sensors with name (e.g. 3VCC)
        /// </summary>
        IList<ISensor> MainboardVoltageSensorsWithName { get; }

        /// <summary>
        /// Fan control sensors
        /// </summary>
        IList<ISensor> MainboardFanControlSensors { get; }

        /// <summary>
        /// Get IO Hardware
        /// </summary>
        IHardware MainboardIOHardware { get; }

        /// <summary>
        /// Mainboard temperature sensors
        /// </summary>
        IList<ISensor> MainboardTemperatureSensors { get; }


        #endregion Properties

        #region Methods

        /// <summary>
        /// Accept settings, e.g. for the Fan-Controller
        /// </summary>
        void AcceptNewSettings();

        /// <summary>
        /// Get a sensor
        /// </summary>
        /// <param name="sensorCategory">The sensor category, e.g. CPU</param>
        /// <param name="sensorName">The sensor name, e.g. CPU Package</param>
        /// <param name="sensorType">The sensor type, e.g. Load</param>
        /// <returns></returns>
        ISensor GetSensor(string sensorCategory, string sensorName, string sensorType);

        /// <summary>
        /// Close the observed computer
        /// </summary>
        void Close();

        #endregion Methods
    }
}