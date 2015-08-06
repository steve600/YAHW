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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Model;

namespace YAHW.Manager
{
    /// <summary>
    /// <para>
    /// Class for managing windows services
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
    public class ServiceManager
    {
        #region Members and Constants

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// Standard CTOR
        /// </summary>
        public ServiceManager()
        {
        }

        #endregion CTOR

        #region Methods

        /// <summary>
        /// Get installed windows services with WMI
        /// </summary>
        /// <returns></returns>
        public IList<WindowsService> GetInstalledWindowsServices()
        {
            IList<WindowsService> result = new List<WindowsService>();

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Service");
                
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    WindowsService s = new WindowsService();

                    s.AcceptPause = Convert.ToBoolean(queryObj["AcceptPause"]);
                    s.AcceptStop = Convert.ToBoolean(queryObj["AcceptStop"]);
                    s.Caption = Convert.ToString(queryObj["Caption"]);
                    s.Description = Convert.ToString(queryObj["Description"]);
                    s.DisplayName = Convert.ToString(queryObj["DisplayName"]);
                    s.Name = Convert.ToString(queryObj["Name"]);
                    s.PathName = Convert.ToString(queryObj["PathName"]);
                    s.ProcessId = Convert.ToInt32(queryObj["ProcessId"]);
                    s.StartMode = Convert.ToString(queryObj["StartMode"]);
                    s.StartName = Convert.ToString(queryObj["StartName"]);
                    s.State = Convert.ToString(queryObj["State"]);

                    result.Add(s);
                }
            }
            catch (ManagementException ex)
            {
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException("ServiceManager: Fehler bei der Ermittlung der Windows-Services", ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }

            return result;
        }

        /// <summary>
        /// Start a service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool StartService(string serviceName)
        {
            try
            {
                var serviceController = ServiceController.GetServices().Where(s => s.ServiceName.Equals(serviceName)).FirstOrDefault();

                if (serviceController != null)
                {
                    serviceController.Start();
                    return true;
                }
            }
            catch (Win32Exception ex1)
            {
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException("ServiceManager: Fehler beim Zugriff auf eine System-API", ex1);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex1);
            }
            catch (InvalidOperationException ex2)
            {
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException("ServiceManager: Der Dienst wurde nicht gefunden", ex2);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex2);
            }

            return false;
        }

        #endregion Methods

        #region Properties

        #endregion Properties
    }
}
