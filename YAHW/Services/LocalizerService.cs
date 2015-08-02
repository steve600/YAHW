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

using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;
using YAHW.Interfaces;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// The global localizer service
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
    public class LocalizerService : ILocalizerService
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="culture"></param>
        public LocalizerService(string culture)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture)));
            this.SetLocale(culture);
        }

        /// <summary>
        /// Set localization
        /// </summary>
        /// <param name="locale"></param>
        public void SetLocale(string locale)
        {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(locale);
        }

        /// <summary>
        /// Set localization
        /// </summary>
        /// <param name="culture"></param>
        public void SetLocale(CultureInfo culture)
        {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = culture;
        }

        /// <summary>
        /// Get localized string from resource dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLocalizedString(string key)
        {
            string uiString;
            LocExtension locExtension = new LocExtension(key);
            locExtension.ResolveLocalizedValue(out uiString);
            return uiString;
        }

        /// <summary>
        /// Returns the active culture
        /// </summary>
        public CultureInfo ActiveCulture
        {
            get
            {
                return LocalizeDictionary.Instance.Culture;
            }
        }
    }
}