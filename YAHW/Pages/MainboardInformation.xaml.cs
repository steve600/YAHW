using System.Windows.Controls;

namespace YAHW.Pages
{
    /// <summary>
    /// Interaction logic for MainboardInformation.xaml
    /// </summary>
    public partial class MainboardInformation : UserControl
    {
        public MainboardInformation()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.MainboardInformationViewModel();
        }
    }
}
