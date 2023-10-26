using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Material recipient for edit
    /// </summary>
    public class MaterialRecipientForEdit
    {

        public List<IEnumItem> Languages { get; set; } = new List<IEnumItem>();
        public MaterialRecipientForm Form { get; set; } = new MaterialRecipientForm();

    }
}
