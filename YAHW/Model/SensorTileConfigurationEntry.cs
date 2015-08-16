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
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YAHW.MVVMBase;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for a the configuration settings of a sensor tile
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
    /// <para>Date: 15.08.2015</para>
    /// </summary>
    public class SensorTileConfigurationEntry : BindableBase
    {
        public SensorTileConfigurationEntry()
        {

        }

        public SensorTileConfigurationEntry(string sensorName, string sensorCategory, string sensorType, int gridRow, int gridColumn)
        {
            this.SensorName = sensorName;
            this.SensorCategory = sensorCategory;
            this.SensorType = sensorType;
            this.GridRow = gridRow;
            this.GridColumn = gridColumn;
        }

        private string sensorName;

        /// <summary>
        /// The sensor name
        /// </summary>
        public string SensorName
        {
            get { return sensorName; }
            set { this.SetProperty<string>(ref this.sensorName, value); }
        }

        private string sensorCategory;

        /// <summary>
        /// Sensor-Category
        /// </summary>
        public string SensorCategory
        {
            get { return sensorCategory; }
            set { this.SetProperty<string>(ref this.sensorCategory, value); }
        }

        private string sensorType;

        /// <summary>
        /// The sensor type
        /// </summary>
        public string SensorType
        {
            get { return sensorType; }
            set { this.SetProperty<string>(ref this.sensorType, value); }
        }
        
        private int gridRow;

        /// <summary>
        /// The grid row
        /// </summary>
        public int GridRow
        {
            get { return gridRow; }
            set { this.SetProperty<int>(ref this.gridRow, value); }
        }

        private int gridColumn;

        /// <summary>
        /// The grid column
        /// </summary>
        public int GridColumn
        {
            get { return gridColumn; }
            set { this.SetProperty<int>(ref this.gridColumn, value); }
        }

        public XElement ToXml()
        {
            return new XElement("SensorTile",
                new XAttribute("SensorName", this.SensorName),
                new XAttribute("SensorCategory", this.SensorCategory),
                new XAttribute("SensorType", this.SensorType),
                new XAttribute("GridRow", this.GridRow),
                new XAttribute("GridColumn", this.GridColumn));
        }
    }
}