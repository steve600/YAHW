// YAHW - Yet Another Hardware Monitor
// Copyright (c) 2015 Steffen Steinbrecher
// Contact and Information: http://csharp-blog.de/category/yahw/
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE

using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using XAHW.Interfaces;
using YAHW.BaseClasses;
using YAHW.Configuration;
using YAHW.Constants;
using YAHW.EventAggregator;
using YAHW.Events;
using YAHW.Helper;
using YAHW.Interfaces;
using YAHW.MVVMBase;
using YAHW.UserControls;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the Tile-Page
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
    /// <para>Date: 14.08.2015</para>
    /// </summary>
    public class TilePageViewModel : ViewModelBase
    {
        #region Members and Constants

        private IOpenHardwareMonitorManagementService openHardwareManagementService = null;
        private TileViewConfigurationFile configurationFile = null;
        private IConfigurationFile applicationConfigFile = null;
        private int numberOfRows = 5;
        private int numberOfColumns = 5;

        #endregion Members and Constants

        /// <summary>
        /// CTOR
        /// </summary>
        public TilePageViewModel()
        {
            this.configurationFile = new TileViewConfigurationFile();

            // Intialize commands
            this.InitializeCommands();

            // Create the main grid
            this.CreateMainGrid(this.numberOfColumns, 150, this.numberOfRows, 110);

            this.openHardwareManagementService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
            this.applicationConfigFile = DependencyFactory.Resolve<IConfigurationFile>(ConfigFileNames.ApplicationConfig);

            for (int r = 0; r < this.numberOfRows; r++)
            {
                for (int c = 0; c < this.numberOfColumns; c++)
                {
                    var tile = this.configurationFile.Tiles.Where(t => t.GridRow == r && t.GridColumn == c).FirstOrDefault();

                    if (tile != null)
                    {
                        var sensor = this.openHardwareManagementService.GetSensor(tile.SensorCategory, tile.SensorName, tile.SensorType);

                        if (sensor != null)
                            this.AddSensorTile(sensor, tile.GridRow, tile.GridColumn, tile.SensorCategory);
                    }
                    else
                    {
                        TileViewDropTarget dropTarget = new TileViewDropTarget();
                        dropTarget.SetValue(Grid.RowProperty, r);
                        dropTarget.SetValue(Grid.ColumnProperty, c);
                        this.MainGrid.Children.Add(dropTarget);
                    }
                }
            }

            // Register for events
            DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<OpenHardwareMonitorManagementServiceTimerTickEvent>().Subscribe(this.OpenHardwareMonitorManagementServiceTimerTickEventHandler, ThreadOption.UIThread);
            DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<SensorTilePositionChangedEvent>().Subscribe(this.SensorTilePositionChangedEventHandler, ThreadOption.UIThread);
            DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<SensorTileAddedEvent>().Subscribe(this.SensorTileAddedEventHandler, ThreadOption.UIThread);
            DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<SensorTileDeletedEvent>().Subscribe(this.SensorTileDeletedEventHandler, ThreadOption.UIThread);
        }

        #region Event-Handler

        /// <summary>
        /// Timer-Tick-Event of the OHM-Service
        /// </summary>
        /// <param name="args"></param>
        private void OpenHardwareMonitorManagementServiceTimerTickEventHandler(OpenHardwareMonitorManagementServiceTimerTickEventArgs args)
        {
            foreach (var c in this.MainGrid.Children)
            {
                if (c is SensorTile)
                {
                    ((SensorTile)c).UpdateValues();
                }
            }
        }

        /// <summary>
        /// Sensor-Tile position changed
        /// </summary>
        /// <param name="args"></param>
        private void SensorTilePositionChangedEventHandler(SensorTilePositionChangedEventArgs args)
        {
            // Update config
            this.configurationFile.UpdateSensorTileConfig(args.SensorTile.HardwareSensor.Name, args.OldGridRow, args.OldGridColumn, args.NewGridRow, args.NewGridColumn);
        }

        /// <summary>
        /// Sensor-Tile position changed
        /// </summary>
        /// <param name="args"></param>
        private void SensorTileAddedEventHandler(SensorTileAddedEventArgs args)
        {
            // Get grid element
            var dt = this.MainGrid.Children.OfType<TileViewDropTarget>().FirstOrDefault();

            if (dt != null)
            {
                // Get grid position
                int gridRow = Convert.ToInt32(dt.GetValue(Grid.RowProperty));
                int gridColumn = Convert.ToInt32(dt.GetValue(Grid.ColumnProperty));

                // Add to dialog
                this.AddSensorTile(args.Sensor, gridRow, gridColumn, args.SensorCategory.ToString());

                // Add to config
                this.configurationFile.InsertTileConfig(args.SensorCategory.ToString(), args.Sensor.Name, args.Sensor.SensorType.ToString(), gridRow, gridColumn);
            }            
        }

        /// <summary>
        /// Sensor-Tile position changed
        /// </summary>
        /// <param name="args"></param>
        private void SensorTileDeletedEventHandler(SensorTileDeletedEventArgs args)
        {
            if (args.DeletedSensorTile != null)
            {
                int gridRow = Convert.ToInt32(args.DeletedSensorTile.GetValue(Grid.RowProperty));
                int gridColumn = Convert.ToInt32(args.DeletedSensorTile.GetValue(Grid.ColumnProperty));

                this.MainGrid.Children.Remove(args.DeletedSensorTile);

                // Create new drop target
                TileViewDropTarget dropTarget = new TileViewDropTarget();
                dropTarget.SetValue(Grid.RowProperty, gridRow);
                dropTarget.SetValue(Grid.ColumnProperty, gridColumn);
                this.MainGrid.Children.Add(dropTarget);

                this.configurationFile.DeleteSensorTile(args.DeletedSensorTile.HardwareSensor.Name, gridRow, gridColumn);
            }
        }

        #endregion Event-Handler

        /// <summary>
        /// Create main grid
        /// </summary>
        /// <param name="numberOfColumns">Number of columns</param>
        /// <param name="columnWidth">The column width</param>
        /// <param name="numberOfRows">Number of rows</param>
        /// <param name="rowHeight">The row height</param>
        private void CreateMainGrid(int numberOfColumns, double columnWidth, int numberOfRows, double rowHeight)
        {
            Grid grid = new Grid();
            grid.AllowDrop = true;

            // Create columns
            for (int i = 0; i < numberOfColumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(columnWidth) });
            }

            // Create rows
            for (int i = 0; i < numberOfRows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(rowHeight) });
            }

            this.MainGrid = grid;
        }

        /// <summary>
        /// Add sensor tile
        /// </summary>
        /// <param name="gridRow"></param>
        /// <param name="gridCol"></param>
        private void AddSensorTile(ISensor sensor, int gridRow, int gridCol, string sensorCategory)
        {
            var s = new SensorTile();
            Color backgroundColor = Colors.White;

            switch (sensorCategory)
            {
                case "CPU":
                    backgroundColor = ColorHelper.GetColorFromString(this.applicationConfigFile.Sections["TileSettings"].Settings["CpuTilesColor"].Value);
                    break;
                case "GPU":
                    backgroundColor = ColorHelper.GetColorFromString(this.applicationConfigFile.Sections["TileSettings"].Settings["GpuTilesColor"].Value);
                    break;
                case "Mainboard":
                    backgroundColor = ColorHelper.GetColorFromString(this.applicationConfigFile.Sections["TileSettings"].Settings["GpuTilesColor"].Value);
                    break;
            }            

            s.HardwareSensor = sensor;
            s.TileBackground = new SolidColorBrush(backgroundColor);
            s.SetValue(Grid.RowProperty, gridRow);
            s.SetValue(Grid.ColumnProperty, gridCol);

            this.MainGrid.Children.Add(s);
        }

        #region Commands

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void InitializeCommands()
        {
            this.AddNewTileCommand = new DelegateCommand(this.OnAddNewTileCommandExecute, this.OnAddNewTileCommandCanExecute);
        }

        /// <summary>
        /// Add new tile command
        /// </summary>
        public ICommand AddNewTileCommand { get; private set; }

        /// <summary>
        /// Can AddNewTileCommand be executed
        /// </summary>
        /// <returns></returns>
        private bool OnAddNewTileCommandCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Execute AddNewTileCommand
        /// </summary>
        private void OnAddNewTileCommandExecute()
        {
            var popup = new Pages.AddTilePopup();
            popup.Owner = Application.Current.MainWindow;
            popup.ShowDialog();
        }

        #endregion Commands

        #region Properties

        private Grid mainGrid;

        /// <summary>
        /// The main grid
        /// </summary>
        public Grid MainGrid
        {
            get { return mainGrid; }
            private set { this.SetProperty<Grid>(ref this.mainGrid, value); }
        }

        #endregion Properties
    }
}