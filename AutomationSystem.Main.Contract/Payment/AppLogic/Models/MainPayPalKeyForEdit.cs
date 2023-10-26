using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Payment.AppLogic.Models
{

    /// <summary>
    /// Main PayPalKey for edit
    /// </summary>
    public class MainPayPalKeyForEdit
    {
        public bool CanDelete { get; set; }
        public List<IEnumItem> Currencies { get; set; } = new List<IEnumItem>();
        public MainPayPalKeyForm Form { get; set; } = new MainPayPalKeyForm();
    }

}
