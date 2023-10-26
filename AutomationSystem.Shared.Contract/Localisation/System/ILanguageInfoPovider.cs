using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Shared.Contract.Localisation.System
{
    /// <summary>
    /// Provides language info
    /// </summary>
    public interface ILanguageInfoPovider
    {

        // get language info by language id
        LanguageInfo GetLanguageInfo(LanguageEnum languageId);

    }

}
