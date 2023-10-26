using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Page model for Distance class page
    /// </summary>
    public class DistanceClassesPageModel
    {
        
        public RegistrationPageStyle ProfilePageStyle { get; set; } = new RegistrationPageStyle();
        public List<ClassPublicDetail> Classes { get; set; } = new List<ClassPublicDetail>();
    }
}