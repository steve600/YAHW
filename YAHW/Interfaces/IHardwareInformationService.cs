using System.Collections.Generic;
using YAHW.Model;

namespace YAHW.Interfaces
{
    /// <summary>
    /// <para>
    /// Interface for a hardware info service
    /// </para>
    /// 
    /// <para>
    /// Interface history:
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
    public interface IHardwareInformationService
    {
        /// <summary>
        /// Try to get mainboard info
        /// </summary>
        /// <returns></returns>
        MainboardInformation GetMainboardInformation();

        /// <summary>
        /// Try to get processor information
        /// </summary>
        /// <returns></returns>
        ProcessorInformation GetProcessorInformation();

        #region GPU

        /// <summary>
        /// Get GPU-Information
        /// </summary>
        /// <returns></returns>
        GPUInformation GetGPUInformation();
        
        #endregion GPU

        #region Physical Memory (RAM)

        /// <summary>
        /// Get physical memory information
        /// </summary>
        /// <returns></returns>
        IList<RAMInformation> GetPhysicalMemoryInformation();

        #endregion Physical Memory (RAM)

        #region HDD

        /// <summary>
        /// Get HHD SMART Information for installed drives
        /// </summary>
        /// <returns></returns>
        Dictionary<int, HDD> GetHddSmartInformation();

        #endregion HDD

        #region Network

        void GetNetworkAdapterInformation();

        #endregion Network
    }
}
