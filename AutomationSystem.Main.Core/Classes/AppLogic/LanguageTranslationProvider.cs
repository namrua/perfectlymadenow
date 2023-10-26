using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using CorabeuControl.Components;
using PerfectlyMadeInc.Helpers.Contract.Routines;
using PerfectlyMadeInc.Helpers.Routines;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class LanguageTranslationProvider : ILanguageTranslationProvider
    {
        private readonly IEnumDatabaseLayer enumDb;
        private readonly ILocalisationService localisationService;

        private readonly IRelationCoder<LanguageEnum, LanguageEnum?> translationRelationCoder;

        public LanguageTranslationProvider(
            IEnumDatabaseLayer enumDb,
            ILocalisationService localisationService)
        {
            this.enumDb = enumDb;
            this.localisationService = localisationService;

            translationRelationCoder = new RelationCoder<LanguageEnum, LanguageEnum?>(1000,
                l => (LanguageEnum)l, f => (long)f,
                l => l == 0 ? null : (LanguageEnum?)l, s => s.HasValue ? (long)s : 0);
        }

        public long GetTranslationCode(LanguageEnum originalLanguageId, LanguageEnum? translationLanguageId)
        {
            return translationRelationCoder.GetCode(originalLanguageId, translationLanguageId);
        }

        public LanguageEnum GetOriginalLanguageId(long code)
        {
            return translationRelationCoder.GetFirst(code);
        }

        public LanguageEnum? GetTranslationLanguageId(long code)
        {
            return translationRelationCoder.GetSecond(code);
        }

        public List<DropDownItem> GetTranslationOptions()
        {
            // gets default language and rest of supported languages
            var defaultId = LocalisationInfo.DefaultLanguage;
            var defLang = enumDb.GetItemById(EnumTypeEnum.Language, (int)defaultId);
            var languagesWithoutDefault = localisationService.GetSupportedLanguages(true);

            // adds default only
            var result = new List<DropDownItem>();
            result.Add(DropDownItem.Item(translationRelationCoder.GetCode(defaultId, null), MainTextHelper.GetTranslation(defLang.Description, null)));

            // adds defaut to XXX
            result.AddRange(languagesWithoutDefault.Select(x =>
                DropDownItem.Item(translationRelationCoder.GetCode(defaultId, (LanguageEnum)x.Id), MainTextHelper.GetTranslation(defLang.Description, x.Description))));

            // adds XXX to default
            result.AddRange(languagesWithoutDefault.Select(x =>
                DropDownItem.Item(translationRelationCoder.GetCode((LanguageEnum)x.Id, defaultId), MainTextHelper.GetTranslation(x.Description, defLang.Description))));

            return result;
        }
    }
}
