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

using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using System.Xml;
using XAHW.Interfaces;
using YAHW.BaseClasses;
using YAHW.Constants;
using YAHW.Interfaces;

namespace YAHW.ViewModels
{
    /// <summary>
    /// <para>
    /// A simple view model for configuring theme, font and accent colors.
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
    public class AppearanceViewModel : ViewModelBase
    {
        private IConfigurationFile applicationConfigFile = null;

        // 20 accent colors from Windows Phone 8
        private Color[] accentColors = new Color[]{
            Color.FromRgb(0xa4, 0xc4, 0x00),   // lime
            Color.FromRgb(0x60, 0xa9, 0x17),   // green
            Color.FromRgb(0x00, 0x8a, 0x00),   // emerald
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x1b, 0xa1, 0xe2),   // cyan
            Color.FromRgb(0x00, 0x50, 0xef),   // cobalt
            Color.FromRgb(0x6a, 0x00, 0xff),   // indigo
            Color.FromRgb(0xaa, 0x00, 0xff),   // violet
            Color.FromRgb(0xf4, 0x72, 0xd0),   // pink
            Color.FromRgb(0xd8, 0x00, 0x73),   // magenta
            Color.FromRgb(0xa2, 0x00, 0x25),   // crimson
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xfa, 0x68, 0x00),   // orange
            Color.FromRgb(0xf0, 0xa3, 0x0a),   // amber
            Color.FromRgb(0xe3, 0xc8, 0x00),   // yellow
            Color.FromRgb(0x82, 0x5a, 0x2c),   // brown
            Color.FromRgb(0x6d, 0x87, 0x64),   // olive
            Color.FromRgb(0x64, 0x76, 0x87),   // steel
            Color.FromRgb(0x76, 0x60, 0x8a),   // mauve
            Color.FromRgb(0x87, 0x79, 0x4e),   // taupe
        };

        private Color selectedAccentColor;
        private LinkCollection themes = new LinkCollection();
        private Link selectedTheme;
        private string selectedFontSize;

        /// <summary>
        /// CTOR
        /// </summary>
        public AppearanceViewModel()
        {
            // add the default themes
            this.themes.Add(new Link { DisplayName = GeneralConstants.ThemeDark, Source = AppearanceManager.DarkThemeSource });
            this.themes.Add(new Link { DisplayName = GeneralConstants.ThemeLight, Source = AppearanceManager.LightThemeSource });

            this.AvailableLanguages = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                            .Where(ci => ci.IetfLanguageTag == "de-DE" || ci.IetfLanguageTag == "en-US")
                                            .ToList();

            // Read config file
            this.ReadConfigFile();

            SyncThemeAndColor();

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        // Read config file
        private void ReadConfigFile()
        {
            this.applicationConfigFile = DependencyFactory.Resolve<IConfigurationFile>(ConfigFileNames.ApplicationConfig);

            if (this.applicationConfigFile != null)
            {
                // Theme
                if (this.applicationConfigFile.Sections["GeneralSettings"].Settings["Theme"] == null)
                {
                    // Add entry
                    this.applicationConfigFile.Sections["GeneralSettings"].Settings.Add("Theme", GeneralConstants.ThemeLight, GeneralConstants.ThemeLight, typeof(System.String));
                    AppearanceManager.Current.ThemeSource = AppearanceManager.LightThemeSource;
                    this.applicationConfigFile.Save();
                }
                else
                {
                    var theme = this.applicationConfigFile.Sections["GeneralSettings"].Settings["Theme"].Value;
                    if (theme.Equals(GeneralConstants.ThemeLight))
                        AppearanceManager.Current.ThemeSource = AppearanceManager.LightThemeSource;
                    else if (theme.Equals(GeneralConstants.ThemeDark))
                        AppearanceManager.Current.ThemeSource = AppearanceManager.DarkThemeSource;
                    // Fallback value
                    else
                        AppearanceManager.Current.ThemeSource = AppearanceManager.LightThemeSource;
                }

                // Font size
                if (this.applicationConfigFile.Sections["GeneralSettings"].Settings["FontSize"] == null)
                {
                    this.applicationConfigFile.Sections["GeneralSettings"].Settings.Add("FontSize", GeneralConstants.FontLarge, GeneralConstants.FontLarge, typeof(System.String));
                    this.SelectedFontSize = GeneralConstants.FontLarge;
                    AppearanceManager.Current.FontSize = FontSize.Large;
                }
                else
                {
                    var fontSize = this.applicationConfigFile.Sections["GeneralSettings"].Settings["FontSize"].Value;
                    if (fontSize.Equals(GeneralConstants.FontLarge))
                    {
                        this.SelectedFontSize = GeneralConstants.FontLarge;
                        AppearanceManager.Current.FontSize = FontSize.Large;
                    }
                    else if (fontSize.Equals(GeneralConstants.FontSmall))
                    {
                        this.SelectedFontSize = GeneralConstants.FontSmall;
                        AppearanceManager.Current.FontSize = FontSize.Small;
                    }
                    else
                    {
                        this.SelectedFontSize = GeneralConstants.FontLarge;
                        AppearanceManager.Current.FontSize = FontSize.Large;
                    }
                }

                // Color
                if (this.applicationConfigFile.Sections["GeneralSettings"].Settings["AccentColor"] == null)
                {
                    this.applicationConfigFile.Sections["GeneralSettings"].Settings.Add("AccentColor", AppearanceManager.Current.AccentColor.ToString(), AppearanceManager.Current.AccentColor.ToString(), typeof(System.String));
                    this.applicationConfigFile.Save();
                }
                else
                {
                    var color = this.applicationConfigFile.Sections["GeneralSettings"].Settings["AccentColor"].Value;
                    if (!String.IsNullOrEmpty(color) && color.Length == 9)
                    {
                        AppearanceManager.Current.AccentColor = this.GetColorFromString(color);
                    }
                }

                // Check if all settings are in config file
                if (this.applicationConfigFile.Sections["GeneralSettings"].Settings["Language"] == null)
                {
                    this.applicationConfigFile.Sections["GeneralSettings"].Settings.Add("Language", "de-DE", "de-DE", typeof(System.String));
                    this.SelectedLanguage = this.AvailableLanguages.Where(l => l.IetfLanguageTag.Equals("de-DE")).FirstOrDefault();
                }
                else
                {
                    var language = this.applicationConfigFile.Sections["GeneralSettings"].Settings["Language"].Value;
                    this.SelectedLanguage = this.AvailableLanguages.Where(l => l.IetfLanguageTag.Equals(language)).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Get path fill brush
        /// </summary>
        /// <returns></returns>
        private SolidColorBrush GetPathFillBrush()
        {
            byte R, G, B;

            SolidColorBrush brush;

            if (AppearanceManager.Current.ThemeSource.Equals(this.themes.FirstOrDefault(l => l.DisplayName.Equals(GeneralConstants.ThemeLight)).Source))
            {
                R = Convert.ToByte("00", 16);
                G = Convert.ToByte("00", 16);
                B = Convert.ToByte("00", 16);
            }
            else
            {
                R = Convert.ToByte("D1", 16);
                G = Convert.ToByte("D1", 16);
                B = Convert.ToByte("D1", 16);
            }
            
            brush = new SolidColorBrush(Color.FromRgb(R, G, B));

            return brush;
        }

        /// <summary>
        /// Convert string to color
        /// </summary>
        /// <param name="c">Color, e.g. #FF1BA1E2</param>
        /// <returns>The converter color struct</returns>
        private Color GetColorFromString(string c)
        {
            byte R, G, B;

            R = Convert.ToByte(c.Substring(3, 2), 16);
            G = Convert.ToByte(c.Substring(5, 2), 16);
            B = Convert.ToByte(c.Substring(7, 2), 16);

            return Color.FromRgb(R, G, B);
        }

        private void SyncThemeAndColor()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            this.SelectedTheme = this.themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));

            // and make sure accent color is up-to-date
            this.SelectedAccentColor = AppearanceManager.Current.AccentColor;
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor")
            {
                SyncThemeAndColor();
            }
        }

        /// <summary>
        /// List with themes
        /// </summary>
        public LinkCollection Themes
        {
            get { return this.themes; }
        }

        /// <summary>
        /// List with font sizes
        /// </summary>
        public string[] FontSizes
        {
            get { return new string[] { GeneralConstants.FontSmall, GeneralConstants.FontLarge }; }
        }

        /// <summary>
        /// List with accent colors
        /// </summary>
        public Color[] AccentColors
        {
            get { return this.accentColors; }
        }

        /// <summary>
        /// The selected theme
        /// </summary>
        public Link SelectedTheme
        {
            get { return this.selectedTheme; }
            set
            {
                if (this.SetProperty<Link>(ref this.selectedTheme, value))
                {
                    // and update the actual theme
                    AppearanceManager.Current.ThemeSource = value.Source;
                    // Path fill brush
                    this.PathFillBrush = this.GetPathFillBrush();
                    // Save to config
                    this.applicationConfigFile.Sections["GeneralSettings"].Settings["Theme"].Value = value.DisplayName;
                    this.applicationConfigFile.Save();
                }
            }
        }

        /// <summary>
        /// The selected font size
        /// </summary>
        public string SelectedFontSize
        {
            get { return this.selectedFontSize; }
            set
            {
                if (this.SetProperty<string>(ref this.selectedFontSize, value))
                {
                    AppearanceManager.Current.FontSize = value == GeneralConstants.FontLarge ? FontSize.Large : FontSize.Small;
                    // Save config
                    this.applicationConfigFile.Sections["GeneralSettings"].Settings["FontSize"].Value = value;
                    this.applicationConfigFile.Save();
                }
            }
        }

        /// <summary>
        /// Selected accent color
        /// </summary>
        public Color SelectedAccentColor
        {
            get { return this.selectedAccentColor; }
            set
            {
                if (this.SetProperty<Color>(ref this.selectedAccentColor, value))
                {
                    // Set value
                    AppearanceManager.Current.AccentColor = value;
                    // Save config
                    this.applicationConfigFile.Sections["GeneralSettings"].Settings["AccentColor"].Value = value.ToString();
                    this.applicationConfigFile.Save();
                }
            }
        }

        /// <summary>
        /// List with availabe languages
        /// </summary>
        public List<CultureInfo> AvailableLanguages
        {
            get;
            private set;
        }

        private CultureInfo selectedLanguage;

        /// <summary>
        /// Selected language
        /// </summary>
        public CultureInfo SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                if (this.SetProperty<CultureInfo>(ref this.selectedLanguage, value))
                {
                    var localizerService = DependencyFactory.Resolve<ILocalizerService>(ServiceNames.LocalizerService);
                    if (localizerService != null)
                    {
                        localizerService.SetLocale(value.IetfLanguageTag);
                    }
                    // Save config
                    this.applicationConfigFile.Sections["GeneralSettings"].Settings["Language"].Value = value.IetfLanguageTag;
                    this.applicationConfigFile.Save();
                }
            }
        }

        private SolidColorBrush pathFillBrush;

        /// <summary>
        /// Path fill brush
        /// </summary>
        public SolidColorBrush PathFillBrush
        {
            get { return pathFillBrush; }
            private set { this.SetProperty<SolidColorBrush>(ref this.pathFillBrush, value); }
        }
    }
}