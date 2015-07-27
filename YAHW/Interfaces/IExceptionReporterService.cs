using System;

namespace YAHW.Interfaces
{
    /// <summary>
    /// <para>
    /// Interface for the exception reporter service
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
    /// <para>Date: 18.12.2014</para>
    /// </summary>
    public interface IExceptionReporterService
    {
        void ReportException(Exception ex);
    }
}
