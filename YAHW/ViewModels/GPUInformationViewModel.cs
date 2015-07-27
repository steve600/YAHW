using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Model;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the RAM-Information page
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
    public class GPUInformationViewModel : ViewModelBase
    {
        #region Members and Constants

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public GPUInformationViewModel()
        {
            this.GPUInformation = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetGPUInformation();
        }

        #endregion CTOR

        #region Properties

        private GPUInformation gpuInformation;

        /// <summary>
        /// GPU-Information
        /// </summary>
        public GPUInformation GPUInformation
        {
            get { return gpuInformation; }
            set { this.SetProperty<GPUInformation>(ref this.gpuInformation, value); }
        }       
        
        #endregion Properties
    }
}
