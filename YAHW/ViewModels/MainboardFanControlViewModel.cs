using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Services;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the CPU-Core workload page
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
    public class MainboardFanControlViewModel : ViewModelBase
    {
        #region Members and Constants

        private OpenHardwareMonitorManagementService openHardwareMonitorManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public MainboardFanControlViewModel()
        {
            this.openHardwareMonitorManagementService = DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
            this.openHardwareMonitorManagementService.UpdateMainboardSensors();

            this.MainContent = new StackPanel();

            foreach (var fc in this.openHardwareMonitorManagementService.MainboardFanControlSensors)
            {
                var fanControl = new UserControls.MainboardFanControl();
                fanControl.FanController = fc;

                this.MainContent.Children.Add(fanControl);
            }
        }

        #endregion CTOR

        #region Properties

        private StackPanel mainContent;

        /// <summary>
        /// The main content
        /// </summary>
        public StackPanel MainContent
        {
            get { return mainContent; }
            set { this.SetProperty<StackPanel>(ref this.mainContent, value); }
        }        

        #endregion Properties
    }
}
