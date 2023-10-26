using System;
using System.Configuration;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Safely encapsulates access to configuration
    /// </summary>
    public static class ConfigHelper
    {

        public static bool ProcessErrors
        {
            get
            {
                try
                {
                    var result = bool.Parse(ConfigurationManager.AppSettings["ProcessErrors"]);
                    return result;
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }

    }

}