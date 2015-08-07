using System;
using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;

namespace YAHW.Services
{
    internal class SimulatedMainboard : IHardware
    {
        private List<IHardware> subHardware;
        private List<ISensor> sensors;

        public SimulatedMainboard()
        {
            IHardware[] hardware = { new SimulatedIOHardware() };
            this.subHardware = new List<IHardware>(hardware);
            this.sensors = new List<ISensor>();
        }

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
            List<ISensor> associatedSensors = new List<ISensor>(10);
            associatedSensors.AddRange(new List<ISensor>( this.Sensors ) );
            this.subHardware.ForEach( 
                subhardwareItem => associatedSensors.AddRange( new List<ISensor>(subhardwareItem.Sensors) ) 
            );
            foreach (var sensor in associatedSensors)
            {
                // Used explicit Cast over adding another subclass
                if (sensor is SimulatedSensor)
                    ((SimulatedSensor)sensor).update();
            }
        }
    }
}