using OpenHardwareMonitor.Hardware;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Events;
using YAHW.Interfaces;
using YAHW.MVVMBase;

namespace YAHW.ViewModels
{
    public enum SensorCategories
    {
        CPU=1,
        GPU=2,
        Mainboard=3
    }

    /// <summary>
    /// <para>
    /// A simple view model for adding an new tile
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
    public class AddTilePopupViewModel : ViewModelBase
    {
        #region Members and Constants

        private IOpenHardwareMonitorManagementService openHardwareMonitorManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public AddTilePopupViewModel()
        {
            // Initilize commands
            this.InitializeCommands();

            // Get services
            this.openHardwareMonitorManagementService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
        }

        #endregion CTOR

        #region Commands

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void InitializeCommands()
        {
            this.AddNewTileCommand = new DelegateCommand<Window>(this.OnAddNewTileCommandExecute, this.OnAddNewTileCommandCanExecute);
        }

        /// <summary>
        /// Raise execute changed command
        /// </summary>
        private void RaiseCanExecuteChanged()
        {
            ((DelegateCommand<Window>)this.AddNewTileCommand).RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Add new tile command
        /// </summary>
        public ICommand AddNewTileCommand { get; private set; }

        /// <summary>
        /// Can AddNewTileCommand be executed
        /// </summary>
        /// <returns></returns>
        private bool OnAddNewTileCommandCanExecute(Window popupWindow)
        {
            return (this.SelectedSensorCategory != null && this.SelectedSensor != null);
        }

        /// <summary>
        /// Execute AddNewTileCommand
        /// </summary>
        private void OnAddNewTileCommandExecute(Window popupWindow)
        {
            // Fire event
            SensorTileAddedEventArgs args = new SensorTileAddedEventArgs(this.SelectedSensorCategory.Value, this.SelectedSensor);
            DependencyFactory.Resolve<IEventAggregator>(GeneralConstants.EventAggregator).GetEvent<SensorTileAddedEvent>().Publish(args);

            // Close dialog
            if (popupWindow != null && popupWindow is Window)
            {
                popupWindow.Close();
            }
        }

        #endregion Commands

        #region Properties

        private Window popupWindow;

        /// <summary>
        /// The window
        /// </summary>
        public Window PopupWindow
        {
            get { return popupWindow; }
            set { this.SetProperty<Window>(ref this.popupWindow, value); }
        }

        private SensorCategories? selectedSensorCategory;

        /// <summary>
        /// Selected sensor type
        /// </summary>
        public SensorCategories? SelectedSensorCategory
        {
            get { return selectedSensorCategory; }
            set
            {
                if (this.SetProperty<SensorCategories?>(ref this.selectedSensorCategory, value))
                {
                    this.OnPropertyChanged(() => this.Sensors);
                    this.RaiseCanExecuteChanged();
                }
            }
        }

        private ISensor selectedSensor;

        /// <summary>
        /// The selected sensor
        /// </summary>
        public ISensor SelectedSensor
        {
            get { return selectedSensor; }
            set
            {
                if (this.SetProperty<ISensor>(ref this.selectedSensor, value))
                {
                    this.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Sensors
        /// </summary>
        public IList<ISensor> Sensors
        {
            get
            {
                switch (this.SelectedSensorCategory)
                {
                    case SensorCategories.CPU:
                        return this.openHardwareMonitorManagementService.CPU.Sensors.ToList();
                    case SensorCategories.GPU:
                        return this.openHardwareMonitorManagementService.GPU.Sensors.ToList();
                    case SensorCategories.Mainboard:
                        return this.openHardwareMonitorManagementService.MainboardSensors;                        
                }

                return null;
            }
        }

        #endregion Properties
    }
}
