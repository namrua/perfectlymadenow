using System.Web;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Home error page model
    /// </summary>
    public class HomeErrorPageModel
    {
        public string Title { get; set; }
        public HtmlString Message { get; set; }

    }
}