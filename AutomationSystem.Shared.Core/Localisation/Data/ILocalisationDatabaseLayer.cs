using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Core.Localisation.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Localisation.Data
{
    /// <summary>
    /// The facade interface for localisation database layer
    /// </summary>
    public interface ILocalisationDatabaseLayer
    {       


        // gets application localisation by module, label and language code [cached]
        AppLocalisation GetAppLocalisationByKeyCached(string module, string label, string languageCode);

        // gets application localisation by id
        AppLocalisation GetAppLocalisationById(long appLocalisationId, bool includeLanguage = false);

        // gets application localisation by module, label and language code
        AppLocalisation GetAppLocalisationByKey(string module, string label, LanguageEnum languageId, bool includeLanguage = false);


        // get application localisation values
        List<AppLocalisation> GetAppLocalisations(AppLocalisationFilter filter, bool includeLanguage = false);

        // inserts or updates application localisation, returns id
        long InsertUpdateAppLocalisation(long originId, LanguageEnum languageId, string value);

        // inserts or updates application localisation, returns id
        long InsertUpdateAppLocalisation(AppLocalisation entity);



        // get enum localisation by key (language code) [Cached]
        EnumLocalisation GetEnumLocalisationByKeyCached(string languageCode, EnumTypeEnum enumTypeId, int itemId);

        // gets all enum localisation by language and type [Cached]
        List<EnumLocalisation> GetEnumLocalisationsByFilterCached(EnumLocalisationFilter filter);

        // get enum localisation by key
        EnumLocalisation GetEnumLocalisationByKey(LanguageEnum languageId, EnumTypeEnum enumTypeId, int itemId, bool includeLanguage = false, bool includeEnumType = false);

        // gets all enum localisation by language and type
        List<EnumLocalisation> GetEnumLocalisationsByFilter(EnumLocalisationFilter filter, bool includeLanguage = false, bool includeEnumType = false);

        // inserts or updates enum localisation
        long InsertUpdateEnumLocalisation(EnumLocalisation enumLocalisation);



        // gets data localisation values
        List<DataLocalisation> GetDataLocalisationByFilter(DataLocalisationFilter filter);

        // saves data localisation, returns id
        void SaveDataLocalisations(DataLocalisationFilter filter, List<DataLocalisation> dataLocalisations);

        // delete data localisation
        void DeleteDataLocalisations(EntityTypeEnum entityType, long entityId, IEnumerable<LanguageEnum> exceptLanguage = null);


    }
   
}
