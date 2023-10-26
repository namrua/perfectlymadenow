using System;
using System.Collections.Generic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Integration.System;
using AutomationSystem.Main.Core.Integration.System.Models;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.PriceLists.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Reports.Data;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Provides complete data of the class
    /// </summary>
    public class ClassDataProvider : IClassDataProvider
    {
        private const ClassIncludes classIncludes = ClassIncludes.ClassType | 
                                                    ClassIncludes.ClassPersons | ClassIncludes.ClassReportSetting | 
                                                    ClassIncludes.ClassBusinessClassExpenses | ClassIncludes.Currency;
        private const ClassRegistrationIncludes registrationIncludes = 
            ClassRegistrationIncludes.AddressesCountry | ClassRegistrationIncludes.ClassRegistrationPayment;
        
        private readonly IClassDatabaseLayer classDb;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IGenericIntegrationDataProvider integrationDataProvider;
        private readonly IFinancialFormDatabaseLayer financialFormDb;
        private readonly IPriceListDatabaseLayer priceListDb;

        private Lazy<Class> cls;
        private Lazy<List<Person>> persons;
        private Lazy<List<ClassRegistration>> approvedRegistrations;
        private Lazy<List<ClassRegistration>> approvedRegistrationsOfAssociatedLecture;
        private Lazy<List<EventLocationInfo>> eventLocationInfo;
        private Lazy<List<RoyaltyFeeRate>> royaltyFeeRates;
        private Lazy<PriceList> classPriceList;
        private Lazy<PriceList> associatedLecturePriceList;

        public ClassDataProvider(
            IClassDatabaseLayer classDb,
            IRegistrationDatabaseLayer registrationDb,
            IPersonDatabaseLayer personDb,
            IGenericIntegrationDataProvider integrationDataProvider,
            IFinancialFormDatabaseLayer financialFormDb,
            IPriceListDatabaseLayer priceListDb)
        {
            this.classDb = classDb;
            this.registrationDb = registrationDb;
            this.personDb = personDb;
            this.integrationDataProvider = integrationDataProvider;
            this.financialFormDb = financialFormDb;
            this.priceListDb = priceListDb;

            cls = new Lazy<Class>(() => throw new InvalidOperationException("ClassDataProvider is not initialized."));
            persons = new Lazy<List<Person>>(() => throw new InvalidOperationException("ClassDataProvider is not initialized."));
            approvedRegistrations = approvedRegistrationsOfAssociatedLecture = 
                new Lazy<List<ClassRegistration>>(() => throw new InvalidOperationException("ClassDataProvider is not initialized."));     
            eventLocationInfo = new Lazy<List<EventLocationInfo>>(() => throw new InvalidOperationException("ClassDataProvider is not initialized."));
            royaltyFeeRates = new Lazy<List<RoyaltyFeeRate>>(() => throw new InvalidOperationException("ClassDataProvider is not initialized."));
            classPriceList = new Lazy<PriceList>(() => throw new InvalidOperationException("ClassDataProvider is not initialized."));
            associatedLecturePriceList = new Lazy<PriceList>(() => throw new InvalidOperationException("ClassDataProvider is not initialized."));
        }

        #region data accessors

        public Class Class => cls.Value;

        public List<Person> Persons => persons.Value;

        public ClassBusiness Business => cls.Value.ClassBusiness;

        public ClassReportSetting ReportSetting => cls.Value.ClassReportSetting;

        public List<ClassRegistration> ApprovedRegistrations => approvedRegistrations.Value;        

        public List<ClassRegistration> ApprovedRegistrationsOfAssociatedLecture =>
            approvedRegistrationsOfAssociatedLecture.Value;

        public List<EventLocationInfo> EventLocationInfo => eventLocationInfo.Value;

        public List<RoyaltyFeeRate> RoyaltyFeeRates => royaltyFeeRates.Value;
        
        public PriceList ClassPriceList => classPriceList.Value;

        public PriceList AssociatedLecturePriceList => associatedLecturePriceList.Value;

        #endregion

        #region data loading

        public void InitializeForClass(long classId)
        {
            cls = new Lazy<Class>(() =>
            {
                var clss = classDb.GetClassById(classId, classIncludes);
                if (clss == null)
                    throw new ArgumentException($"There is no Class with id {classId}.");
                return clss;
            });
            persons = new Lazy<List<Person>>(() => personDb.GetPersonsByIds(ClassConvertor.GetClassPersonIdsIncludingReportSetting(Class), PersonIncludes.AddressCountry));

            approvedRegistrations = new Lazy<List<ClassRegistration>>(() =>
            {
                var filter = new RegistrationFilter { ClassId = classId, RegistrationState = RegistrationState.Approved };
                var result = registrationDb.GetRegistrationsByFilter(filter, registrationIncludes);
                return result;
            });

            approvedRegistrationsOfAssociatedLecture = new Lazy<List<ClassRegistration>>(() =>
            {
                if (!Business.AssociatedLectureId.HasValue)
                    return new List<ClassRegistration>();
                var filter = new RegistrationFilter { ClassId = Business.AssociatedLectureId.Value, RegistrationState = RegistrationState.Approved };
                var result = registrationDb.GetRegistrationsByFilter(filter, registrationIncludes);
                return result;
            });

            eventLocationInfo = new Lazy<List<EventLocationInfo>>(() => integrationDataProvider.GetEventLocationInfo(Class.IntegrationTypeId, Class.IntegrationEntityId));

            royaltyFeeRates = new Lazy<List<RoyaltyFeeRate>>(() => financialFormDb.GetRoyaltyFeeRatesByCurrencyId(Class.CurrencyId));

            classPriceList = new Lazy<PriceList>(() => priceListDb.GetPriceListById(Class.PriceListId, PriceListIncludes.PriceListItems));

            associatedLecturePriceList = new Lazy<PriceList>(() =>
            {
                if (Business.AssociatedLectureId.HasValue)
                {
                    var associatedLecture = classDb.GetClassById(Business.AssociatedLectureId.Value, ClassIncludes.PriceListPriceListItems);
                    return associatedLecture.PriceList;
                }

                return null;
            });
        }

        #endregion
    }
}
