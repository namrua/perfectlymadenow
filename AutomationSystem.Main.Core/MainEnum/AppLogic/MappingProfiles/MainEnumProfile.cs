using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using PerfectlyMadeInc.Helpers.Mapping;

namespace AutomationSystem.Main.Core.MainEnum.AppLogic.MappingProfiles
{
    public class MainEnumProfile : Profile
    {
        public MainEnumProfile(IEnumDatabaseLayer enumDb)
        {
            var languageTypeConvertor = new TypeConverterMapper<IEnumItem, LanguageEnum, string>(
                () => enumDb.GetItemsByFilter(EnumTypeEnum.Language), x => (LanguageEnum)x.Id, x => x.Description);

            CreateMap<LanguageEnum, string>().ConvertUsing(languageTypeConvertor);
        }
    }
}
