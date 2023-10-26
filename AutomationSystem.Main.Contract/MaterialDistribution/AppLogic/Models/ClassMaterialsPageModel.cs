using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Encapsulates class materials page model
    /// </summary>
    public class ClassMaterialsPageModel
    {

        [DisplayName("Class")]
        public ClassShortDetail Class { get; set; } = new ClassShortDetail();

        [DisplayName("Detail")]
        public ClassMaterialDetail Detail { get; set; } = new ClassMaterialDetail();

        [DisplayName("PDF Materials")]
        public List<ClassMaterialFileDetail> Materials { get; set; } = new List<ClassMaterialFileDetail>();

        [DisplayName("State")]
        public ClassMaterialState ClassMaterialState { get; set; }

        public bool AreMaterialsDisabled { get; set; }
        public string MaterialsDisabledMessage { get; set; }
        public bool CanUnlockMaterials { get; set; }
        public bool CanSendNotification { get; set; }
        public bool CanLockMaterials { get; set; }

    }
}
