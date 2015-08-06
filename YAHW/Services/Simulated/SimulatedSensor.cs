using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Collections;

namespace YAHW.Services
{
    class SimulatedSensor : ISensor
    {

        private String name = "Simulated Sensor";
        private SensorType sensorType = SensorType.Temperature;

        public IControl Control
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IHardware Hardware
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Identifier Identifier
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Index
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsDefaultHidden
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public float? Max
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public float? Min
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
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public IReadOnlyArray<IParameter> Parameters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public SensorType SensorType
        {
            get
            {
                return this.sensorType;
            }
            set
            {
                this.SensorType = value;
            }
        }

        public float? Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<SensorValue> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Accept(IVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public void ResetMax()
        {
            throw new NotImplementedException();
        }

        public void ResetMin()
        {
            throw new NotImplementedException();
        }

        public void Traverse(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
