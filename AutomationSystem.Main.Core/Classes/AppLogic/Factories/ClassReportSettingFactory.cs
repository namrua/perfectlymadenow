using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Factories
{
    /// <summary>
    /// Create ClassReportSetting objects
    /// </summary>
    public class ClassReportSettingFactory : IClassReportSettingFactory
    {
        private readonly IPersonDatabaseLayer personDb;

        public ClassReportSettingFactory(IPersonDatabaseLayer personDb)
        {
            this.personDb = personDb;
        }

        public ClassReportSettingForEdit CreateClassReportSettingForEdit(long profileId)
        {
            var result = new ClassReportSettingForEdit();
            result.PersonHelper = new PersonHelper(personDb.GetMinimizedPersonsByProfileId(profileId));
            return result;
        }
    }
}
