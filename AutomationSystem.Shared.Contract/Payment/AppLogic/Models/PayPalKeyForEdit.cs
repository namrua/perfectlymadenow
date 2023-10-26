using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Payment.AppLogic.Models
{
    public class PayPalKeyForEdit
    {
        public List<IEnumItem> Currencies { get; set; } = new List<IEnumItem>();
        public PayPalKeyForm Form { get; set; } = new PayPalKeyForm();
        public bool CanDelete { get; set; }
    }
}
