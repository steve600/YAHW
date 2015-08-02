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

namespace YAHW.Model
{
	/// <summary>
	/// <para>
	/// Class that represents a configuration setting
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
	/// <para>Date: 29.07.2015</para>
	/// </summary>
	public class ConfigurationSetting
	{
		#region CTOR

		/// <summary>
		/// CTOR
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		public ConfigurationSetting(string name, string defaultValue)
		{
			this.Name = name;
			this.DefaultValue = defaultValue;
		}

		/// <summary>
		/// CTOR
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		public ConfigurationSetting(string name, string defaultValue, string value, Type dataType)
		{
			this.Name = name;
			this.DefaultValue = defaultValue;
			this.Value = value;
			this.DataType = dataType;
		}

		#endregion CTOR

		#region Properties

		private string name;

		/// <summary>
		/// Name of the setting
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string value;

		/// <summary>
		/// Value of the setting
		/// </summary>
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}

		private string defaultValue;

		/// <summary>
		/// Default value of the setting
		/// </summary>
		public string DefaultValue
		{
			get { return defaultValue; }
			set { defaultValue = value; }
		}

		private Type dataType = typeof(System.String);

		/// <summary>
		/// The datatype of the setting
		/// </summary>
		public Type DataType
		{
			get { return this.dataType; }
			set { this.dataType = value; }
		}

		private bool wasInFile;

		/// <summary>
		/// Flag if setting was in file
		/// </summary>
		public bool WasInFile
		{
			get { return wasInFile; }
			set { wasInFile = value; }
		}		

		#endregion Properties
	}
}
