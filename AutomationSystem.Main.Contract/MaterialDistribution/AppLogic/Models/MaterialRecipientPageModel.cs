using System.Collections.Generic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Material recipient page model
    /// </summary>
    public class MaterialRecipientPageModel
    {
        public bool IsMaterialsDisabled { get; set; }
        public string MaterialsDisabledMessage { get; set; }

        public long ClassMaterialRecipientId { get; set; }
        public RecipientId RecipientId { get; set; }
        public ClassShortDetail Class { get; set; } = new ClassShortDetail();
        public ClassMaterialState ClassMaterialState { get; set; }
        public MaterialRecipientDetail Detail { get; set; } = new MaterialRecipientDetail();
        public List<ClassMaterialFileDetail> Materials { get; set; } = new List<ClassMaterialFileDetail>();
        public bool CanUnlockMaterials { get; set; }
        public bool CanSendNotification { get; set; }
        public bool CanLockMaterials { get; set; }
    }
}
