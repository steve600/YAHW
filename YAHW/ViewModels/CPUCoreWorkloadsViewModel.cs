using System;
using System.Linq;
using System.Windows.Threading;
using YAHW.BaseClasses;
using YAHW.Constants;
using System.Windows.Controls;
using YAHW.UserControls;
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
    public class CPUCoreWorkloadsViewModel : ViewModelBase
    {
        #region Members and Constants

        private DispatcherTimer timer = null;
        private OpenHardwareMonitorManagementService openHardwareManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public CPUCoreWorkloadsViewModel()
        {
            this.timer = new DispatcherTimer();

            this.openHardwareManagementService = DependencyFactory.Resolve<OpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);

            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        #endregion CTOR

        #region Event-Handler

        /// <summary>
        /// Timer-Tick Event-Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            if (this.openHardwareManagementService.CPU != null)
            {
                // Update hardware item
                this.openHardwareManagementService.CPU.Update();

                // Get core workload
                foreach (var sensor in this.openHardwareManagementService.CPUCoreWorkloadSensors)
                {
                    var chart = (from r in this.MainContent.Children.OfType<CPUCoreWorkloadChartUserControl>()
                                where r.CoreName == sensor.Name
                                select r).FirstOrDefault();

                    if (chart == null)
                    {
                        var newChart = new CPUCoreWorkloadChartUserControl();
                        newChart.CoreName = sensor.Name;
                        newChart.CurrentCoreWorkload = (sensor.Value != null) ? (double)sensor.Value.Value : default(double);
                        this.MainContent.Children.Add(newChart);
                    }
                    else
                    {
                        chart.CurrentCoreWorkload = (sensor.Value != null) ? (double)sensor.Value.Value : default(double);
                    }
                }
            }
        }

        #endregion Event-Handler

        #region Properties

        private StackPanel mainContent = new StackPanel();

        /// <summary>
        /// Main-Content
        /// </summary>
        public StackPanel MainContent
        {
            get { return mainContent; }
            set { this.SetProperty<StackPanel>(ref this.mainContent, value); }
        } 
        
        #endregion Properties
    }
}