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
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using YAHW.Constants;
using YAHW.Helper;
using YAHW.Interfaces;
using YAHW.Model;
using YAHW.MVVMBase;
using YAHW.Services;
using YAHW.UserControls;

namespace YAHW.Hardware
{
    /// <summary>
    /// <para>
    /// Class for controlling a fan sensor
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
    public class FanController : BindableBase
    {
        #region Members and Constants

        private FanControllerService fanControllerService = null;

        // Timer
        private DispatcherTimer timer = null;

        private double timerIntervall = 1000;

        // Smoothed data points
        private IList<DataPoint> smoothedPoints = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// Standard CTOR
        /// </summary>
        /// <param name="sensor"></param>
        public FanController(ISensor sensor)
        {
            // Set sensor
            this.FanSensor = sensor;

            // Get Fan-Controller-Service
            this.fanControllerService = DependencyFactory.Resolve<FanControllerService>(ServiceNames.FanControllerService);

            if (this.FanSensor != null)
            {
                this.ReadFanControllerSettings();
            }

            // Create user control
            var userControl = new MainboardFanController();
            userControl.FanController = this;
            this.MainboardFanUserControl = userControl;

            // Create timer
            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(this.timerIntervall);
            timer.Tick += timer_Tick;

            // Start timer
            timer.Start();
        }

        #endregion CTOR

        #region EventHandler

        /// <summary>
        /// Timer-Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // Check if Advanced-Mode is enabled
            if (IsAdvancedModeEnabled)
            {
                // Set Fan-Speed
                this.SetFanSpeed();
                // Clear chart
                if (this.MainboardFanUserControl != null && this.MainboardFanUserControl is MainboardFanController)
                {
                    ((MainboardFanController)this.MainboardFanUserControl).UpdateChart();
                }
            }

            // Update Fan-Speed display values
            this.CurrentFanSpeedValue = this.FanSensor.Value.Value;
            this.MinFanSpeedValue = this.FanSensor.Min.Value;
            this.MaxFanSpeedValue = this.FanSensor.Max.Value;
        }

        #endregion EventHandler

        #region Methods

        /// <summary>
        /// Read Fan-Controller-Settings
        /// </summary>
        private void ReadFanControllerSettings()
        {
            try
            {
                // Read XML-File
                XDocument xdoc = XDocument.Load(DirectoryConstants.FanControllerSettingsConfig);

                // Check if elements exists
                if (xdoc.Descendants("FanControllerSettings").FirstOrDefault().HasElements)
                {
                    XElement settings = (from xml in xdoc.Descendants("FanController")
                                         where xml.Attribute("Name").Value == this.FanSensor.Name
                                         select xml).FirstOrDefault();

                    // If settings for the fan controller not found -> create and save settings
                    if (settings == null)
                    {
                        this.SelectedMainboardTemperatureSensor = this.fanControllerService.MainboardTemperatureSensors.FirstOrDefault();
                        this.SelectedFanControllerTemplate = this.fanControllerService.FanControllerTemplates.FirstOrDefault();
                        this.CanSetFanSpeed = false;
                        this.CurrentFanSpeedValue = this.FanSensor.Value.Value;
                        this.SelectedFanSpeedValue = this.FanSensor.Value.Value;
                        xdoc.Element("FanControllerSettings").Add(this.GetSettingsAsXml());
                        xdoc.Save(DirectoryConstants.FanControllerSettingsConfig);
                    }
                    else
                    {
                        // Set settings from XML-File
                        this.IsDefaultModeEnabled = XmlConvert.ToBoolean(settings.Element("IsDefaultModeEnabled").Value);
                        this.IsAdvancedModeEnabled = XmlConvert.ToBoolean(settings.Element("IsAdvancedModeEnabled").Value);
                        this.SelectedMainboardTemperatureSensor = this.fanControllerService.MainboardTemperatureSensors.Where(sensor => sensor.Name.Equals(settings.Element("TemperatureSensor").Value)).FirstOrDefault();
                        this.SelectedFanControllerTemplate = this.fanControllerService.FanControllerTemplates.Where(template => template.Name.Equals(settings.Element("FanControllerTemplate").Value)).FirstOrDefault();

                        if (IsDefaultModeEnabled || IsAdvancedModeEnabled)
                        {
                            this.CanSetFanSpeed = false;
                            // Set current value reported from controller
                            this.SelectedFanSpeedValue = this.FanSensor.Value.Value;
                        }
                        else
                        {
                            this.CanSetFanSpeed = true;
                            // Set value from config
                            var valueFromSettings = (float)XmlConvert.ToDouble(settings.Element("SelectedFanSpeedValue").Value);

                            if (valueFromSettings == 0)
                                valueFromSettings = this.FanSensor.Value.Value;

                            this.SelectedFanSpeedValue = valueFromSettings;
                        }
                    }
                }
                // If no elements found -> create the first one with default values
                else
                {
                    this.SelectedMainboardTemperatureSensor = this.fanControllerService.MainboardTemperatureSensors.FirstOrDefault();
                    this.SelectedFanControllerTemplate = this.fanControllerService.FanControllerTemplates.FirstOrDefault();
                    this.CanSetFanSpeed = false;
                    this.CurrentFanSpeedValue = this.FanSensor.Value.Value;
                    this.SelectedFanSpeedValue = this.FanSensor.Value.Value;
                    xdoc.Element("FanControllerSettings").Add(this.GetSettingsAsXml());
                    xdoc.Save(DirectoryConstants.FanControllerSettingsConfig);
                }
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorReadingConfigFile");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        /// <summary>
        /// Write Fan-Controller-Settings
        /// </summary>
        private void WriteFanControllerSettings(string settingName, object value)
        {
            try
            {
                // Read XML-File
                XDocument xdoc = XDocument.Load(DirectoryConstants.FanControllerSettingsConfig);

                // Check if elements exists
                if (xdoc.Descendants("FanControllerSettings").FirstOrDefault().HasElements)
                {
                    XElement settings = (from xml in xdoc.Descendants("FanController")
                                         where xml.Attribute("Name").Value == this.FanSensor.Name
                                         select xml).FirstOrDefault();

                    if (settings != null)
                    {
                        settings.SetElementValue(settingName, value);

                        xdoc.Save(DirectoryConstants.FanControllerSettingsConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorWritingConfigFile");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        /// <summary>
        /// Get the settings as XML
        /// </summary>
        /// <returns></returns>
        private XElement GetSettingsAsXml()
        {
            return new XElement("FanController", new XAttribute("Name", this.FanSensor.Name),
                new XElement("IsDefaultModeEnabled", XmlConvert.ToString(this.IsDefaultModeEnabled)),
                new XElement("IsAdvancedModeEnabled", XmlConvert.ToString(this.IsAdvancedModeEnabled)),
                new XElement("SelectedFanSpeedValue", XmlConvert.ToString(this.SelectedFanSpeedValue)),
                new XElement("TemperatureSensor", (this.SelectedMainboardTemperatureSensor != null) ? this.SelectedMainboardTemperatureSensor.Name : string.Empty),
                new XElement("FanControllerTemplate", (this.SelectedFanControllerTemplate != null) ? this.SelectedFanControllerTemplate.Name : string.Empty));
        }

        /// <summary>
        /// Set FAN-Speed dynamic -> for Advanced-Mode
        /// </summary>
        private void SetFanSpeed()
        {
            if (this.FanSensor != null && this.SelectedMainboardTemperatureSensor != null)
            {
                try
                {
                    // Get value
                    var values = this.smoothedPoints.Where(p => p.X > this.SelectedMainboardTemperatureSensor.Value.Value && p.X < (this.SelectedMainboardTemperatureSensor.Value.Value + 0.5));

                    if (values != null && values.Count() > 0)
                    {
                        var newValue = values.Max(p => p.Y);
                        // Set value
                        this.FanSensor.Control.SetSoftware((float)newValue);
                        // Accept
                        DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService).AcceptNewSettings();
                    }
                }
                catch (Exception ex)
                {
                    // Set default mode!
                    this.IsDefaultModeEnabled = true;

                    var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorSettingFanSpeed");
                    // Log-Exception
                    DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                    // Show exception
                    DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
                }
            }
        }

        /// <summary>
        /// Set Fan-Speed with a fixed value
        /// </summary>
        /// <param name="newValue">The new value</param>
        private void SetFanSpeed(float newValue)
        {
            if (this.FanSensor != null && this.SelectedMainboardTemperatureSensor != null)
            {
                try
                {
                    // Set value
                    this.FanSensor.Control.SetSoftware(newValue);
                    // Accept
                    DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService).AcceptNewSettings();
                }
                catch (Exception ex)
                {
                    // Set default mode!
                    this.IsDefaultModeEnabled = true;

                    var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorSettingFanSpeed");
                    // Log-Exception
                    DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                    // Show exception
                    DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
                }
            }
        }

        /// <summary>
        /// Calculate smooth points to detect Y-Values (WORKAROUND -> there should be better solution???)
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <returns></returns>
        private List<DataPoint> CalculateSmoothedDataPoints(List<DataPoint> dataPoints)
        {
            //var wpfSeries = this.fanSpeedChart.Series[0] as OxyPlot.Wpf.LineSeries;
            //OxyPlot.Series.LineSeries s = wpfSeries.InternalSeries as OxyPlot.Series.LineSeries;

            double ToleranceDivisor = 200;
            //double tolerance = Math.Abs(Math.Max(s.MaxX - s.MinX, s.MaxY - s.MinY) / ToleranceDivisor);
            double tolerance = Math.Abs(Math.Max(100.0 - 0.0, 100.0 - 0.0) / ToleranceDivisor);

            return CanonicalSplineHelper.CreateSpline(dataPoints, 0.5, null, false, tolerance);
        }

        #endregion Methods

        #region Properties

        private ISensor fanSensor;

        /// <summary>
        /// The fan sensor
        /// </summary>
        public ISensor FanSensor
        {
            get { return fanSensor; }
            private set { this.SetProperty<ISensor>(ref this.fanSensor, value); }
        }

        /// <summary>
        /// Reference to the fan controller service
        /// </summary>
        public FanControllerService FanControllerService
        {
            get
            {
                return this.fanControllerService;
            }
        }

        private ISensor selectedMainboardTemperatureSensor;

        /// <summary>
        /// The selected mainboard temperature sensor
        /// </summary>
        public ISensor SelectedMainboardTemperatureSensor
        {
            get { return selectedMainboardTemperatureSensor; }
            set
            {
                if (this.SetProperty<ISensor>(ref this.selectedMainboardTemperatureSensor, value))
                {
                    if (value != null)
                    {
                        // Write settings to config
                        this.WriteFanControllerSettings("TemperatureSensor", this.SelectedMainboardTemperatureSensor.Name);
                    }
                }
            }
        }

        private FanControllerTemplate selectedFanControllerTemplate;

        /// <summary>
        /// The selected fan controller template
        /// </summary>
        public FanControllerTemplate SelectedFanControllerTemplate
        {
            get { return selectedFanControllerTemplate; }
            set
            {
                if (this.SetProperty<FanControllerTemplate>(ref this.selectedFanControllerTemplate, value))
                {
                    if (value != null)
                    {
                        // Calculate new DataPoints for FanCurve
                        this.smoothedPoints = this.CalculateSmoothedDataPoints(value.DataPoints.ToList());
                        // Write settings to config
                        this.WriteFanControllerSettings("FanControllerTemplate", this.SelectedFanControllerTemplate.Name);
                    }
                }
            }
        }

        private bool isDefaultModeEnabled = true;

        /// <summary>
        /// Flag if default mode enabled
        /// </summary>
        public bool IsDefaultModeEnabled
        {
            get { return isDefaultModeEnabled; }
            set
            {
                if (this.SetProperty<bool>(ref this.isDefaultModeEnabled, value))
                {
                    if (value)
                    {
                        // Clear chart
                        if (this.MainboardFanUserControl != null && this.MainboardFanUserControl is MainboardFanController)
                        {
                            ((MainboardFanController)this.MainboardFanUserControl).ClearChart();
                        }

                        // Set controller default mode
                        this.FanSensor.Control.SetDefault();
                        DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService).AcceptNewSettings();

                        // Disable advanced mode
                        this.IsAdvancedModeEnabled = false;

                        // Disable slider
                        this.CanSetFanSpeed = false;
                    }
                    else
                    {
                        this.CanSetFanSpeed = true;
                    }

                    // Write settings to config
                    this.WriteFanControllerSettings("IsDefaultModeEnabled", value);
                }
            }
        }

        private bool isAdvancedModeEnabled;

        /// <summary>
        /// Flag if advanced mode is enabled
        /// </summary>
        public bool IsAdvancedModeEnabled
        {
            get { return isAdvancedModeEnabled; }
            set
            {
                if (this.SetProperty<bool>(ref this.isAdvancedModeEnabled, value))
                {
                    if (value)
                    {
                        this.CanSetFanSpeed = false;
                    }
                    else
                    {
                        // Clear chart
                        if (this.MainboardFanUserControl != null && this.MainboardFanUserControl is MainboardFanController)
                        {
                            ((MainboardFanController)this.MainboardFanUserControl).ClearChart();
                        }

                        // Enable slider
                        if (!IsDefaultModeEnabled)
                        {
                            this.CanSetFanSpeed = true;
                            // Set fan speed to last selected value
                            this.SetFanSpeed(this.SelectedFanSpeedValue);
                        }
                    }

                    // Write settings to config
                    this.WriteFanControllerSettings("IsAdvancedModeEnabled", value);
                }
            }
        }

        private float selectedFanSpeedValue;

        /// <summary>
        /// The selected fan speed value
        /// </summary>
        public float SelectedFanSpeedValue
        {
            get { return selectedFanSpeedValue; }
            set
            {
                if (this.SetProperty<float>(ref this.selectedFanSpeedValue, value))
                {
                    // Set new value
                    this.FanSensor.Control.SetSoftware(value);
                    // Accept
                    DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService).AcceptNewSettings();
                    // Write to Config-File
                    this.WriteFanControllerSettings("SelectedFanSpeedValue", value);
                }
            }
        }

        private float currentFanSpeedValue;

        /// <summary>
        /// Current fan speed value
        /// </summary>
        public float CurrentFanSpeedValue
        {
            get { return currentFanSpeedValue; }
            set { this.SetProperty<float>(ref this.currentFanSpeedValue, value); }
        }

        private float minFanSpeedValue;

        /// <summary>
        /// Min fan speed value
        /// </summary>
        public float MinFanSpeedValue
        {
            get { return minFanSpeedValue; }
            set { this.SetProperty<float>(ref this.minFanSpeedValue, value); }
        }

        private float maxFanSpeedValue;

        /// <summary>
        /// Max fan speed value
        /// </summary>
        public float MaxFanSpeedValue
        {
            get { return maxFanSpeedValue; }
            set { this.SetProperty<float>(ref this.maxFanSpeedValue, value); }
        }

        private bool canSetFanSpeed;

        /// <summary>
        /// Flag if fan speed can be set
        /// </summary>
        public bool CanSetFanSpeed
        {
            get { return canSetFanSpeed; }
            set { this.SetProperty<bool>(ref this.canSetFanSpeed, value); }
        }

        private UserControl mainboardFanUserControl;

        /// <summary>
        /// The UserControl for the UI
        /// </summary>
        public UserControl MainboardFanUserControl
        {
            get { return mainboardFanUserControl; }
            private set { this.SetProperty<UserControl>(ref this.mainboardFanUserControl, value); }
        }

        #endregion Properties
    }
}