using System.Linq;
using CorabeuControl.Components;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Contract.Payment.Data.Model;
using AutomationSystem.Main.Core.PriceLists.Data;

namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic
{
    public class DistanceProfileFactory : IDistanceProfileFactory
    {
        private readonly IPaymentDatabaseLayer paymentDb;
        private readonly IPriceListDatabaseLayer priceListDb;
        private readonly IPersonDatabaseLayer personDb;

        public DistanceProfileFactory(
            IPaymentDatabaseLayer paymentDb,
            IPriceListDatabaseLayer priceListDb,
            IPersonDatabaseLayer personDb)
        {
            this.paymentDb = paymentDb;
            this.priceListDb = priceListDb;
            this.personDb = personDb;
        }

        public DistanceProfileForEdit CreateDistanceProfileForEdit(long profileId, long? currentPriceListId = null, long? currentPayPalKeyId = null)
        {
            var payPalFilter = new PayPalKeyFilter
            {
                CurrencyId = CurrencyEnum.USD,
                UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                UserGroupId = profileId,
                IsActive = true
            };
            var payPalKeys = paymentDb.GetActivePayPalKeys(currentPayPalKeyId, payPalFilter);

            var priceLists = priceListDb.GetActivePriceLists(currentPriceListId, PriceListTypeEnum.WwaClass, CurrencyEnum.USD);
            var persons = personDb.GetMinimizedPersonsByProfileId(profileId);
            
            var result = new DistanceProfileForEdit
            {
                PriceLists = priceLists.Select(x => DropDownItem.Item(x.PriceListId, x.Name)).ToList(),
                PayPalKeys = payPalKeys.Select(x => DropDownItem.Item(x.PayPalKeyId, x.Name)).ToList(),
                Persons = new PersonHelper(persons),
            };

            return result;
        }
    }
}
