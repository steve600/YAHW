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
	/// Dictionary that holds configuration settings
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
	public class ConfigurationSettings : DictionaryBase
	{
		/* Implementierung des Indizierers */
		public ConfigurationSetting this[string name]
		{
			set { this.Dictionary[name] = value; }
			get { return (ConfigurationSetting)this.Dictionary[name]; }
		}

		/* Implementierung der Add-Methode */
		public void Add(string settingName, string defaultValue)
		{
			this.Dictionary.Add(settingName, new ConfigurationSetting(settingName, defaultValue));
		}

		/// <summary>
		/// Add setting
		/// </summary>
		/// <param name="settingName">The name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <param name="value">The value.</param>
		public void Add(string settingName, string defaultValue, string value, Type dataType)
		{
			this.Dictionary.Add(settingName, new ConfigurationSetting(settingName, defaultValue, value, dataType));
		}

		/// <summary>
		/// Contains
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(string key)
		{
			return (Dictionary.Contains(key));
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
