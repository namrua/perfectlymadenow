using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Mapping;
using PerfectlyMadeInc.Helpers.Routines;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.Classes.AppLogic.MappingProfiles
{
    /// <summary>
    /// Maps ClassPreference related objects
    /// </summary>
    public class ClassPreferenceProfile : Profile
    {
        public ClassPreferenceProfile(IClassDatabaseLayer classDb)
        {
            var classPreferenceTypeConvertor = new TypeConverterMapper<RegistrationColorScheme, RegistrationColorSchemeEnum, string>(
                classDb.GetRegistrationColorSchemes, x => x.RegistrationColorSchemeId, x => x.Description);

            CreateMap<RegistrationColorSchemeEnum, string>().ConvertUsing(classPreferenceTypeConvertor);
            CreateMap<ClassPreference, ClassPreferenceForm>()
                .ForMember(dest => dest.OriginHeaderPictureId, opt => opt.MapFrom(src => src.HeaderPictureId));
            CreateMap<ClassPreference, ClassPreferenceDetail>()
                .BeforeMap((src, dest) =>
                    {
                        EntityHelper.CheckForNull(src.ClassPreferenceExpenses, "ClassPreferenceExpenses", "ClassPreference");
                        EntityHelper.CheckForNull(src.LocationInfo, "LocationInfo", "ClassPreference", src.LocationInfoId.HasValue);
                        EntityHelper.CheckForNull(src.LocationInfo?.Address, "LocationInfo.Address", "ClassPreference", src.LocationInfoId.HasValue);
                        EntityHelper.CheckForNull(src.Currency, "Currency", "ClassPreference");
                    })
                .ForMember(dest => dest.RegistrationColorScheme, opt => opt.MapFrom(src => src.RegistrationColorSchemeId))
                .ForMember(dest => dest.Expenses, opt => opt.MapFrom(src => src.ClassPreferenceExpenses))
                .ForMember(dest => dest.LocationInfo, opt => opt.MapFrom(src => src.LocationInfoId.HasValue
                    ? MainTextHelper.GetFullName(src.LocationInfo.Address.FirstName, src.LocationInfo.Address.LastName)
                    : null))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => MainTextHelper.GetCurrencyFullName(src.Currency.Description, src.Currency.Name)))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency.Name));
            CreateMap<ClassPreferenceForm, ClassPreference>()
                .ForMember(dest => dest.HomepageUrl, opt => opt.MapFrom(src => MainTextHelper.GetExternalLink(src.HomepageUrl)))
                .ForMember(dest => dest.HeaderPictureId, opt => opt.MapFrom(src => src.OriginHeaderPictureId));
        }
    }
}
