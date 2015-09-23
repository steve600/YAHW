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
using Prism.Mvvm;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class that represents a Windows-Service
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
    public class WindowsService : BindableBase
    {
        private bool acceptPause;

        public bool AcceptPause
        {
            get { return acceptPause; }
            set { this.SetProperty<bool>(ref this.acceptPause, value); }
        }

        private bool acceptStop;

        public bool AcceptStop
        {
            get { return acceptStop; }
            set { this.SetProperty<bool>(ref this.acceptStop, value); }
        }

        private string caption;

        public string Caption
        {
            get { return caption; }
            set { this.SetProperty<string>(ref this.caption, value); }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { this.SetProperty<string>(ref this.description, value); }
        }

        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { this.SetProperty<string>(ref this.displayName, value); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        private string pathName;

        public string PathName
        {
            get { return pathName; }
            set { this.SetProperty<string>(ref this.pathName, value); }
        }

        private int processId;

        public int ProcessId
        {
            get { return processId; }
            set { this.SetProperty<int>(ref this.processId, value); }
        }

        private string startMode;

        public string StartMode
        {
            get { return startMode; }
            set { this.SetProperty<string>(ref this.startMode, value); }
        }

        private string startName;

        public string StartName
        {
            get { return startName; }
            set { this.SetProperty<string>(ref this.startName, value); }
        }
        
        private ServiceControllerStatus state;

        public ServiceControllerStatus State
        {
            get { return state; }
            set { this.SetProperty<ServiceControllerStatus>(ref this.state, value); }
        }
    }
}