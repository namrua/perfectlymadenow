using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Routines;
using System;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.MappingProfiles
{
    public class FormerClassProfile : Profile
    {
        public FormerClassProfile()
        {
            CreateMap<FormerClass, FormerClassDetail>()
                .BeforeMap((src, dest) =>
                {
                    EntityHelper.CheckForNull(src.ClassType, "ClassType", "FormerClass");
                    EntityHelper.CheckForNull(src.Profile, "Profile", "FormerClass", src.ProfileId.HasValue);
                })
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile == null ? null : src.Profile.Name))
                .ForMember(dest => dest.ClassType, opt => opt.MapFrom(src => src.ClassType.Description))
                .ForMember(dest => dest.ClassDate, opt => opt.MapFrom(src => MainTextHelper.GetEventDate(src.EventStart, src.EventEnd, null)))
                .ForMember(dest => dest.FullClassDate, opt => opt.MapFrom(
                    src => $"{MainTextHelper.GetEventDate(src.EventStart, src.EventEnd, null)}, {src.EventStart.Year}"))
                .ForMember(dest => dest.ClassTitle, opt => opt.MapFrom(
                    src => MainTextHelper.GetEventOneLineHeader(src.EventStart, src.EventEnd, src.Location, src.ClassType.Description, null)));

            CreateMap<FormerClass, FormerClassForm>();

            CreateMap<FormerClassForm, FormerClass>()
                .ForMember(dest => dest.ClassTypeId, opt => opt.MapFrom(src => src.ClassTypeId ?? 0))
                .ForMember(dest => dest.EventStart, opt => opt.MapFrom(src => src.EventStart ?? default(DateTime)))
                .ForMember(dest => dest.EventEnd, opt => opt.MapFrom(src => src.EventEnd ?? default(DateTime)));

            CreateMap<FormerClass, FormerClassListItem>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.ClassType, "ClassType", "FormerClass"))
                .ForMember(dest => dest.ClassType, opt => opt.MapFrom(src => src.ClassType.Description))
                .ForMember(dest => dest.ClassDate, opt => opt.MapFrom(src => MainTextHelper.GetEventDate(src.EventStart, src.EventEnd, null)))
                .ForMember(dest => dest.FullClassDate, opt => opt.MapFrom(
                    src => $"{MainTextHelper.GetEventDate(src.EventStart, src.EventEnd, null)}, {src.EventStart.Year}"))
                .ForMember(dest => dest.ClassTitle, opt => opt.MapFrom(
                    src => MainTextHelper.GetEventOneLineHeader(src.EventStart, src.EventEnd, src.Location, src.ClassType.Description, null)));
        }
    }
}
