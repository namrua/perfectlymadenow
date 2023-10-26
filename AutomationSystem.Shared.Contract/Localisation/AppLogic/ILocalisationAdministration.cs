using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Localisation.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic
{

    /// <summary>
    /// Provides logic for localisation administration
    /// </summary>
    public interface ILocalisationAdministration
    {

        // gets language localisation summary
        LanguageLocalisationSummary GetLanguageLocalisationSummary();

        // gets app localisation list
        AppLocalisationList GetAppLocalisationList(LanguageEnum languageId);

        // gets app localisation for edit
        AppLocalisationForEdit GetAppLocalisatonForEdit(long originId, LanguageEnum languageId);

        // gets app localisation for edit by form
        AppLocalisationForEdit GetFormAppLocalisationForEdit(AppLocalisationForm form, AppLocalisationValidationResult validationResult);

        // validates app localisation
        AppLocalisationValidationResult ValidateAppLocalisation(AppLocalisationForm form);

        // saves app localisation
        void SaveAppLocalisation(AppLocalisationForm form);



        // gets enum type list
        EnumTypeList GetEnumTypeList(LanguageEnum languageId);

        // gets enum localisation list
        EnumLocalisationList GetEnumLocalisationList(LanguageEnum languageId, EnumTypeEnum enumTypeId);

        // gets enum localisation for edit
        EnumLocalisationForEdit GetEnumLocalisationForEdit(LanguageEnum languageId, EnumTypeEnum enumTypeId, int itemId);

        // gets enum localisation for edit
        EnumLocalisationForEdit GetFormEnumLocalisationForEdit(EnumLocalisationForm form);

        // saves enum localisation
        void SaveEnumLocalisation(EnumLocalisationForm form);

    }

}
