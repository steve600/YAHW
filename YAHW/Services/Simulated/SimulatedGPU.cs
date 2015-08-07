﻿using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAHW.Services.Simulated
{
    internal class SimulatedGPU : IHardware
    {

        private HardwareType hardwareType = HardwareType.GpuAti;
        private List<ISensor> sensors;

        public SimulatedGPU() {
            ISensor[] sensorArray = {
                //TODO: get actual names from OHW GUI for consistency
                SimulatedSensor.getSimulatedSensor(SensorType.Load, "GPU Total"),
                SimulatedSensor.getSimulatedSensor(SensorType.Power, "GPU Cores"),
                SimulatedSensor.getSimulatedSensor(SensorType.Temperature, "GPU Package"),
                SimulatedSensor.getSimulatedSensor(SensorType.Load, "Core"),
                SimulatedSensor.getSimulatedSensor(SensorType.Temperature, "Core"),
                SimulatedSensor.getSimulatedSensor(SensorType.Clock, "Core"),
                SimulatedSensor.getSimulatedSensor(SensorType.Clock, "Memory"),
                SimulatedSensor.getSimulatedSensor(SensorType.Control, "Fan"),
            };
            this.sensors = new List<ISensor>(sensorArray);
        }
    
        public HardwareType HardwareType
        {
            get
            {
                return this.hardwareType;
            }
            set
            {
                this.hardwareType = value;
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
                return this.sensors.ToArray();
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