using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Helps manage registration page style
    /// </summary>
    public static class RegistrationPageStyleHelper
    {

        // sets page style
        public static void SetStyle(RegistrationPageStyle style, dynamic viewBag)
        {
            viewBag.RegistrationPageStyle = style;
        }

        // gets page style
        public static RegistrationPageStyle GetStyle(dynamic viewBag)
        {
            var style = viewBag.RegistrationPageStyle;
            var result = style as RegistrationPageStyle;
            if (result == null)
                return new RegistrationPageStyle();
            return result;
        }

        // gets bundle by color scheme
        public static string GetBundleByColorScheme(RegistrationColorSchemeEnum colorSchemeId)
        {
            switch (colorSchemeId)
            {
                case RegistrationColorSchemeEnum.Limet:
                    return "~/HomeLimet/css";
                case RegistrationColorSchemeEnum.Ocean:
                    return "~/HomeOcean/css";
                default:
                    return "~/HomeLimet/css";
            }
        }

    }

}