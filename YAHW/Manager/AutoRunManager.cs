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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Model;

namespace YAHW.Manager
{
    /// <summary>
    /// <para>
    /// Class for managing autostart entries
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
    public class AutoRunManager
    {
        #region Members and Constants

        private const string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        private const string runKey64 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Run";

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public AutoRunManager()
        { 
        }

        #endregion CTOR

        /// <summary>
        /// Detect run key entries (via Registry)
        /// </summary>
        /// <param name="userSpecific">Flag if the user specific autorun entries should be selected</param>
        /// <returns></returns>
        public IList<AutoRunGroup> DetectRunKeyEntries()
        {
            List<AutoRunGroup> result = new List<AutoRunGroup>();

            result.Add(GetRegistryAutorunKey(Registry.CurrentUser, runKey));
            result.Add(GetRegistryAutorunKey(Registry.LocalMachine, runKey));
            result.Add(GetRegistryAutorunKey(Registry.LocalMachine, runKey64));
            
            return result;
        }

        private AutoRunGroup GetRegistryAutorunKey(RegistryKey baseKey, string runKey)
        {
            AutoRunGroup result = new AutoRunGroup();

            using (RegistryKey startupKey = baseKey.OpenSubKey(runKey))
            {
                if (startupKey != null)
                {
                    var valueNames = startupKey.GetValueNames();

                    result.GlobalPath = startupKey.ToString();

                    // Name => File path
                    result.AutoRunEntries = (from ar in valueNames
                                             where startupKey.GetValueKind(ar) == RegistryValueKind.String
                                             select new AutoRunEntry() { Name = ar, Location = startupKey.GetValue(ar).ToString(), IsActive = true }).ToList();
                }
            }

            return result;
        }

        /// <summary>
        /// Detect autorun entries with WMI
        /// </summary>
        /// <returns></returns>
        public IList<AutoRunEntry> DetectRunKeyEntriesWithWMI()
        {
            List<AutoRunEntry> result = new List<AutoRunEntry>();

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_StartupCommand");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    var entry = new AutoRunEntry();

                    entry.Caption = Convert.ToString(queryObj["Caption"]);
                    entry.Command = Convert.ToString(queryObj["Command"]);
                    entry.Description = Convert.ToString(queryObj["Description"]);
                    entry.Location = Convert.ToString(queryObj["Location"]);
                    entry.Name = Convert.ToString(queryObj["Name"]);
                    entry.User = Convert.ToString(queryObj["User"]);
                    entry.UserSID = Convert.ToString(queryObj["UserSID"]);

                    result.Add(entry);
                }
            }
            catch (ManagementException ex)
            {
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException("Fehler bei der Ermittlung der RunKey-Entries", ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }

            return result;
        }
    }
}
