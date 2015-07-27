using System.Collections.Generic;
using YAHW.BaseClasses;
using YAHW.SystemInformation;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// A simple view model the SystemInformation-Page
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
    public class SystemInformationViewModel : ViewModelBase
    {
        #region Operating System

        /// <summary>
        /// Operating-System
        /// </summary>
        public string OperatingSystem
        {
            get
            {
                return OSVersionInfo.OsVersionCompleteString;
            }
        }

        #endregion Operating System

        #region .NET Framework

        /// <summary>
        /// Installed .NET Framework versions
        /// </summary>
        public IList<DotNetFrameworkInfo.NetFrameworkVersionInfo> InstalledDotNetFrameworkVersions
        {
            get
            {
                return DotNetFrameworkInfo.InstalledDotNetVersions();
            }
        }

        #endregion .NET Framework
    }
}
