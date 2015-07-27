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
        public MainWindowViewModel()
        {
            // Register services
            DependencyFactory.RegisterInstance<ILocalizerService>(ServiceNames.LocalizerService, new LocalizerService("de"));
            DependencyFactory.RegisterInstance<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService, new OpenHardwareMonitorManagementService());
            DependencyFactory.RegisterInstance<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService, new WmiHardwareInfoService());
            DependencyFactory.RegisterInstance<IExceptionReporterService>(ServiceNames.ExceptionReporterService, new ExceptionReporterService());
        }
    }
}