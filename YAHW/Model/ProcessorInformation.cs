using YAHW.MVVMBase;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for Processor-Information
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
    /// <para>Author: Steffen Steinbrecher</para>
    /// <para>Date: 12.07.2015</para>
    /// </summary>
    public class ProcessorInformation : BindableBase
    {
        private string caption;

        /// <summary>
        /// Caption
        /// </summary>
        public string Caption
        {
            get { return caption; }
            set { this.SetProperty<string>(ref this.caption, value); }
        }

        private uint currentClockSpeed;

        /// <summary>
        /// Current clock speed
        /// </summary>
        public uint CurrentClockSpeed
        {
            get { return currentClockSpeed; }
            set { this.SetProperty<uint>(ref this.currentClockSpeed, value); }
        }        

        private string description;

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { this.SetProperty<string>(ref this.description, value); }
        }

        private uint extClock;

        /// <summary>
        /// Clock speed
        /// </summary>
        public uint ExtClock
        {
            get { return extClock; }
            set { this.SetProperty<uint>(ref this.extClock, value); }
        }

        private uint l2CacheSize;

        /// <summary>
        /// L2 Cache-Size
        /// </summary>
        public uint L2CacheSize
        {
            get { return l2CacheSize; }
            set { this.SetProperty<uint>(ref this.l2CacheSize, value); }
        }

        private uint l3CacheSize;

        /// <summary>
        /// L3 Cache-Size
        /// </summary>
        public uint L3CacheSize
        {
            get { return l3CacheSize; }
            set { this.SetProperty<uint>(ref this.l3CacheSize, value); }
        }

        private string manufacturer;

        /// <summary>
        /// Manufacturer
        /// </summary>
        public string Manufacturer
        {
            get { return manufacturer; }
            set { this.SetProperty<string>(ref this.manufacturer, value); }
        }

        private string maxClockSpeed;

        /// <summary>
        /// Max. clock speed
        /// </summary>
        public string MaxClockSpeed
        {
            get { return maxClockSpeed; }
            set { this.SetProperty<string>(ref this.maxClockSpeed, value); }
        }

        private string name;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        private uint numberOfCores;

        /// <summary>
        /// Number of cores
        /// </summary>
        public uint NumberOfCores
        {
            get { return numberOfCores; }
            set { this.SetProperty<uint>(ref this.numberOfCores, value); }
        }

        private uint numberOfLogicalProcessors;

        /// <summary>
        /// Number of logical processors
        /// </summary>
        public uint NumberOfLogicalProcessors
        {
            get { return numberOfLogicalProcessors; }
            set { this.SetProperty<uint>(ref this.numberOfLogicalProcessors, value); }
        }        
    }
}