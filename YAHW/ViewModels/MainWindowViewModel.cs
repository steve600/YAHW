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