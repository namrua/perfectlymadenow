using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.PriceLists.Data.Models;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.EntityIntegration.System;
using AutomationSystem.Shared.Contract.Files.System;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System.Collections.Generic;
using Xunit;
using Profile = AutomationSystem.Main.Model.Profile;

namespace AutomationSystem.Main.Tests.Classes.System
{
    public class ClassServiceTests
    {
        private readonly Mock<IClassConvertor> classConvertorMock;
        private readonly Mock<IClassBusinessConvertor> classBusinessConvertorMock;
        private readonly Mock<IClassStyleConvertor> classStyleConvertorMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<ICoreFileService> coreFileServiceMock;
        private readonly Mock<IClassDatabaseLayer> classDbMock;
        private readonly Mock<IGenericEntityIntegrationProvider> integrationProviderMock;
        private readonly Mock<IEventDispatcher> eventDispatcherMock;
        private readonly Mock<IPriceListDatabaseLayer> priceListDbMock;
        private readonly Mock<IProfileDatabaseLayer> profileDbMock;
        private readonly Mock<IClassEventFactory> classEventFactoryMock;
        private readonly Mock<IClassTypeResolver> classTypeResolverMock;

        public ClassServiceTests()
        {
            classConvertorMock = new Mock<IClassConvertor>();
            classBusinessConvertorMock = new Mock<IClassBusinessConvertor>();
            classStyleConvertorMock = new Mock<IClassStyleConvertor>();
            identityResolverMock = new Mock<IIdentityResolver>();
            mainMapperMock = new Mock<IMainMapper>();
            coreFileServiceMock = new Mock<ICoreFileService>();
            classDbMock = new Mock<IClassDatabaseLayer>();
            integrationProviderMock = new Mock<IGenericEntityIntegrationProvider>();
            eventDispatcherMock = new Mock<IEventDispatcher>();
            priceListDbMock = new Mock<IPriceListDatabaseLayer>();
            profileDbMock = new Mock<IProfileDatabaseLayer>();
            classEventFactoryMock = new Mock<IClassEventFactory>();
            classTypeResolverMock = new Mock<IClassTypeResolver>();
        }

        #region GetNewClassForm() tests

        [Fact]
        public void GetNewClassForm_ArbitraryFormConfiguration_FormConfigurationInvariantPropertiesSet()
        {
            // arrange
            classTypeResolverMock
                .Setup(e => e.GetClassFormConfigurationByClassCategoryId(It.IsAny<ClassCategoryEnum>()))
                .Returns(new ClassFormConfiguration());
            var service = CreateService();

            // act
            var form = service.GetNewClassForm(1, ClassCategoryEnum.PrivateMaterialClass, CurrencyEnum.MXN, CreatePersonHelperWithDefaultPersons());

            // assert
            Assert.Equal(1, form.ProfileId);
            Assert.Equal(ClassCategoryEnum.PrivateMaterialClass, form.ClassCategoryId);
            Assert.Equal(CurrencyEnum.MXN, form.ProfileCurrencyId);
            Assert.Equal(ClassState.New, form.ClassState);
            Assert.Equal(103, form.GuestInstructorId);
            Assert.Equal(new List<long> { 104, 105 }, form.InstructorIds);

            classTypeResolverMock.Verify(e => e.GetClassFormConfigurationByClassCategoryId(ClassCategoryEnum.PrivateMaterialClass), Times.Once);
        }

        [Theory]
        [InlineData(false, TimeZoneEnum.HawaiianStandardTime)]
        [InlineData(true, TimeZoneEnum.UTC)]
        public void GetNewClassForm_ByShowOnlyDates_ExpectedTimeZoneIdIsSet(bool showOnlyDates, TimeZoneEnum expectedTimeZoneId)
        {
            // arrange
            classTypeResolverMock
                .Setup(e => e.GetClassFormConfigurationByClassCategoryId(It.IsAny<ClassCategoryEnum>()))
                .Returns(new ClassFormConfiguration { ShowOnlyDates = showOnlyDates });
            var service = CreateService();

            // act
            var form = service.GetNewClassForm(1, ClassCategoryEnum.PrivateMaterialClass, CurrencyEnum.MXN, CreatePersonHelperWithDefaultPersons());

            // assert
            Assert.Equal(expectedTimeZoneId, form.TimeZoneId);
        }

        [Theory]
        [InlineData(false, 101)]
        [InlineData(true, 102)]
        public void GetNewClassForm_ByUseDistanceCoordinator_ExpectedCoordinatorIdIsSet(bool useDistanceCoordinator, long expectedCoordinatorId)
        {
            // arrange
            classTypeResolverMock
                .Setup(e => e.GetClassFormConfigurationByClassCategoryId(It.IsAny<ClassCategoryEnum>()))
                .Returns(new ClassFormConfiguration { UseDistanceCoordinator = useDistanceCoordinator });
            var service = CreateService();

            // act
            var form = service.GetNewClassForm(1, ClassCategoryEnum.PrivateMaterialClass, CurrencyEnum.MXN, CreatePersonHelperWithDefaultPersons());

            // assert
            Assert.Equal(expectedCoordinatorId, form.CoordinatorId);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void GetNewClassForm_ByIsWwaAllowedValue_ExpectedIsWwaFormAllowedIsSet(bool? isWwaAllowedValue, bool expectedIsWwaFormAllowed)
        {
            // arrange
            classTypeResolverMock
                .Setup(e => e.GetClassFormConfigurationByClassCategoryId(It.IsAny<ClassCategoryEnum>()))
                .Returns(new ClassFormConfiguration { IsWwaAllowedValue = isWwaAllowedValue });
            var service = CreateService();

            // act
            var form = service.GetNewClassForm(1, ClassCategoryEnum.PrivateMaterialClass, CurrencyEnum.MXN, CreatePersonHelperWithDefaultPersons());

            // assert
            Assert.Equal(expectedIsWwaFormAllowed, form.IsWwaFormAllowed);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void GetNewClassForm_ByShowApprovedStaffIds_DefaultStaffsAssigned(bool showApprovedStaffIds, bool isDefaultStaffsAssigned)
        {
            // arrange
            classTypeResolverMock
                .Setup(e => e.GetClassFormConfigurationByClassCategoryId(It.IsAny<ClassCategoryEnum>()))
                .Returns(new ClassFormConfiguration { ShowApprovedStaffIds = showApprovedStaffIds });
            var service = CreateService();

            // act
            var form = service.GetNewClassForm(1, ClassCategoryEnum.PrivateMaterialClass, CurrencyEnum.MXN, CreatePersonHelperWithDefaultPersons());

            // assert
            if (isDefaultStaffsAssigned)
            {
                Assert.Equal(new List<long> { 106, 107 }, form.ApprovedStaffIds);
            }
            else
            {
                Assert.Empty(form.ApprovedStaffIds);
            }
        }

        [Theory]
        [InlineData(false, 999)]
        [InlineData(true, null)]
        public void GetNewClassForm_ByShowIntegrationCode_ExpectedIntegrationCodeIdIsSet(bool showIntegrationCode, long? expectedIntegrationCode)
        {
            // arrange
            classTypeResolverMock
                .Setup(e => e.GetClassFormConfigurationByClassCategoryId(It.IsAny<ClassCategoryEnum>()))
                .Returns(new ClassFormConfiguration { ShowIntegrationCode = showIntegrationCode });
            classConvertorMock.Setup(e => e.GetNoIntegrationCode()).Returns(999);
            var service = CreateService();

            // act
            var form = service.GetNewClassForm(1, ClassCategoryEnum.PrivateMaterialClass, CurrencyEnum.MXN, CreatePersonHelperWithDefaultPersons());

            // assert
            Assert.Equal(expectedIntegrationCode, form.IntegrationCode);
        }

        [Theory]
        [InlineData(false, ClassConvertor.NoPaymentId)]
        [InlineData(true, null)]
        public void GetNewClassForm_ByShowPayPalKey_ExpectedPayPalKeyIdIsSet(bool showPayPalKey, long? expectedPayPalKeyId)
        {
            // arrange
            classTypeResolverMock
                .Setup(e => e.GetClassFormConfigurationByClassCategoryId(It.IsAny<ClassCategoryEnum>()))
                .Returns(new ClassFormConfiguration { ShowPayPalKey = showPayPalKey });
            var service = CreateService();

            // act
            var form = service.GetNewClassForm(1, ClassCategoryEnum.PrivateMaterialClass, CurrencyEnum.MXN, CreatePersonHelperWithDefaultPersons());

            // assert
            Assert.Equal(expectedPayPalKeyId, form.PayPalKeyId);
        }

        #endregion

        #region GetClassesWithClassFormsByIds() tests

        [Fact]
        public void GetClassesWithClassFormsByIds_ForSpecifiedIds_ClassWithClassFormsCreated()
        {
            // arrange
            var classes = new List<Class>
            {
                new Class(),
                new Class(),
                new Class()
            };
            var classForms = new List<ClassForm>
            {
                new ClassForm(),
                new ClassForm(),
                new ClassForm()
            };
            var classIds = new List<long>();
            classDbMock.Setup(e => e.GetClassesByIds(It.IsAny<IEnumerable<long>>(), It.IsAny<ClassIncludes>())).Returns(classes);
            classConvertorMock.SetupSequence(e => e.ConvertToClassForm(It.IsAny<Class>()))
                .Returns(classForms[0])
                .Returns(classForms[1])
                .Returns(classForms[2]);

            var service = CreateService();

            // act
            var result = service.GetClassesWithClassFormsByIds(classIds);
            Assert.Collection(
                result,
                item =>
                {
                    Assert.Same(classes[0], item.Class);
                    Assert.Same(classForms[0], item.ClassForm);
                },
                item =>
                {
                    Assert.Same(classes[1], item.Class);
                    Assert.Same(classForms[1], item.ClassForm);
                },
                item =>
                {
                    Assert.Same(classes[2], item.Class);
                    Assert.Same(classForms[2], item.ClassForm);
                });
            classDbMock.Verify(e => e.GetClassesByIds(classIds, ClassIncludes.ClassPersons));
        }

        #endregion

        #region InsertClass(ClassForm, EnvironmentTypeEnum) tests

        [Fact]
        public void InsertClass_ClassIsInserted_CurrencyIsGetFromPriceList()
        {
            // arrange
            SetupInsertClassPriceListProfile();
            SetupInsertClass();

            var service = CreateService();

            // act
            service.InsertClass(new ClassForm { PriceListId = 4 }, null);

            // assert
            classDbMock.Verify(e => e.InsertClass(It.Is<Class>(x => x.CurrencyId == CurrencyEnum.MXN)), Times.Once);
            priceListDbMock.Verify(e => e.GetPriceListById(4, PriceListIncludes.None), Times.Once);
        }

        [Fact]
        public void InsertClass_ClassIsInserted_ClassPreferencesGetFromProfile()
        {
            // arrange
            var profile = new Profile
            {
                ClassPreference = new ClassPreference()
            };
            SetupInsertClassPriceListProfile(profile: profile);
            SetupInsertClass();

            var service = CreateService();

            // act
            service.InsertClass(new ClassForm { ProfileId = 5 }, null);

            // assert
            mainMapperMock.Verify(e => e.Map<ClassReportSetting>(profile.ClassPreference), Times.Once);
            profileDbMock.Verify(e => e.GetProfileById(5, ProfileIncludes.ClassPreferenceClassPreferenceExpenses));
        }

        #endregion

        #region InsertClass(ClassForm, CurrencyEnum, ClassPreference, EnvironmentTypeEnum) tests

        [Fact]
        public void InsertClass_ClassForm_InsertedIntoDatabase()
        {
            // arrange
            var classBusiness = new ClassBusiness();
            var classReportSettings = new ClassReportSetting();
            var classStyle = new ClassStyle();
            SetupInsertClass(classBusiness, classReportSettings, classStyle);

            var classToAssert = (Class)null;
            classDbMock.Setup(e => e.InsertClass(It.IsAny<Class>()))
                .Callback((Class c) => classToAssert = c)
                .Returns(333);

            var service = CreateService();

            // act
            var result = service.InsertClass(new ClassForm(), CurrencyEnum.MXN, new ClassPreference(), null);

            // assert
            Assert.Equal(333, result);

            Assert.NotNull(classToAssert);
            Assert.Equal(CurrencyEnum.MXN, classToAssert.CurrencyId);
            Assert.Equal(444, classToAssert.OwnerId);
            Assert.Equal(EnvironmentTypeEnum.Production, classToAssert.EnvironmentTypeId);
            Assert.Same(classBusiness, classToAssert.ClassBusiness);
            Assert.Same(classReportSettings, classToAssert.ClassReportSetting);
            Assert.Same(classStyle, classToAssert.ClassStyle);
        }

        [Fact]
        public void InsertClass_StyleHasHeaderPictureId_PictureClonedAndSaved()
        {
            // arrange
            var classStyle = new ClassStyle
            {
                HeaderPictureId = 200,
            };
            SetupInsertClass(classStyle: classStyle);
            coreFileServiceMock.Setup(e => e.CloneFile(It.IsAny<long>(), It.IsAny<string>())).Returns(201);

            var service = CreateService();

            // act
            service.InsertClass(new ClassForm(), CurrencyEnum.MXN, new ClassPreference(), null);

            // assert
            coreFileServiceMock.Verify(e => e.CloneFile(200, string.Format(ClassService.HeaderPictureNamePattern, "new-class")), Times.Once());
            classDbMock.Verify(e => e.InsertClass(It.Is<Class>(x => x.ClassStyle.HeaderPictureId == 201)), Times.Once);
        }

        [Fact]
        public void InsertClass_ClassIsIntegrated_IntegrationProviderAttachEntity()
        {
            // arrange
            var dbClass = new Class
            {
                IntegrationEntityId = 50,
                IntegrationTypeId = IntegrationTypeEnum.WebExProgram
            };
            SetupInsertClass(dbClass: dbClass);
            classDbMock.Setup(e => e.InsertClass(It.IsAny<Class>())).Returns(333);

            var service = CreateService();

            // act
            service.InsertClass(new ClassForm(), CurrencyEnum.MXN, new ClassPreference(), null);

            // assert
            integrationProviderMock.Verify(e => e.AttachEntity(EntityTypeEnum.MainClass, 333, IntegrationTypeEnum.WebExProgram, 50, false), Times.Once);
        }

        [Fact]
        public void InsertClass_ClassInserted_ClassCreatedEventRaised()
        {
            // arrange
            SetupInsertClass();
            classDbMock.Setup(e => e.InsertClass(It.IsAny<Class>())).Returns(333);

            var service = CreateService();

            // act
            service.InsertClass(new ClassForm(), CurrencyEnum.MXN, new ClassPreference(), null);

            // assert
            eventDispatcherMock.Verify(e => e.Dispatch(It.Is<ClassCreatedEvent>(x => x.ClassId == 333)), Times.Once());
        }

        #endregion

        #region UpdateClass(ClassForm, Class, bool)

        [Fact]
        public void UpdateClass_ForGivenPriceList_CurrencyIdIsUpdated()
        {
            // assert
            classConvertorMock.Setup(e => e.ConvertToClass(It.IsAny<ClassForm>())).Returns(new Class());
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(new PriceList { CurrencyId = CurrencyEnum.MXN });

            var service = CreateService();

            // act
            service.UpdateClass(new ClassForm { PriceListId = 4 }, new Class(), true);

            // assert
            priceListDbMock.Verify(e => e.GetPriceListById(4, PriceListIncludes.None), Times.Once);
            classDbMock.Verify(e => e.UpdateClass(It.Is<Class>(x => x.CurrencyId == CurrencyEnum.MXN), true), Times.Once);
        }

        #endregion

        #region UpdateClass(ClassForm, CurrencyEnum, Class, bool) tests

        [Fact]
        public void UpdateClass_ClassForm_ConvertedAndSaved()
        {
            // assert
            var form = new ClassForm();
            var cls = new Class();
            classConvertorMock.Setup(e => e.ConvertToClass(It.IsAny<ClassForm>())).Returns(cls);

            var service = CreateService();

            // act
            service.UpdateClass(form, CurrencyEnum.MXN, new Class(), true);

            // assert
            classConvertorMock.Verify(e => e.ConvertToClass(form), Times.Once);
            classDbMock.Verify(e => e.UpdateClass(It.Is<Class>(x => x.CurrencyId == CurrencyEnum.MXN), true));
        }

        [Fact]
        public void UpdateClass_ForFullyEditable_AttachEntityIsCalled()
        {
            // assert
            var form = new ClassForm
            {
                ClassId = 1
            };
            var cls = new Class
            {
                IntegrationTypeId = IntegrationTypeEnum.WebExProgram,
                IntegrationEntityId = 11
            };
            classConvertorMock.Setup(e => e.ConvertToClass(It.IsAny<ClassForm>())).Returns(cls);

            var service = CreateService();

            // act
            service.UpdateClass(form, CurrencyEnum.MXN, new Class(), true);

            // assert
            integrationProviderMock.Verify(e => e.AttachEntity(EntityTypeEnum.MainClass, 1, IntegrationTypeEnum.WebExProgram, 11, true), Times.Once);
        }

        [Fact]
        public void UpdateClass_ForNotFullyEditable_AttachEntityIsNotCalled()
        {
            // arrange
            classConvertorMock.Setup(e => e.ConvertToClass(It.IsAny<ClassForm>())).Returns(new Class());

            var service = CreateService();

            // act
            service.UpdateClass(new ClassForm(), CurrencyEnum.MXN, new Class(), false);

            // assert
            integrationProviderMock.Verify(
                e => e.AttachEntity(It.IsAny<EntityTypeEnum>(), It.IsAny<long>(), It.IsAny<IntegrationTypeEnum>(), It.IsAny<long?>(), It.IsAny<bool>()),
                Times.Never);
        }

        [Fact]
        public void UpdateClass_WhenPersonChangeEventCreated_EventIsRaised()
        {
            // arrange
            var origCls = new Class();
            var updateClass = new Class();
            var personChangedEvent = new ClassPersonsChangedEvent(1);
            classConvertorMock.Setup(e => e.ConvertToClass(It.IsAny<ClassForm>())).Returns(updateClass);
            classEventFactoryMock.Setup(e => e.CreateClassPersonsChangedEventWhenChanged(It.IsAny<Class>(), It.IsAny<Class>())).Returns(personChangedEvent);

            var service = CreateService();

            // act
            service.UpdateClass(new ClassForm(), CurrencyEnum.MXN, origCls, false);

            // assert
            classEventFactoryMock.Verify(e => e.CreateClassPersonsChangedEventWhenChanged(origCls, updateClass), Times.Once);
            eventDispatcherMock.Verify(e => e.Dispatch(personChangedEvent), Times.Once);
        }

        [Fact]
        public void UpdateClass_WhenPersonsNotChanged_EventIsNorRaised()
        {
            // arrange
            classConvertorMock.Setup(e => e.ConvertToClass(It.IsAny<ClassForm>())).Returns(new Class());
            classEventFactoryMock.Setup(e => e.CreateClassPersonsChangedEventWhenChanged(It.IsAny<Class>(), It.IsAny<Class>())).Returns((ClassPersonsChangedEvent)null);

            var service = CreateService();

            // act
            service.UpdateClass(new ClassForm(), CurrencyEnum.MXN, new Class(), false);

            // assert
            eventDispatcherMock.Verify(e => e.Dispatch(It.IsAny<ClassPersonsChangedEvent>()), Times.Never());
        }

        #endregion

        #region private methods

        private void SetupInsertClassPriceListProfile(Profile profile = null, PriceList priceList = null)
        {
            profile = profile ?? new Profile
            {
                ClassPreference = new ClassPreference()
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);

            priceList = priceList ?? new PriceList
            {
                CurrencyId = CurrencyEnum.MXN
            };
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(priceList);
        }

        private void SetupInsertClass(ClassBusiness classBusiness = null, ClassReportSetting classReportSetting = null, ClassStyle classStyle = null, Class dbClass = null)
        {
            dbClass = dbClass ?? new Class();
            classBusiness = classBusiness ?? new ClassBusiness();
            classReportSetting = classReportSetting ?? new ClassReportSetting();
            classStyle = classStyle ?? new ClassStyle();
            classConvertorMock.Setup(e => e.ConvertToClass(It.IsAny<ClassForm>())).Returns(dbClass);
            classBusinessConvertorMock.Setup(e => e.CreateClassBusinessByClassPreference(It.IsAny<Class>(), It.IsAny<ClassPreference>())).Returns(classBusiness);
            mainMapperMock.Setup(e => e.Map<ClassReportSetting>(It.IsAny<object>())).Returns(classReportSetting);
            classStyleConvertorMock.Setup(e => e.CreateClassStyleByClassPreference(It.IsAny<ClassPreference>(), It.IsAny<bool>())).Returns(classStyle);
            identityResolverMock.Setup(e => e.GetOwnerId()).Returns(444);
        }

        private IPersonHelper CreatePersonHelperWithDefaultPersons()
        {
            var result = new Mock<IPersonHelper>();
            result.Setup(e => e.GetDefaultPersonId(PersonRoleTypeEnum.Coordinator)).Returns(101);
            result.Setup(e => e.GetDefaultPersonId(PersonRoleTypeEnum.DistanceDoordinator)).Returns(102);
            result.Setup(e => e.GetDefaultPersonId(PersonRoleTypeEnum.GuestInstructor)).Returns(103);
            result.Setup(e => e.GetDefaultPersonIds(PersonRoleTypeEnum.Instructor)).Returns(new List<long> { 104, 105 });
            result.Setup(e => e.GetDefaultPersonIds(PersonRoleTypeEnum.ApprovedStaff)).Returns(new List<long> { 106, 107 });
            return result.Object;
        }

        private ClassService CreateService()
        {
            return new ClassService(
                classConvertorMock.Object,
                classBusinessConvertorMock.Object,
                classStyleConvertorMock.Object,
                identityResolverMock.Object,
                mainMapperMock.Object,
                coreFileServiceMock.Object,
                classDbMock.Object,
                integrationProviderMock.Object,
                eventDispatcherMock.Object,
                priceListDbMock.Object,
                profileDbMock.Object,
                classEventFactoryMock.Object,
                classTypeResolverMock.Object);
        }

        #endregion
    }
}
