using OpenHardwareMonitor.Collections;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// Simulation of Open Hardware Monitor Library compliant sensor
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
    internal class SimulatedSensor : ISensor
    {
        #region Fields

        private String name = "Simulated Sensor";
        private SensorType sensorType = SensorType.Temperature;
        private DispatcherTimer timer = null;
        private float? value = 0;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// CTOR
        /// </summary>
        public SimulatedSensor()
        {
            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        #endregion Constructors

        #region Properties

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
                return 95;
            }
        }

        public float? Min
        {
            get
            {
                return 5;
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
                this.sensorType = value;
            }
        }

        public float? Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public IEnumerable<SensorValue> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion Properties

        #region Methods

        public static ISensor getSimulatedSensor(SensorType sensorType, string sensorName)
        {
            SimulatedSensor sim = new SimulatedSensor();
            sim.SensorType = sensorType;
            sim.Name = sensorName;
            return sim;
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

        public void update()
        {
            Random rand = new Random();
            this.Value = rand.Next(0, 100);
        }

        /// <summary>
        /// Timer-Tick Event-Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            update();
        }

        #endregion Methods
    }
}