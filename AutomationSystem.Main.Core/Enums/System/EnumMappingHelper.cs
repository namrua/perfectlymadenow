using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.Enums.System
{
    public class EnumMappingHelper : IEnumMappingHelper
    {

        private readonly Lazy<Dictionary<string, CountryEnum>> countryMap;

        public EnumMappingHelper(IEnumDatabaseLayer enumDb)
        {
            countryMap = new Lazy<Dictionary<string, CountryEnum>>(() => {
                var result = enumDb.GetItemsByFilter(EnumTypeEnum.Country).ToDictionary(x => x.Description.ToLower(), y => (CountryEnum)y.Id);

                // use lowercase
                result["united states"] = CountryEnum.US;
                result["us"] = CountryEnum.US;
                result["uk"] = CountryEnum.GB;
                result["korea"] = CountryEnum.KR;
                return result;
            });
        }

        public CountryEnum? TypeMapCountry(string country)
        {
            if(countryMap.Value.TryGetValue(country.Trim().ToLower(), out var result))
            {
                return result;
            }

            return null;
        }
    }
}
