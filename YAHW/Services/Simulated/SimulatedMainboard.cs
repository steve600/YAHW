using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// Simulation of Open Hardware Monitor Library compliant mainboard hardware component
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
    internal class SimulatedMainboard : IHardware
    {
        #region Fields

        private List<ISensor> sensors;
        private List<IHardware> subHardware;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// CTOR
        /// </summary>
        public SimulatedMainboard()
        {
            IHardware[] hardware = { new SimulatedIOHardware() };
            this.subHardware = new List<IHardware>(hardware);
            this.sensors = new List<ISensor>();
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
                return HardwareType.Mainboard;
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
                return this.subHardware.ToArray();
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
            List<ISensor> associatedSensors = new List<ISensor>(10);
            associatedSensors.AddRange(new List<ISensor>(this.Sensors));
            this.subHardware.ForEach(
                subhardwareItem => associatedSensors.AddRange(new List<ISensor>(subhardwareItem.Sensors))
            );
            foreach (var sensor in associatedSensors)
            {
                // Used explicit Cast over adding another subclass
                if (sensor is SimulatedSensor)
                    ((SimulatedSensor)sensor).update();
            }
        }

        #endregion Methods
    }
}