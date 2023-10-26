using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using AutomationSystem.Shared.Contract.Localisation.AppLogic.Models;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Core.HtmlValidation.System;
using AutomationSystem.Shared.Core.Localisation.Data;
using AutomationSystem.Shared.Core.Localisation.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Localisation.AppLogic
{
    /// <summary>
    /// Provides logic for localisation administration
    /// </summary>
    public class LocalisationAdministration : ILocalisationAdministration
    {

        private readonly ILocalisationDatabaseLayer localisationDb;
        private readonly ILocalisationService localisationService;       
        private readonly IEnumDatabaseLayer enumDb;

        private readonly IHtmlValidatorFactory htmlValidatorFactory;

        // constructor
        public LocalisationAdministration(ILocalisationDatabaseLayer localisationDb, IEnumDatabaseLayer enumDb,
            ILocalisationService localisationService)
        {
            this.localisationDb = localisationDb;            
            this.enumDb = enumDb;
            this.localisationService = localisationService;

            htmlValidatorFactory = new HtmlValidatorFactory();
        }


        #region app localisation

        // gets language localisation summary
        public LanguageLocalisationSummary GetLanguageLocalisationSummary()
        {
           
            var languages = localisationService.GetAllLanguages();
            var appLocalisations = localisationDb.GetAppLocalisations(new AppLocalisationFilter(), includeLanguage: true);

            // creates dictionary of language summary items indexed by languageId
            var langSummaryDictionary = appLocalisations.GroupBy(x => x.LanguageId)
                .ToDictionary(x => x.Key, GetLanguageLocalisationSummaryItem);


            // creates language localisation summary
            var result = new LanguageLocalisationSummary()
            {
                FullyLocalisedCount = langSummaryDictionary[LocalisationInfo.DefaultLanguage].LocalisedCount,
                Items = langSummaryDictionary.Values.ToList(),
            };

            // adds misssing languages
            foreach (var missingLanguage in languages.Where(x => !langSummaryDictionary.ContainsKey((LanguageEnum)x.Id)))
            {
                result.Items.Add(new LanguageLocalisationSummaryItem
                {
                    LanguageId = (LanguageEnum)missingLanguage.Id,
                    LanguageCode = missingLanguage.Name,
                    Language = missingLanguage.Description
                });
            }

            // fill whether localisation is complete for each language and return result
            foreach (var item in result.Items)
                item.IsComplete = item.LocalisedCount < result.FullyLocalisedCount;
            return result;
        }
      

        public AppLocalisationList GetAppLocalisationList(LanguageEnum languageId)
        {
            // tries to load the language      
            var language = localisationService.TryGetLanguage(languageId);
            if (language == null)
            {
                throw new ArgumentException($"There is no language with id {languageId}.");
            }

            // loads app localisations of language and creates dictionary mapped by module language pair            
            var appLocalisations = localisationDb.GetAppLocalisations(new AppLocalisationFilter(languageId: languageId));
            var mappedAppLocalisations = appLocalisations.ToDictionary(x => GetAppLocKey(x.Module, x.Label), y => y);

            // loads default language app localisations
            var originAppLocalisations = languageId == LocalisationInfo.DefaultLanguage
                ? appLocalisations
                : localisationDb.GetAppLocalisations(new AppLocalisationFilter(languageId: LocalisationInfo.DefaultLanguage));

            // creates result from defautl app localisations
            var result = new AppLocalisationList()
            {
                LanguageId = (LanguageEnum) language.Id,
                Language = language            
            };          

            // creates list items
            var index = 0;
            foreach (var origItem in originAppLocalisations.OrderBy(x => x.Module).ThenBy(x => x.Label))
            {
                // creates item form origin
                var item = new AppLocalisationListItem
                {
                    Module = origItem.Module,
                    Label = origItem.Label,
                    OriginAppLocalisationId = origItem.AppLocalisationId,
                    Index = index++,
                };

                // adds localised item data
                if (mappedAppLocalisations.TryGetValue(GetAppLocKey(origItem.Module, origItem.Label), out var langAppLoc))                                 
                    item.Value = langAppLoc.Value;
                
                result.Items.Add(item);
            }
            return result;
        }


        // gets app localisation for edit
        public AppLocalisationForEdit GetAppLocalisatonForEdit(long originId, LanguageEnum languageId)
        {                       
            // initializes result
            var result = InitializeAppLocalisationForEdit(languageId, originId, out var origin);
          
            // loads lang app localisation
            var item = (languageId == LocalisationInfo.DefaultLanguage)
                ? origin
                : localisationDb.GetAppLocalisationByKey(origin.Module, origin.Label, languageId);

            // assembles form
            result.Form = new AppLocalisationForm
            {
                LanguageId = (LanguageEnum) result.Language.Id,
                AppLocalisationOriginId = origin.AppLocalisationId,              
                Value = item?.Value ?? ""              
            };

            // validates form
            result.ValidationResult = ValidateAppLocalisation(result.Form);
            return result;
        }


        // gets app localisation for edit by form
        public AppLocalisationForEdit GetFormAppLocalisationForEdit(AppLocalisationForm form, AppLocalisationValidationResult validationResult)
        {            
            var result = InitializeAppLocalisationForEdit(form.LanguageId, form.AppLocalisationOriginId, out var origin);
            result.Form = form;
            result.ValidationResult = validationResult;
            return result;
        }


        // validates app localisation
        public AppLocalisationValidationResult ValidateAppLocalisation(AppLocalisationForm form)
        {
            var result = new AppLocalisationValidationResult();
            if (string.IsNullOrEmpty(form.Value))
                result.ValidationMessages.Add("Html text is empty.");
            var htmlValidator = htmlValidatorFactory.GetAppLocalisationValidator();
            result.ValidationMessages.AddRange(htmlValidator.Validate(form.Value));
            result.IsValid = result.ValidationMessages.Count == 0;
            return result;
        }

        // saves app localisation
        public void SaveAppLocalisation(AppLocalisationForm form)
        {
            if (!localisationService.IsLanguage(form.LanguageId))
            {
                throw new ArgumentException($"There is no valid language with id {form.LanguageId}.");
            }

            localisationDb.InsertUpdateAppLocalisation(form.AppLocalisationOriginId, form.LanguageId, form.Value);
        }

        #endregion


        #region enum localisation

        // gets enum type list
        public EnumTypeList GetEnumTypeList(LanguageEnum languageId)
        {
            // check if language exists
            var language = localisationService.TryGetLanguage(languageId);
            if (language == null)
            {
                throw new ArgumentException($"There is no valid language with id {languageId}.");
            }

            // gets enum types
            var enumTypes = enumDb.GetEnumTypes();

            // loads enum types and assembles result
            var result = new EnumTypeList 
            {
                LanguageId = (LanguageEnum)language.Id,
                Language = language,
                Items = enumTypes.Select(x => new EnumTypeListItem {
                    EnumTypeId = x.EnumTypeId,
                    EnumTypeCode = x.Name,
                    EnumType = x.Description
                }).ToList()
            };
            return result;
        }

        // gets enum localisation list
        public EnumLocalisationList GetEnumLocalisationList(LanguageEnum languageId, EnumTypeEnum enumTypeId)
        {
            // check if language exists
            var language = localisationService.TryGetLanguage(languageId);
            if (language == null)
            {
                throw new ArgumentException($"There is no valid language with id {languageId}.");
            }

            // loads enumtype entity and check if it extists
            var enumType = enumDb.GetEnumTypeById(enumTypeId);
            if (enumType == null)
                throw new ArgumentException($"There is no Enum type with id {enumTypeId}.");

            // loads localised and origin enum items
            var enumFilter = new EnumLocalisationFilter(languageId, enumTypeId);
            var localisedItemsMap = localisationDb.GetEnumLocalisationsByFilter(enumFilter).ToDictionary(x => x.ItemId, y => y);          
            var originEnumItems = enumDb.GetItemsByFilter(enumTypeId);

            // assembles result
            var result = new EnumLocalisationList
            {
                LanguageId = (LanguageEnum)language.Id,
                Language = language,
                EnumType = enumType
            };           

            // creates items
            foreach (var enumItem in originEnumItems.OrderBy(x => x.Id))
            {
                // creates item from origin enum
                var item = new EnumLocalisationListItem
                {
                    ItemId = enumItem.Id,
                    Name = enumItem.Name,
                    Description = enumItem.Description
                };

                // adds localisation
                if (localisedItemsMap.TryGetValue(enumItem.Id, out var localisedItem))
                {
                    if (!string.IsNullOrEmpty(localisedItem.Name))
                    {
                        item.Name = localisedItem.Name;
                        item.IsNameLocalised = true;
                    }
                    if (!string.IsNullOrEmpty(localisedItem.Description))
                    {
                        item.Description = localisedItem.Description;
                        item.IsDescriptionLocalised = true;
                    }
                }
                result.Items.Add(item);
            }

            return result;
        }


        // gets enum localisation for edit
        public EnumLocalisationForEdit GetEnumLocalisationForEdit(LanguageEnum languageId, EnumTypeEnum enumTypeId, int itemId)
        {           
            // initializes EnumLocalisationForEdit
            var result = InitializeEnumLocalisationForEdit(languageId, enumTypeId, itemId);

            // initializes form
            result.Form = new EnumLocalisationForm
            {
                LanguageId = languageId,
                EnumTypeId = enumTypeId,
                ItemId = itemId,                
            };

            // add localised strings
            var enumLocalisation = localisationDb.GetEnumLocalisationByKey(languageId, enumTypeId, itemId);                       
            if (enumLocalisation != null)
            {
                result.Form.Name = enumLocalisation.Name;
                result.Form.Description = enumLocalisation.Description;
            }
            return result;
        }


        // gets enum localisation for edit
        public EnumLocalisationForEdit GetFormEnumLocalisationForEdit(EnumLocalisationForm form)
        {
            // initializes EnumLocalisationForEdit
            var result = InitializeEnumLocalisationForEdit(form.LanguageId, form.EnumTypeId, form.ItemId);
            result.Form = form;
            return result;
        }

        // saves enum localisation
        public void SaveEnumLocalisation(EnumLocalisationForm form)
        {
            // validates input 
            if (!localisationService.IsLanguage(form.LanguageId))
            {
                throw new ArgumentException($"There is no valid language with id {form.LanguageId}.");
            }
            if (enumDb.GetEnumTypeById(form.EnumTypeId) == null)
            {
                throw new ArgumentException($"There is no Enum type with id {form.EnumTypeId}.");
            }
            if (enumDb.GetItemById(form.EnumTypeId, form.ItemId) == null)
            {
                throw new ArgumentException($"There is no enum item of type {form.EnumTypeId} with id {form.ItemId}.");
            }

            // saves new values
            var enumLocalisation = new EnumLocalisation
            {
                LanguageId = form.LanguageId,
                EnumTypeId = form.EnumTypeId,
                ItemId = form.ItemId,
                Name = TrimAndMakeEmptyNull(form.Name),
                Description = TrimAndMakeEmptyNull(form.Description)
            };
            localisationDb.InsertUpdateEnumLocalisation(enumLocalisation);
        }

        #endregion



        #region private methods

        // creates LocalisationSummaryItem from collection of app localisations
        private LanguageLocalisationSummaryItem GetLanguageLocalisationSummaryItem(IEnumerable<AppLocalisation> appLocalisations)
        {
            var items = appLocalisations.First();
            var result = new LanguageLocalisationSummaryItem
            {
                LanguageId = items.LanguageId,
                LanguageCode = items.Language.Name,
                Language = items.Language.Description,
                LocalisedCount = appLocalisations.Count(z => !string.IsNullOrEmpty(z.Value))
            };
            return result;
        }


        // initializes AppLocalisationForEdit
        private AppLocalisationForEdit InitializeAppLocalisationForEdit(LanguageEnum languageId, long originId, out AppLocalisation origin)
        {
            // check if language exists
            var language = localisationService.TryGetLanguage(languageId);
            if (language == null)
            {
                throw new ArgumentException($"There is no valid language with id {languageId}.");
            }

            // loads origin app localisation
            origin = localisationDb.GetAppLocalisationById(originId);
            if (origin == null)
                throw new ArgumentException($"There is no origin Application localisation with id {originId}.");           

            // assembles result
            var result = new AppLocalisationForEdit
            {
                Language = language,
                Module = origin.Module,
                Label = origin.Label,
                OriginalValue = origin.Value,
                IsDefaultLanguage = languageId == LocalisationInfo.DefaultLanguage,
            };
            return result;
        }


        // initializes EnumLocalisationForEdit 
        private EnumLocalisationForEdit InitializeEnumLocalisationForEdit(LanguageEnum languageId, EnumTypeEnum enumTypeId, int itemId)
        {
            // check if language exists
            var language = localisationService.TryGetLanguage(languageId);
            if (language == null)
            {
                throw new ArgumentException($"There is no valid language with id {languageId}.");
            }

            // loads enumtype entity and check if it extists
            var enumType = enumDb.GetEnumTypeById(enumTypeId);
            if (enumType == null)
                throw new ArgumentException($"There is no Enum type with id {enumTypeId}.");

            // loads origin enum item and check it if exists
            var originItem = enumDb.GetItemsByFilter(enumTypeId, new EnumItemFilter(id: itemId)).FirstOrDefault();
            if (originItem == null)
                throw new ArgumentException($"There si no enum item of type {enumTypeId} with id {itemId}.");


            // initializes EnumLocalisationForEdit
            var result = new EnumLocalisationForEdit
            {
                EnumType = enumType,
                Language = language,               
                SystemName = originItem.Name,
                SystemDescription = originItem.Description
            };
            return result;
        }
        
        // trim and make empty string nullstring
        private string TrimAndMakeEmptyNull(string value)
        {
            if (value == null) return null;
            value = value.Trim();
            return string.IsNullOrEmpty(value) ? null : value;
        }


        // gets app loc key assembled from module and label
        private string GetAppLocKey(string module, string label)
        {
            return $"{module}.{label}";
        }

        #endregion

    }

}
