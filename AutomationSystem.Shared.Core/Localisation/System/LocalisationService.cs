using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Contract.Preferences.System;
using AutomationSystem.Shared.Core.Localisation.Data;
using AutomationSystem.Shared.Core.Localisation.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Localisation.System
{
    /// <summary>
    /// Localisation service
    /// </summary>
    public class LocalisationService : ILocalisationService
    {

        // private component
        private readonly ILocalisationDatabaseLayer localisationDb;
        private readonly ICorePreferenceProvider persistenceProvider;
        private readonly ILanguagePersistor languagePersistor;
        private readonly IEnumDatabaseLayer enumDb;        

        // constructor
        public LocalisationService(ILocalisationDatabaseLayer localisationDb, ICorePreferenceProvider persistenceProvider,
            ILanguagePersistor languagePersistor, IEnumDatabaseLayer enumDb)
        {
            this.localisationDb = localisationDb;
            this.persistenceProvider = persistenceProvider;
            this.languagePersistor = languagePersistor;
            this.enumDb = enumDb;
        }


        #region languages

        // gets current language code
        public string CurrentLanguageCode => languagePersistor.CurrentLanguageCode;

        // get currrent culture info
        public CultureInfo CurrentCultureInfo => languagePersistor.CurrentCultureInfo;

        // get current language enum or null if it is not supported
        public LanguageEnum? CurrentLanguageEnum
        {
            get
            {
                var code = languagePersistor.CurrentLanguageCode;
                var language = enumDb.GetItemsByFilter(EnumTypeEnum.Language, new EnumItemFilter(name: code)).FirstOrDefault();
                return language == null ? null : (LanguageEnum?)language.Id;
            }
        }


        // sets currrent language
        public void SetLanguage(string code)
        {
            var supportedLanguages = persistenceProvider.GetSupportedLanguages();
            if (!supportedLanguages.Contains(code)) return;
            languagePersistor.ChangeLanguage(code);
        }


        // tries to get language, returns null whether language is not supported
        public IEnumItem TryGetSupportedLanguage(LanguageEnum languageId)
        {
            var language = enumDb.GetItemById(EnumTypeEnum.Language, (int)languageId);
            var supportedLanguages = persistenceProvider.GetSupportedLanguages();
            return language != null && supportedLanguages.Contains(language.Name) ? language : null;
        }

        // tries to get a language by Id
        public IEnumItem TryGetLanguage(LanguageEnum languageId)
        {
            return enumDb.GetItemById(EnumTypeEnum.Language, (int)languageId);
        }


        // gets supported languages
        public List<IEnumItem> GetSupportedLanguages(bool omitDefault = false)
        {
            var supportedLanguages = persistenceProvider.GetSupportedLanguages();
            if (omitDefault)
                supportedLanguages.Remove(LocalisationInfo.DefaultLanguageCode);
            var result = enumDb.GetItemsByFilter(EnumTypeEnum.Language)
                .Where(x => supportedLanguages.Contains(x.Name)).ToList();            
            return result;
        }

        // gets all languages
        public List<IEnumItem> GetAllLanguages()
        {
            var result = enumDb.GetItemsByFilter(EnumTypeEnum.Language);
            return result;
        }

        // gets language info map
        public ILanguageInfoPovider GetLanguageInfoProvider()
        {
            var languages = enumDb.GetItemsByFilter(EnumTypeEnum.Language);
            var result = new LanguageInfoPovider(languages);
            return result;
        }      

        // checks whether language is supported
        public bool IsSupported(LanguageEnum languageId)
        {
            var language = TryGetSupportedLanguage(languageId);
            return language != null;
        }

        // checks whether language exists
        public bool IsLanguage(LanguageEnum languageId)
        {
            var language = TryGetLanguage(languageId);
            return language != null;
        }

        #endregion


        #region localised items      

        // gets localised app html string                       
        public HtmlString GetLocalisedHtmlString(string module, string label)
        {
            return new HtmlString(GetLocalisedString(module, label));
        }

        // gets localised string
        public string GetLocalisedString(string module, string label)
        {
            var currentLanguageCode = languagePersistor.CurrentLanguageCode;
            var result = GetLocalisedString(module, label, currentLanguageCode);
            return result;
        }

        // gets localised string specified
        public string GetLocalisedString(string module, string label, string languageCode)
        {            
            var appLogic = localisationDb.GetAppLocalisationByKeyCached(module, label, languageCode);
            //if ((string.IsNullOrEmpty(appLogic?.Value)) && languageCode != LocalisationInfo.DefaultLanguageCode)
            //    appLogic = localisationDb.GetAppLocalisationByKeyCached(module, label, LocalisationInfo.DefaultLanguageCode);
            //return appLogic == null ? "" : (appLogic.Value ?? "");
            var result = string.IsNullOrEmpty(appLogic?.Value) ? $"{module}.{label}" : appLogic.Value;
            return result;
        }


        // gets languages by ids
        public List<IEnumItem> GetLocalisedLanguagesByIds(params LanguageEnum?[] languages)
        {           
            var supportedLanguages = persistenceProvider.GetSupportedLanguages();
            var paramLanguages = new HashSet<int>(languages.Where(x => x.HasValue).Select(x => (int)x.Value));
            var items = enumDb.GetItemsByFilter(EnumTypeEnum.Language)
                    .Where(x => supportedLanguages.Contains(x.Name))
                    .Where(x => paramLanguages.Contains(x.Id)).ToList();                         
            var result = LocaliseEnumItems(EnumTypeEnum.Language, items, true);
            return result;
        }

        // gets localised enum item
        public IEnumItem GetLocalisedEnumItem(EnumTypeEnum enumTypeId, int itemId)
        {
            var currentLanguageCode = languagePersistor.CurrentLanguageCode;
            var result = GetLocalisedEnumItemSpecified(enumTypeId, itemId, currentLanguageCode);
            return result;
        }

        // gets localised enum item with specified langcode
        public IEnumItem GetLocalisedEnumItemSpecified(EnumTypeEnum enumTypeId, int itemId, string languageCode)
        {
            var origin = enumDb.GetItemsByFilter(enumTypeId, new EnumItemFilter(id: itemId)).FirstOrDefault();
            if (origin == null)
                throw new ArgumentException($"Unknow enum item of type {enumTypeId} with id {itemId}");
            var enumLocalisation = localisationDb.GetEnumLocalisationByKeyCached(languageCode, enumTypeId, itemId);
            var result = LocaliseEnumItem(origin, enumLocalisation);
            return result;
        }

        // gets localised enums by filter
        public List<IEnumItem> GetLocalisedEnumItemsByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter = null)
        {
            var items = enumDb.GetItemsByFilter(enumTypeId, filter);
            var result = LocaliseEnumItems(enumTypeId, items, filter?.Id == null);
            return result;
        }

        // gets enum map by filter
        public Dictionary<int, IEnumItem> GetLocalisedEnumItemMapByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter)
        {
            var items = enumDb.GetItemsByFilter(enumTypeId, filter);
            var result = LocaliseEnumItems(enumTypeId, items, filter?.Id == null);
            return result.ToDictionary(x => x.Id, y => y);
        }

        #endregion



        #region data localisation

        // gets entity data localisation    
        // WARNING: it is possible that result EntityDataLocalisation.LanguageId is null
        public EntityDataLocalisation GetEntityDataLocalisationById(EntityTypeEnum entityType, long entityId, LanguageEnum language)
        {
            var filter = new DataLocalisationFilter(language, entityType, entityId);            
            var dataLocalisations = localisationDb.GetDataLocalisationByFilter(filter);

            // creates entity data localisation
            var result = new EntityDataLocalisation(entityType, entityId, language);          
            foreach (var dataLocalisation in dataLocalisations)
            {
                result.LanguageId = result.LanguageId;
                result.AddColumnValue(dataLocalisation.Column, dataLocalisation.Value);
            }
            return result;
        }

        // gets entities data localisation by entity ids
        public List<EntityDataLocalisation> GetEntityDataLocalisationByIds(EntityTypeEnum entityType, IEnumerable<long> entityId)
        {
            var filter = new DataLocalisationFilter(null, entityType, entityId.ToArray());           
            var dataLocalisations = localisationDb.GetDataLocalisationByFilter(filter);

            // creates entity data localisation
            var result = new List<EntityDataLocalisation>();
            foreach (var entityGroup in dataLocalisations.GroupBy(x => new { x.EntityId, x.LanguageId }))
            {
                var entityResult = new EntityDataLocalisation(entityType, entityGroup.Key.EntityId, entityGroup.Key.LanguageId);               
                foreach (var dataLocalisation in entityGroup)
                {
                    entityResult.LanguageId = entityResult.LanguageId;
                    entityResult.AddColumnValue(dataLocalisation.Column, dataLocalisation.Value);
                }
                result.Add(entityResult);
            }
            return result;
        }

        // clears data localisation
        public void ClearDataLocalisation(EntityTypeEnum entityType, long entityId, IEnumerable<LanguageEnum> exceptLanguage = null)
        {
            localisationDb.DeleteDataLocalisations(entityType, entityId, exceptLanguage);
        }

        // saves entity data localisation
        public void SaveEntityDataLocalisation(EntityDataLocalisation localisation)
        {           
            var filter = new DataLocalisationFilter(localisation.LanguageId, localisation.EntityTypeId, localisation.EntityId);
            var toUpdateList = new List<DataLocalisation>();
            foreach (var pair in localisation.ColumnMap)
            {
                if (string.IsNullOrEmpty(pair.Value)) continue;
                var toUpdate = new DataLocalisation();
                toUpdate.LanguageId = localisation.LanguageId;
                toUpdate.EntityTypeId = localisation.EntityTypeId;
                toUpdate.EntityId = localisation.EntityId;
                toUpdate.Column = pair.Key;
                toUpdate.Value = pair.Value;
                toUpdateList.Add(toUpdate);
            }
            localisationDb.SaveDataLocalisations(filter, toUpdateList);
        }

        #endregion


        #region private fields

        // localises enum items
        private List<IEnumItem> LocaliseEnumItems(EnumTypeEnum enumTypeId, IEnumerable<IEnumItem> items, bool useIdsInFilter)
        {
            // loads localisation items
            var filter = new EnumLocalisationFilter
            {
                EnumTypeId = enumTypeId,
                LanguageCode = languagePersistor.CurrentLanguageCode,
                ItemIds = useIdsInFilter ? items.Select(x => x.Id) : null
            };
            var enumLocalisationMap = localisationDb.GetEnumLocalisationsByFilterCached(filter).ToDictionary(x => x.ItemId, y => y);

            // creates result
            var result = new List<IEnumItem>();
            foreach (var item in items)
            {
                enumLocalisationMap.TryGetValue(item.Id, out var localisation);
                result.Add(LocaliseEnumItem(item, localisation));
            }
            return result;
        }

        // localises one item, keeps origin (it is forbiden to revrite origin entities)
        private IEnumItem LocaliseEnumItem(IEnumItem origin, EnumLocalisation localisation)
        {
            if (localisation == null)
                return origin;
            var result = new EnumItem();
            result.Id = origin.Id;
            result.Name = string.IsNullOrEmpty(localisation.Name) ? origin.Name : localisation.Name;
            result.Description = string.IsNullOrEmpty(localisation.Description) ? origin.Description : localisation.Description;
            return result;
        }       

        #endregion

    }

}
