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

using OpenHardwareMonitor.Hardware;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using YAHW.Constants;
using YAHW.Hardware;
using YAHW.Interfaces;
using YAHW.Model;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// Class for managing the fan controllers
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
    public class FanControllerService
    {
        #region Members and Constants

        OpenHardwareMonitorManagementService openHardwareMonitorManagementService = null;

        #endregion Members and Constants

        /// <summary>
        /// Standard CTOR
        /// </summary>
        public FanControllerService()
        {
            // Get OHW-Management-Service
            this.openHardwareMonitorManagementService = DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);

            // Read the fan controller templates (Standard, Silent, ...)
            this.ReadFanControllerTemplates();

            // Check if file exists -> if not create one
            if (!System.IO.File.Exists(DirectoryConstants.FanControllerSettingsConfig))
            {
                // Create the settings file
                this.CreateSettingsFile();
            }
        }

        #region Methods

        /// <summary>
        /// Start service
        /// </summary>
        public void Start()
        {
            if (this.openHardwareMonitorManagementService != null)
            {
                if (this.openHardwareMonitorManagementService.MainboardFanControlSensors != null &&
                    this.openHardwareMonitorManagementService.MainboardFanControlSensors.Count > 0)
                {
                    foreach (var s in this.openHardwareMonitorManagementService.MainboardFanControlSensors)
                    {
                        this.FanControllers.Add(new FanController(s));
                    }
                }
                else
                {
                    this.IsDisabled = true;
                }
            }
        }

        /// <summary>
        ///  Create settings file
        /// </summary>
        private void CreateSettingsFile()
        {
            try
            {
                // Create new XML-File
                XDocument xdoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                    new XComment("Settings for the detected Fan-Controllers"),
                    new XElement("FanControllerSettings"));

                xdoc.Save(DirectoryConstants.FanControllerSettingsConfig);
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorCreatingConfigFile");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        /// <summary>
        /// Read Fan-Controller-Templates
        /// </summary>
        private void ReadFanControllerTemplates()
        {
            ObservableCollection<FanControllerTemplate> templates = new ObservableCollection<FanControllerTemplate>();

            try
            {
                XElement xmlDoc = XElement.Load(DirectoryConstants.FanControllerTemplatesConfig);

                foreach (var t in xmlDoc.Descendants("FanControllerTemplate"))
                {
                    FanControllerTemplate template = new FanControllerTemplate();

                    template.Name = t.Attribute("Name").Value != null ? t.Attribute("Name").Value : "n.a.";

                    foreach (var s in t.Descendants("Setting"))
                    {
                        double x = s.Attribute("Temperature").Value != null ? XmlConvert.ToDouble(s.Attribute("Temperature").Value) : default(double);
                        double y = s.Attribute("FanVoltageInPercent").Value != null ? XmlConvert.ToDouble(s.Attribute("FanVoltageInPercent").Value) : default(double);

                        template.DataPoints.Add(new DataPoint(x, y));
                    }

                    templates.Add(template);
                }

                this.FanControllerTemplates = templates;
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorReadingFanControllerTemplates");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        #endregion Methods

        #region Properties

        private ObservableCollection<FanControllerTemplate> fanControllerTemplates;

        /// <summary>
        /// List with fan controller templates
        /// </summary>
        public ObservableCollection<FanControllerTemplate> FanControllerTemplates
        {
            get { return fanControllerTemplates; }
            set { fanControllerTemplates = value; }
        }

        /// <summary>
        /// List with mainboard temperature sensors
        /// </summary>
        public IList<ISensor> MainboardTemperatureSensors
        {
            get { return this.openHardwareMonitorManagementService.MainboardTemperatureSensors; }
        }

        private IList<FanController> fanControllers = new List<FanController>();

        /// <summary>
        /// List with fan controllers
        /// </summary>
        public IList<FanController> FanControllers
        {
            get { return fanControllers; }
            set { fanControllers = value; }
        }
        
        private bool isDisabled;

        /// <summary>
        /// Flag if service is disabled
        /// </summary>
        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        #endregion Properties
    }
}