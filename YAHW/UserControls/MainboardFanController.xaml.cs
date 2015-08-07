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
    /// Interaktionslogik für MainboardFanController.xaml
    /// </summary>
    public partial class MainboardFanController : UserControl
    {
        public MainboardFanController()
        {
            InitializeComponent();

            // Set ComboBoxes
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

            if (this.FanController.SelectedMainboardTemperatureSensor != null)
            {
                if (this.FanController.SelectedMainboardTemperatureSensor.Value != null)
                {
                    p.X = this.FanController.SelectedMainboardTemperatureSensor.Value.Value;
                }
            }

            if (this.FanController.FanSensor != null)
            {
                if (this.FanController.FanSensor.Value != null)
                {
                    p.Y = this.FanController.FanSensor.Value.Value;
                }
            }

            // Annotation description
            p.Text = String.Format(DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlActualValueAnnotation"),
                                   String.Format("{0:f2}", p.X),
                                   String.Format("{0:f2}", p.Y));

            this.fanSpeedChart.Annotations.Add(p);

            this.fanSpeedChart.InvalidatePlot(true);
        }

        /// <summary>
        /// The associated fan controller
        /// </summary>
        public FanController FanController
        {
            get { return (FanController)GetValue(FanControllerProperty); }
            set { SetValue(FanControllerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FanController.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FanControllerProperty =
            DependencyProperty.Register("FanController", typeof(FanController), typeof(MainboardFanController), new PropertyMetadata(null));
    }
}
