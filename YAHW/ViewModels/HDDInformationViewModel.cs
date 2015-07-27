using System.Collections.Generic;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Model;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the HDD-Information page
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
    public class HDDInformationViewModel : ViewModelBase
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public HDDInformationViewModel()
        {
            this.HDDSmartInformation = DependencyFactory.Resolve<IHardwareInformationService>(ServiceNames.WmiHardwareInformationService).GetHddSmartInformation();
        }

        private Dictionary<int, HDD> hddSmartInformation;

        /// <summary>
        /// Dictionary with SMART information
        /// </summary>
        public Dictionary<int, HDD> HDDSmartInformation
        {
            get { return hddSmartInformation; }
            set { this.SetProperty<Dictionary<int, HDD>>(ref this.hddSmartInformation, value); }
        }        
    }
}