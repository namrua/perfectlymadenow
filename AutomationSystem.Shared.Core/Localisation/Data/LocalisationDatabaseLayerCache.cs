using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Core.Localisation.Data.Extensions;
using AutomationSystem.Shared.Core.Localisation.Data.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Contract.Database;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Shared.Core.Localisation.Data
{

    // todo: unmock FEEDERS!!!

    /// <summary>
    /// Cached localisation database layer
    /// </summary>
    public class LocalisationDatabaseLayerCache : ILocalisationDatabaseLayer, ICacheFeeder<AppLocalisation>, ICacheFeeder<EnumLocalisation>
    {

        // private components
        private readonly ILocalisationDatabaseLayer localisationDb;       
        private readonly IDataCache<string, AppLocalisation> appLocalisationCache;
        private readonly IDataCache<string, EnumLocalisation> enumLocalisationCache;

        // constructor
        public LocalisationDatabaseLayerCache(ILocalisationDatabaseLayer localisationDb)
        {
            this.localisationDb = localisationDb;           
            appLocalisationCache = new DataCache<string, AppLocalisation>(this, GetAppLocalisationKey);
            enumLocalisationCache = new DataCache<string, EnumLocalisation>(this, GetEnumLocalisationKey);
        }


        //#region appLocalisation

        // gets application localisation by module, label and language code
        public AppLocalisation GetAppLocalisationByKeyCached(string module, string label, string languageCode)
        {
            var result = appLocalisationCache.GetItemById(GetAppLocalisationKey(module, label, languageCode));
            return result;
        }

        // gets application localisation by id
        public AppLocalisation GetAppLocalisationById(long appLocalisationId, bool includeLanguage = false)
        {
            return localisationDb.GetAppLocalisationById(appLocalisationId, includeLanguage);
        }

        // gets application localisation by module, label and language code
        public AppLocalisation GetAppLocalisationByKey(string module, string label, LanguageEnum languageId,
            bool includeLanguage = false)
        {
            return localisationDb.GetAppLocalisationByKey(module, label, languageId, includeLanguage);
        }

        // get application localisation values
        public List<AppLocalisation> GetAppLocalisations(AppLocalisationFilter filter, bool includeLanguage = false)
        {
            return localisationDb.GetAppLocalisations(filter, includeLanguage);
        }

        // inserts or updates application localisation, returns id
        public long InsertUpdateAppLocalisation(long originId, LanguageEnum languageId, string value)
        {
            var result = localisationDb.InsertUpdateAppLocalisation(originId, languageId, value);
            appLocalisationCache.SetAsExpired();
            return result;
        }

        // inserts or updates application localisation, returns id
        public long InsertUpdateAppLocalisation(AppLocalisation entity)
        {
            var result = localisationDb.InsertUpdateAppLocalisation(entity);
            appLocalisationCache.SetAsExpired();
            return result;
        }

        //#endregion


        //#region enum localisation


        // get enum localisation by key (language code) [Cached]
        public EnumLocalisation GetEnumLocalisationByKeyCached(string languageCode, EnumTypeEnum enumTypeId, int itemId)
        {
            var result = enumLocalisationCache.GetItemById(GetEnumLocalisationKey(languageCode, enumTypeId, itemId));
            return result;
        }

        // gets all enum localisation by language and type [Cached]
        public List<EnumLocalisation> GetEnumLocalisationsByFilterCached(EnumLocalisationFilter filter)
        {
            var result = enumLocalisationCache.GetListByQuery(x => x.Filter(filter));
            return result;
        }

        // get enum localisation by key
        public EnumLocalisation GetEnumLocalisationByKey(LanguageEnum languageId, EnumTypeEnum enumTypeId, int itemId,
            bool includeLanguage = false, bool includeEnumType = false)
        {
            return localisationDb.GetEnumLocalisationByKey(languageId, enumTypeId, itemId, includeLanguage, includeEnumType);
        }

        // gets all enum localisation by language and type
        public List<EnumLocalisation> GetEnumLocalisationsByFilter(EnumLocalisationFilter filter,
            bool includeLanguage = false, bool includeEnumType = false)
        {
            return localisationDb.GetEnumLocalisationsByFilter(filter, includeLanguage, includeEnumType);
        }

        // inserts or updates enum localisation
        public long InsertUpdateEnumLocalisation(EnumLocalisation enumLocalisation)
        {
            var result = localisationDb.InsertUpdateEnumLocalisation(enumLocalisation);
            enumLocalisationCache.SetAsExpired();
            return result;
        }

        //#endregion


        #region data localisation

        // gets data localisation values
        public List<DataLocalisation> GetDataLocalisationByFilter(DataLocalisationFilter filter)
        {
            return localisationDb.GetDataLocalisationByFilter(filter);
        }

        // saves data localisation, returns id
        public void SaveDataLocalisations(DataLocalisationFilter filter, List<DataLocalisation> dataLocalisations)
        {
            localisationDb.SaveDataLocalisations(filter, dataLocalisations);
        }

        // delete data localisation
        public void DeleteDataLocalisations(EntityTypeEnum entityType, long entityId,
            IEnumerable<LanguageEnum> exceptLanguage = null)
        {
            localisationDb.DeleteDataLocalisations(entityType, entityId, exceptLanguage);
        }

        #endregion


        #region feeders             

        // feeder for app localisation cache
        List<AppLocalisation> ICacheFeeder<AppLocalisation>.GetData()
        {
            return localisationDb.GetAppLocalisations(null, true);          
        }

        // feeder for enum localisation cache
        List<EnumLocalisation> ICacheFeeder<EnumLocalisation>.GetData()
        {
            return localisationDb.GetEnumLocalisationsByFilter(null, true);            
        }

        #endregion


        #region private fields

        // gets app localisation key
        private string GetAppLocalisationKey(AppLocalisation localisation)
        {
            return GetAppLocalisationKey(localisation.Module, localisation.Label, localisation.Language.Name);
        }

        // gets app localisation key
        private string GetAppLocalisationKey(string modul, string label, string codeLanguage)
        {
            return $"{modul}#{label}#{codeLanguage}";
        }


        // gets app localisation key
        private string GetEnumLocalisationKey(EnumLocalisation localisation)
        {
            return GetEnumLocalisationKey(localisation.Language.Name, localisation.EnumTypeId, localisation.ItemId);
        }

        // gets app localisation key
        private string GetEnumLocalisationKey(string codeLanguage, EnumTypeEnum enumTypeId, int itemId)
        {
            return $"{codeLanguage}#{enumTypeId}#{itemId}";
        }

        #endregion

    }

}
