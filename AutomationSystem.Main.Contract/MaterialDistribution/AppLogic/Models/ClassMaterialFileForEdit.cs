using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Class material file foredit
    /// </summary>
    public class ClassMaterialFileForEdit
    {
        public List<IEnumItem> Languages { get; set; } = new List<IEnumItem>();
        public ClassMaterialFileForm Form = new ClassMaterialFileForm();
    }
}
