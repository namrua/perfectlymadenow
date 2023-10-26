using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.PriceLists.System;
using AutomationSystem.Shared.Contract.Enums.Data;

namespace AutomationSystem.Main.Core.PriceLists.AppLogic
{
    public class PriceListFactory : IPriceListFactory
    {
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IPriceListTypeResolver priceListTypeResolver;

        public PriceListFactory(IEnumDatabaseLayer enumDb, IPriceListTypeResolver priceListTypeResolver)
        {
            this.enumDb = enumDb;
            this.priceListTypeResolver = priceListTypeResolver;
        }

        public PriceListForEdit InitializePriceListForEdit(PriceListTypeEnum priceListTypeId)
        {
            var registrationTypes = priceListTypeResolver.GetRegistrationTypesForPriceList(priceListTypeId);
            var result = new PriceListForEdit
            {
                RegistrationTypeDescriptions = enumDb.GetItemsByFilter(EnumTypeEnum.MainRegistrationType)
                    .Where(x => registrationTypes.Contains((RegistrationTypeEnum)x.Id))
                    .ToDictionary(x => (RegistrationTypeEnum)x.Id, y => y.Description),
                Currencies = enumDb.GetItemsByFilter(EnumTypeEnum.Currency)
            };

            return result;
        }
    }
}
