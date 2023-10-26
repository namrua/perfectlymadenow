namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Encapsulates names of actions and views used in the registration controller for correct visualisation of registrations
    /// </summary>
    public class RegistrationControllerInfo
    {

        public string ViewForDetail { get; set; }
        public string ViewForForm { get; set; }
        public string ActionForSave { get; set; }             
        public string PartialViewDetailForHome { get; set; }

    }

}
