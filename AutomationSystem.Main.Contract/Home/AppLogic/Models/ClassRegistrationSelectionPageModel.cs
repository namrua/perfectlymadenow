using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Class registration selection page model
    /// </summary>
    public class ClassRegistrationSelectionPageModel
    {
        public ClassPublicDetail Class { get; set; }
        public string CurrencyCode { get; set; }
        public List<RegistrationTypeListItem> RegistrationTypes { get; set; }

        // constructor
        public ClassRegistrationSelectionPageModel()
        {
            Class = new ClassPublicDetail();
            RegistrationTypes = new List<RegistrationTypeListItem>();
        }
    }
}