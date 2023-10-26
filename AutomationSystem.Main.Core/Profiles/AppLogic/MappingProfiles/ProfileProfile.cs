using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Main.Core.Profiles.AppLogic.MappingProfiles
{
    /// <summary>
    /// Maps Profile related objects
    /// </summary>
    public class ProfileProfile : AutoMapper.Profile
    {
        public ProfileProfile()
        {
            CreateMap<ProfileForm, Profile>();
            CreateMap<Profile, ProfileDetail>();
            CreateMap<Profile, ProfileForm>()
                .ForMember(dest => dest.OriginMoniker, opt => opt.MapFrom(src => src.Moniker));
            CreateMap<Profile, ProfileListItem>();
            CreateMap<Profile, RegistrationPageStyle>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.ClassPreference, "ClassPreference", "Profile"))
                .ForMember(dest => dest.ColorSchemeId, opt => opt.MapFrom(src => src.ClassPreference.RegistrationColorSchemeId))
                .ForMember(dest => dest.HeaderPictureId, opt => opt.MapFrom(src => src.ClassPreference.HeaderPictureId))
                .ForMember(dest => dest.HomepageUrl, opt => opt.MapFrom(src =>  src.ClassPreference.HomepageUrl ?? string.Format(ProfileConstants.ProfileHomepage, src.Moniker)));
        }
    }
}
