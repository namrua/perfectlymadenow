using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Routines;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.MappingProfiles
{
    public class RegistrationPaymentProfile : Profile
    {
        public RegistrationPaymentProfile()
        {
            CreateMap<ClassRegistrationPayment, RegistrationPaymentDetail>();
            CreateMap<ClassRegistrationPayment, RegistrationPaymentForm>();
            CreateMap<ClassRegistration, RegistrationPaymentForm>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.ClassRegistrationPayment, "ClassRegistrationPayment", "ClassRegistration"))
                .AfterMap((src, dest, context) => context.Mapper.Map(src.ClassRegistrationPayment, dest));
            CreateMap<RegistrationPaymentForm, ClassRegistrationPayment>();
            
            CreateMap<PayPalRecord, PayloadAddressDetail>()
                .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.SaRecipientName))
                .ForMember(dest => dest.Line1, opt => opt.MapFrom(src => src.SaLine1))
                .ForMember(dest => dest.Line2, opt => opt.MapFrom(src => src.SaLine2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.SaCity))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.SaState))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.SaPostalCode))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.SaCountryCode))
                .ForMember(dest => dest.FullCity, opt => opt.MapFrom(src => MainTextHelper.GetAddressCityState(src.SaCity, src.SaState, src.SaPostalCode)))
                .ForMember(dest => dest.FullStreet, opt => opt.MapFrom(src => MainTextHelper.GetAddressStreet(src.SaLine1, src.SaLine2)));
            CreateMap<PayPalRecord, PayPalRecordDetail>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => MainTextHelper.GetFullName(src.FirstName, src.LastName)));
        }
    }
}
