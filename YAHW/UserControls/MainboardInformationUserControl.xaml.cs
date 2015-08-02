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

using System.Windows;
using System.Windows.Controls;
using YAHW.Model;

namespace YAHW.UserControls
{
    /// <summary>
    /// <para>
    /// Interaction logic for MainboardInformationUserControl.xaml
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
    public partial class MainboardInformationUserControl : UserControl
    {
        public MainboardInformationUserControl()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        /// <summary>
        /// Mainboard informations
        /// </summary>
        public MainboardInformation MainboardInfo
        {
            get { return (MainboardInformation)GetValue(MainboardInfoProperty); }
            set { SetValue(MainboardInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainboardInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainboardInfoProperty =
            DependencyProperty.Register("MainboardInfo", typeof(MainboardInformation), typeof(MainboardInformationUserControl), new PropertyMetadata(null));

        #endregion Dependency Properties
    }
}