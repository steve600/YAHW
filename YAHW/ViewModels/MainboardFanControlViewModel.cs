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

using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Services;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// ViewModel-Class for the CPU-Core workload page
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
    public class MainboardFanControlViewModel : ViewModelBase
    {
        #region Members and Constants

        private IOpenHardwareMonitorManagementService openHardwareMonitorManagementService = null;

        #endregion Members and Constants

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        public MainboardFanControlViewModel()
        {
            this.openHardwareMonitorManagementService = DependencyFactory.Resolve<IOpenHardwareMonitorManagementService>(ServiceNames.OpenHardwareMonitorManagementService);
            this.openHardwareMonitorManagementService.UpdateMainboardSensors();

            this.MainContent = new StackPanel();

            if (this.openHardwareMonitorManagementService.MainboardFanControlSensors != null &&
                this.openHardwareMonitorManagementService.MainboardFanControlSensors.Count > 0)
            {
                foreach (var fc in this.openHardwareMonitorManagementService.MainboardFanControlSensors)
                {
                    var fanControl = new UserControls.MainboardFanControl();
                    fanControl.FanController = fc;

                    this.MainContent.Children.Add(fanControl);
                }
            }
            else
            {
                this.IsDialogDisabled = true;
            }
        }

        #endregion CTOR

        #region Properties

        private StackPanel mainContent;

        /// <summary>
        /// The main content
        /// </summary>
        public StackPanel MainContent
        {
            get { return mainContent; }
            set { this.SetProperty<StackPanel>(ref this.mainContent, value); }
        }        

        #endregion Properties
    }
}