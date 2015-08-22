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
using System.Xml;
using System.Xml.Linq;
using YAHW.Constants;
using YAHW.Interfaces;
using YAHW.Model;

namespace YAHW.Configuration
{
    /// <summary>
    /// <para>
    /// Class for a the configuration settings of a sensor tile
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
    /// <para>Date: 15.08.2015</para>
    /// </summary>
    public class TileViewConfigurationFile
    {
        private string configFile = string.Empty;

        /// <summary>
        /// CTOR
        /// </summary>
        public TileViewConfigurationFile()
        {
            this.configFile = DirectoryConstants.TileViewSettingsConfig;

            // Check if file exists -> if not create one
            if (!System.IO.File.Exists(this.configFile))
            {
                // Create the settings file
                this.CreateSettingsFile();
            }
            else
            {
                // Read settings file
                this.ReadSettingsFile();
            }
        }

        /// <summary>
        ///  Create settings file
        /// </summary>
        private void CreateSettingsFile()
        {
            try
            {
                // Create new XML-File
                XDocument xdoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                    new XComment("Settings for the tile view page"),
                    new XElement("SensorTiles"));

                xdoc.Save(this.configFile);
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorCreatingConfigFile");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        /// <summary>
        /// Read the settings file
        /// </summary>
        private void ReadSettingsFile()
        {
            try
            {
                XElement xmlDoc = XElement.Load(this.configFile);

                foreach (var t in xmlDoc.Descendants("SensorTile"))
                {
                    SensorTileConfigurationEntry configEntry = new SensorTileConfigurationEntry();

                    configEntry.SensorName = t.Attribute("SensorName").Value != null ? t.Attribute("SensorName").Value : string.Empty;
                    configEntry.SensorCategory = t.Attribute("SensorCategory").Value != null ? t.Attribute("SensorCategory").Value : string.Empty;
                    configEntry.SensorType = t.Attribute("SensorType").Value != null ? t.Attribute("SensorType").Value : string.Empty;
                    configEntry.GridColumn = t.Attribute("GridColumn").Value != null ? XmlConvert.ToInt32(t.Attribute("GridColumn").Value) : default(int);
                    configEntry.GridRow = t.Attribute("GridRow").Value != null ? XmlConvert.ToInt32(t.Attribute("GridRow").Value) : default(int);

                    this.Tiles.Add(configEntry);
                }
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorReadingFanControllerTemplates");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        /// <summary>
        /// Insert new tile config
        /// </summary>
        /// <param name="sensorCategory">Sensor category</param>
        /// <param name="sensorName">Sensor name</param>
        /// <param name="sensorType">Sensor type</param>
        /// <param name="gridRow">The grid row</param>
        /// <param name="gridColumn">The grid column</param>
        public void InsertTileConfig(string sensorCategory, string sensorName, string sensorType, int gridRow, int gridColumn)
        {
            try
            {
                SensorTileConfigurationEntry configEntry = new SensorTileConfigurationEntry(sensorName, sensorCategory, sensorType, gridRow, gridColumn);

                XDocument xmlDoc = XDocument.Load(this.configFile);

                xmlDoc.Element("SensorTiles").Add(configEntry.ToXml());

                xmlDoc.Save(this.configFile);
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorReadingFanControllerTemplates");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        /// <summary>
        /// Update config
        /// </summary>
        /// <param name="sensorName">The sensor name</param>
        /// <param name="oldGridRow">Old grid row</param>
        /// <param name="oldGridColumn">Old grid column</param>
        /// <param name="newGridRow">New grid row</param>
        /// <param name="newGridColumn">New grid column</param>
        public void UpdateSensorTileConfig(string sensorName, int oldGridRow, int oldGridColumn, int newGridRow, int newGridColumn)
        {
            try
            {
                XElement xmlDoc = XElement.Load(this.configFile);

                var node = xmlDoc.Descendants("SensorTile").Where(x => x.Attribute("SensorName").Value.Equals(sensorName) &&
                                                                       x.Attribute("GridColumn").Value.Equals(oldGridColumn.ToString()) &&
                                                                       x.Attribute("GridRow").Value.Equals(oldGridRow.ToString())).FirstOrDefault();

                node.Attribute("GridColumn").SetValue(newGridColumn);
                node.Attribute("GridRow").SetValue(newGridRow);

                xmlDoc.Save(this.configFile);
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorReadingFanControllerTemplates");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        /// <summary>
        /// Delete tile sensor
        /// </summary>
        /// <param name="sensorName">The sensor name</param>
        /// <param name="gridRow">The grid row</param>
        /// <param name="gridColumn">The grid column</param>
        public void DeleteSensorTile(string sensorName, int gridRow, int gridColumn)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(this.configFile);

                var tileConfig = xmlDoc.Element("SensorTiles").Elements().Where(tc => tc.Attribute("SensorName").Value.Equals(sensorName) &&
                                                                                      tc.Attribute("GridRow").Value.Equals(XmlConvert.ToString(gridRow)) &&
                                                                                      tc.Attribute("GridColumn").Value.Equals(XmlConvert.ToString(gridColumn))).FirstOrDefault();

                if (tileConfig != null)
                    tileConfig.Remove();

                xmlDoc.Save(this.configFile);
            }
            catch (Exception ex)
            {
                var msg = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService).GetLocalizedString("MainboardFanControlErrorReadingFanControllerTemplates");
                // Log-Exception
                DependencyFactory.Resolve<ILoggingService>(ServiceNames.LoggingService).LogException(msg, ex);
                // Show exception
                DependencyFactory.Resolve<IExceptionReporterService>(ServiceNames.ExceptionReporterService).ReportException(ex);
            }
        }

        private IList<SensorTileConfigurationEntry> tiles = new List<SensorTileConfigurationEntry>();

        public IList<SensorTileConfigurationEntry> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
    }
}