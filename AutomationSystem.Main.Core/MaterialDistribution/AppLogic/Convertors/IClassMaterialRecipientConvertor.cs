using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors
{
    /// <summary>
    /// Converts objects related to ClassMaterialRecipient entity
    /// </summary>
    public interface IClassMaterialRecipientConvertor
    {
        // creates initial ClassMaterialRecipient object
        ClassMaterialRecipient CreateInitialClassMaterialRecipient(long classMaterialId, RecipientId recipientId, LanguageEnum? preselectedLanguageId = null);

        // initializes MaterialRecipientForEdit
        MaterialRecipientForEdit InitializeMaterialRecipientForEdit(Class cls);

        // converts ClassMaterialRecipient to MaterialRecipientDetail
        MaterialRecipientDetail ConvertToMaterialRecipientDetail(ClassMaterialRecipient materialRecipient);

        // converts ClassMaterialRecipient to MaterialRecipientForm
        MaterialRecipientForm ConvertToMaterialRecipientForm(ClassMaterialRecipient materialRecipient);

        // converts MaterialRecipientForm to ClassMaterialRecipient
        ClassMaterialRecipient ConvertToClassMaterialRecipient(MaterialRecipientForm form);
    }
}
