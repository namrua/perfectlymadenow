using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Encapsulates full material request info
    /// </summary>
    public class MaterialRequestFullInfo
    {
        public Class Class { get; set; }
        public ClassMaterial ClassMaterial { get; set; }
        public ClassMaterialRecipient ClassMaterialRecipient { get; set; }
        public MaterialAvailabilityResult Availability { get; set; }
    }
}
