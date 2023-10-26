using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Model;
using System;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    public class MaterialAvailabilityResolver : IMaterialAvailabilityResolver
    {
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IClassMaterialBusinessRules classMaterialBusinessRules;

        public MaterialAvailabilityResolver(IClassOperationChecker classOperationChecker, IClassMaterialBusinessRules classMaterialBusinessRules)
        {
            this.classOperationChecker = classOperationChecker;
            this.classMaterialBusinessRules = classMaterialBusinessRules;
        }

        public MaterialAvailabilityResult ResolveClassRestrictions(Class cls, ClassMaterial classMaterial)
        {
            var result = new MaterialAvailabilityResult();
            result.AreMaterialsAvailable = false;
            var utcNow = DateTime.UtcNow;

            // check class status
            var classState = ClassConvertor.GetClassState(cls);
            if (classOperationChecker.IsOperationDisabled(ClassOperation.MaterialDistribution, classState))
            {
                result.Message = $"Invalid class state: {classState}";
                return result;
            }

            // checks class end
            if (classMaterialBusinessRules.IsLockedByClassEndDate(cls.EventEndUtc, utcNow))
            {
                result.Message = "Class's end time exceeded";
                return result;
            }

            // checks automation lock
            if (classMaterial.AutomationLockTimeUtc.HasValue && classMaterial.AutomationLockTimeUtc < utcNow)
            {
                result.Message = "Automation lock time exceeded";
                return result;
            }

            // checks whether class materials are unlocked
            if (!classMaterial.IsUnlocked)
            {
                result.Message = "Class materials are not unlocked";
                return result;
            }

            result.AreMaterialsAvailable = true;
            return result;
        }

        public  MaterialAvailabilityResult ResolveMaterialRecipientRestrictions(ClassMaterialRecipient materialRecipient)
        {
            var result = new MaterialAvailabilityResult();
            result.AreMaterialsAvailable = false;

            // checks whether class material recipient is not locked
            if (materialRecipient.IsLocked)
            {
                result.Message = "Material recipient is locked";
                return result;
            }

            result.AreMaterialsAvailable = true;
            return result;
        }

        public MaterialAvailabilityResult ResolveFileRestrictions(ClassMaterialFile materialFile, ClassMaterialRecipient materialRecipient)
        {
            var result = new MaterialAvailabilityResult();
            result.AreMaterialsAvailable = false;

            if (materialFile.LanguageId != materialRecipient.LanguageId)
            {
                result.Message = $"Language of material recipient ({materialRecipient.LanguageId}) and file ({materialFile.LanguageId}) does not match";
                return result;
            }

            result.AreMaterialsAvailable = true;
            return result;
        }

        public MaterialAvailabilityResult ResolveDownloadRestrictions(ClassMaterialRecipient materialRecipient, int downloadCount)
        {
            var result = new MaterialAvailabilityResult();
            result.AreMaterialsAvailable = false;

            if (materialRecipient.DownloadLimit.HasValue && downloadCount >= materialRecipient.DownloadLimit)
            {
                result.Message = $"Download limit reached: current {downloadCount}, limit {materialRecipient.DownloadLimit.Value}";
                return result;
            }

            result.AreMaterialsAvailable = true;
            return result;
        }
    }
}
