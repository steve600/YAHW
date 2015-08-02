using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAHW.Constants
{
    public static class DirectoryConstants
    {
        /// <summary>
        /// The application data folder
        /// </summary>
        public static string ApplicationDataFolder
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YAHW");
            }
        }

        /// <summary>
        /// The file with the fan controller templates
        /// </summary>
        public static string FanControllerTemplatesConfig
        {
            get
            {
                return System.IO.Path.Combine(System.Environment.CurrentDirectory, @"ConfigFiles\FanControllerTemplates.xml");
            }
        }

        /// <summary>
        /// The file with the fan controller settings
        /// </summary>
        public static string FanControllerSettingsConfig
        {
            get
            {
                return System.IO.Path.Combine(ApplicationDataFolder, "FanControllerSettings.xml");
            }
        }
    }
}