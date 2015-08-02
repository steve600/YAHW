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
    /// Class for GPU-Information
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
    public class GPUInformation : BindableBase
    {
        private string cpation;

        /// <summary>
        /// Caption
        /// </summary>
        public string Caption
        {
            get { return cpation; }
            set { this.SetProperty<string>(ref this.cpation, value); }
        }

        private UInt32 currentBitsPerPixel;

        /// <summary>
        /// Current Bits per Pixel
        /// </summary>
        public UInt32 CurrentBitsPerPixel
        {
            get { return currentBitsPerPixel; }
            set { this.SetProperty<UInt32>(ref this.currentBitsPerPixel, value); }
        }

        private UInt32 currentHorizontalResolution;

        /// <summary>
        /// Current horizontal resolution
        /// </summary>
        public UInt32 CurrentHorizontalResolution
        {
            get { return currentHorizontalResolution; }
            set { this.SetProperty<UInt32>(ref this.currentHorizontalResolution, value); }
        }

        private UInt64 currentNumberOfColors;

        /// <summary>
        /// Current number of colors
        /// </summary>
        public UInt64 CurrentNumberOfColors
        {
            get { return currentNumberOfColors; }
            set { this.SetProperty<UInt64>(ref this.currentNumberOfColors, value); }
        }

        private UInt32 currentRefreshRate;

        /// <summary>
        /// Current refresh rate
        /// </summary>
        public UInt32 CurrentRefreshRate
        {
            get { return currentRefreshRate; }
            set { this.SetProperty<UInt32>(ref this.currentRefreshRate, value); }
        }

        private UInt32 currentVerticalResolution;

        /// <summary>
        /// Current vertical resolution
        /// </summary>
        public UInt32 CurrentVerticalResolution
        {
            get { return currentVerticalResolution; }
            set { this.SetProperty<UInt32>(ref this.currentVerticalResolution, value); }
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

        private DateTime? driverDate;

        /// <summary>
        /// Driver date
        /// </summary>
        public DateTime? DriverDate
        {
            get { return driverDate; }
            set { this.SetProperty<DateTime?>(ref this.driverDate, value); }
        }

        private string driverVersion;

        /// <summary>
        /// Driver version
        /// </summary>
        public string DriverVersion
        {
            get { return driverVersion; }
            set { this.SetProperty<string>(ref this.driverVersion, value); }
        }

        private string infFilename;

        /// <summary>
        /// INF-Filename
        /// </summary>
        public string InfFilename
        {
            get { return infFilename; }
            set { this.SetProperty<string>(ref this.infFilename, value); }
        }

        private string[] installedDisplayDrivers;

        /// <summary>
        /// Installed display drivers
        /// </summary>
        public string[] InstalledDisplayDrivers
        {
            get { return installedDisplayDrivers; }
            set { this.SetProperty<string[]>(ref this.installedDisplayDrivers, value); }
        }

        private UInt32 maxRefreshRate;

        /// <summary>
        /// Max refresh rate
        /// </summary>
        public UInt32 MaxRefreshRate
        {
            get { return maxRefreshRate; }
            set { this.SetProperty<UInt32>(ref this.maxRefreshRate, value); }
        }

        private UInt32 minRefreshRate;

        /// <summary>
        /// Min refresh rate
        /// </summary>
        public UInt32 MinRefreshRate
        {
            get { return minRefreshRate; }
            set { this.SetProperty<UInt32>(ref this.minRefreshRate, value); }
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

        private string videoModeDescription;

        /// <summary>
        /// Video-Mode
        /// </summary>
        public string VideoModeDescription
        {
            get { return videoModeDescription; }
            set { this.SetProperty<string>(ref this.videoModeDescription, value); }
        }

        private string videoProcessor;

        /// <summary>
        /// Video processor
        /// </summary>
        public string VideoProcessor
        {
            get { return videoProcessor; }
            set { this.SetProperty<string>(ref this.videoProcessor, value); }
        }
    }
}