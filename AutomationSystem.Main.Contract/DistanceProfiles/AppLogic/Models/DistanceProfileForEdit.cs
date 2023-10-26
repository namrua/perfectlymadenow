using AutomationSystem.Main.Contract.Persons.AppLogic;
using CorabeuControl.Components;
using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models
{
    public class DistanceProfileForEdit
    {
        public DistanceProfileForm Form { get; set; } = new DistanceProfileForm();
        public List<DropDownItem> PriceLists { get; set; } = new List<DropDownItem>();
        public List<DropDownItem> PayPalKeys { get; set; } = new List<DropDownItem>();
        public IPersonHelper Persons { get; set; } = new EmptyPersonHelper();
    }
}
