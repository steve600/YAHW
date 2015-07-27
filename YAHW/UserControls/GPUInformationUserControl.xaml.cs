using System.Windows;
using System.Windows.Controls;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for GPUInformationUserControl.xaml
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
    public partial class GPUInformationUserControl : UserControl
    {
        public GPUInformationUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// GPU-Informations
        /// </summary>
        public Model.GPUInformation GPUInformations
        {
            get { return (Model.GPUInformation)GetValue(GPUInformationsProperty); }
            set { SetValue(GPUInformationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GPUInformations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GPUInformationsProperty =
            DependencyProperty.Register("GPUInformations", typeof(Model.GPUInformation), typeof(GPUInformationUserControl), new PropertyMetadata(null));

        /// <summary>
        /// Show details button
        /// </summary>
        public bool ShowDetailsButton
        {
            get { return (bool)GetValue(ShowDetailsButtonProperty); }
            set { SetValue(ShowDetailsButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowDetailsButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowDetailsButtonProperty =
            DependencyProperty.Register("ShowDetailsButton", typeof(bool), typeof(GPUInformationUserControl), new PropertyMetadata(true));
    }
}