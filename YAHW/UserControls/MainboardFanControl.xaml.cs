using OpenHardwareMonitor.Hardware;
using System.Windows;
using System.Windows.Controls;

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
        public MainboardFanControl()
        {
            InitializeComponent();
        }

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
            DependencyProperty.Register("FanController", typeof(ISensor), typeof(MainboardFanControl), new PropertyMetadata(null)); 
    }
}