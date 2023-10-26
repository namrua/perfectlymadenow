using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Button extensions for HtmlHelper class
    /// </summary>
    public static class ButtonExtensions 
    {

        // creates button group
        public static HtmlButtonHelper ButtonGroup(this HtmlHelper html, ButtonGroupType buttonGroupType)
        {
            var result = new HtmlButtonHelper(html.ViewContext.Writer, buttonGroupType);
            return result;
        }

    }
    
}
