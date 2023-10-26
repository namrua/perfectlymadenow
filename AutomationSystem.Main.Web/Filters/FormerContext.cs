
namespace AutomationSystem.Main.Web.Filters
{
    /// <summary>
    /// Encapsulates former pages context
    /// </summary>
    public class FormerContext
    {
        
        // base page context
        public FormerBasePages Base { get; set; }      
        public long? PickForRegistrationId { get; set; }
        
        // constructor
        public FormerContext(FormerBasePages basePages, long? pickForRegistrationId = null)
        {
            Base = basePages;
            PickForRegistrationId = pickForRegistrationId;
        }

    }

}