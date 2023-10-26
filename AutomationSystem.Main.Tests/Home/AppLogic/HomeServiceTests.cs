using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.Home.AppLogic;
using AutomationSystem.Main.Core.Home.AppLogic.Comparers;
using AutomationSystem.Main.Core.Home.AppLogic.Convertors;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Handlers;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Core.Registrations.AppLogic;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Contract.Payment.Integration;
using AutomationSystem.Shared.Contract.Preferences.System;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Classes.System;
using Xunit;
using Profile = AutomationSystem.Main.Model.Profile;

namespace AutomationSystem.Main.Tests.Home.AppLogic
{
    public class HomeServiceTests
    {
        private const string ProfileMoniker = "ProfileMoniker";
        private readonly DateTime EventStart = new DateTime(2020, 1, 1);
        private readonly DateTime EventEnd = new DateTime(2020, 2, 2);

        private readonly Mock<IDistanceAndWwaClassComparer> comparerMock;
        private readonly Mock<ILocalisationService> localisationServiceMock;
        private readonly Mock<IRegistrationLogicProviderLocalised> registrationLogicProviderMock;
        private readonly Mock<IRegistrationDatabaseLayer> registrationDbMock;
        private readonly Mock<IClassDatabaseLayer> classDbMock;
        private readonly Mock<IPriceListDatabaseLayer> priceListDbMock;
        private readonly Mock<IPaymentDatabaseLayer> paymentDbMock;
        private readonly Mock<ICorePreferenceProvider> corePreferenceMock;
        private readonly Mock<IFormerDatabaseLayer> formerDbMock;
        private readonly Mock<IMainAsyncRequestManager> requestManagerMock;
        private readonly Mock<IPersonDatabaseLayer> personDbMock;
        private readonly Mock<IClassMaterialDistributionHandler> materialDistributionHandlerMock;
        private readonly Mock<IProfileDatabaseLayer> profileDbMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<IIncidentLogger> incidentLoggerMock;
        private readonly Mock<IPublicPaymentResolver> publicPaymentResolverMock;
        private readonly Mock<IHomeWorkflowManager> workflowManagerMock;
        private readonly Mock<IPayPalBraintreeProviderFactory> payPalBraintreeProviderFactoryMock;
        private readonly Mock<IClassOperationChecker> classOperationCheckerMock;
        private readonly Mock<IEmailTypeResolver> emailTypeResolverMock;
        private readonly Mock<IRegistrationEmailService> registrationEmailServiceMock;
        private readonly Mock<IWwaRegistrationSplitter> registrationSplitterMock;
        private readonly Mock<IHomeConvertor> homeConvertorMock;
        private readonly Mock<IHomePaymentConvertor> paymentConvertorMock;
        private readonly Mock<IRegistrationLastClassConvertor> lastClassConvertorMock;
        private readonly Mock<IFormerFilterForReviewProvider> formerFilterForReviewProviderMock;
        private readonly Mock<IClassTypeResolver> classTypeResolverMock;

        public HomeServiceTests()
        {
            comparerMock = new Mock<IDistanceAndWwaClassComparer>();
            localisationServiceMock = new Mock<ILocalisationService>();
            registrationLogicProviderMock = new Mock<IRegistrationLogicProviderLocalised>();
            registrationDbMock = new Mock<IRegistrationDatabaseLayer>();
            classDbMock = new Mock<IClassDatabaseLayer>();
            priceListDbMock = new Mock<IPriceListDatabaseLayer>();
            paymentDbMock = new Mock<IPaymentDatabaseLayer>();
            corePreferenceMock = new Mock<ICorePreferenceProvider>();
            formerDbMock = new Mock<IFormerDatabaseLayer>();
            requestManagerMock = new Mock<IMainAsyncRequestManager>();
            personDbMock = new Mock<IPersonDatabaseLayer>();
            materialDistributionHandlerMock = new Mock<IClassMaterialDistributionHandler>();
            profileDbMock = new Mock<IProfileDatabaseLayer>();
            mainMapperMock = new Mock<IMainMapper>();
            incidentLoggerMock = new Mock<IIncidentLogger>();
            publicPaymentResolverMock = new Mock<IPublicPaymentResolver>();
            workflowManagerMock = new Mock<IHomeWorkflowManager>();
            payPalBraintreeProviderFactoryMock = new Mock<IPayPalBraintreeProviderFactory>();
            classOperationCheckerMock = new Mock<IClassOperationChecker>();
            emailTypeResolverMock = new Mock<IEmailTypeResolver>();
            registrationEmailServiceMock = new Mock<IRegistrationEmailService>();
            registrationSplitterMock = new Mock<IWwaRegistrationSplitter>();
            homeConvertorMock = new Mock<IHomeConvertor>();
            paymentConvertorMock = new Mock<IHomePaymentConvertor>();
            lastClassConvertorMock = new Mock<IRegistrationLastClassConvertor>();
            formerFilterForReviewProviderMock = new Mock<IFormerFilterForReviewProvider>();
            classTypeResolverMock = new Mock<IClassTypeResolver>();
        }

        #region GetHomePageModel() tests

        [Fact]
        public void GetHomePageModel_ExpectedIncludesPassToGetProfileByMoniker()
        {
            // arrange
            SetupGetHomePageModel();
            var service = CreateHomeService();

            // act
            service.GetHomePageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            profileDbMock.Verify(e => e.GetProfileByMoniker(ProfileMoniker, ProfileIncludes.ClassPreference), Times.Once);
        }

        [Fact]
        public void GetHomePageModel_ExpectedIncludesPassToGetClassesByFilter()
        {
            // arrange
            SetupGetHomePageModel();
            var service = CreateHomeService();

            // act
            service.GetHomePageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            classDbMock.Verify(e => e.GetClassesByFilter(It.Is<ClassFilter>(x => x.ClassState == ClassState.InRegistration), ClassIncludes.ClassPersons), Times.Once);
        }

        [Fact]
        public void GetHomePageModel_NoProfileInDb_ThrowsHomeServiceException()
        {
            // arrange
            SetupGetHomePageModel();
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns((Profile)null);
            var service = CreateHomeService();

            // act & assert
            Assert.Throws<HomeServiceException>(() => service.GetHomePageModel(EnvironmentTypeEnum.Test, ProfileMoniker));
        }

        [Fact]
        public void GetHomePageModel_ProfileMoniker_ReturnsHomePageModelWithSetProfileMonikerAndMappedPageStyle()
        {
            // arrange
            var pageStyle = new RegistrationPageStyle();
            var profile = new Profile();
            SetupGetHomePageModel();
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<RegistrationPageStyle>(It.IsAny<Profile>())).Returns(pageStyle);
            var service = CreateHomeService();

            // act
            var result = service.GetHomePageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            Assert.Equal(ProfileMoniker, result.ProfileMoniker);
            Assert.Same(pageStyle, result.ProfilePageStyle);
            mainMapperMock.Verify(e => e.Map<RegistrationPageStyle>(profile), Times.Once);

        }

        [Fact]
        public void GetHomePageModel_ProfileMoniker_ReturnsHomePageModelWithSetClasses()
        {
            // arrange
            var cls = new Class();
            var classes = new List<Class> {cls};
            var clsDetail = new ClassPublicDetail
            {
                ClassId = 5,
                IsWwaFormAllowed = false,
                ClassCategoryId = ClassCategoryEnum.Class
            };
            var splitClasses = new List<ClassPublicDetail>
            {
                new ClassPublicDetail
                {
                    ClassId = 5,
                    MarkedAsWwa = false
                }
            };
            var personsMinimized = new List<PersonMinimized>();
            SetupGetHomePageModel();
            classDbMock.Setup(e => e.GetClassesByFilter(It.IsAny<ClassFilter>(), It.IsAny<ClassIncludes>())).Returns(classes);
            personDbMock.Setup(e => e.GetMinimizedPersonsByProfileId(It.IsAny<long?>())).Returns(personsMinimized);
            homeConvertorMock.Setup(e => e.ConvertToClassPublicDetailWithInstructors(It.IsAny<Class>(), It.IsAny<PersonHelper>())).Returns(clsDetail);
            registrationSplitterMock.Setup(e => e.SplitWwaClasses(It.IsAny<List<ClassPublicDetail>>())).Returns(splitClasses);
            var service = CreateHomeService();

            // act
            var result = service.GetHomePageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            Assert.Same(splitClasses, result.Classes);
            Assert.Collection(result.Classes,
                item =>
                {
                    Assert.Equal(5, item.ClassId);
                    Assert.False(item.MarkedAsWwa);
                });
            homeConvertorMock.Verify(e => e.ConvertToClassPublicDetailWithInstructors(cls, It.Is<PersonHelper>(x => x.persons == personsMinimized)), Times.Once);
            registrationSplitterMock.Verify(e => e.SplitWwaClasses(It.Is<List<ClassPublicDetail>>(x => x.Any(y => y.ClassId == 5))));
        }

        #endregion

        #region GetDistanceClassesPageModel() tests

        [Fact]
        public void GetDistanceClassesPageModel_NoProfileInDb_ThrowsHomeServiceException()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns((Profile)null);
            var service = CreateHomeService();

            // act & assert
            Assert.Throws<HomeServiceException>(() => service.GetDistanceClassesPageModel(EnvironmentTypeEnum.Test, ProfileMoniker));
        }


        [Fact]
        public void GetDistanceClassesPageModel_ExpectedIncludesPassToGetProfileByMoniker()
        {
            // arrange
            SetupGetDistanceClassesPageModel();
            var service = CreateHomeService();

            // act
            service.GetDistanceClassesPageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            profileDbMock.Verify(e => e.GetProfileByMoniker(ProfileMoniker, ProfileIncludes.ClassPreference), Times.Once);
        }

        [Fact]
        public void GetDistanceClassesPageModel_ExpectedIncludesPassToGetClassesByFilter()
        {
            // arrange
            SetupGetDistanceClassesPageModel();
            var service = CreateHomeService();

            // act
            service.GetDistanceClassesPageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            classDbMock.Verify(e => e.GetClassesByFilter(It.Is<ClassFilter>(x => x.ClassCategoryId == ClassCategoryEnum.DistanceClass), ClassIncludes.ClassPersons), Times.Once);
            classDbMock.Verify(e => e.GetClassesByFilter(It.Is<ClassFilter>(x => x.IsWwaAllowed == true), ClassIncludes.ClassPersons), Times.Once);
        }

        [Fact]
        public void GetDistanceClassesPageModel_Profile_IsMappedToRegistrationPageStyle()
        {
            // arrange
            var profile = new Profile();
            var regPageStyle = new RegistrationPageStyle();
            SetupGetDistanceClassesPageModel();
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<RegistrationPageStyle>(It.IsAny<Profile>())).Returns(regPageStyle);
            var service = CreateHomeService();

            // act
            var result = service.GetDistanceClassesPageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            Assert.Same(regPageStyle, result.ProfilePageStyle);
            profileDbMock.Verify(e => e.GetProfileByMoniker(ProfileMoniker, ProfileIncludes.ClassPreference), Times.Once);
            mainMapperMock.Verify(e => e.Map<RegistrationPageStyle>(profile), Times.Once);
        }
        
        [Fact]
        public void GetDistanceClassesPageModel_ProfileMoniker_ReturnsDistanceClassPageModelWithOrderedDetails()
        {
            // arrange
            var distanceClasses = new List<Class> { CreateClass(ClassCategoryEnum.DistanceClass) };
            var wwaClasses = new List<Class> { CreateClass(ClassCategoryEnum.Class, 2) };
            SetupGetDistanceClassesPageModel();
            classDbMock.Setup(e => e.GetClassesByFilter(It.Is<ClassFilter>(x => x.ClassCategoryId == ClassCategoryEnum.DistanceClass), It.IsAny<ClassIncludes>())).Returns(distanceClasses);
            classDbMock.Setup(e => e.GetClassesByFilter(It.Is<ClassFilter>(x => x.IsWwaAllowed == true), It.IsAny<ClassIncludes>())).Returns(wwaClasses);
            homeConvertorMock.Setup(e => e.ConvertToClassPublicDetailWithInstructors(It.Is<Class>(x => x.ClassId == 1), It.IsAny<PersonHelper>()))
                .Returns(new ClassPublicDetail { ClassId = 1 });
            homeConvertorMock.Setup(e => e.ConvertToClassPublicDetailWithInstructors(It.Is<Class>(x => x.ClassId == 2), It.IsAny<PersonHelper>()))
                .Returns(new ClassPublicDetail { ClassId = 2 });
            var service = CreateHomeService();

            // act
            var result = service.GetDistanceClassesPageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            Assert.Collection(result.Classes,
                item => Assert.Equal(2, item.ClassId),
                item => Assert.Equal(1, item.ClassId));
        }

        [Fact]
        public void GetDistanceClassesPageModel_WwaAndDistanceClasses_RemoveDuplicateDistanceClasses()
        {
            // arrange
            var distanceClasses = new List<Class>
            {
                CreateClass(ClassCategoryEnum.DistanceClass),
                CreateClass(ClassCategoryEnum.DistanceClass, 2, ClassTypeEnum.BasicOnline)
            };
            var wwaClasses = new List<Class>
            {
                CreateClass(ClassCategoryEnum.Class, 14),
                CreateClass(ClassCategoryEnum.Class, 20, ClassTypeEnum.Basic2)
            };
            SetupGetDistanceClassesPageModel();
            classDbMock.Setup(e => e.GetClassesByFilter(It.Is<ClassFilter>(x => x.ClassCategoryId == ClassCategoryEnum.DistanceClass), It.IsAny<ClassIncludes>())).Returns(distanceClasses);
            classDbMock.Setup(e => e.GetClassesByFilter(It.Is<ClassFilter>(x => x.IsWwaAllowed == true), It.IsAny<ClassIncludes>())).Returns(wwaClasses);
            comparerMock.Setup(e => e.Equals(wwaClasses[0], distanceClasses[0])).Returns(true);
            var service = CreateHomeService();

            // act
            service.GetDistanceClassesPageModel(EnvironmentTypeEnum.Test, ProfileMoniker);

            // assert
            homeConvertorMock.Verify(e => e.ConvertToClassPublicDetailWithInstructors(It.Is<Class>(x => x.ClassId == 14), It.IsAny<PersonHelper>()), Times.Once);
            homeConvertorMock.Verify(e => e.ConvertToClassPublicDetailWithInstructors(It.Is<Class>(x => x.ClassId == 20), It.IsAny<PersonHelper>()), Times.Once);
            homeConvertorMock.Verify(e => e.ConvertToClassPublicDetailWithInstructors(It.Is<Class>(x => x.ClassId == 2), It.IsAny<PersonHelper>()), Times.Once);
            homeConvertorMock.Verify(e => e.ConvertToClassPublicDetailWithInstructors(It.Is<Class>(x => x.ClassId == 1), It.IsAny<PersonHelper>()), Times.Never);

        }

        #endregion

        #region private methods

        private HomeService CreateHomeService()
        {
            return new HomeService(
                comparerMock.Object,
                localisationServiceMock.Object,
                registrationLogicProviderMock.Object,
                registrationDbMock.Object,
                classDbMock.Object,
                priceListDbMock.Object,
                paymentDbMock.Object,
                corePreferenceMock.Object,
                formerDbMock.Object,
                requestManagerMock.Object,
                personDbMock.Object,
                materialDistributionHandlerMock.Object,
                profileDbMock.Object,
                mainMapperMock.Object,
                incidentLoggerMock.Object,
                publicPaymentResolverMock.Object,
                workflowManagerMock.Object,
                payPalBraintreeProviderFactoryMock.Object,
                classOperationCheckerMock.Object,
                homeConvertorMock.Object,
                paymentConvertorMock.Object,
                formerFilterForReviewProviderMock.Object,
                lastClassConvertorMock.Object,
                emailTypeResolverMock.Object,
                registrationEmailServiceMock.Object,
                registrationSplitterMock.Object,
                classTypeResolverMock.Object);
        }

        private void SetupGetHomePageModel()
        {
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            classDbMock.Setup(e => e.GetClassesByFilter(It.IsAny<ClassFilter>(), It.IsAny<ClassIncludes>())).Returns(new List<Class>());
            personDbMock.Setup(e => e.GetMinimizedPersonsByProfileId(It.IsAny<long?>())).Returns(new List<PersonMinimized>());
            mainMapperMock.Setup(e => e.Map<RegistrationPageStyle>(It.IsAny<Profile>())).Returns(new RegistrationPageStyle());
            publicPaymentResolverMock.Setup(e => e.IsPublicPaymentAllowedForClass(It.IsAny<Class>())).Returns(true);
            homeConvertorMock.Setup(e => e.ConvertToClassPublicDetailWithInstructors(It.IsAny<Class>(), It.IsAny<PersonHelper>())).Returns(new ClassPublicDetail());
            registrationSplitterMock.Setup(e => e.SplitWwaClasses(It.IsAny<List<ClassPublicDetail>>())).Returns(new List<ClassPublicDetail>());
        }

        private void SetupGetDistanceClassesPageModel()
        {
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            classDbMock.Setup(e => e.GetClassesByFilter(It.IsAny<ClassFilter>(), It.IsAny<ClassIncludes>())).Returns(new List<Class>());
            personDbMock.Setup(e => e.GetMinimizedPersonsByProfileId(It.IsAny<long?>())).Returns(new List<PersonMinimized>());
            mainMapperMock.Setup(e => e.Map<RegistrationPageStyle>(It.IsAny<Profile>())).Returns(new RegistrationPageStyle());
            publicPaymentResolverMock.Setup(e => e.IsPublicPaymentAllowedForClass(It.IsAny<Class>())).Returns(true);
            homeConvertorMock.Setup(e => e.ConvertToClassPublicDetailWithInstructors(It.IsAny<Class>(), It.IsAny<PersonHelper>())).Returns(new ClassPublicDetail());
        }

        private Class CreateClass(ClassCategoryEnum classCategoryId, long classId = 1, ClassTypeEnum classTypeId = ClassTypeEnum.Basic)
        {
            return new Class
            {
                ClassId = classId,
                EventStart = EventStart,
                EventEnd = EventEnd,
                ClassTypeId = classTypeId,
                Location = "location",
                ClassCategoryId = classCategoryId,
                IsWwaFormAllowed = true
            };
        }
        #endregion
    }
}
