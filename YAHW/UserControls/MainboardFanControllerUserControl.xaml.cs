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
using YAHW.Constants;
using YAHW.Hardware;
using YAHW.Interfaces;
using YAHW.Services;

namespace YAHW.UserControls
{
    /// <summary>
    /// Interaktionslogik für MainboardFanControllerUserControl.xaml
    /// </summary>
    public partial class MainboardFanControllerUserControl : UserControl
    {
        public MainboardFanControllerUserControl(IFanController fanController)
        {
            // Set fan controller
            this.FanController = fanController;
            // Initialize components
            InitializeComponent();
        }
        
        /// <summary>
        /// Update chart
        /// </summary>
        public void UpdateChart()
        {
            this.DrawActualValue();
        }

        /// <summary>
        /// Clear chart
        /// </summary>
        public void ClearChart()
        {
            // Delete annotations
            this.fanSpeedChart.Annotations.Clear();
            this.fanSpeedChart.InvalidatePlot(true);
        }

        /// <summary>
        /// Draw actual value to chart
        /// </summary>
        private void DrawActualValue()
        {
            // Clear current annotations
            this.fanSpeedChart.Annotations.Clear();

            // Create new annotation
            var p = new OxyPlot.Wpf.PointAnnotation();

            p.X = this.FanController.SelectedTemperatureSensorCurrentValue;
            p.Y = this.FanController.CurrentFanSpeedValue;

            // Annotation description
            p.Text = String.Format(DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlActualValueAnnotation"),
                                   String.Format("{0:f2}", p.X),
                                   String.Format("{0:f2}", p.Y));

            this.fanSpeedChart.Annotations.Add(p);

            this.fanSpeedChart.InvalidatePlot(false);
        }

        /// <summary>
        /// The associated fan controller
        /// </summary>
        public IFanController FanController
        {
            get { return (IFanController)GetValue(FanControllerProperty); }
            set { SetValue(FanControllerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainboardFanControllerUserControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FanControllerProperty =
            DependencyProperty.Register("MainboardFanControllerUserControl", typeof(IFanController), typeof(MainboardFanControllerUserControl), new PropertyMetadata(null));
    }
}
