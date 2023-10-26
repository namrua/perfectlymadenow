namespace PerfectlyMadeInc.WebEx.Connectors.Integration.Model
{
    /// <summary>
    /// technical information for person 
    /// </summary>
    public class WebExAdditionalInfo
    {
        // public properties
        public WebExPersonType PersonType { get; set; }
        public WebExRole Role { get; set; }
        public bool SendEmailInvitation { get; set; }
        public WebExStatus Status { get; set; }

        public WebExAdditionalInfo()
        {
            Status = WebExStatus.Accept;
            Role = WebExRole.Attendee;
            PersonType = WebExPersonType.Visitor;
            SendEmailInvitation = false;
        }

    }
    
}
