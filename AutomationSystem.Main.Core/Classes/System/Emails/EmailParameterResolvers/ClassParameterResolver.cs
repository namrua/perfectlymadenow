using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Classes.System.Emails.EmailParameterResolvers
{

    /// <summary>
    /// Resolves email parameters from Class entity
    /// </summary>
    public class ClassParameterResolver : IEmailParameterResolverWithBinding<Class>
    {
        
        private const string Header = "{{Class.Header}}";
        private const string HeaderOneLine = "{{Class.HeaderOneLine}}";
        private const string Type = "{{Class.Type}}";
        private const string Location = "{{Class.Location}}";
        private const string Date = "{{Class.Date}}";
        private const string Time = "{{Class.Time}}";
        private const string TimeZone = "{{Class.TimeZone}}";
        private const string Coordinator = "{{Class.Coordinator}}";
        private const string Team = "{{Class.Team}}";


        private Class cls;
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;
        private readonly IClassDatabaseLayer classDb;
        private readonly IPersonDatabaseLayer personDb;


        // lazy loading for class persons
        private Lazy<IPersonHelper> personHelper;
        private Lazy<List<ClassPerson>> classPersons;

        // accessors to data
        public IPersonHelper PersonHelper => personHelper.Value;
        public List<ClassPerson> ClassPersons => classPersons.Value;

        // constructor
        public ClassParameterResolver(IEmailServiceHelper helper, IPersonDatabaseLayer personDb, IClassDatabaseLayer classDb)
        {
            this.helper = helper;
            this.classDb = classDb;
            this.personDb = personDb;
            supportedParameters = new HashSet<string>(new [] { Header, HeaderOneLine, Type, Location, Date, Time, TimeZone, Coordinator, Team });
            
            personHelper = new Lazy<IPersonHelper>(() => throw new InvalidOperationException("ClassParameterResolver was not bind to data."));
            classPersons = new Lazy<List<ClassPerson>>(() => throw new InvalidOperationException("ClassParameterResolver was not bind to data."));
        }



        // binds value to email parameters
        public void Bind(Class data)
        {
            cls = data;
            classPersons = new Lazy<List<ClassPerson>>(() => classDb.GetClassPersonsByClassId(cls.ClassId));
            personHelper = new Lazy<IPersonHelper>(() => new PersonHelper(personDb.GetMinimizedPersonsByProfileId(data.ProfileId)));
        }

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (cls == null)
                throw new InvalidOperationException("Class was not binded.");

            object resultObject;
            var langInfo = helper.LanguageInfoProvider.GetLanguageInfo(languageId);           
            switch (parameterNameWithBrackets)
            {
                case Header:
                    // gets language
                    var language = helper.GetLocalisedEnumItem(langInfo, EnumTypeEnum.Language, cls.OriginLanguageId).Description;
                    if (cls.TransLanguageId.HasValue)
                        language = MainTextHelper.GetEventLanguageInfo(language,
                            helper.GetLocalisedEnumItem(langInfo, EnumTypeEnum.Language, cls.TransLanguageId.Value).Description,
                            helper.GetLocalisedText(langInfo, "With"));

                    // assembles header
                    resultObject = string.Format(helper.GetLocalisedText(langInfo, "ClassHeader"),
                        helper.GetLocalisedEnumItem(langInfo, EnumTypeEnum.MainClassType, cls.ClassTypeId).Description,
                        cls.Location, language, MainTextHelper.GetEventDate(cls.EventStart, cls.EventEnd, langInfo.CultureInfo),                        
                        MainTextHelper.GetEventDays(cls.EventStart, cls.EventEnd, helper.GetLocalisedText(langInfo, "And"), langInfo.CultureInfo),
                        MainTextHelper.GetEventTime(cls.EventStart, cls.EventEnd, helper.GetLocalisedText(langInfo, "To"), langInfo.CultureInfo),
                        helper.GetLocalisedEnumItem(langInfo, EnumTypeEnum.TimeZone, cls.TimeZoneId).Name
                    );
                    break;

                case HeaderOneLine:
                    resultObject = MainTextHelper.GetEventOneLineHeader(cls.EventStart, cls.EventEnd, cls.Location,
                        helper.GetLocalisedEnumItem(langInfo, EnumTypeEnum.MainClassType, cls.ClassTypeId).Description, langInfo.CultureInfo);
                    break;

                case Type:
                    resultObject = helper.GetLocalisedEnumItem(langInfo, EnumTypeEnum.MainClassType, cls.ClassTypeId).Description;
                    break;

                case Location:
                    resultObject = cls.Location;
                    break;

                case Date:
                    resultObject = MainTextHelper.GetEventDate(cls.EventStart, cls.EventEnd, langInfo.CultureInfo);
                    break;

                case Time:
                    resultObject = MainTextHelper.GetEventTime(cls.EventStart, cls.EventEnd,
                        helper.GetLocalisedText(langInfo, "To"), langInfo.CultureInfo);
                    break;

                case TimeZone:
                    resultObject = helper.GetLocalisedEnumItem(langInfo, EnumTypeEnum.TimeZone, cls.TimeZoneId).Name;
                    break;

                case Coordinator:
                    resultObject = PersonHelper.GetPersonNameById(cls.CoordinatorId);
                    break;

                case Team:                                       
                    var teamComposer = new PersonDistinctComposer<string>(x => PersonHelper.GetPersonNameById(x));

                    // remembers coordinator which is added to the end of list (but we don't need duplicities)
                    var coordinatorName = teamComposer.AddPerson(cls.CoordinatorId);
                    teamComposer.Pop();                 
                
                    // assembles list 
                    teamComposer.AddPerson(cls.GuestInstructorId);
                    teamComposer.AddClassPersonsWithRole(ClassPersons, PersonRoleTypeEnum.Instructor);
                    teamComposer.AddClassPersonsWithRole(ClassPersons, PersonRoleTypeEnum.ApprovedStaff);

                    // gets list of the team and add coordinatorName at the end
                    var team = teamComposer.Pop();
                    team.Add(coordinatorName);
                    resultObject = string.Join("\n", team.Where(x => x != null));
                    break;

                default:
                    return null;
            }
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }
       
    }

}
