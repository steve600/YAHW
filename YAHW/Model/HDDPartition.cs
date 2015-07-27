using System;
using YAHW.MVVMBase;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for a HDD-Partition
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
    public class HDDPartition : BindableBase
    {
        private string diskName;

        /// <summary>
        /// The name of the disk
        /// </summary>
        public string DiskName
        {
            get { return diskName; }
            set { this.SetProperty<string>(ref this.diskName, value); }
        }

        private string partitionName;

        /// <summary>
        /// The name of the partition
        /// </summary>
        public string PartitionName
        {
            get { return partitionName; }
            set { this.SetProperty<string>(ref this.partitionName, value); }
        }

        private string driveLetter;

        /// <summary>
        /// The drive letter of the partition
        /// </summary>
        public string DriveLetter
        {
            get { return driveLetter; }
            set { this.SetProperty<string>(ref this.driveLetter, value); }
        }

        private UInt64 freeSpace;

        /// <summary>
        /// The free space of the partition
        /// </summary>
        public UInt64 FreeSpace
        {
            get { return freeSpace; }
            set { this.SetProperty<UInt64>(ref this.freeSpace, value); }
        }

        private double freeSpaceInPercent;

        /// <summary>
        /// The free space in percent
        /// </summary>
        public double FreeSapceInPercent
        {
            get { return freeSpaceInPercent; }
            set { this.SetProperty<double>(ref this.freeSpaceInPercent, value); }
        }

        private UInt64 totalSpace;

        /// <summary>
        /// Total space of the partition
        /// </summary>
        public UInt64 TotalSpace
        {
            get { return totalSpace; }
            set { this.SetProperty<UInt64>(ref this.totalSpace, value); }
        }
    }
}
