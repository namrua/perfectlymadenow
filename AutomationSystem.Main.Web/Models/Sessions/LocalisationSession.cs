using System.Collections.Generic;

namespace AutomationSystem.Main.Web.Models.Sessions
{
    /// <summary>
    /// Localisation session
    /// </summary>
    public class LocalisationSession
    {

        // public properties       
        public List<long> AppLocalisationOrder { get; set; }
        public List<int> EnumLocalisationOrder { get; set; }

        // constructor
        public LocalisationSession()
        {
            AppLocalisationOrder = new List<long>();
            EnumLocalisationOrder = new List<int>();
        }

    }

}