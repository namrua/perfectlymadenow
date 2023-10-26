using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.System.Models
{
    /// <summary>
    /// Encapsulates info about localisation
    /// </summary>
    public class LocalisationInfo
    {

        // constants
        public const string DefaultLanguageCode = "en";
        public const LanguageEnum DefaultLanguage = LanguageEnum.En;
        public const string DefaultCurrencyCode = "USD";
        public const CurrencyEnum DefaultCurrency = CurrencyEnum.USD;
        public const CountryEnum DefaultCountry = CountryEnum.US;

    }

}
