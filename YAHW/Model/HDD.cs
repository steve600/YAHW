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
using System.Linq;
using System.Collections.Generic;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for HDD-Information (Initial-Version taken from here: http://www.know24.net/blog/C+WMI+HDD+SMART+Information.aspx)
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
    public class HDD
    {
        public HDD()
        {
            this.Attributes = new Dictionary<int, SMARTData>() {
                {0x00, new SMARTData("Invalid")},
                {0x01, new SMARTData("Raw read error rate")},
                {0x02, new SMARTData("Throughput performance")},
                {0x03, new SMARTData("Spinup time")},
                {0x04, new SMARTData("Start/Stop count")},
                {0x05, new SMARTData("Reallocated sector count")},
                {0x06, new SMARTData("Read channel margin")},
                {0x07, new SMARTData("Seek error rate")},
                {0x08, new SMARTData("Seek timer performance")},
                {0x09, new SMARTData("Power-on hours count")},
                {0x0A, new SMARTData("Spinup retry count")},
                {0x0B, new SMARTData("Calibration retry count")},
                {0x0C, new SMARTData("Power cycle count")},
                {0x0D, new SMARTData("Soft read error rate")},
                {0xB8, new SMARTData("End-to-End error")},
                {0xBE, new SMARTData("Airflow Temperature")},
                {0xBF, new SMARTData("G-sense error rate")},
                {0xC0, new SMARTData("Power-off retract count")},
                {0xC1, new SMARTData("Load/Unload cycle count")},
                {0xC2, new SMARTData("HDD temperature")},
                {0xC3, new SMARTData("Hardware ECC recovered")},
                {0xC4, new SMARTData("Reallocation count")},
                {0xC5, new SMARTData("Current pending sector count")},
                {0xC6, new SMARTData("Offline scan uncorrectable count")},
                {0xC7, new SMARTData("UDMA CRC error rate")},
                {0xC8, new SMARTData("Write error rate")},
                {0xC9, new SMARTData("Soft read error rate")},
                {0xCA, new SMARTData("Data Address Mark errors")},
                {0xCB, new SMARTData("Run out cancel")},
                {0xCC, new SMARTData("Soft ECC correction")},
                {0xCD, new SMARTData("Thermal asperity rate (TAR)")},
                {0xCE, new SMARTData("Flying height")},
                {0xCF, new SMARTData("Spin high current")},
                {0xD0, new SMARTData("Spin buzz")},
                {0xD1, new SMARTData("Offline seek performance")},
                {0xDC, new SMARTData("Disk shift")},
                {0xDD, new SMARTData("G-sense error rate")},
                {0xDE, new SMARTData("Loaded hours")},
                {0xDF, new SMARTData("Load/unload retry count")},
                {0xE0, new SMARTData("Load friction")},
                {0xE1, new SMARTData("Load/Unload cycle count")},
                {0xE2, new SMARTData("Load-in time")},
                {0xE3, new SMARTData("Torque amplification count")},
                {0xE4, new SMARTData("Power-off retract count")},
                {0xE6, new SMARTData("GMR head amplitude")},
                {0xE7, new SMARTData("Temperature")},
                {0xF0, new SMARTData("Head flying hours")},
                {0xFA, new SMARTData("Read error retry rate")},
                /* slot in any new codes you find in here */
            };
        }

        public int Index { get; set; }
        public bool IsOK { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Serial { get; set; }
        public string Firmware { get; set; }
        public UInt64 TotalSize { get; set; }

        private Dictionary<int, SMARTData> attributes;
        public Dictionary<int, SMARTData> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        private IList<HDDPartition> partitions = new List<HDDPartition>();

        /// <summary>
        /// List with HDD-Partitions
        /// </summary>
        public IList<HDDPartition> Partitions
        {
            get { return partitions; }
            set { partitions = value; }
        }

        public IList<HDDPartition> PartitionsWithDriveLetter
        {
            get
            {
                if (this.Partitions != null && this.Partitions.Count > 0)
                {
                    return this.Partitions.Where(p => !String.IsNullOrEmpty(p.DriveLetter)).ToList();
                }

                return null;
            }
        }
    }
}