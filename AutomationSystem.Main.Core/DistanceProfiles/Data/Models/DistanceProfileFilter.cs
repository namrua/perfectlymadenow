using System.Collections.Generic;

namespace AutomationSystem.Main.Core.DistanceProfiles.Data.Models
{
    public class DistanceProfileFilter
    {
        public bool? IsActive { get; set; }
        public List<long> ExcludeIds { get; set; }
    }
}
