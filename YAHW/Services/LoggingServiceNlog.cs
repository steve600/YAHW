// YAHW - Yet Another Hardware Monitor
// Copyright (c) 2015 Steffen Steinbrecher
// Contact and Information: http://csharp-blog.de/category/yahw/
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE

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
    /// Service for logging (based on NLOG)
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
