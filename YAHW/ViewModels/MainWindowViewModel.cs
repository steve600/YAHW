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

using FirstFloor.ModernUI.Presentation;
using System;
using System.Reflection;
using System.Windows.Input;
using XAHW.Interfaces;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.MVVMBase;
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
            // Initialize commands
            this.InitializeCommands();

            // Read config file
            IConfigurationFile configFile = this.LoadApplicationConfigFile();

            // Register services
            DependencyFactory.RegisterInstance<ILocalizerService>(ServiceNames.LocalizerService, new LocalizerService("de-DE"));
            DependencyFactory.RegisterInstance<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService, new OpenHardwareMonitorManagementService());
            DependencyFactory.RegisterInstance<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService, new WmiHardwareInfoService());
            DependencyFactory.RegisterInstance<IExceptionReporterService>(ServiceNames.ExceptionReporterService, new ExceptionReporterService());
            DependencyFactory.RegisterInstance<ILoggingService>(ServiceNames.LoggingService, new LoggingServiceNLog());
            DependencyFactory.RegisterInstance<FanControllerService>(ServiceNames.FanControllerService, new FanControllerService());
            //DependencyFactory.RegisterInstance(typeof(FanControllerService), ServiceNames.FanControllerService, new FanControllerService());

            // Start Fan-Controller-Service
            DependencyFactory.Resolve<FanControllerService>(ServiceNames.FanControllerService).Start();

            // Application title
            string appVersion = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainWindowTitle");
            this.ApplicationTitle = String.Format(appVersion, Assembly.GetEntryAssembly().GetName().Version.ToString());

            // Create appearance ViewModel
            DependencyFactory.RegisterInstance<AppearanceViewModel>(GeneralConstants.ApperanceViewModel, new AppearanceViewModel());
        }

        /// <summary>
        /// Load configuration file
        /// </summary>
        /// <returns></returns>
        private IConfigurationFile LoadApplicationConfigFile()
        {
            // Create config file
            IConfigurationFile configFile = new XmlConfigurationFile(DirectoryConstants.ApplicationConfig);

            if (!configFile.Load())
            {
                // Add section GeneralSettings
                configFile.Sections.Add("GeneralSettings");
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

        #region Commands

        #region Properties

        private string applicationTitle;

        /// <summary>
        /// The application title
        /// </summary>
        public string ApplicationTitle
        {
            get { return applicationTitle; }
            private set { this.SetProperty<string>(ref this.applicationTitle, value); }
        }

        #endregion Properties

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void InitializeCommands()
        {
            this.CloseApplicationCommand = new DelegateCommand(this.CloseApplicationCommandExecute, this.CloseApplicationCommandCanExecute);
        }

        /// <summary>
        /// Close application command
        /// </summary>
        public ICommand CloseApplicationCommand { get; private set; }

        /// <summary>
        /// Close application command can exectue
        /// </summary>
        public bool CloseApplicationCommandCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Close application command execute
        /// </summary>
        public void CloseApplicationCommandExecute()
        {
            System.Environment.Exit(0);
        }

        #endregion Commands
    }
}