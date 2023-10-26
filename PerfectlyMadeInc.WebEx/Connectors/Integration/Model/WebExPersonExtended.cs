namespace PerfectlyMadeInc.WebEx.Connectors.Integration.Model
{
    /// <summary>
    /// full information about person
    /// </summary>
    public class WebExPersonExtended
    {
        // public properties
        public WebExPerson PersonInfo { get; set; }
        public WebExAdditionalInfo AdditionalInfo { get; set; }

        // constructor
        public WebExPersonExtended()
        {
            PersonInfo = new WebExPerson();
            AdditionalInfo = new WebExAdditionalInfo();
        }
    }
    
}
