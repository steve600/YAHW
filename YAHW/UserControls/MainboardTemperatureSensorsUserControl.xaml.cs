using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for MainboardTemperatureSensorsUserControl.xaml
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
    public partial class MainboardTemperatureSensorsUserControl : UserControl
    {
        public MainboardTemperatureSensorsUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// List with temperature sensors
        /// </summary>
        public IList<ISensor> TemperatureSensors
        {
            get { return (IList<ISensor>)GetValue(TemperatureSensorsProperty); }
            set { SetValue(TemperatureSensorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemperatureSensors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemperatureSensorsProperty =
            DependencyProperty.Register("TemperatureSensors", typeof(IList<ISensor>), typeof(MainboardTemperatureSensorsUserControl), new PropertyMetadata(null));
    }
}
