using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Routines;
using System.Collections.Generic;
using System.Linq;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.MappingProfiles
{
    public class DistanceClassTemplateProfile : Profile
    {
        public DistanceClassTemplateProfile(ILanguageTranslationProvider languageProvider, IDistanceClassTemplateHelper helper)
        {
            
            CreateMap<DistanceClassTemplate, DistanceClassTemplateListItem>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.ClassType, "ClassType", "DistanceClassTemplate"))
                .ForMember(dest => dest.OriginLanguage, opt => opt.MapFrom(src => src.OriginLanguageId))
                .ForMember(dest => dest.TransLanguage, opt => opt.MapFrom(src => src.TransLanguageId))
                .ForMember(dest => dest.TemplateState, opt => opt.MapFrom(src => helper.GetDistanceClassTemplateState(src)))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => GetDistanceClassTemplateTitle(src)));
            CreateMap<DistanceClassTemplate, DistanceClassTemplateDetail>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.ClassType, "ClassType", "DistanceClassTemplate"))
                .ForMember(dest => dest.OriginLanguage, opt => opt.MapFrom(src => src.OriginLanguageId))
                .ForMember(dest => dest.TransLanguage, opt => opt.MapFrom(src => src.TransLanguageId))
                .ForMember(dest => dest.TemplateState, opt => opt.MapFrom(src => helper.GetDistanceClassTemplateState(src)))
                .ForMember(dest => dest.ClassType, opt => opt.MapFrom(src => src.ClassType.Description))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => GetDistanceClassTemplateTitle(src)))
                .ForMember(dest => dest.GuestInstructor, opt => opt.Ignore());
            CreateMap<DistanceClassTemplate, DistanceClassTemplateForm>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.DistanceClassTemplatePersons, "DistanceClassTemplatePersons", "DistanceClassTemplate"))
                .ForMember(dest => dest.InstructorIds, opt => opt.MapFrom(src => GetInstructorIds(src)))
                .ForMember(dest => dest.TranslationCode, opt => opt.MapFrom(src => languageProvider.GetTranslationCode(src.OriginLanguageId, src.TransLanguageId)));
            CreateMap<DistanceClassTemplateForm, DistanceClassTemplate>()
                .ForMember(dest => dest.OriginLanguageId, opt => opt.MapFrom(src => languageProvider.GetOriginalLanguageId(src.TranslationCode ?? 0)))
                .ForMember(dest => dest.TransLanguageId, opt => opt.MapFrom(src => languageProvider.GetTranslationLanguageId(src.TranslationCode ?? 0)))
                .ForMember(dest => dest.DistanceClassTemplatePersons, opt => opt.MapFrom(src => GetDistanceClassTemplatePersons(src)));
            CreateMap<DistanceClassTemplate, DistanceClassTemplateCompletionForm>();
            CreateMap<DistanceClassTemplate, DistanceClassTemplateCompletionShortDetail>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.ClassType, "ClassType", "DistanceClassTemplate"))
                .ForMember(dest => dest.TemplateState, opt => opt.MapFrom(src => helper.GetDistanceClassTemplateState(src)))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => GetDistanceClassTemplateTitle(src)));
        }
        
        #region private methods
        private string GetDistanceClassTemplateTitle(DistanceClassTemplate template)
        {
            var result = MainTextHelper.GetEventOneLineHeader(template.EventStart, template.EventEnd, template.Location, template.ClassType.Description);
            return result;
        }

        private List<long> GetInstructorIds(DistanceClassTemplate template)
        {
            var ids = template.DistanceClassTemplatePersons.Where(x => x.RoleTypeId == PersonRoleTypeEnum.Instructor).Select(x => x.PersonId).ToList();
            return ids;
        }

        private List<DistanceClassTemplatePerson> GetDistanceClassTemplatePersons(DistanceClassTemplateForm form)
        {
            var result = new List<DistanceClassTemplatePerson>();
            foreach (var id in form.InstructorIds)
            {
                var person = new DistanceClassTemplatePerson
                {
                    PersonId = id,
                    DistanceClassTemplateId = form.DistanceClassTemplateId,
                    RoleTypeId = PersonRoleTypeEnum.Instructor
                };
                result.Add(person);
            }

            return result;
        }
        #endregion
    }
}
