namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Encapsulates result of materials availability resolving
    /// </summary>
    public class MaterialAvailabilityResult
    {
        public bool AreMaterialsAvailable { get; set; } = true;
        public string Message { get; set; }
    }
}
