using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    public interface IMaterialAvailabilityResolver
    {
        MaterialAvailabilityResult ResolveClassRestrictions(Class cls, ClassMaterial classMaterial);

        MaterialAvailabilityResult ResolveMaterialRecipientRestrictions(ClassMaterialRecipient materialRecipient);

        MaterialAvailabilityResult ResolveFileRestrictions(ClassMaterialFile materialFile, ClassMaterialRecipient materialRecipient);

        MaterialAvailabilityResult ResolveDownloadRestrictions(ClassMaterialRecipient materialRecipient, int downloadCount);


    }
}
