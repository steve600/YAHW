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

namespace YAHW.CommonControls
{
    /// <summary>
    /// Interaktionslogik für DialogDisabledControl.xaml
    /// </summary>
    public partial class DialogDisabledControl : UserControl
    {
        public DialogDisabledControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The text to show if dialog disabled
        /// </summary>
        public string DialogDisabledText
        {
            get { return (string)GetValue(DialogDisabledTextProperty); }
            set { SetValue(DialogDisabledTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DialogDisabledText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogDisabledTextProperty =
            DependencyProperty.Register("DialogDisabledText", typeof(string), typeof(DialogDisabledControl), new PropertyMetadata(""));


    }
}
