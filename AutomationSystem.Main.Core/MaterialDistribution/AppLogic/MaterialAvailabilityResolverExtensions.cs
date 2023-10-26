using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{

    /// <summary>
    /// Resolves availability of materials
    /// </summary>
    public static class MaterialAvailabilityResolverExtensions
    {
        public static MaterialAvailabilityResult MergeResults(this MaterialAvailabilityResult first, MaterialAvailabilityResult second)
        {
            var result = new MaterialAvailabilityResult
            {
                AreMaterialsAvailable = first.AreMaterialsAvailable && second.AreMaterialsAvailable,
                Message = first.Message ?? second.Message
            };
            return result;
        }
    }
}
