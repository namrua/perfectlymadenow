using AutomationSystem.Shared.Contract.Identities.AppLogic.Models;
using AutomationSystem.Shared.Model;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Shared.Core.Identities.AppLogic.MappingProfiles
{

    /// <summary>
    /// Maps UserProfile related objects
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserShortDetail>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.GoogleAccount));
        }
    }
}
