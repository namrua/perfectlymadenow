using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports;
using AutomationSystem.Main.Model;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.Classes.AppLogic.MappingProfiles
{
    /// <summary>
    /// Maps ClassReportSetting related objects
    /// </summary>
    public class ClassReportSettingProfile : Profile
    {
        public ClassReportSettingProfile()
        {
            CreateMap<ClassPreference, ClassReportSetting>()
                .ForMember(dest => dest.LocationInfo, opt => opt.Ignore());
            CreateMap<ClassReportSetting, ClassReportSettingForm>();
            CreateMap<ClassReportSettingForm, ClassReportSetting>();
        }
    }
}
