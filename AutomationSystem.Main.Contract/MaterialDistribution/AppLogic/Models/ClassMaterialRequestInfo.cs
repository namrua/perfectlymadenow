using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Encapsulates basic information of material request
    /// </summary>
    public class ClassMaterialRequestInfo
    {

        public long ClassId { get; set; }
        public long ClassMaterialId { get; set; }
        public long ClassMaterialRecipientId { get; set; }
        public LanguageEnum? LanguageId { get; set; }
        public MaterialAvailabilityResult Availability { get; set; }

    }
}
