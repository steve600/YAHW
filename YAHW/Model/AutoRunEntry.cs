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

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class that represents an Auto-Run-Entry
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
    public class AutoRunEntry : BindableBase
    {
        private string caption;

        /// <summary>
        /// Capption
        /// </summary>
        public string Caption
        {
            get { return caption; }
            set { this.SetProperty<string>(ref this.caption, value); }
        }

        private string command;

        /// <summary>
        /// The command
        /// </summary>
        public string Command
        {
            get { return command; }
            set { this.SetProperty<string>(ref this.command, value); }
        }

        private string description;

        /// <summary>
        /// The description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { this.SetProperty<string>(ref this.description, value); }
        }

        private string location;

        /// <summary>
        /// The location
        /// </summary>
        public string Location
        {
            get { return location; }
            set { this.SetProperty<string>(ref this.location, value); }
        }

        private string name;

        /// <summary>
        /// The name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        private string user;

        /// <summary>
        /// The user
        /// </summary>
        public string User
        {
            get { return user; }
            set { this.SetProperty<string>(ref this.user, value); }
        }

        private string userSID;

        /// <summary>
        /// The User-SID
        /// </summary>
        public string UserSID
        {
            get { return userSID; }
            set { this.SetProperty<string>(ref this.userSID, value); }
        }

        private bool isActive;

        /// <summary>
        /// Flag if entry is active
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { this.SetProperty<bool>(ref this.isActive, value); }
        }

    }
}