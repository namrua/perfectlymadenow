using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.Profiles.AppLogic.Models
{
    /// <summary>
    /// Profile filter
    /// </summary>
    public class ProfileFilter
    {

        // filtering by list of profile ids - null, no filter
        public List<long> ProfileIds { get; set; }

        public List<long> ExcludeProfileIds { get; set; }

        // determines whether default profile should be included into selection
        // this is not used in DB filtering - rather holds information
        public bool IncludeDefaultProfile { get; set; }

    }


}
