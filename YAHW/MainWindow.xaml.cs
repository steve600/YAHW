using FirstFloor.ModernUI.Windows.Controls;
using YAHW.ViewModels;

namespace YAHW
{
    /// <summary>
    /// <para>
    /// Interaction logic for MainWindow.xaml
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
    public partial class MainWindow : ModernWindow
    {
        #region Members and Constants

        private MainWindowViewModel viewModel = null;

        #endregion Members and Constants

        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new MainWindowViewModel();
            this.DataContext = this.viewModel;

            // Register-Events
            this.Closed += MainWindow_Closed;
        }

        /// <summary>
        /// Window close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            if (this.viewModel != null)
                this.viewModel.ShutdownApplication();
        }
    }
}
