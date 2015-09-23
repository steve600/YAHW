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
using Prism.Mvvm;
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
    public class MainboardFanController : BindableBase, IFanController
    {
        #region Members and Constants

        private IFanControllerService fanControllerService = null;
        private IOpenHardwareMonitorManagementService ohmManagementService = null;

        // Smoothed data points
        private IList<DataPoint> smoothedPoints = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// Standard CTOR
        /// </summary>
        /// <param name="sensor"></param>
        public MainboardFanController(string sensorName)
        {
            // Set sensor
            this.FanSensorName = sensorName;

            // Get Fan-Controller-Service
            this.fanControllerService = DependencyFactory.Resolve<IFanControllerService>(ServiceNames.MainboardFanControllerService);
            // Get OpenHardwareMonitorManagementService
            this.ohmManagementService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);

            if (!String.IsNullOrEmpty(this.FanSensorName))
            {
                this.ReadFanControllerSettings();
            }

            // Create user control
            this.FanControllerUserControl = new MainboardFanControllerUserControl(this);
        }

        #endregion CTOR

        #region EventHandler

        public void UpdateValues()
        {
            // Check if Advanced-Mode is enabled
            if (IsAdvancedModeEnabled)
            {
                // Set Fan-Speed
                this.SetFanSpeed();
                // Update chart
                if (this.FanControllerUserControl != null && this.FanControllerUserControl is MainboardFanControllerUserControl)
                {
                    ((MainboardFanControllerUserControl)this.FanControllerUserControl).UpdateChart();
                }
            }

            this.OnPropertyChanged(() => this.CurrentFanSpeedValue);
            this.OnPropertyChanged(() => this.MinFanSpeedValue);
            this.OnPropertyChanged(() => this.MaxFanSpeedValue);
        }

        #endregion EventHandler

        #region Methods

        /// <summary>
        /// Get fan sensor by name
        /// </summary>
        /// <param name="name">The name of the sensor</param>
        /// <returns></returns>
        private ISensor GetFanSensor()
        {
            return this.ohmManagementService.MainboardFanControlSensors.Where(s => s.Name.Equals(this.FanSensorName)).FirstOrDefault();
        }

        /// <summary>
        /// Get current value of the sensor
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private float GetFanSensorValue()
        {
            var s = this.GetFanSensor();

            if (s != null && s.Value != null)
            {
                if (s.Value.HasValue)
                    return s.Value.Value;
            }

            return default(float);
        }

        /// <summary>
        /// Get min value of the sensor
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private float GetFanSensorMinValue()
        {
            var s = this.GetFanSensor();

            if (s != null && s.Min != null)
            {
                if (s.Min.HasValue)
                    return s.Min.Value;
            }

            return default(float);
        }

        /// <summary>
        /// Get max value of the sensor
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private float GetFanSensorMaxValue()
        {
            var s = this.GetFanSensor();

            if (s != null && s.Max != null)
            {
                if (s.Max.HasValue)
                    return s.Max.Value;
            }

            return default(float);
        }

        /// <summary>
        /// Set fan sensor value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newValue"></param>
        private void SetFanSensorValue(float newValue)
        {
            var s = this.GetFanSensor();

            if (s != null)
            {
                // Set value
                s.Control.SetSoftware(newValue);
                // Accept
                this.ohmManagementService.AcceptNewSettings();
            }
        }

        /// <summary>
        /// Set fan sensor default value
        /// </summary>
        /// <param name="name"></param>
        private void SetFanSensorDefaultValue()
        {
            var s = this.GetFanSensor();

            if (s != null)
            {
                // Set value
                s.Control.SetDefault();
                // Accept
                this.ohmManagementService.AcceptNewSettings();

                OnPropertyChanged(() => this.CurrentFanSpeedValue);
                OnPropertyChanged(() => this.MinFanSpeedValue);
                OnPropertyChanged(() => this.MaxFanSpeedValue);
            }
        }

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
                    XElement settings = (from xml in xdoc.Descendants("MainboardFanController")
                                         where xml.Attribute("Name").Value == this.FanSensorName
                                         select xml).FirstOrDefault();

                    // If settings for the fan controller not found -> create and save settings
                    if (settings == null)
                    {
                        this.SelectedTemperatureSensor = this.fanControllerService.TemperatureSensors.FirstOrDefault();
                        this.SelectedFanControllerTemplate = this.fanControllerService.FanControllerTemplates.FirstOrDefault();
                        this.CanSetFanSpeed = false;
                        this.SelectedFanSpeedValue = this.GetFanSensorValue();
                        xdoc.Element("FanControllerSettings").Add(this.GetSettingsAsXml());
                        xdoc.Save(DirectoryConstants.FanControllerSettingsConfig);
                    }
                    else
                    {
                        // Set settings from XML-File
                        this.IsDefaultModeEnabled = XmlConvert.ToBoolean(settings.Element("IsDefaultModeEnabled").Value);
                        this.IsAdvancedModeEnabled = XmlConvert.ToBoolean(settings.Element("IsAdvancedModeEnabled").Value);
                        this.SelectedTemperatureSensor = this.fanControllerService.TemperatureSensors.Where(sensor => sensor.Name.Equals(settings.Element("TemperatureSensor").Value)).FirstOrDefault();
                        this.SelectedFanControllerTemplate = this.fanControllerService.FanControllerTemplates.Where(template => template.Name.Equals(settings.Element("FanControllerTemplate").Value)).FirstOrDefault();

                        // Set value from config
                        var fanSpeedValue = (float)XmlConvert.ToDouble(settings.Element("SelectedFanSpeedValue").Value);

                        if (IsDefaultModeEnabled || IsAdvancedModeEnabled)
                        {
                            this.CanSetFanSpeed = false;

                            if (IsDefaultModeEnabled)
                                this.SetFanSensorDefaultValue();

                            // Set current value reported from controller
                            this.SelectedFanSpeedValue = fanSpeedValue;
                        }
                        else
                        {
                            this.CanSetFanSpeed = true;

                            if (fanSpeedValue == 0)
                                fanSpeedValue = this.GetFanSensorValue();

                            this.SelectedFanSpeedValue = fanSpeedValue;
                        }
                    }
                }
                // If no elements found -> create the first one with default values
                else
                {
                    this.SelectedTemperatureSensor = this.fanControllerService.TemperatureSensors.FirstOrDefault();
                    this.SelectedFanControllerTemplate = this.fanControllerService.FanControllerTemplates.FirstOrDefault();
                    this.CanSetFanSpeed = false;
                    this.SelectedFanSpeedValue = this.GetFanSensorValue();
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
                    XElement settings = (from xml in xdoc.Descendants("MainboardFanController")
                                         where xml.Attribute("Name").Value == this.FanSensorName
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
            return new XElement("MainboardFanController", new XAttribute("Name", this.FanSensorName),
                new XElement("IsDefaultModeEnabled", XmlConvert.ToString(this.IsDefaultModeEnabled)),
                new XElement("IsAdvancedModeEnabled", XmlConvert.ToString(this.IsAdvancedModeEnabled)),
                new XElement("SelectedFanSpeedValue", XmlConvert.ToString(this.SelectedFanSpeedValue)),
                new XElement("TemperatureSensor", (this.SelectedTemperatureSensor != null) ? this.SelectedTemperatureSensor.Name : string.Empty),
                new XElement("FanControllerTemplate", (this.SelectedFanControllerTemplate != null) ? this.SelectedFanControllerTemplate.Name : string.Empty));
        }

        /// <summary>
        /// Set FAN-Speed dynamic -> for Advanced-Mode
        /// </summary>
        private void SetFanSpeed()
        {
            if (!String.IsNullOrEmpty(this.FanSensorName) && this.SelectedTemperatureSensor != null)
            {
                try
                {
                    // Get value for current temperature
                    //var values = this.smoothedPoints.Where(p => p.X > this.SelectedTemperatureSensorCurrentValue && p.X < (this.SelectedTemperatureSensorCurrentValue + 0.5));

                    IList<DataPoint> values = new List<DataPoint>();
                    var tempValue = this.SelectedTemperatureSensorCurrentValue;

                    foreach (var d in this.smoothedPoints)
                    {
                        if (d.X > tempValue && d.X < (tempValue + 0.5))
                            values.Add(d);
                    }

                    if (values != null && values.Count() > 0)
                    {
                        var newValue = values.Max(p => p.Y);
                        
                        // Set value
                        this.SetFanSensorValue((float)newValue);

                        OnPropertyChanged(() => this.CurrentFanSpeedValue);
                        OnPropertyChanged(() => this.MinFanSpeedValue);
                        OnPropertyChanged(() => this.MaxFanSpeedValue);
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
            if (!String.IsNullOrEmpty(this.FanSensorName) && this.SelectedTemperatureSensor != null)
            {
                try
                {
                    // Set new value
                    this.SetFanSensorValue(newValue);

                    OnPropertyChanged(() => this.CurrentFanSpeedValue);
                    OnPropertyChanged(() => this.MinFanSpeedValue);
                    OnPropertyChanged(() => this.MaxFanSpeedValue);
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

        private string fanSensorName;

        /// <summary>
        /// The fan sensor
        /// </summary>
        public string FanSensorName
        {
            get { return fanSensorName; }
            private set { this.SetProperty<string>(ref this.fanSensorName, value); }
        }

        /// <summary>
        /// Reference to the fan controller service
        /// </summary>
        public IFanControllerService FanControllerService
        {
            get
            {
                return this.fanControllerService;
            }
        }

        private ISensor selectedTemperatureSensor;

        /// <summary>
        /// The selected mainboard temperature sensor
        /// </summary>
        public ISensor SelectedTemperatureSensor
        {
            get { return selectedTemperatureSensor; }
            set
            {
                if (this.SetProperty<ISensor>(ref this.selectedTemperatureSensor, value))
                {
                    if (value != null)
                    {
                        // Write settings to config
                        this.WriteFanControllerSettings("TemperatureSensor", this.SelectedTemperatureSensor.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Get current temperature sensor value
        /// </summary>
        /// <returns></returns>
        public float SelectedTemperatureSensorCurrentValue
        {
            get
            {
                if (this.SelectedTemperatureSensor != null)
                {
                    var tempSensor = this.ohmManagementService.MainboardTemperatureSensors.Where(s => s.Name.Equals(this.SelectedTemperatureSensor.Name)).FirstOrDefault();

                    if (tempSensor != null && tempSensor.Value != null)
                    {
                        return tempSensor.Value.Value;
                    }
                }

                return default(float);
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
                        if (this.FanControllerUserControl != null && this.FanControllerUserControl is MainboardFanControllerUserControl)
                        {
                            ((MainboardFanControllerUserControl)this.FanControllerUserControl).ClearChart();
                        }

                        // Set controller default mode
                        this.SetFanSensorDefaultValue();

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
                        if (this.FanControllerUserControl != null && this.FanControllerUserControl is MainboardFanControllerUserControl)
                        {
                            ((MainboardFanControllerUserControl)this.FanControllerUserControl).ClearChart();
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
                    if (!this.IsDefaultModeEnabled && !this.IsAdvancedModeEnabled)
                    {
                        // Set new value
                        this.SetFanSensorValue(value);
                        // Write to Config-File
                        this.WriteFanControllerSettings("SelectedFanSpeedValue", value);
                    }

                    this.OnPropertyChanged(() => this.CurrentFanSpeedValue);
                    this.OnPropertyChanged(() => this.MinFanSpeedValue);
                    this.OnPropertyChanged(() => this.MaxFanSpeedValue);
                }
            }
        }

        /// <summary>
        /// Current fan speed value
        /// </summary>
        public float CurrentFanSpeedValue
        {
            get
            {
                return this.GetFanSensorValue();
            }
        }

        /// <summary>
        /// Min fan speed value
        /// </summary>
        public float MinFanSpeedValue
        {
            get { return this.GetFanSensorMinValue(); }
        }

        /// <summary>
        /// Max fan speed value
        /// </summary>
        public float MaxFanSpeedValue
        {
            get { return this.GetFanSensorMaxValue(); }
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
        public UserControl FanControllerUserControl
        {
            get { return mainboardFanUserControl; }
            private set { this.SetProperty<UserControl>(ref this.mainboardFanUserControl, value); }
        }

        #endregion Properties
    }
}