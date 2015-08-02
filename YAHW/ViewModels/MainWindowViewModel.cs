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
using XAHW.Interfaces;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Services;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// A simple view model the Main-Page
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
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public MainWindowViewModel()
        {
            IConfigurationFile configFile = this.LoadApplicationConfigFile();

            // Register services
            DependencyFactory.RegisterInstance<ILocalizerService>(ServiceNames.LocalizerService, new LocalizerService(configFile.Sections["GeneralSettings"].Settings["Language"].Value));
            DependencyFactory.RegisterInstance<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService, new OpenHardwareMonitorManagementService());
            DependencyFactory.RegisterInstance<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService, new WmiHardwareInfoService());
            DependencyFactory.RegisterInstance<IExceptionReporterService>(ServiceNames.ExceptionReporterService, new ExceptionReporterService());
            DependencyFactory.RegisterInstance<ILoggingService>(ServiceNames.LoggingService, new LoggingServiceNLog());
        }

        /// <summary>
        /// Load configuration file
        /// </summary>
        /// <returns></returns>
        private IConfigurationFile LoadApplicationConfigFile()
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YAHW");

            // Create directory if not exists
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            // Create config file
            IConfigurationFile configFile = new XmlConfigurationFile(System.IO.Path.Combine(path, "ApplicationSettings.xml"));

            if (!configFile.Load())
            {
                // Add section GeneralSettings
                configFile.Sections.Add("GeneralSettings");

                // Add settings
                configFile.Sections["GeneralSettings"].Settings.Add("Language", "de", "de", typeof(System.String));
                configFile.Sections["GeneralSettings"].Settings.Add("Theme", "Light", "Light", typeof(System.String));
            }
            else
            {
                // Check if all settings are in config file
                if (configFile.Sections["GeneralSettings"].Settings["Language"] == null)
                    configFile.Sections["GeneralSettings"].Settings.Add("Language", "de", "de", typeof(System.String));
                if (configFile.Sections["GeneralSettings"].Settings["Theme"] == null)
                    configFile.Sections["GeneralSettings"].Settings.Add("Theme", "Light", "Light", typeof(System.String));
            }

            // Save config file
            configFile.Save();

            // Register global application file
            DependencyFactory.RegisterInstance<IConfigurationFile>(ConfigFileNames.ApplicationConfig, configFile);

            return configFile;
        }

        /// <summary>
        /// Close application
        /// </summary>
        public void ShutdownApplication()
        {
            var hardwareService = DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);

            if (hardwareService != null)
            {
                hardwareService.Close();
            }
        }
    }
}