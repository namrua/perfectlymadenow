using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Model for home page model
    /// </summary>
    public class HomePageModel
    {

        public string ProfileMoniker { get; set; }
        public RegistrationPageStyle ProfilePageStyle { get; set; } = new RegistrationPageStyle();
        public List<ClassPublicDetail> Classes { get; set; }

        // constructor
        public HomePageModel()
        {
            Classes = new List<ClassPublicDetail>();
        }
    }
}