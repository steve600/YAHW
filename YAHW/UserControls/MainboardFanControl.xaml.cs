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
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using System.Collections.Generic;
using System.Xml.Linq;
using YAHW.Model;
using System.Xml;
using System;
using System.Collections.ObjectModel;
using YAHW.Services;
using YAHW.Constants;
using OxyPlot.Series;
using System.Windows.Data;
using System.Linq;
using YAHW.Helper;
using YAHW.Interfaces;
using System.Windows.Threading;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for MainboardFanControl.xaml
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
    public partial class MainboardFanControl : UserControl
    {
        #region Members and Constants

        // Timer
        private DispatcherTimer timer = null;
        private double timerIntervall = 1000;

        // OpenHardwareManagement-Service
        private IOpenHardwareMonitorManagementService service = null;

        // Smoothed data points
        private IList<DataPoint> smoothedPoints = null;

        #endregion Members and Constants

        /// <summary>
        /// CTOR
        /// </summary>
        public MainboardFanControl()
        {
            InitializeComponent();

            // Create timer
            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(this.timerIntervall);
            timer.Tick += timer_Tick;

            // Read the fan controller templates (Standard, Silent, ...)
            this.ReadFanControllerTemplates();

            // Resolve service
            this.service = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);

            if (service != null)
            {
                this.MainboardTemperatureSensors = this.service.MainboardTemperatureSensors;

                if (this.MainboardTemperatureSensors.Count >= 1)
                {
                    this.SelectedMainboardTemperatureSensor = this.MainboardTemperatureSensors[0];
                }
            }

            // Start timer
            this.timer.Start();
        }

        #region EventHandler

        /// <summary>
        /// Timer-Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            // Check if Advanced-Mode is enabled
            if (IsAdvancedModeEnabled)
            {
                // Set Fan-Speed
                this.SetFanSpeed();
                // Draw actual value to graph
                this.DrawActualValue();
            }

            // Update Fan-Speed display values
            this.CurrentFanSpeedValue = this.FanController.Value.Value;
            this.MinFanSpeedValue = this.FanController.Min.Value;
            this.MaxFanSpeedValue = this.FanController.Max.Value;
        }

        #endregion EventHandler

        #region Private Methods

        /// <summary>
        /// Calculate smooth points to detect Y-Values (WORKAROUND -> there should be better solution???)
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <returns></returns>
        private List<DataPoint> CalculateSmoothedPoints(List<DataPoint> dataPoints)
        {
            //var wpfSeries = this.fanSpeedChart.Series[0] as OxyPlot.Wpf.LineSeries;
            //OxyPlot.Series.LineSeries s = wpfSeries.InternalSeries as OxyPlot.Series.LineSeries;

            double ToleranceDivisor = 200;
            //double tolerance = Math.Abs(Math.Max(s.MaxX - s.MinX, s.MaxY - s.MinY) / ToleranceDivisor);
            double tolerance = Math.Abs(Math.Max(100.0 - 0.0, 100.0 - 0.0) / ToleranceDivisor);

            return CanonicalSplineHelper.CreateSpline(dataPoints, 0.5, null, false, tolerance);
        }

        /// <summary>
        /// Set FAN-Speed dynamic -> for Advanced-Mode
        /// </summary>
        private void SetFanSpeed()
        {
            if (this.FanController != null && this.SelectedMainboardTemperatureSensor != null)
            {
                try
                {
                    // Get value
                    var values = this.smoothedPoints.Where(p => p.X > this.SelectedMainboardTemperatureSensor.Value.Value && p.X < (this.SelectedMainboardTemperatureSensor.Value.Value + 0.5));

                    if (values != null && values.Count() > 0)
                    {
                        var newValue = values.Max(p => p.Y);
                        // Set value
                        this.FanController.Control.SetSoftware((float)newValue);
                        // Accept
                        DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService).AcceptNewSettings();
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
            if (this.FanController != null && this.SelectedMainboardTemperatureSensor != null)
            {
                try
                {
                    // Set value
                    this.FanController.Control.SetSoftware(newValue);
                    // Accept
                    DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService).AcceptNewSettings();
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
        /// Draw actual value to chart
        /// </summary>
        private void DrawActualValue()
        {
            // Clear current annotations
            this.fanSpeedChart.Annotations.Clear();

            // Create new annotation
            var p = new OxyPlot.Wpf.PointAnnotation();

            if (this.SelectedMainboardTemperatureSensor != null)
            {
                if (this.SelectedMainboardTemperatureSensor.Value != null)
                {
                    p.X = this.SelectedMainboardTemperatureSensor.Value.Value;
                }
            }

            if (this.FanController != null)
            {
                if (this.FanController.Value != null)
                {
                    p.Y = this.FanController.Value.Value;
                }
            }

            // Annotation description
            p.Text = String.Format(DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlActualValueAnnotation"),
                                   String.Format("{0:f2}", p.X),
                                   String.Format("{0:f2}", p.Y));

            this.fanSpeedChart.Annotations.Add(p);

            this.fanSpeedChart.InvalidatePlot(true);
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

                if (FanControllerTemplates.Count >= 1)
                    this.SelectedFanControllerTemplate = this.FanControllerTemplates[0];
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

        /// <summary>
        /// Read Fan-Controller-Settings
        /// </summary>
        private void ReadFanControllerSettings()
        {
            // Check if file exists -> if not create one
            if (!System.IO.File.Exists(DirectoryConstants.FanControllerSettingsConfig))
            {
                // Create the settings file
                this.CreateSettingsFile();
            }

            try
            {
                // Read XML-File
                XDocument xdoc = XDocument.Load(DirectoryConstants.FanControllerSettingsConfig);

                // Check if elements exists
                if (xdoc.Descendants("FanControllerSettings").FirstOrDefault().HasElements)
                {
                    XElement settings = (from xml in xdoc.Descendants("FanController")
                                         where xml.Attribute("Name").Value == this.FanController.Name
                                         select xml).FirstOrDefault();

                    // If settings for the fan controller not found -> create and save settings
                    if (settings == null)
                    {
                        xdoc.Element("FanControllerSettings").Add(this.GetSettingsAsXml());
                        xdoc.Save(DirectoryConstants.FanControllerSettingsConfig);
                    }
                    else
                    {
                        // Set settings from XML-File
                        this.IsDefaultModeEnabled = XmlConvert.ToBoolean(settings.Element("IsDefaultModeEnabled").Value);
                        this.IsAdvancedModeEnabled = XmlConvert.ToBoolean(settings.Element("IsAdvancedModeEnabled").Value);
                        this.SelectedMainboardTemperatureSensor = this.MainboardTemperatureSensors.Where(sensor => sensor.Name.Equals(settings.Element("TemperatureSensor").Value)).FirstOrDefault();
                        this.SelectedFanControllerTemplate = this.FanControllerTemplates.Where(template => template.Name.Equals(settings.Element("FanControllerTemplate").Value)).FirstOrDefault();

                        if (IsDefaultModeEnabled || IsAdvancedModeEnabled)
                        {
                            this.IsFanSpeedSliderEnabled = false;
                            // Set current value reported from controller
                            this.SelectedFanSpeedValue = this.FanController.Value.Value;
                        }
                        else
                        {
                            this.IsFanSpeedSliderEnabled = true;
                            // Set value from config
                            var valueFromSettings = (float)XmlConvert.ToDouble(settings.Element("SelectedFanSpeedValue").Value);

                            if (valueFromSettings == 0)
                                valueFromSettings = this.FanController.Value.Value;

                            this.SelectedFanSpeedValue = valueFromSettings;
                        }
                    }
                }
                // If no elements found -> create the first one
                else
                {
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
                                         where xml.Attribute("Name").Value == this.FanController.Name
                                         select xml).FirstOrDefault();

                    settings.SetElementValue(settingName, value);

                    xdoc.Save(DirectoryConstants.FanControllerSettingsConfig);
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
            return new XElement("FanController", new XAttribute("Name", this.FanController.Name),
                new XElement("IsDefaultModeEnabled", XmlConvert.ToString(this.IsDefaultModeEnabled)),
                new XElement("IsAdvancedModeEnabled", XmlConvert.ToString(this.IsAdvancedModeEnabled)),
                new XElement("SelectedFanSpeedValue", XmlConvert.ToString(this.SelectedFanSpeedValue)),
                new XElement("TemperatureSensor", (this.SelectedMainboardTemperatureSensor != null) ? this.SelectedMainboardTemperatureSensor.Name : string.Empty),
                new XElement("FanControllerTemplate", (this.SelectedFanControllerTemplate != null) ? this.SelectedFanControllerTemplate.Name : string.Empty));
        }

        #endregion Private Methods

        #region Callbacks

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnFanControllerChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainboardFanControl instance = (MainboardFanControl)sender;
            instance.FanControllerChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void FanControllerChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            if (this.FanController != null)
            {
                // Read settings for fan controller
                this.ReadFanControllerSettings();
            }
        }

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSelectedFanSpeedValueChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainboardFanControl instance = (MainboardFanControl)sender;
            instance.SelectedFanSpeedValueChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void SelectedFanSpeedValueChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            try
            {
                if (args != null)
                {
                    if (args.NewValue is float)
                    {
                        // Set new value
                        this.FanController.Control.SetSoftware((float)args.NewValue);
                        // Accept
                        DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService).AcceptNewSettings();
                        // Write to Config-File
                        this.WriteFanControllerSettings("SelectedFanSpeedValue", args.NewValue);
                    }
                }
            }
            catch (Exception ex)
            {
                // If an Excecption occurs -> Set default mode!
                this.IsDefaultModeEnabled = true;

                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorSettingFanSpeed");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnIsAdvancedModeEnabledChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainboardFanControl instance = (MainboardFanControl)sender;
            instance.IsAdvancedModeEnabledChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void IsAdvancedModeEnabledChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is bool && (bool)args.NewValue)
            {
                this.IsFanSpeedSliderEnabled = false;
            }
            else
            {
                // Delete annotations
                this.fanSpeedChart.Annotations.Clear();
                this.fanSpeedChart.InvalidatePlot(true);

                // Enable slider
                if (!IsDefaultModeEnabled)
                {
                    this.IsFanSpeedSliderEnabled = true;
                    // Set fan speed to last selected value
                    this.SetFanSpeed(this.SelectedFanSpeedValue);
                }
            }

            if (args.OldValue != null)
            {
                this.WriteFanControllerSettings("IsAdvancedModeEnabled", args.NewValue);
            }
        }

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnIsDefaultModeEnabledChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainboardFanControl instance = (MainboardFanControl)sender;
            instance.IsDefaultModeEnabledChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void IsDefaultModeEnabledChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is bool && (bool)args.NewValue)
            {
                // Delete annotations
                this.fanSpeedChart.Annotations.Clear();
                this.fanSpeedChart.InvalidatePlot(true);

                // Set controller default mode
                this.FanController.Control.SetDefault();
                DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService).AcceptNewSettings();

                // Disable advanced mode
                this.IsAdvancedModeEnabled = false;

                // Disable slider
                this.IsFanSpeedSliderEnabled = false;
            }
            else
            {
                this.IsFanSpeedSliderEnabled = true;
            }

            if (args.OldValue != null)
            {
                this.WriteFanControllerSettings("IsDefaultModeEnabled", args.NewValue);
            }
        }

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSelectedFanControllerTemplateChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainboardFanControl instance = (MainboardFanControl)sender;
            instance.SelectedFanControllerTemplateChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void SelectedFanControllerTemplateChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue != null && args.NewValue is FanControllerTemplate)
            {
                FanControllerTemplate fanControllerTemplate = args.NewValue as FanControllerTemplate;

                // Calculate new DataPoints for FanCurve
                this.smoothedPoints = this.CalculateSmoothedPoints(fanControllerTemplate.DataPoints.ToList());

                if (args.OldValue != null)
                {
                    // Write new value to config
                    this.WriteFanControllerSettings("FanControllerTemplate", fanControllerTemplate.Name);
                }
            }
        }

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSelectedMainboardTemperatureSensorChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainboardFanControl instance = (MainboardFanControl)sender;
            instance.SelectedMainboardTemperatureSensorChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void SelectedMainboardTemperatureSensorChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                if (args.NewValue != null && args.NewValue is ISensor)
                {
                    this.WriteFanControllerSettings("TemperatureSensor", ((ISensor)args.NewValue).Name);
                }
            }
        }

        #endregion Callbacks

        #region Dependency Properties

        /// <summary>
        /// Fan-Controller
        /// </summary>
        public ISensor FanController
        {
            get { return (ISensor)GetValue(FanControllerProperty); }
            set { SetValue(FanControllerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FanController.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FanControllerProperty =
            DependencyProperty.Register("FanController", typeof(ISensor), typeof(MainboardFanControl), new PropertyMetadata(OnFanControllerChangedPropertyCallback));

        /// <summary>
        /// Flag if Default-Mode is enabeld
        /// </summary>
        public bool IsDefaultModeEnabled
        {
            get { return (bool)GetValue(IsDefaultModeEnabledProperty); }
            set { SetValue(IsDefaultModeEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDefaultModeEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDefaultModeEnabledProperty =
            DependencyProperty.Register("IsDefaultModeEnabled", typeof(bool), typeof(MainboardFanControl), new PropertyMetadata(true, OnIsDefaultModeEnabledChangedPropertyCallback));

        /// <summary>
        /// Flag if Fan-Speed-Slider is enabled
        /// </summary>
        public bool IsFanSpeedSliderEnabled
        {
            get { return (bool)GetValue(IsFanSpeedSliderEnabledProperty); }
            set { SetValue(IsFanSpeedSliderEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFanSpeedSliderEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFanSpeedSliderEnabledProperty =
            DependencyProperty.Register("IsFanSpeedSliderEnabled", typeof(bool), typeof(MainboardFanControl), new PropertyMetadata(false));

        /// <summary>
        /// Fan-Controller-Templates
        /// </summary>
        public ObservableCollection<FanControllerTemplate> FanControllerTemplates
        {
            get { return (ObservableCollection<FanControllerTemplate>)GetValue(FanControllerTemplatesProperty); }
            set { SetValue(FanControllerTemplatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FanControllerTemplates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FanControllerTemplatesProperty =
            DependencyProperty.Register("FanControllerTemplates", typeof(ObservableCollection<FanControllerTemplate>), typeof(MainboardFanControl), new PropertyMetadata(null));

        /// <summary>
        /// Selected Fan-Controller-Template
        /// </summary>
        public FanControllerTemplate SelectedFanControllerTemplate
        {
            get { return (FanControllerTemplate)GetValue(SelectedFanControllerTemplateProperty); }
            set { SetValue(SelectedFanControllerTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFanControllerTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFanControllerTemplateProperty =
            DependencyProperty.Register("SelectedFanControllerTemplate", typeof(FanControllerTemplate), typeof(MainboardFanControl), new PropertyMetadata(null, OnSelectedFanControllerTemplateChangedPropertyCallback));

        /// <summary>
        /// List with Mainboard-Temperature-Sensors
        /// </summary>
        public IList<ISensor> MainboardTemperatureSensors
        {
            get { return (IList<ISensor>)GetValue(MainboardTemperatureSensorsProperty); }
            set { SetValue(MainboardTemperatureSensorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainboardTemperatureSensors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainboardTemperatureSensorsProperty =
            DependencyProperty.Register("MainboardTemperatureSensors", typeof(IList<ISensor>), typeof(MainboardFanControl), new PropertyMetadata(null));

        /// <summary>
        /// The selected Mainboard-Temperature-Sensor
        /// </summary>
        public ISensor SelectedMainboardTemperatureSensor
        {
            get { return (ISensor)GetValue(SelectedMainboardTemperatureSensorProperty); }
            set { SetValue(SelectedMainboardTemperatureSensorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedMainboardTemperatureSensor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedMainboardTemperatureSensorProperty =
            DependencyProperty.Register("SelectedMainboardTemperatureSensor", typeof(ISensor), typeof(MainboardFanControl), new PropertyMetadata(null, OnSelectedMainboardTemperatureSensorChangedPropertyCallback));

        /// <summary>
        /// Flag if advanced mode is enabled
        /// </summary>
        public bool IsAdvancedModeEnabled
        {
            get { return (bool)GetValue(IsAdvancedModeEnabledProperty); }
            set { SetValue(IsAdvancedModeEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAdvancedModeEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAdvancedModeEnabledProperty =
            DependencyProperty.Register("IsAdvancedModeEnabled", typeof(bool), typeof(MainboardFanControl), new PropertyMetadata(false, OnIsAdvancedModeEnabledChangedPropertyCallback));

        /// <summary>
        /// Selected fan speed value
        /// </summary>
        public float SelectedFanSpeedValue
        {
            get { return (float)GetValue(SelectedFanSpeedValueProperty); }
            set { SetValue(SelectedFanSpeedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFanSpeedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFanSpeedValueProperty =
            DependencyProperty.Register("SelectedFanSpeedValue", typeof(float), typeof(MainboardFanControl), new PropertyMetadata(OnSelectedFanSpeedValueChangedPropertyCallback));

        /// <summary>
        /// Current Fan-Speed-Value
        /// </summary>
        public float CurrentFanSpeedValue
        {
            get { return (float)GetValue(CurrentFanSpeedValueProperty); }
            set { SetValue(CurrentFanSpeedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentFanSpeedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentFanSpeedValueProperty =
            DependencyProperty.Register("CurrentFanSpeedValue", typeof(float), typeof(MainboardFanControl), new PropertyMetadata(default(float)));

        /// <summary>
        /// Min-Fan-Speed-Value
        /// </summary>
        public float MinFanSpeedValue
        {
            get { return (float)GetValue(MinFanSpeedValueProperty); }
            set { SetValue(MinFanSpeedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinFanSpeedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinFanSpeedValueProperty =
            DependencyProperty.Register("MinFanSpeedValue", typeof(float), typeof(MainboardFanControl), new PropertyMetadata(default(float)));

        /// <summary>
        /// Max-Fan-Speed-Value
        /// </summary>
        public float MaxFanSpeedValue
        {
            get { return (float)GetValue(MaxFanSpeedValueProperty); }
            set { SetValue(MaxFanSpeedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxFanSpeedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxFanSpeedValueProperty =
            DependencyProperty.Register("MaxFanSpeedValue", typeof(float), typeof(MainboardFanControl), new PropertyMetadata(default(float)));

        #endregion Dependency Properties
    }
}