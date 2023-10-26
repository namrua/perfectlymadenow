namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Registration agreement page model
    /// </summary>
    public class RegistrationAgreementPageModel
    {
        public ClassPublicDetail Class { get; set; }    
        public RegistrationAgreementForm Form { get; set; }

        // constructor
        public RegistrationAgreementPageModel()
        {
            Class= new ClassPublicDetail();
            Form = new RegistrationAgreementForm();            
        }
    }
}