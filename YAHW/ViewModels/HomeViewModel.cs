using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Model;
using YAHW.Services;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// A simple view model the Home-Page
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
    public class HomeViewModel : ViewModelBase
    {
        #region Members and Constants

        private OpenHardwareMonitorManagementService openHardwareManagementService = null;

        #endregion Members and Constants

        /// <summary>
        /// CTOR
        /// </summary>
        public HomeViewModel()
        {
            this.openHardwareManagementService = DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
            this.MainboardInfo = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetMainboardInformation();
            this.ProcessorInformation = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetProcessorInformation();
            this.GPUInformation = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetGPUInformation();
        }

        private MainboardInformation mainboardInfo;

        /// <summary>
        /// Mainboard informations
        /// </summary>
        public MainboardInformation MainboardInfo
        {
            get { return mainboardInfo; }
            set { this.SetProperty<MainboardInformation>(ref this.mainboardInfo, value); }
        }

        private ProcessorInformation processorInformation;

        /// <summary>
        /// Processor Information
        /// </summary>
        public ProcessorInformation ProcessorInformation
        {
            get { return processorInformation; }
            set { this.SetProperty<ProcessorInformation>(ref this.processorInformation, value); }
        }

        private GPUInformation gpuInformation;

        /// <summary>
        /// GPU-Information
        /// </summary>
        public GPUInformation GPUInformation
        {
            get { return gpuInformation; }
            set { this.SetProperty<GPUInformation>(ref this.gpuInformation, value); }
        }
        
    }
}