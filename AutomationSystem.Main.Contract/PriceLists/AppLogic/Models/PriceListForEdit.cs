using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.PriceLists.AppLogic.Models
{
    /// <summary>
    /// Price list for edit
    /// </summary>
    public class PriceListForEdit
    {
        // public properties
        public PriceListForm Form { get; set; }

        public List<IEnumItem> Currencies { get; set; }

        public Dictionary<RegistrationTypeEnum, string> RegistrationTypeDescriptions { get; set; }      

        // constructor
        public PriceListForEdit()
        {
            Form = new PriceListForm();
            RegistrationTypeDescriptions = new Dictionary<RegistrationTypeEnum, string>();
        }
    }
}