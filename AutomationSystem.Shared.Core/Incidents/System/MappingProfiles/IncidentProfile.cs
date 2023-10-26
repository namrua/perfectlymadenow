using AutoMapper;
using AutomationSystem.Shared.Contract.Incidents.AppLogic.Models;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Database;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Shared.Core.Incidents.System.MappingProfiles
{
    public class IncidentProfile : Profile
    {
        public IncidentProfile()
        {
            CreateMap<Incident, IncidentDetail>()
                .BeforeMap((src, dest) =>
                {
                    EntityHelper.CheckForNull(src.EntityType, "EntityType", "Incident", src.EntityTypeId.HasValue);
                    EntityHelper.CheckForNull(src.IncidentType, "IncidentType", "Incident");
                    EntityHelper.CheckForNull(src.IncidentChildren, "IncidentChildren", "Incident");
                })
                .ForMember(dest => dest.InnerIncidentsCount, opt => opt.MapFrom(src => src.IncidentChildren.Count))
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityType == null ? null : src.EntityType.Description))
                .ForMember(dest => dest.IncidentType, opt => opt.MapFrom(src => src.IncidentType.Description))
                .ForMember(dest => dest.CanBeReported, opt => opt.MapFrom(src => src.CanBeReport));

            CreateMap<IncidentForLog, Incident>()
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => DatabaseHelper.TrimNVarchar(src.Message, true, 255)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => DatabaseHelper.TrimNVarchar(src.Description, false, null)))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => DatabaseHelper.TrimNVarchar(src.IpAddress, false, 40)))
                .ForMember(dest => dest.RequestUrl, opt => opt.MapFrom(src => DatabaseHelper.TrimNVarchar(src.RequestUrl, false, 2048)))
                .ForMember(dest => dest.CanBeReport, opt => opt.MapFrom(src => src.CanBeReported))
                .ForMember(dest => dest.IncidentChildren, opt => opt.MapFrom(src => src.Children));
        }
    }
}
