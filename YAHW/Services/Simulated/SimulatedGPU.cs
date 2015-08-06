using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAHW.Services.Simulated
{
    class SimulatedGPU : IHardware
    {

        HardwareType hardwareType = HardwareType.GpuAti;
        List<ISensor> sensors = new List<ISensor>(5);

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
            set
            {
                this.sensors = value.ToList();
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
            throw new NotImplementedException();
        }
    }
}
