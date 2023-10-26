using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Integration.System;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Preferences.System;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Core.Reports.Data;
using AutomationSystem.Main.Core.Reports.System.DataProviders;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;
using AutomationSystem.Main.Core.Reports.System.ReportAvailabilityResolvers;
using AutomationSystem.Main.Core.Reports.System.ReportGenerators;
using AutomationSystem.Shared.Contract.Localisation.System;

namespace AutomationSystem.Main.Core.Reports.System
{
    /// <summary>
    /// Creates class report components
    /// </summary>
    public class ClassReportFactory : IClassReportFactory
    {
        private readonly ILocalisationService localisation;
        private readonly IClassDatabaseLayer classDb;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IMainPreferenceProvider mainPreferences;
        private readonly IGenericIntegrationDataProvider integrationDataProvider;
        private readonly IFinancialFormDatabaseLayer financialFormDb;
        private readonly IPriceListDatabaseLayer priceListDb;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        private readonly Dictionary<ClassReportType, IClassReportGenerator> classReportGenerators;

        public ClassReportFactory(
            ILocalisationService localisation,
            IClassDatabaseLayer classDb, 
            IRegistrationDatabaseLayer registrationDb,
            IPersonDatabaseLayer personDb,
            IMainPreferenceProvider mainPreferences, 
            IGenericIntegrationDataProvider integrationDataProvider,
            IFinancialFormDatabaseLayer financialFormDb,
            IPriceListDatabaseLayer priceListDb,
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.localisation = localisation;
            this.classDb = classDb;
            this.registrationDb = registrationDb;
            this.personDb = personDb;
            this.mainPreferences = mainPreferences;
            this.integrationDataProvider = integrationDataProvider;
            this.financialFormDb = financialFormDb;
            this.priceListDb = priceListDb;
            this.registrationTypeResolver = registrationTypeResolver;

            classReportGenerators = new Dictionary<ClassReportType, IClassReportGenerator>();
            InitializeClassReportGenerators();
        }
        
        public IClassReportComponents InitializeClassReportComponentsForClass(long classId)
        {
            // initializes class data provider
            var data = new ClassDataProvider(classDb, registrationDb, personDb, integrationDataProvider, financialFormDb, priceListDb);
            data.InitializeForClass(classId);

            // initializes common data provider
            var common = new CommonReportDataProvider(localisation, data, registrationTypeResolver);

            // resolves class type category and rest of components
            IReportAvailabilityResolver availabilityResolver;
            IFinancialFormsDataProvider financial = null;
            switch (data.Class.ClassCategoryId)
            {
                // online class
                case ClassCategoryEnum.Class:
                    var logic = new ClassFinancialBusinessLogic(data, mainPreferences);
                    financial = new FinancialFormsDataProvider(data, logic);
                    availabilityResolver = new ClassReportAvailabilityResolver(data, logic);
                    break;

                // online lecture
                case ClassCategoryEnum.Lecture:
                    availabilityResolver = new LectureReportAvailabilityResolver();
                    break;

                // unknown type - allows no reports
                default:
                    availabilityResolver = new EmptyReportAvailabilityResolver();
                    break;
            }

            // assembles result
            var result = new ClassReportComponents(data, common, financial, availabilityResolver);
            return result;
        }

        public IClassReportGenerator GetClassReportGeneratorByReportType(ClassReportType reportType)
        {
            if (!classReportGenerators.TryGetValue(reportType, out var result))
                throw new InvalidOperationException(
                    $"There is no registered ClassReportGenerator for report type {reportType}");            
            return result;
        }

        public List<IClassReportGenerator> GetClassReportGeneratorsByReportTypes(IEnumerable<ClassReportType> reportTypes)
        {
            var result = reportTypes.Select(GetClassReportGeneratorByReportType).ToList();
            return result;
        }

        #region private methods

        private void InitializeClassReportGenerators()
        {
            classReportGenerators.Add(ClassReportType.CrfClass, new CrfClassReportGenerator());
            classReportGenerators.Add(ClassReportType.CrfLecture, new CrfLectureReportGenerator());
            classReportGenerators.Add(ClassReportType.CrfWwaClass, new CrfWwaClassReportGenerator());
            classReportGenerators.Add(ClassReportType.FoiRoyaltyForm, new FoiRoyaltyFormReportGenerator());
            classReportGenerators.Add(ClassReportType.FaClosingStatement, new FaClosingStatementReportGenerator());
            classReportGenerators.Add(ClassReportType.GuestInstructorClosingStatement, new GuestInstructorClosingStatementReportGenerator());
            classReportGenerators.Add(ClassReportType.CountriesReport, new CountriesReportGenerator());           
        }

        #endregion
    }
}
