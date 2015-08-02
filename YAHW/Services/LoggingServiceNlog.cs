using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YAHW.Interfaces;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// Service for logging (base on NLOG)
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
    public class LoggingServiceNLog : ILoggingService
    {
        #region Members and Constants

        Logger logger = null;

        #endregion Members and Constants

        /// <summary>
        /// CTOR
        /// </summary>
        public LoggingServiceNLog()
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="message">The title.</param>
        /// <param name="ex">The Exception.</param>
        /// <param name="memberName">The caller member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        public void LogException(string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string msgTemplate = "{0}"
                                   + System.Environment.NewLine
                                   + System.Environment.NewLine
                                   + "Caller: {1}"
                                   + System.Environment.NewLine
                                   + "File: {2}"
                                   + System.Environment.NewLine
                                   + "Line-Number: {3}"
                                   + System.Environment.NewLine
                                   + System.Environment.NewLine;

            string msg = String.Format(msgTemplate, new object[] { message, callerMemberName, sourceFilePath, sourceLineNumber });

            logger.Log(LogLevel.Error, ex, msgTemplate, new object[] { message, callerMemberName, sourceFilePath, sourceLineNumber });
        }
    }
}
