using System.Windows;
using System.Windows.Controls;
using YAHW.Model;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for CPUInformationUserControl.xaml
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
    public partial class CPUInformationUserControl : UserControl
    {
        public CPUInformationUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// CPU-Information
        /// </summary>
        public ProcessorInformation CPUInformation
        {
            get { return (ProcessorInformation)GetValue(CPUInformationProperty); }
            set { SetValue(CPUInformationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CPUInformation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CPUInformationProperty =
            DependencyProperty.Register("CPUInformation", typeof(ProcessorInformation), typeof(CPUInformationUserControl), new PropertyMetadata(null));
    }
}