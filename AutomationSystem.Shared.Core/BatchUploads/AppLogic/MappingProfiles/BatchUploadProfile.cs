using AutoMapper;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Shared.Core.BatchUploads.AppLogic.MappingProfiles
{
    public class BatchUploadProfile : Profile
    {
        public BatchUploadProfile()
        {
            CreateMap<BatchUpload, BatchUploadListItem>()
                .BeforeMap((src, dest) =>
                {
                    EntityHelper.CheckForNull(src.BatchUploadType, "BatchUploadType", "BatchUpload");
                    EntityHelper.CheckForNull(src.BatchUploadState, "BatchUploadState", "BatchUpload");
                })
                .ForMember(dest => dest.BatchUploadType, opt => opt.MapFrom(src => src.BatchUploadType.Description))
                .ForMember(dest => dest.BatchUploadState, opt => opt.MapFrom(src => src.BatchUploadState.Description));
        }
    }
}
