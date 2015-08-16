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
using YAHW.EventAggregator;
using YAHW.Events;
using YAHW.Helper;

namespace YAHW.UserControls
{
    /// <summary>
    /// Interaktionslogik für TileViewDropTarget.xaml
    /// </summary>
    public partial class TileViewDropTarget : UserControl
    {
        public TileViewDropTarget()
        {
            InitializeComponent();
        }

        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            // Set Background-Color
            var b = new SolidColorBrush(Colors.LightGray);
            b.Opacity = 20;
            this.Background = b;
        }

        private void UserControl_DragLeave(object sender, DragEventArgs e)
        {
            // Reset-Background-Color
            this.Background = Brushes.Transparent;
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            //var parent = UIHelper.GetParentOfType<Grid>(this);

            // Get Row and Column of the Drop-Target
            int gridRow = Convert.ToInt32(this.GetValue(Grid.RowProperty));
            int gridCol = Convert.ToInt32(this.GetValue(Grid.ColumnProperty));

            // Get tile
            var tile = e.Data.GetData("Tile") as SensorTile;

            if (tile != null)
            {
                int orgGridRow = Convert.ToInt32(tile.GetValue(Grid.RowProperty));
                int orgGridCol = Convert.ToInt32(tile.GetValue(Grid.ColumnProperty));

                // Set position for tile
                tile.SetValue(Grid.RowProperty, gridRow);
                tile.SetValue(Grid.ColumnProperty, gridCol);

                // Set position for drop target
                this.SetValue(Grid.RowProperty, orgGridRow);
                this.SetValue(Grid.ColumnProperty, orgGridCol);

                // Fire event
                SensorTilePositionChangedEventArgs args = new SensorTilePositionChangedEventArgs(orgGridRow, orgGridCol, gridRow, gridCol, tile);
                DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<SensorTilePositionChangedEvent>().Publish(args);
            }
            
            // Reset-Background-Color
            this.Background = Brushes.Transparent;
        }
    }
}
