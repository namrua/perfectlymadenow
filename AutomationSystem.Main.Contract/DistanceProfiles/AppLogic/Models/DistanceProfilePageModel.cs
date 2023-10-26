using CorabeuControl.Components;
using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models
{
    public class DistanceProfilePageModel
    {
        public List<DistanceProfileListItem> Items { get; set; } = new List<DistanceProfileListItem>();

        public List<DropDownItem> Profiles { get; set; } = new List<DropDownItem>();
    }
}
