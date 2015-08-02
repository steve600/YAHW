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
using YAHW.MVVMBase;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for a HDD-Partition
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
    public class HDDPartition : BindableBase
    {
        private string diskName;

        /// <summary>
        /// The name of the disk
        /// </summary>
        public string DiskName
        {
            get { return diskName; }
            set { this.SetProperty<string>(ref this.diskName, value); }
        }

        private string partitionName;

        /// <summary>
        /// The name of the partition
        /// </summary>
        public string PartitionName
        {
            get { return partitionName; }
            set { this.SetProperty<string>(ref this.partitionName, value); }
        }

        private string driveLetter;

        /// <summary>
        /// The drive letter of the partition
        /// </summary>
        public string DriveLetter
        {
            get { return driveLetter; }
            set { this.SetProperty<string>(ref this.driveLetter, value); }
        }

        private UInt64 freeSpace;

        /// <summary>
        /// The free space of the partition
        /// </summary>
        public UInt64 FreeSpace
        {
            get { return freeSpace; }
            set { this.SetProperty<UInt64>(ref this.freeSpace, value); }
        }

        private double freeSpaceInPercent;

        /// <summary>
        /// The free space in percent
        /// </summary>
        public double FreeSapceInPercent
        {
            get { return freeSpaceInPercent; }
            set { this.SetProperty<double>(ref this.freeSpaceInPercent, value); }
        }

        private UInt64 totalSpace;

        /// <summary>
        /// Total space of the partition
        /// </summary>
        public UInt64 TotalSpace
        {
            get { return totalSpace; }
            set { this.SetProperty<UInt64>(ref this.totalSpace, value); }
        }
    }
}
