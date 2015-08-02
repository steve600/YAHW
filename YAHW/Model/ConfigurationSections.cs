using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAHW.Model
{
	/// <summary>
	/// <para>
	/// Dictionary that holds configuration sections
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
	public class ConfigurationSections : DictionaryBase
	{
		/* Implementierung des Indizierers */
		public ConfigurationSection this[string name]
		{
			set { this.Dictionary[name] = value; }
			get { return (ConfigurationSection)this.Dictionary[name]; }
		}
		/* Implementierung der Add-Methode */
		public void Add(string name)
		{
			this.Dictionary.Add(name, new ConfigurationSection(name));
		}
		/* Implementierung der Remove-Methode */
		public void Remove(string name)
		{
			this.Dictionary.Remove(name);
		}
		/* Implementierung der Values-Eigenschaft zum Durchgehen mit foreach */
		public ICollection Values
		{
			get { return Dictionary.Values; }
		}
		/* Implementierung der Keys-Eigenschaft zum Durchgehen mit foreach */
		public ICollection Keys
		{
			get { return Dictionary.Keys; }
		}
	}
}
