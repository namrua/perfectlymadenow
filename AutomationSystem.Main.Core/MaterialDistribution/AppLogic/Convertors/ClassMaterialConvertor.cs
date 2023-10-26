using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.TimeZones.System;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors
{
    /// <summary>
    /// Converts objects related to ClassMaterial entity
    /// </summary>
    public class ClassMaterialConvertor : IClassMaterialConvertor
    {

        private readonly ITimeZoneService timeZoneService;
        private readonly IMaterialPasswordGenerator materialPasswordGenerator;

        // constructor
        public ClassMaterialConvertor(ITimeZoneService timeZoneService, IMaterialPasswordGenerator materialPasswordGenerator)
        {
            this.timeZoneService = timeZoneService;
            this.materialPasswordGenerator = materialPasswordGenerator;
        }


        // creates initial ClassMaterial entity
        public ClassMaterial CreateInitialClassMaterial(long classId)
        {
            var result = new ClassMaterial
            {
                CoordinatorPassword = materialPasswordGenerator.GeneratePassword(),
                ClassId = classId
            };
            return result;
        }


        // Converts ClassMaterial to ClassMaterialDetail
        public ClassMaterialDetail ConvertToClassMaterialDetail(ClassMaterial material)
        {
            var result = new ClassMaterialDetail
            {
                CoordinatorPassword = material.CoordinatorPassword,
                AutomationLockTime = material.AutomationLockTime,
                IsLocked = material.IsLocked,
                Locked = material.Locked,
                IsUnlocked = material.IsUnlocked,
                Unlocked = material.Unlocked,
            };
            return result;
        }


        // converts ClassMaterial to ClassMaterialForm 
        public ClassMaterialForm ConvertToClassMaterialForm(ClassMaterial material, long classId)
        {
            var result = new ClassMaterialForm
            {
                ClassId = classId,
                CoordinatorPassword = material.CoordinatorPassword,
                AutomationLockTime = material.AutomationLockTime,
            };
            return result;
        }

        // converts ClassMaterialForm to ClassMaterial
        public ClassMaterial ConvertToClassMaterial(ClassMaterialForm form, TimeZoneEnum timeZoneId)
        {
            var result = new ClassMaterial
            {
                CoordinatorPassword = form.CoordinatorPassword,
                AutomationLockTime = form.AutomationLockTime,
                AutomationLockTimeUtc = timeZoneService.GetUtcDateTime(form.AutomationLockTime, timeZoneId),
            };
            return result;
        }

    }

}
