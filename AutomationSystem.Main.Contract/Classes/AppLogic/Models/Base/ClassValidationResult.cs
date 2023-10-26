using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base
{
    /// <summary>
    /// Class validation result
    /// </summary>
    public class ClassValidationResult
    {
        public bool IsValid { get; set; }
        public bool IsInconsistentClassAndPriceListType { get; set; }
        public ClassTypeEnum? ForbiddenClassTypeId { get; set; }
        public long? ForbiddenPriceListId { get; set; }
    }
}