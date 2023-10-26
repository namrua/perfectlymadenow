using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors
{
    /// <summary>
    /// Converts objects related to ClassMaterial entity
    /// </summary>
    public interface IClassMaterialConvertor
    {
        // creates initial ClassMaterial entity
        ClassMaterial CreateInitialClassMaterial(long classId);

        // converts ClassMaterial to ClassMaterialDetail
        ClassMaterialDetail ConvertToClassMaterialDetail(ClassMaterial material);

        // converts ClassMaterial to ClassMaterialForm 
        ClassMaterialForm ConvertToClassMaterialForm(ClassMaterial material, long classId);

        // converts ClassMaterialForm to ClassMaterial
        ClassMaterial ConvertToClassMaterial(ClassMaterialForm form, TimeZoneEnum timeZoneId);

    }
}
