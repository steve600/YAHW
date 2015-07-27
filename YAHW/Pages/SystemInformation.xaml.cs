using System.Windows.Controls;
using YAHW.ViewModels;

namespace YAHW.Pages
{
    /// <summary>
    /// <para>
    /// Interaction logic for SystemInformation.xaml
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
    public partial class SystemInformation : UserControl
    {
        public SystemInformation()
        {
            InitializeComponent();
            this.DataContext = new SystemInformationViewModel();
        }
    }
}
