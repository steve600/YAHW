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

using System.Collections.Generic;
using YAHW.Model;

namespace YAHW.Interfaces
{
    /// <summary>
    /// <para>
    /// Interface for a hardware info service
    /// </para>
    /// 
    /// <para>
    /// Interface history:
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
    public interface IHardwareInformationService
    {
        /// <summary>
        /// Try to get mainboard info
        /// </summary>
        /// <returns></returns>
        MainboardInformation GetMainboardInformation();

        /// <summary>
        /// Try to get processor information
        /// </summary>
        /// <returns></returns>
        ProcessorInformation GetProcessorInformation();

        #region GPU

        /// <summary>
        /// Get GPU-Information
        /// </summary>
        /// <returns></returns>
        GPUInformation GetGPUInformation();
        
        #endregion GPU

        #region Physical Memory (RAM)

        /// <summary>
        /// Get physical memory information
        /// </summary>
        /// <returns></returns>
        IList<RAMInformation> GetPhysicalMemoryInformation();

        #endregion Physical Memory (RAM)

        #region HDD

        /// <summary>
        /// Get HHD SMART Information for installed drives
        /// </summary>
        /// <returns></returns>
        Dictionary<int, HDD> GetHddSmartInformation();

        #endregion HDD

        #region Network

        void GetNetworkAdapterInformation();

        #endregion Network
    }
}
