using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YAHW.ViewModels;

namespace YAHW.Pages
{
    /// <summary>
    /// Interaction logic for AddTilePopup.xaml
    /// </summary>
    public partial class AddTilePopup : ModernWindow
    {
        public AddTilePopup()
        {
            InitializeComponent();
            this.DataContext = new AddTilePopupViewModel();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
