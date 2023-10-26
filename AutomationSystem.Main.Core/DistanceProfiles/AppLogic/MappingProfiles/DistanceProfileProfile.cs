using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Routines;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic.MappingProfiles
{
    public class DistanceProfileProfile : Profile
    {
        public DistanceProfileProfile()
        {
            CreateMap<DistanceProfile, DistanceProfileListItem>()
                .BeforeMap((src, dest) => 
                    {
                        EntityHelper.CheckForNull(src.Profile, "Profile", "DistanceProfile");
                        EntityHelper.CheckForNull(src.PriceList, "PriceList", "DistanceProfile");
                        EntityHelper.CheckForNull(src.DistanceCoordinator, "DistanceCoordinator", "DistanceProfile");
                        EntityHelper.CheckForNull(src.DistanceCoordinator.Address, "DistanceCoordinator.Address", "DistanceProfile");
                    })
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile.Name))
                .ForMember(dest => dest.PriceList, opt => opt.MapFrom(src => src.PriceList.Name))
                .ForMember(dest => dest.DistanceCoordinator, opt => opt.MapFrom(src => MainTextHelper.GetFullName(src.DistanceCoordinator.Address.FirstName, src.DistanceCoordinator.Address.LastName)));
            CreateMap<DistanceProfile, DistanceProfileDetail>()
                .BeforeMap((src, dest) =>
                {
                    EntityHelper.CheckForNull(src.Profile, "Profile", "DistanceProfile");
                    EntityHelper.CheckForNull(src.PriceList, "PriceList", "DistanceProfile");
                    EntityHelper.CheckForNull(src.DistanceCoordinator, "DistanceCoordinator", "DistanceProfile");
                    EntityHelper.CheckForNull(src.DistanceCoordinator.Address, "DistanceCoordinator.Address", "DistanceProfile");
                })
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile.Name))
                .ForMember(dest => dest.PriceList, opt => opt.MapFrom(src => src.PriceList.Name))
                .ForMember(dest => dest.DistanceCoordinator, opt => opt.MapFrom(src => MainTextHelper.GetFullName(src.DistanceCoordinator.Address.FirstName, src.DistanceCoordinator.Address.LastName)));
            CreateMap<DistanceProfileForm, DistanceProfile>();
            CreateMap<DistanceProfile, DistanceProfileForm>()
                .ForMember(dest => dest.CurrentPayPalKeyId, opt => opt.MapFrom(src => src.PayPalKeyId))
                .ForMember(dest => dest.CurrentPriceListId, opt => opt.MapFrom(src => src.PriceListId));
        }
    }
}
