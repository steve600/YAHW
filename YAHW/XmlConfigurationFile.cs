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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using XAHW.Interfaces;
using YAHW.Model;

namespace YAHW
{
	/// <summary>
	/// <para>
	/// Class that represents a XML-configuration file
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
	/// <para>Date: 18.12.2014</para>
	/// </summary>
	public class XmlConfigurationFile : IConfigurationFile
	{
		#region Members and Constants

		#endregion Members and Constants

		#region CTOR

		/// <summary>
		/// CTOR
		/// </summary>
		/// <param name="fileName">The file name</param>
		public XmlConfigurationFile(string fileName)
		{
			this.FileName = fileName;
			this.Sections = new ConfigurationSections();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Load configuration
		/// </summary>
		/// <returns></returns>
		public bool Load()
		{
			// Variable für den Rückgabewert
			bool returnValue = true;
			
			// XmlDocument-Objekt für die Einstellungs-Datei erzeugen
			XDocument xdoc = null;

			// Datei laden
			try
			{
				//xmlDoc.Load(this.fileName);
				xdoc = XDocument.Load(this.fileName);
			}
			catch (IOException ex1)
			{
				System.Diagnostics.Debug.WriteLine(new IOException("Fehler beim Laden der Konfigurationsdatei '" + this.fileName + "': " + ex1.Message));
				return false;
			}
			catch (XmlException ex2)
			{
				System.Diagnostics.Debug.WriteLine("Fehler beim Laden der Konfigurationsdatei '" + this.fileName + "': " + ex2.Message, ex2);
				return false;
			}

			// Sections einlesen
			foreach (var s in xdoc.Element("Configuration").Elements())
			{
				// Dictionary-Element für die Section erzeugen
				this.Sections.Add(s.Name.LocalName);

				// Settings einlesen
				foreach (var setting in s.Elements())
				{
					if (setting != null)
					{
						string name = setting.Name.LocalName;
						string value = setting.Value == null ? string.Empty : setting.Value;
						string defaultValue = setting.Attribute("DefaultValue") == null ? string.Empty : setting.Attribute("DefaultValue").Value;
						Type dataType = setting.Attribute("DataType") == null ? typeof(System.String) : Type.GetType(setting.Attribute("DataType").Value);
						
						this.Sections[s.Name.LocalName].Settings.Add(name, defaultValue, value, dataType);
					}
				}
			}

			// Ergebnis zurückmelden
			return returnValue;
		}

		/// <summary>
		/// Save configuration
		/// </summary>
		public void Save()
		{
			// XmlDocument-Objekt für die Einstellungs-Datei erzeugen
			XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

			XmlDocument xmlDoc = new XmlDocument();
			// Skelett der XML-Datei erzeugen
			xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" " + "standalone=\"yes\"?><Configuration></Configuration>");
			
			// Alle Sektionen durchgehen und die Einstellungen schreiben
			foreach (ConfigurationSection section in this.Sections.Values)
			{
				// Element für die Sektion erzeugen und anfügen
				XmlElement sectionElement = xmlDoc.CreateElement(section.Name);
				xmlDoc.DocumentElement.AppendChild(sectionElement);
				
				// Alle Einstellungen der Sektion durchlaufen
				foreach (ConfigurationSetting setting in section.Settings.Values)
				{
					// Einstellungs-Element erzeugen und anfügen
					XmlElement settingElement = xmlDoc.CreateElement(setting.Name);
					settingElement.SetAttribute("DefaultValue", setting.DefaultValue);
					settingElement.SetAttribute("DataType", setting.DataType.FullName);
					settingElement.InnerText = setting.Value;
					sectionElement.AppendChild(settingElement);
				}
			}
			
			// Datei speichern
			try
			{
				xmlDoc.Save(this.fileName);
			}
			catch (IOException ex1)
			{
				throw new IOException("Fehler beim Speichern der " + "Konfigurationsdatei '" + this.fileName + "': " + ex1.Message);
			}
			catch (XmlException ex2)
			{
				throw new XmlException("Fehler beim Speichern der " + " Konfigurationsdatei '" + this.fileName + "': " + ex2.Message, ex2);
			}
		}

		#endregion Methods

		private string fileName;

		/// <summary>
		/// The file name
		/// </summary>
		public string FileName
		{
			get { return fileName; }
			private set { fileName = value; }
		}

		private ConfigurationSections sections;

		/// <summary>
		/// Sections of the configuration file
		/// </summary>
		public ConfigurationSections Sections
		{
			get { return sections; }
			set { sections = value; }
		}
	}
}