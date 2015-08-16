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
    /// Interaction logic for TilePage.xaml
    /// </summary>
    public partial class TilePage : UserControl
    {
        public TilePage()
        {
            InitializeComponent();
            this.DataContext = new TilePageViewModel();
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            if (this.mainContent.Content != null && this.mainContent.Content is Grid)
            {
                ((Grid)this.mainContent.Content).ShowGridLines = false;
            }
        }
    }
}