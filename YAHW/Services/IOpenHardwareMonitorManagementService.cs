using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenHardwareMonitor.Hardware;

namespace YAHW.Services
{
    public interface IOpenHardwareMonitorManagementService
    {
        IHardware CPU { get; }
        double CPUClockSpeed { get; }
        ObservableCollection<ISensor> CPUCoreClockSpeedSensors { get; }
        ISensor CPUCorePowerConsumptionSensor { get; }
        ObservableCollection<ISensor> CPUCoreTemperatureSensors { get; }
        ObservableCollection<ISensor> CPUCoreWorkloadSensors { get; }
        ISensor CPUPowerConsumptionSensor { get; }
        ISensor CPUTemperatureSensor { get; }
        ISensor CPUWorkloadSensor { get; }
        IHardware GPU { get; }
        double GPUCoreClockSpeed { get; }
        ObservableCollection<ISensor> GPUCoreClockSpeedSensors { get; }
        ISensor GPUCorePowerConsumptionSensor { get; }
        ObservableCollection<ISensor> GPUCoreTemperatureSensors { get; }
        ObservableCollection<ISensor> GPUCoreWorkloadSensors { get; }
        ObservableCollection<ISensor> GPUFanSpeedSensors { get; }
        string GPUManufactorName { get; }
        double GPUMemoryClockSpeed { get; }
        ObservableCollection<ISensor> GPUMemoryClockSpeedSensors { get; }
        ISensor GPUTemperatureSensor { get; }
        ISensor GPUWorkloadSensor { get; }
        IHardware Mainboard { get; }
        ISensor Mainboard3VCC { get; }
        ISensor Mainboard3VSB { get; }
        ISensor MainboardAVCC { get; }
        ISensor MainboardCPUCoreTemperature { get; }
        ISensor MainboardCPUVCore { get; }
        IList<ISensor> MainboardFanControlSensors { get; }
        IHardware MainboardIOHardware { get; }
        IList<ISensor> MainboardTemperatureSensors { get; }
        ISensor MainboardVBAT { get; }
        ISensor MainboardVTT { get; }

        void AcceptNewSettings();
        void Close();
        void UpdateMainboardSensors();
    }
}