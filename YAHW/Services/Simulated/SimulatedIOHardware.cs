using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// Simulation of Open Hardware Monitor Library compliant IO hardware component
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
    internal class SimulatedIOHardware : IHardware
    {
        #region Fields

        private List<ISensor> sensors;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// CTOR
        /// </summary>
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