using System;
using OpenHardwareMonitor.Hardware;
using YAHW.Services.Simulated;
using System.Collections.Generic;

namespace YAHW.Services
{
    internal class SimulatedIOHardware : IHardware
    {
        private List<ISensor> sensors;

        public SimulatedIOHardware()
        {
            ISensor[] sensorArray = {
                SimulatedSensor.getSimulatedSensor(SensorType.Voltage, "CPU VCore"),
                SimulatedSensor.getSimulatedSensor(SensorType.Voltage, "AVCC"),
                SimulatedSensor.getSimulatedSensor(SensorType.Voltage, "3VCC"),
                SimulatedSensor.getSimulatedSensor(SensorType.Voltage, "3VSB"),
                SimulatedSensor.getSimulatedSensor(SensorType.Voltage, "VBAT"),
                SimulatedSensor.getSimulatedSensor(SensorType.Voltage, "VTT"),
                SimulatedSensor.getSimulatedSensor(SensorType.Temperature, "CPU Core"),
                SimulatedSensor.getSimulatedSensor(SensorType.Control, "Fan"),
            };
            this.sensors = new List<ISensor>(sensorArray);
        }
        public HardwareType HardwareType
        {
            get
            {
                return HardwareType.SuperIO;
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
                return this.GetType().Name;
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
            foreach (var sensor in this.Sensors) {
                // Used explicit Cast over adding another subclass
                if (sensor is SimulatedSensor)
                    ((SimulatedSensor)sensor).update();
            }
        }
    }
}