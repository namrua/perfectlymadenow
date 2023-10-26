using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.Enums.System
{
    public interface IEnumMappingHelper
    {
        CountryEnum? TypeMapCountry(string country);
    }
}
