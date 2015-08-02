using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YAHW.Interfaces
{
    /// <summary>
    /// <para>
    /// Interface for the logging service
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
    /// <para>Date: 31.07.2015</para>
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="message">The title.</param>
        /// <param name="ex">The Exception.</param>
        /// <param name="memberName">The caller member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        void LogException(string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
    }
}
