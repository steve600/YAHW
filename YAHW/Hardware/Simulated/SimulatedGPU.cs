using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;

namespace YAHW.Hardware.Simulated
{
    /// <summary>
    /// <para>
    /// Simulation of Open Hardware Monitor Library compliant GPU hardware component
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
    internal class SimulatedGPU : IHardware
    {
        #region Fields

        private HardwareType hardwareType = HardwareType.GpuAti;
        private List<ISensor> sensors;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// CTOR
        /// </summary>
        public SimulatedGPU()
        {
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

        #endregion Constructors

        #region Events

        public event SensorEventHandler SensorAdded;

        public event SensorEventHandler SensorRemoved;

        #endregion Events

        #region Properties

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

        #endregion Properties

        #region Methods

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

        #endregion Methods
    }
}