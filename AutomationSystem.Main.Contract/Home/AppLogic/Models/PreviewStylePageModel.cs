using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{

    /// <summary>
    /// Page model for preview of class style
    /// </summary>
    public class PreviewStylePageModel
    {

        public string BackUrl { get; set; }
        public List<IEnumItem> Countries { get; set; }
        public AddressForm Form { get; set; } = new AddressForm();

    }

}
