using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Shared.Core.Localisation.System
{
    /// <summary>
    /// Provides language info
    /// </summary>
    public class LanguageInfoPovider : ILanguageInfoPovider
    {

        // private fields
        private readonly Dictionary<LanguageEnum, LanguageInfo> languageMap; 

        // constructor
        public LanguageInfoPovider(IEnumerable<IEnumItem> languages)
        {
            languageMap = languages.ToDictionary(x => (LanguageEnum)x.Id, y => new LanguageInfo(y));
        }

        // get language info by language id
        public LanguageInfo GetLanguageInfo(LanguageEnum languageId)
        {
            if (!languageMap.TryGetValue(languageId, out var result))
                throw new ArgumentException($"Unknown language with id {languageId}.");
            return result;       
        }

    }

}
