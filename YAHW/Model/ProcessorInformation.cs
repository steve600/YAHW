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

using YAHW.MVVMBase;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for Processor-Information
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
    public class ProcessorInformation : BindableBase
    {
        private string caption;

        /// <summary>
        /// Caption
        /// </summary>
        public string Caption
        {
            get { return caption; }
            set { this.SetProperty<string>(ref this.caption, value); }
        }

        private uint currentClockSpeed;

        /// <summary>
        /// Current clock speed
        /// </summary>
        public uint CurrentClockSpeed
        {
            get { return currentClockSpeed; }
            set { this.SetProperty<uint>(ref this.currentClockSpeed, value); }
        }        

        private string description;

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { this.SetProperty<string>(ref this.description, value); }
        }

        private uint extClock;

        /// <summary>
        /// Clock speed
        /// </summary>
        public uint ExtClock
        {
            get { return extClock; }
            set { this.SetProperty<uint>(ref this.extClock, value); }
        }

        private uint l2CacheSize;

        /// <summary>
        /// L2 Cache-Size
        /// </summary>
        public uint L2CacheSize
        {
            get { return l2CacheSize; }
            set { this.SetProperty<uint>(ref this.l2CacheSize, value); }
        }

        private uint l3CacheSize;

        /// <summary>
        /// L3 Cache-Size
        /// </summary>
        public uint L3CacheSize
        {
            get { return l3CacheSize; }
            set { this.SetProperty<uint>(ref this.l3CacheSize, value); }
        }

        private string manufacturer;

        /// <summary>
        /// Manufacturer
        /// </summary>
        public string Manufacturer
        {
            get { return manufacturer; }
            set { this.SetProperty<string>(ref this.manufacturer, value); }
        }

        private string maxClockSpeed;

        /// <summary>
        /// Max. clock speed
        /// </summary>
        public string MaxClockSpeed
        {
            get { return maxClockSpeed; }
            set { this.SetProperty<string>(ref this.maxClockSpeed, value); }
        }

        private string name;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        private uint numberOfCores;

        /// <summary>
        /// Number of cores
        /// </summary>
        public uint NumberOfCores
        {
            get { return numberOfCores; }
            set { this.SetProperty<uint>(ref this.numberOfCores, value); }
        }

        private uint numberOfLogicalProcessors;

        /// <summary>
        /// Number of logical processors
        /// </summary>
        public uint NumberOfLogicalProcessors
        {
            get { return numberOfLogicalProcessors; }
            set { this.SetProperty<uint>(ref this.numberOfLogicalProcessors, value); }
        }        
    }
}