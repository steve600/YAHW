using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YAHW.Services
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
        /// GPU-Core Clock Speed
        /// </summary>
        double GPUCoreClockSpeed { get; }

        /// <summary>
        /// GPU-Core ClockSpeed-Sensors
        /// </summary>
        ObservableCollection<ISensor> GPUCoreClockSpeedSensors { get; }

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
        /// GPU-Memory Clock Speed
        /// </summary>
        double GPUMemoryClockSpeed { get; }

        /// <summary>
        /// GPU-Memory ClockSpeed-Sensors
        /// </summary>
        ObservableCollection<ISensor> GPUMemoryClockSpeedSensors { get; }

        /// <summary>
        /// The GPU Temperature-Sensor
        /// </summary>
        ISensor GPUTemperatureSensor { get; }

        /// <summary>
        /// GPU-Workload Sensor (TOTAL)
        /// </summary>
        ISensor GPUWorkloadSensor { get; }

        /// <summary>
        /// Mainboard IO-Hardware
        /// </summary>
        IHardware Mainboard { get; }

        /// <summary>
        /// Mainboard 3VCC
        /// </summary>
        ISensor Mainboard3VCC { get; }

        /// <summary>
        /// Mainboard 3VSB
        /// </summary>
        ISensor Mainboard3VSB { get; }

        /// <summary>
        /// Mainboard AVCC
        /// </summary>
        ISensor MainboardAVCC { get; }

        /// <summary>
        /// Mainboard CPU-Core Temperature
        /// </summary>
        ISensor MainboardCPUCoreTemperature { get; }

        /// <summary>
        /// The CPU-VCore
        /// </summary>
        ISensor MainboardCPUVCore { get; }

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

        /// <summary>
        /// Mainboard VBAT
        /// </summary>
        ISensor MainboardVBAT { get; }

        /// <summary>
        /// Mainboard VTT
        /// </summary>
        ISensor MainboardVTT { get; }

        #endregion Properties

        #region Methods

        void AcceptNewSettings();

        void Close();

        void UpdateMainboardSensors();

        #endregion Methods
    }
}