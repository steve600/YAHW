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

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YAHW.BaseClasses;
using YAHW.Manager;
using YAHW.Model;
using YAHW.MVVMBase;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the service management
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
    class ServiceManagementViewModel : ViewModelBase
    {
        #region Members and Constants

        private ServiceManager serviceManager = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public ServiceManagementViewModel()
        {
            this.InitializeCommands();

            this.serviceManager = new ServiceManager();
            this.InstalledServices = this.serviceManager.GetInstalledWindowsServices();
        }

        #endregion CTOR

        #region Commands

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void InitializeCommands()
        {
            this.StartServiceCommand = new DelegateCommand(this.OnStartServiceCommandExecute, this.OnStartServiceCommandCanExecute);
            this.StopServiceCommand = new DelegateCommand(this.OnStopServiceCommandExecute, this.OnStopServiceCommandCanExecute);
            this.RestartServiceCommand = new DelegateCommand(this.OnRestartServiceCommandExecute, this.OnRestartServiceCommandCanExecute);
        }

        /// <summary>
        /// Start service command
        /// </summary>
        public ICommand StartServiceCommand { private set; get; }

        /// <summary>
        /// Method when StartServiceCommand is being executed
        /// </summary>
        public void OnStartServiceCommandExecute()
        {
            if (this.SelectedWindowsService != null)
            {
                this.serviceManager.StartService(this.SelectedWindowsService.Name);
                this.SelectedWindowsService.State = this.serviceManager.GetServiceState(this.SelectedWindowsService.Name);
            }
        }

        /// <summary>
        /// Method that checks if StartServiceCommand can be executed
        /// </summary>
        /// <returns></returns>
        public bool OnStartServiceCommandCanExecute()
        {
            return (this.SelectedWindowsService.State == ServiceControllerStatus.Paused ||
                    this.SelectedWindowsService.State == ServiceControllerStatus.Stopped);
        }

        /// <summary>
        /// Stop service command
        /// </summary>
        public ICommand StopServiceCommand { private set; get; }

        /// <summary>
        /// Method when StopServiceCommand is being executed
        /// </summary>
        public void OnStopServiceCommandExecute()
        {
            if (this.SelectedWindowsService != null)
            {
                this.serviceManager.StopService(this.SelectedWindowsService.Name);
                this.SelectedWindowsService.State = this.serviceManager.GetServiceState(this.SelectedWindowsService.Name);
            }
        }

        /// <summary>
        /// Method that checks if StopServiceCommand can be executed
        /// </summary>
        /// <returns></returns>
        public bool OnStopServiceCommandCanExecute()
        {
            return (this.SelectedWindowsService.State == ServiceControllerStatus.Running);
        }

        /// <summary>
        /// Restart service command
        /// </summary>
        public ICommand RestartServiceCommand { private set; get; }

        /// <summary>
        /// Method when RestartServiceCommand is being executed
        /// </summary>
        public void OnRestartServiceCommandExecute()
        {
            if (this.SelectedWindowsService != null)
            {
                this.serviceManager.RestartService(this.SelectedWindowsService.Name);

                this.SelectedWindowsService.State = this.serviceManager.GetServiceState(this.SelectedWindowsService.Name);
            }
        }

        /// <summary>
        /// Method that checks if RestartServiceCommand can be executed
        /// </summary>
        /// <returns></returns>
        public bool OnRestartServiceCommandCanExecute()
        {
            return (this.SelectedWindowsService.State == ServiceControllerStatus.Running);
        }

        #endregion Commands

        #region Properties

        private IList<WindowsService> installedServices;

        /// <summary>
        /// List with installed services
        /// </summary>
        public IList<WindowsService> InstalledServices
        {
            get { return installedServices; }
            private set { this.SetProperty<IList<WindowsService>>(ref this.installedServices, value); }
        }

        private WindowsService selectedWindowsService;

        /// <summary>
        /// The selected windows service
        /// </summary>
        public WindowsService SelectedWindowsService
        {
            get { return selectedWindowsService; }
            set
            {
                if (this.SetProperty<WindowsService>(ref this.selectedWindowsService, value))
                {
                    ((DelegateCommand)this.StartServiceCommand).RaiseCanExecuteChanged();
                    ((DelegateCommand)this.StopServiceCommand).RaiseCanExecuteChanged();
                }
            }
        }

        #endregion Properties
    }
}
