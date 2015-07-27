using System.Globalization;

namespace YAHW.Interfaces
{
    /// <summary>
    /// <para>
    /// Interface for the localizer service
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
    /// <para>Date: 18.12.2014</para>
    /// </summary>
    public interface ILocalizerService
    {
        /// <summary>
        /// Set language
        /// </summary>
        /// <param name="locale"></param>
        void SetLocale(string locale);

        /// <summary>
        /// Set language
        /// </summary>
        /// <param name="culture"></param>
        void SetLocale(CultureInfo culture);

        /// <summary>
        /// Get a localized string by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetLocalizedString(string key);

        /// <summary>
        /// Gets the active culture
        /// </summary>
        CultureInfo ActiveCulture { get; }
    }
}