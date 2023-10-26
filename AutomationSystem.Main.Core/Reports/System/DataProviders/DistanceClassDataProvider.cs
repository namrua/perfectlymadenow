using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Distance class data provider
    /// </summary>
    public class DistanceClassDataProvider : IDistanceClassDataProvider
    {
        private const ClassRegistrationIncludes registrationIncludes = ClassRegistrationIncludes.AddressesCountry 
                                                                       | ClassRegistrationIncludes.Class | ClassRegistrationIncludes.ClassRegistrationPayment;

        private readonly IPersonDatabaseLayer personDb;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        private Lazy<DistanceCrfReportParameters> parameters;
        private Lazy<Person> distanceCoordinator;
        private Lazy<List<ClassRegistration>> registrationsWithClasses;

        // constructor
        public DistanceClassDataProvider(
            IRegistrationDatabaseLayer registrationDb,
            IPersonDatabaseLayer personDb,
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationDb = registrationDb;
            this.personDb = personDb;
            this.registrationTypeResolver = registrationTypeResolver;

            var notInitialized = new InvalidOperationException("DistanceClassDataProvider is not initialized.");
            parameters = new Lazy<DistanceCrfReportParameters>(() => throw notInitialized);
            distanceCoordinator = new Lazy<Person>(() => throw notInitialized);
            registrationsWithClasses = new Lazy<List<ClassRegistration>>(() => throw notInitialized);
        }

        #region data accessors

        public DistanceCrfReportParameters Parameters => parameters.Value;

        public Person DistanceCoordinator => distanceCoordinator.Value;

        public List<ClassRegistration> RegistrationsWithClasses => registrationsWithClasses.Value;

        #endregion

        #region data loading

        public void InitializeForDistanceReport(DistanceCrfReportParameters reportParameters)
        {
            parameters = new Lazy<DistanceCrfReportParameters>(() => reportParameters);
            distanceCoordinator = new Lazy<Person>(() =>
            {
                var person = personDb.GetPersonById(reportParameters.DistanceCoordinatorId, PersonIncludes.Address);
                if (person == null)
                    throw new ArgumentException($"There is no Person with id {reportParameters.DistanceCoordinatorId}.");
                return person;
            });
            registrationsWithClasses = new Lazy<List<ClassRegistration>>(() => 
                registrationDb.GetApprovedDistanceRegistrations(reportParameters, registrationIncludes)
                    .Where(x => registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId)).ToList());
        }

        #endregion
    }
}
