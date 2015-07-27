using System;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for performance data (PSApiWrapper)
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
    public class PerfomanceInfoData
    {
        public Int64 CommitTotalPages;
        public Int64 CommitLimitPages;
        public Int64 CommitPeakPages;
        public Int64 PhysicalTotalBytes;
        public Int64 PhysicalAvailableBytes;
        public Int64 SystemCacheBytes;
        public Int64 KernelTotalBytes;
        public Int64 KernelPagedBytes;
        public Int64 KernelNonPagedBytes;
        public Int64 PageSizeBytes;
        public int HandlesCount;
        public int ProcessCount;
        public int ThreadCount;
    }
}
