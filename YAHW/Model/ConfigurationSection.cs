using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAHW.Model
{
	/// <summary>
	/// <para>
	/// Class that represents a configuration section
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
	/// <para>Date: 29.07.2014</para>
	/// </summary>
	public class ConfigurationSection
	{
		// Name of the section
		public string Name;
		// ConfigurationSettings for the section
		public ConfigurationSettings Settings;
		
		/// <summary>
		/// CTOR
		/// </summary>
		/// <param name="sectionName">Name of the section</param>
		public ConfigurationSection(string sectionName)
		{
			this.Name = sectionName;
			this.Settings = new ConfigurationSettings();
		}
	}
}
