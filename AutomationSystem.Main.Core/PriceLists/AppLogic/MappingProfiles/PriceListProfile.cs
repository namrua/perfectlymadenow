using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.PriceLists.System;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Routines;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.PriceLists.AppLogic.MappingProfiles
{
    /// <summary>
    /// Maps PriceList related objects
    /// </summary>
    public class PriceListProfile : Profile
    {
        private readonly IPriceListTypeResolver priceListTypeResolver;

        public PriceListProfile(IPriceListTypeResolver priceListTypeResolver)
        {
            this.priceListTypeResolver = priceListTypeResolver;

            CreateMap<PriceListForm, PriceList>()
                .ForMember(dest => dest.PriceListItems, opt => opt.MapFrom(src => GetPriceListItemsFromPriceListForm(src)));
            CreateMap<PriceListItem, PriceListItemDetail>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.RegistrationType, "RegistrationType", "PriceListItem"))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RegistrationType.Description));
            CreateMap<PriceList, PriceListDetail>()
                .BeforeMap((src, dest) =>
                    {
                        EntityHelper.CheckForNull(src.PriceListItems, "PriceListItems", "PriceList");
                        EntityHelper.CheckForNull(src.PriceListType, "PriceListType", "PriceList");
                        EntityHelper.CheckForNull(src.Currency, "Currency", "PriceList");
                    })
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => MainTextHelper.GetCurrencyFullName(src.Currency.Description, src.Currency.Name)))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency.Name))
                .ForMember(dest => dest.PriceListType, opt => opt.MapFrom(src => src.PriceListType.Description))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => GetPriceListState(src.IsApprovded, src.IsDiscarded)))
                .ForMember(dest => dest.PriceListItems, opt => opt.MapFrom(src => src.PriceListItems));
            CreateMap<PriceList, PriceListListItem>()
                .BeforeMap((src, dest) =>
                    {
                        EntityHelper.CheckForNull(src.PriceListType, "PriceListType", "PriceList");
                        EntityHelper.CheckForNull(src.Currency, "Currency", "PriceList");
                    })
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency.Name))
                .ForMember(dest => dest.PriceListType, opt => opt.MapFrom(src => src.PriceListType.Description))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => GetPriceListState(src.IsApprovded, src.IsDiscarded)));
            CreateMap<PriceList, PriceListForm>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.PriceListItems, "PriceListItems", "PriceList"))
                .AfterMap(SetAllPricesForPriceListForm);
            CreateMap<PriceListForm, List<PriceListItemForValidation>>()
                .ForCtorParam("collection", opt => opt.MapFrom(src => GetItemsForValidation(src)));
        }

        #region private methods
        private PriceListState GetPriceListState(bool isApproved, bool isDiscarted)
        {
            var result = PriceListState.New;
            if (isApproved && !isDiscarted)
                result = PriceListState.Approved;
            if (isDiscarted)
                result = PriceListState.Discard;
            return result;
        }

        private List<PriceListItem> GetPriceListItemsFromPriceListForm(PriceListForm form)
        {
            var result = GetItemsFromPriceListForm(form, (registrationType, price) => new PriceListItem
            {
                PriceListId = form.PriceListId,
                RegistrationTypeId = registrationType,
                Price = price ?? 0
            });
            return result;
        }

        private List<PriceListItemForValidation> GetItemsForValidation(PriceListForm form)
        {           
            var result = GetItemsFromPriceListForm(form, (registrationType, price) => new PriceListItemForValidation
            {
                Price = price
            });
            return result;
        }

        private List<T> GetItemsFromPriceListForm<T>(PriceListForm form, Func<RegistrationTypeEnum, decimal?, T> itemCreator)
        {
            var types = priceListTypeResolver.GetRegistrationTypesForPriceList(form.PriceListTypeId);
            var list = new List<T>();
            foreach(var registrationType in types)
            {
                var price = GetPriceFromPriceListForm(registrationType, form);
                var item = itemCreator(registrationType, price);
                list.Add(item);
            }
            return list;
        }

        private decimal? GetPriceFromPriceListForm(RegistrationTypeEnum registrationType, PriceListForm form)
        {
            switch (registrationType)
            {
                case RegistrationTypeEnum.NewAdult:
                    return form.NewAdult;
                case RegistrationTypeEnum.NewAdultWeekOfClass:
                    return form.NewAdultWeekOfClass;
                case RegistrationTypeEnum.NewChild:
                    return form.NewChild;
                case RegistrationTypeEnum.ReviewAdult:
                    return form.ReviewAdult;
                case RegistrationTypeEnum.ReviewChild:
                    return form.ReviewChild;
                case RegistrationTypeEnum.WWA:
                    return form.WWA;
                case RegistrationTypeEnum.LectureRegistration:
                    return form.LectureRegistration;
                case RegistrationTypeEnum.MaterialRegistration:
                    return form.MaterialRegistration;
                default: throw new ArgumentOutOfRangeException($"There is no Registration type enum {registrationType}.");
            }
        }

        private void SetAllPricesForPriceListForm(PriceList priceList, PriceListForm form)
        {
            foreach(var priceListItem in priceList.PriceListItems)
            {
                SetPriceForPriceListForm(priceListItem.RegistrationTypeId, form, priceListItem.Price);
            }
        }

        private void SetPriceForPriceListForm(RegistrationTypeEnum registrationType, PriceListForm form, decimal price)
        {
            switch (registrationType)
            {
                case RegistrationTypeEnum.NewAdult:
                    form.NewAdult = price;
                    break;
                case RegistrationTypeEnum.NewAdultWeekOfClass:
                    form.NewAdultWeekOfClass = price;
                    break;
                case RegistrationTypeEnum.NewChild:
                    form.NewChild = price;
                    break;
                case RegistrationTypeEnum.ReviewAdult:
                    form.ReviewAdult = price;
                    break;
                case RegistrationTypeEnum.ReviewChild:
                    form.ReviewChild = price;
                    break;
                case RegistrationTypeEnum.WWA:
                    form.WWA = price;
                    break;
                case RegistrationTypeEnum.LectureRegistration:
                    form.LectureRegistration = price;
                    break;
                case RegistrationTypeEnum.MaterialRegistration:
                    form.MaterialRegistration = price;
                    break;
                default: throw new ArgumentOutOfRangeException($"There is no Registration type enum {registrationType}.");
            }
        }
        #endregion
    }
}
