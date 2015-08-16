using OpenHardwareMonitor.Hardware;
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
using System.Windows.Threading;
using YAHW.Helper;

namespace YAHW.UserControls
{
    /// <summary>
    /// Interaktionslogik für SensorTile.xaml
    /// </summary>
    public partial class SensorTile : UserControl
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public SensorTile()
        {
            InitializeComponent();
        }

        #region Drag&Drop

        private void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = UIHelper.GetParentOfType<Grid>(sender as UserControl);

            if (parent != null && parent is Grid)
            {
                ((Grid)parent).ShowGridLines = true;
            }
        }

        private void layoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            SensorTile tile = sender as SensorTile;
            if (tile != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject dragData = new DataObject("Tile", this);
                DragDrop.DoDragDrop(tile, dragData, DragDropEffects.Move);
            }
        }

        #endregion Drag&Drop

        #region Callbacks

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnHardwareSensorChangedPropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SensorTile instance = (SensorTile)sender;
            instance.HardwareSensorChangedPropertyCallback(e); // Call non-static
        }

        /// <summary>
        /// This is a non-static version of the dep. property changed event
        /// </summary>
        /// <param name="args"></param>
        void HardwareSensorChangedPropertyCallback(DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue != null && args.NewValue is ISensor)
            {
                var s = args.NewValue as ISensor;

                switch (s.SensorType)
                {
                    case SensorType.Load:
                        this.SensorIcon = TryFindResource("appbar_graph_line") as Canvas;
                        break;
                    case SensorType.Temperature:
                        this.SensorIcon = TryFindResource("appbar_thermometer_celcius") as Canvas;
                        break;
                    case SensorType.Clock:
                        this.SensorIcon = TryFindResource("speedometer") as Canvas;
                        break;
                    case SensorType.Fan:
                        this.SensorIcon = TryFindResource("appbar_fan_box") as Canvas;
                        break;
                    case SensorType.Power:
                        this.SensorIcon = TryFindResource("flash") as Canvas;
                        break;
                    case SensorType.Voltage:
                        this.SensorIcon = TryFindResource("flash") as Canvas;
                        break;
                    default:
                        break;
                }

                this.DisplayText = s.Name;

                this.UpdateValues();
            }
        }

        /// <summary>
        /// Update values
        /// </summary>
        public void UpdateValues()
        {
            switch (this.HardwareSensor.SensorType)
            {
                case SensorType.Voltage:
                    this.CurrentValue = string.Format("{0:F4} V", this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F4} V", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F4} V", this.HardwareSensor.Max);
                    break;
                case SensorType.Clock:
                    this.CurrentValue = string.Format("{0:F1} Mhz", 1e-3f * this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F1} Mhz", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F1} Mhz", this.HardwareSensor.Max);
                    break;
                case SensorType.Load:
                    this.CurrentValue = string.Format("{0:F0} %", this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F0} %", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F0} %", this.HardwareSensor.Max);
                    break;
                case SensorType.Temperature:
                    this.CurrentValue = string.Format("{0:F0} °C", this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F0} °C", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F0} °C", this.HardwareSensor.Max);
                    break;
                case SensorType.Fan:
                    this.CurrentValue = string.Format("{0:F1} %", 1e-3f * this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F1} %", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F1} %", this.HardwareSensor.Max);
                    break;
                case SensorType.Flow:
                    this.CurrentValue = string.Format("{0:F1}", 1e-3f * this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F1}", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F1}", this.HardwareSensor.Max);
                    break;
                case SensorType.Control:
                    this.CurrentValue = string.Format("{0:F0}", this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F0}", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F0}", this.HardwareSensor.Max);
                    break;
                case SensorType.Level:
                    this.CurrentValue = string.Format("{0:F0}", this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F0}", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F0}", this.HardwareSensor.Max);
                    break;
                case SensorType.Power:
                    this.CurrentValue = string.Format("{0:F0} V", this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F0} V", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F0} V", this.HardwareSensor.Max);
                    break;
                case SensorType.Data:
                    this.CurrentValue = string.Format("{0:F0}", this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F0}", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F0}", this.HardwareSensor.Max);
                    break;
                case SensorType.Factor:
                    this.CurrentValue = string.Format("{0:F0}", this.HardwareSensor.Value);
                    this.MinValue = string.Format("{0:F0}", this.HardwareSensor.Min);
                    this.MaxValue = string.Format("{0:F0}", this.HardwareSensor.Max);
                    break;
            }
        }

        #endregion Callbacks

        /// <summary>
        /// The hardware sensor
        /// </summary>
        public ISensor HardwareSensor
        {
            get { return (ISensor)GetValue(HardwareSensorProperty); }
            set { SetValue(HardwareSensorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HardwareSensor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HardwareSensorProperty =
            DependencyProperty.Register("HardwareSensor", typeof(ISensor), typeof(SensorTile), new PropertyMetadata(OnHardwareSensorChangedPropertyCallback));

        /// <summary>
        /// Sensor icon
        /// </summary>
        public Visual SensorIcon
        {
            get { return (Visual)GetValue(SensorIconProperty); }
            set { SetValue(SensorIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SensorIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SensorIconProperty =
            DependencyProperty.Register("SensorIcon", typeof(Visual), typeof(SensorTile), new PropertyMetadata(null));

        /// <summary>
        /// The display text
        /// </summary>
        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register("DisplayText", typeof(string), typeof(SensorTile), new PropertyMetadata(null));

        /// <summary>
        /// Current value
        /// </summary>
        public string CurrentValue
        {
            get { return (string)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(string), typeof(SensorTile), new PropertyMetadata(null));

        /// <summary>
        /// Min-Value
        /// </summary>
        public string MinValue
        {
            get { return (string)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(string), typeof(SensorTile), new PropertyMetadata(null));

        /// <summary>
        /// Max-Value
        /// </summary>
        public string MaxValue
        {
            get { return (string)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(string), typeof(SensorTile), new PropertyMetadata(null));
    }
}
