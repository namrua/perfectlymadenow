using System.Collections.Generic;
using System.Globalization;
using System.Web;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Shared.Contract.Localisation.System
{
    /// <summary>
    /// Localisation service
    /// </summary>
    public interface ILocalisationService
    {

        #region languages

        // gets current language code
        string CurrentLanguageCode { get; }

        // get currrent culture info
        CultureInfo CurrentCultureInfo { get; }

        // get current language enum or null if it is not supported
        LanguageEnum? CurrentLanguageEnum { get; }

        // sets currrent language
        void SetLanguage(string code);

        // tries to get language, returns null whether language is not supported
        IEnumItem TryGetSupportedLanguage(LanguageEnum languageId);

        // tries to get a language
        IEnumItem TryGetLanguage(LanguageEnum languageId);

        // gets supported languages
        List<IEnumItem> GetSupportedLanguages(bool omitDefault = false);

        // gets all languages
        List<IEnumItem> GetAllLanguages();

        // gets language info map
        ILanguageInfoPovider GetLanguageInfoProvider();

        // checks whether language is supported
        bool IsSupported(LanguageEnum languageId);

        // checks whether language is valid
        bool IsLanguage(LanguageEnum languageId);

        #endregion


        #region localised items

        // gets localised html string
        HtmlString GetLocalisedHtmlString(string module, string label);

        // gets localised string
        string GetLocalisedString(string module, string label);

        // gets localised string specified
        string GetLocalisedString(string module, string label, string languageCode);

        // gets localised enum item
        IEnumItem GetLocalisedEnumItem(EnumTypeEnum enumTypeId, int itemId);

        // gets localised enum item with specified langcode
        IEnumItem GetLocalisedEnumItemSpecified(EnumTypeEnum enumTypeId, int itemId, string languageCode);

        // gets localised enums by filter
        List<IEnumItem> GetLocalisedEnumItemsByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter = null);

        // gets enum map by filter
        Dictionary<int, IEnumItem> GetLocalisedEnumItemMapByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter);

        // gets languages by ids
        List<IEnumItem> GetLocalisedLanguagesByIds(params LanguageEnum?[] languages);

        #endregion    


        #region data localisation

        // gets entity data localisation
        EntityDataLocalisation GetEntityDataLocalisationById(EntityTypeEnum entityType, long entityId, LanguageEnum language);

        // gets entities data localisation by entity ids
        List<EntityDataLocalisation> GetEntityDataLocalisationByIds(EntityTypeEnum entityType, IEnumerable<long> entityId);

        // saves entity data localisation
        void SaveEntityDataLocalisation(EntityDataLocalisation localisation);

        // clears data localisation
        void ClearDataLocalisation(EntityTypeEnum entityType, long entityId, IEnumerable<LanguageEnum> exceptLanguage = null);

        #endregion

    }

}
