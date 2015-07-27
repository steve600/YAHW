using System.Windows;
using System.Windows.Controls;
using YAHW.Model;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for MainboardInformationUserControl.xaml
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
    public partial class MainboardInformationUserControl : UserControl
    {
        public MainboardInformationUserControl()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        /// <summary>
        /// Mainboard informations
        /// </summary>
        public MainboardInformation MainboardInfo
        {
            get { return (MainboardInformation)GetValue(MainboardInfoProperty); }
            set { SetValue(MainboardInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainboardInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainboardInfoProperty =
            DependencyProperty.Register("MainboardInfo", typeof(MainboardInformation), typeof(MainboardInformationUserControl), new PropertyMetadata(null));

        #endregion Dependency Properties
    }
}