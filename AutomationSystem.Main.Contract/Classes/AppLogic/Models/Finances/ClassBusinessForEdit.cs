using System.Collections.Generic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Class business for edit
    /// </summary>
    public class ClassBusinessForEdit
    {
        public bool AreFinancialFormsAllowed { get; set; }
        public List<DropDownItem> Lectures { get; set; } = new List<DropDownItem>();
        public ClassBusinessForm Form { get; set; } = new ClassBusinessForm();
        public string CurrencyCode { get; set; }
    }
}