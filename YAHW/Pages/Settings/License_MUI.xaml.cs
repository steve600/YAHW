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

namespace YAHW.Pages.Settings
{
    /// <summary>
    /// Interaction logic for License_MUI.xaml
    /// </summary>
    public partial class License_MUI : UserControl
    {
        public License_MUI()
        {
            InitializeComponent();

            string licenseFile = System.IO.Path.Combine(Environment.CurrentDirectory, "Licenses", "MUI.txt");

            if (System.IO.File.Exists(licenseFile))
            {
                this.txtLicense.BBCode = System.IO.File.ReadAllText(licenseFile);
            }
            else
            {
                this.txtLicense.BBCode = "n.a.";
            }
        }
    }
}
