using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    public class DistanceReportDataProviderFactory : IDistanceReportDataProviderFactory
    {
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public DistanceReportDataProviderFactory(
            IRegistrationDatabaseLayer registrationDb,
            IPersonDatabaseLayer personDb,
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationDb = registrationDb;
            this.personDb = personDb;
            this.registrationTypeResolver = registrationTypeResolver;
        }

        public IDistanceReportDataProvider CreateDistanceReportDataProvider(DistanceCrfReportParameters parameters)
        {
            var data = new DistanceClassDataProvider(registrationDb, personDb, registrationTypeResolver);
            data.InitializeForDistanceReport(parameters);
            var provider = new DistanceReportDataProvider(data);
            return provider;
        }
    }
}
