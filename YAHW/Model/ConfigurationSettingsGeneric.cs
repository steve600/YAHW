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
	public class ConfigurationSettingsGeneric<Setting> : IDictionary<string, Setting>
	{
		private Dictionary<string, Setting> internalDict = new Dictionary<string, Setting>();

		#region IDictionary<string,Setting> Members

		public void Add(string key, Setting value)
		{
			this.internalDict.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return internalDict.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return internalDict.Keys; }
		}

		public bool Remove(string key)
		{
			return internalDict.Remove(key);
		}

		public bool TryGetValue(string key, out Setting value)
		{
			return internalDict.TryGetValue(key, out value);
		}

		public ICollection<Setting> Values
		{
			get { return this.internalDict.Values; }
		}

		public Setting this[string key]
		{
			get
			{
				return this.internalDict[key];
			}
			set
			{
				internalDict[key] = value;
			}
		}

		#endregion

		#region ICollection<KeyValuePair<string,Setting>> Members

		public void Add(KeyValuePair<string, Setting> item)
		{
			this.internalDict.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			this.internalDict.Clear();
		}

		public bool Contains(KeyValuePair<string, Setting> item)
		{
			return this.internalDict.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, Setting>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return this.internalDict.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(KeyValuePair<string, Setting> item)
		{
			return this.internalDict.Remove(item.Key);
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,Setting>> Members

		public IEnumerator<KeyValuePair<string, Setting>> GetEnumerator()
		{
			return this.internalDict.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.internalDict.GetEnumerator();
		}

		#endregion
	}
}
