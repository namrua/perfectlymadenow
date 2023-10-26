using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Factories
{
    /// <summary>
    /// Create ClassPreference related objects
    /// </summary>
    public class ClassPreferenceFactory : IClassPreferenceFactory
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IEnumDatabaseLayer enumDb;

        public ClassPreferenceFactory(IClassDatabaseLayer classDb, IPersonDatabaseLayer personDb, IEnumDatabaseLayer enumDb)
        {
            this.classDb = classDb;
            this.personDb = personDb;
            this.enumDb = enumDb;
        }

        public ClassPreference CreateDefaultClassPreference()
        {
            var result = new ClassPreference
            {
                RegistrationColorSchemeId = RegistrationColorSchemeEnum.Limet,
                LocationCode = "UNKNOWN",
                SendCertificatesByEmail = true,
                CurrencyId = LocalisationInfo.DefaultCurrency
            };
            return result;
        }

        public ClassPreferenceForEdit CreateClassPreferenceForEdit(long profileId)
        {
            var result = new ClassPreferenceForEdit();
            result.ColorSchemes = classDb.GetRegistrationColorSchemes();
            result.PersonHelper = new PersonHelper(personDb.GetMinimizedPersonsByProfileId(profileId));
            result.Currencies = enumDb.GetItemsByFilter(EnumTypeEnum.Currency);
            return result;
        }
    }
}
