using System;
using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;

namespace YAHW.Services
{
    internal class SimulatedCPU : IHardware
    {
        private List<ISensor> sensors;

        public SimulatedCPU()
        {
            ISensor[] sensorArray = {
                //TODO: get actual names from OHW GUI for consistency
                SimulatedSensor.getSimulatedSensor(SensorType.Load, "CPU Total"),
                SimulatedSensor.getSimulatedSensor(SensorType.Power, "CPU Package"),
                SimulatedSensor.getSimulatedSensor(SensorType.Power, "CPU Cores"),
                SimulatedSensor.getSimulatedSensor(SensorType.Temperature, "CPU Package"),
                SimulatedSensor.getSimulatedSensor(SensorType.Load, "Core"),
                SimulatedSensor.getSimulatedSensor(SensorType.Temperature, "Core"),
                SimulatedSensor.getSimulatedSensor(SensorType.Clock, "Core")
            };
            this.sensors = new List<ISensor>(sensorArray);
        }

        public HardwareType HardwareType
        {
            get
            {
                return HardwareType.CPU;
            }
        }

        public Identifier Identifier
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IHardware Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ISensor[] Sensors
        {
            get
            {
                return sensors.ToArray();
            }
        }

        public IHardware[] SubHardware
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event SensorEventHandler SensorAdded;
        public event SensorEventHandler SensorRemoved;

        public void Accept(IVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public string GetReport()
        {
            throw new NotImplementedException();
        }

        public void Traverse(IVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            foreach (var sensor in this.Sensors)
            {
                // Used explicit Cast over adding another subclass
                if (sensor is SimulatedSensor)
                    ((SimulatedSensor)sensor).update();
            }
        }
    }
}