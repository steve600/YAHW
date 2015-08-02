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
    /// Class for RAM-Bank information
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
    public class RAMInformation : BindableBase
    {
        private string bankLabel;

        /// <summary>
        /// The bank label
        /// </summary>
        public string BankLabel
        {
            get { return bankLabel; }
            set { this.SetProperty<string>(ref this.bankLabel, value); }
        }

        private UInt64 capacity;

        /// <summary>
        /// The capacity
        /// </summary>
        public UInt64 Capacity
        {
            get { return capacity; }
            set { this.SetProperty<UInt64>(ref this.capacity, value); }
        }

        /// <summary>
        /// Capacity in MegaBytes
        /// </summary>
        public UInt64 CapacityInMegaBytes
        {
            get
            {
                return this.Capacity / (1024 * 1024);
            }
        }

        private UInt16 dataWidth;

        /// <summary>
        /// The Data-Width 
        /// </summary>
        public UInt16 DataWidth
        {
            get { return dataWidth; }
            set { this.SetProperty<UInt16>(ref this.dataWidth, value); }
        }
        
        private string deviceLocator;

        /// <summary>
        /// Device locator
        /// </summary>
        public string DeviceLocator
        {
            get { return deviceLocator; }
            set { this.SetProperty<string>(ref this.deviceLocator, value); }
        }        

        private UInt16 formFactor;

        /// <summary>
        /// The form factor
        /// </summary>
        public UInt16 FormFactor
        {
            get { return formFactor; }
            set { this.SetProperty<UInt16>(ref this.formFactor, value); }
        }

        /// <summary>
        /// Form-Factor description, e.g. DIMM
        /// </summary>
        public string FormFactorDescription
        {
            get
            {
                switch (this.FormFactor)
                {
                    case 0:
                        return "Unknown";
                    case 1:
                        return "Other";
                    case 2:
                        return "SIP";
                    case 3:
                        return "DIP";
                    case 4:
                        return "ZIP";
                    case 5:
                        return "SOJ";
                    case 6:
                        return "Proprietary";
                    case 7:
                        return "SIMM";
                    case 8:
                        return "DIMM";
                    case 9:
                        return "TSOP";
                    case 10:
                        return "PGA";
                    case 11:
                        return "RIMM";
                    case 12:
                        return "SODIMM";
                    case 13:
                        return "SRIMM";
                    case 14:
                        return "SMD";
                    case 15:
                        return "SSMP";
                    case 16:
                        return "QFP";
                    case 17:
                        return "TQFP";
                    case 18:
                        return "SOIC";
                    case 19:
                        return "LCC";
                    case 20:
                        return "PLCC";
                    case 21:
                        return "BGA";
                    case 22:
                        return "FPBGA";
                    case 23:
                        return "LGA";
                    default:
                        return "n.a.";
                }
            }
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

        private UInt16 memoryType;

        /// <summary>
        /// Memory-Type
        /// </summary>
        public UInt16 MemoryType
        {
            get { return memoryType; }
            set { this.SetProperty<UInt16>(ref this.memoryType, value); }
        }

        /// <summary>
        /// Memory-Type description, e.g. SRAM
        /// </summary>
        public string MemoryTypeDescription
        {
            get
            {
                switch (this.MemoryType)
                {
                    case 0:
                        return "Unknown";
                    case 1:
                        return "Other";
                    case 2:
                        return "DRAM";
                    case 3:
                        return "Sychnronous DRAM";
                    case 4:
                        return "Cache DRAM";
                    case 5:
                        return "EDO";
                    case 6:
                        return "EDRAM";
                    case 7:
                        return "VRAM";
                    case 8:
                        return "SRAM";
                    case 9:
                        return "RAM";
                    case 10:
                        return "ROM";
                    case 11:
                        return "Flash";
                    case 12:
                        return "EEPROM";
                    case 13:
                        return "FEPROM";
                    case 14:
                        return "EPROM";
                    case 15:
                        return "CDRAM";
                    case 16:
                        return "3DRAM";
                    case 17:
                        return "SDRAM";
                    case 18:
                        return "SGRAM";
                    case 19:
                        return "RDRAM";
                    case 20:
                        return "DDR";
                    case 21:
                        return "DDR2";
                    case 22:
                        return "DDR FB-DIMM";
                    case 24:
                        return "DDR3";
                    case 25:
                        return "FBD2";
                    default:
                        return "n.a.";
                }
            }
        }

        private string model;

        /// <summary>
        /// The model
        /// </summary>
        public string Model
        {
            get { return model; }
            set { this.SetProperty<string>(ref this.model, value); }
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

        private string partNumber;

        /// <summary>
        /// The part number
        /// </summary>
        public string PartNumber
        {
            get { return partNumber; }
            set { this.SetProperty<string>(ref this.partNumber, value); }
        }

        private string serialNumber;

        /// <summary>
        /// Serial number
        /// </summary>
        public string SerialNumber
        {
            get { return serialNumber; }
            set { this.SetProperty<string>(ref this.serialNumber, value); }
        }

        private UInt32 speed;

        /// <summary>
        /// Speed
        /// </summary>
        public UInt32 Speed
        {
            get { return speed; }
            set { this.SetProperty<UInt32>(ref this.speed, value); }
        }

        private string tag;

        /// <summary>
        /// Tag
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { this.SetProperty<string>(ref this.tag, value); }
        }

        private UInt16 totalWidth;

        /// <summary>
        /// Total width
        /// </summary>
        public UInt16 TotalWidth
        {
            get { return totalWidth; }
            set { this.SetProperty<UInt16>(ref this.totalWidth, value); }
        }

        private UInt16 typeDetail;

        /// <summary>
        /// Type detail
        /// </summary>
        public UInt16 TypeDetail
        {
            get { return typeDetail; }
            set { this.SetProperty<UInt16>(ref this.typeDetail, value); }
        }
    }
}