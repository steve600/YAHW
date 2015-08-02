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
    /// Interaction logic for CPUInformationUserControl.xaml
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
    public partial class CPUInformationUserControl : UserControl
    {
        public CPUInformationUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// CPU-Information
        /// </summary>
        public ProcessorInformation CPUInformation
        {
            get { return (ProcessorInformation)GetValue(CPUInformationProperty); }
            set { SetValue(CPUInformationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CPUInformation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CPUInformationProperty =
            DependencyProperty.Register("CPUInformation", typeof(ProcessorInformation), typeof(CPUInformationUserControl), new PropertyMetadata(null));
    }
}