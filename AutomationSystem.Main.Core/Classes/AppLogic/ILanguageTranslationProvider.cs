using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.Components;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public interface ILanguageTranslationProvider
    {
        List<DropDownItem> GetTranslationOptions();

        long GetTranslationCode(LanguageEnum originalLanguageId, LanguageEnum? translationLanguageId);

        LanguageEnum GetOriginalLanguageId(long code);

        LanguageEnum? GetTranslationLanguageId(long code);
    }
}
