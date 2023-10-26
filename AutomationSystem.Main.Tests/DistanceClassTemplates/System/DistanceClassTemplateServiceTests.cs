using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Core.Certificates.System;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Model;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.DistanceProfiles.Data.Models;
using AutomationSystem.Main.Model;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.System
{
    public class DistanceClassTemplateServiceTests
    {
        private const long ProfileId = 1;
        private const long ClassId = 100;
        private const long ClassActionId = 200;
        private const long DistanceProfileId = 11;
        private const long DistanceTemplateId = 22;
        private const long TranslationCode = 2001;
        private const string CertificateRootPath = "path";

        private readonly Mock<IDistanceClassTemplateClassDatabaseLayer> distanceTemplateClassDbMock;
        private readonly Mock<IDistanceClassTemplateDatabaseLayer> distanceTemplateDbMock;
        private readonly Mock<IDistanceProfileDatabaseLayer> distanceProfileDbMock;
        private readonly Mock<ILanguageTranslationProvider> languageTranslationProviderMock;
        private readonly Mock<IClassService> classServiceMock;
        private readonly Mock<IClassActionService> classActionServiceMock;
        private readonly Mock<ICertificateService> certificateServiceMock;
        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public DistanceClassTemplateServiceTests()
        {
            distanceTemplateClassDbMock = new Mock<IDistanceClassTemplateClassDatabaseLayer>();
            distanceTemplateDbMock = new Mock<IDistanceClassTemplateDatabaseLayer>();
            distanceProfileDbMock = new Mock<IDistanceProfileDatabaseLayer>();
            languageTranslationProviderMock = new Mock<ILanguageTranslationProvider>();
            classServiceMock = new Mock<IClassService>();
            classActionServiceMock = new Mock<IClassActionService>();
            certificateServiceMock = new Mock<ICertificateService>();
            classDbMock = new Mock<IClassDatabaseLayer>();

            languageTranslationProviderMock.Setup(e => e.GetTranslationCode(It.IsAny<LanguageEnum>(), It.IsAny<LanguageEnum?>())).Returns(TranslationCode);
        }

        #region PopulateDistanceClassesForDistanceProfile() tests

        [Fact]
        public void PopulateDistanceClassesForDistanceProfile_ExpectedIncludesPassedWhenDistanceProfileLoaded()
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(new DistanceProfile());
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new List<DistanceClassTemplate>());
            var service = CreateService();

            // act
            service.PopulateDistanceClassesForDistanceProfile(DistanceProfileId);

            // assert
            distanceProfileDbMock.Verify(e => e.GetDistanceProfileById(DistanceProfileId, DistanceProfileIncludes.ProfileClassPreference), Times.Once);
        }

        [Fact]
        public void PopulateDistanceClassesForDistanceProfile_ExpectedFilterAndIncludesPassedWhenDistanceProfileLoaded()
        {
            // arrange
            var expectedFromRegistrationEnd = DateTime.Today.AddDays(1);
            var expectedExcludeIds = new List<long> {1, 2, 3};
            DistanceClassTemplateFilter filter = null;
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(new DistanceProfile());
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Callback((DistanceClassTemplateFilter f, DistanceClassTemplateIncludes i) => filter = f)
                .Returns(new List<DistanceClassTemplate>());
            distanceTemplateClassDbMock.Setup(e => e.GetFilledDistanceTemplateIdsForDistanceProfileId(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(expectedExcludeIds);

            var service = CreateService();

            // act
            service.PopulateDistanceClassesForDistanceProfile(DistanceProfileId);

            // assert
            distanceTemplateDbMock.Verify(
                e => e.GetDistanceClassTemplatesByFilter(
                    It.IsAny<DistanceClassTemplateFilter>(),
                    DistanceClassTemplateIncludes.DistanceClassTemplatePersons),
                Times.Once);
            distanceTemplateClassDbMock.Verify(e => e.GetFilledDistanceTemplateIdsForDistanceProfileId(DistanceProfileId, expectedFromRegistrationEnd), Times.Once);

            Assert.NotNull(filter);
            Assert.Equal(expectedFromRegistrationEnd, filter.FromRegistrationEnd);
            Assert.Equal(expectedExcludeIds, filter.ExcludeIds);
            Assert.Equal(DistanceClassTemplateState.Approved, filter.TemplateState);
        }

        [Fact]
        public void PopulateDistanceClassForDistanceProfile_ForTemplateAndProfile_NewDistanceClassIsCreated()
        {
            // arrange
            var distanceProfile = CreateDistanceProfile();
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(distanceProfile);
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new List<DistanceClassTemplate> { CreateDistanceClassTemplate() });

            var newClassForm = new ClassForm();
            classServiceMock.Setup(e => e.GetNewClassForm(It.IsAny<long>(), It.IsAny<ClassCategoryEnum>(), It.IsAny<CurrencyEnum>(), It.IsAny<IPersonHelper>()))
                .Returns(newClassForm);

            var formToAssert = (ClassForm)null;
            classServiceMock.Setup(e => e.InsertClass(It.IsAny<ClassForm>(), It.IsAny<CurrencyEnum>(), It.IsAny<ClassPreference>(), It.IsAny<EnvironmentTypeEnum?>()))
                .Callback((ClassForm f, CurrencyEnum c, ClassPreference cp, EnvironmentTypeEnum? e) => formToAssert = f);

            var service = CreateService();

            // act
            service.PopulateDistanceClassesForDistanceProfile(DistanceProfileId);

            // assert
            Assert.NotNull(formToAssert);
            AssertClassFormForDistanceProfileParams(formToAssert);
            AssertClassFormForDistanceTemplate(formToAssert);
            classServiceMock.Verify(e => e.GetNewClassForm(ProfileId, ClassCategoryEnum.DistanceClass, DistanceClassTemplateService.DistanceClassCurrency, It.IsAny<IPersonHelper>()), Times.Once);
            classServiceMock.Verify(e => e.InsertClass(newClassForm, DistanceClassTemplateService.DistanceClassCurrency, distanceProfile.Profile.ClassPreference, null), Times.Once);
        }

        [Fact]
        public void PopulateDistanceClassForDistanceProfile_ForNewlyCreatedClass_RelationWithProfileAndTemplateCreated()
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(CreateDistanceProfile());
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new List<DistanceClassTemplate> { CreateDistanceClassTemplate() });
            classServiceMock.Setup(e => e.GetNewClassForm(It.IsAny<long>(), It.IsAny<ClassCategoryEnum>(), It.IsAny<CurrencyEnum>(), It.IsAny<IPersonHelper>()))
                .Returns(new ClassForm());
            classServiceMock.Setup(e => e.InsertClass(It.IsAny<ClassForm>(), It.IsAny<CurrencyEnum>(), It.IsAny<ClassPreference>(), It.IsAny<EnvironmentTypeEnum?>()))
                .Returns(ClassId);

            var service = CreateService();

            // act
            service.PopulateDistanceClassesForDistanceProfile(DistanceProfileId);

            // assert
            distanceTemplateClassDbMock.Verify(e => e.InsertDistanceClassTemplateClass(DistanceTemplateId, DistanceProfileId, ClassId), Times.Once);
        }

        #endregion

        #region PopulateDistanceClassesForTemplate() tests

        [Fact]
        public void PopulateDistanceClassesForTemplate_ExpectedIncludesPassedWhenDistanceTemplateLoaded()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            distanceProfileDbMock.Setup(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(),
                    It.IsAny<DistanceProfileIncludes>())).Returns(new List<DistanceProfile>());

            var service = CreateService();

            // act
            service.PopulateDistanceClassesForDistanceTemplate(DistanceTemplateId);

            // assert
            distanceTemplateDbMock.Verify(e => e.GetDistanceClassTemplateById(DistanceTemplateId, DistanceClassTemplateIncludes.DistanceClassTemplatePersons), Times.Once);
        }

        [Fact]
        public void PopulateDistanceClassForTemplate_ExpectedIncludesAndFilterPassedWhenDistanceProfilesLoaded()
        {
            // arrange
            var expectedExcludeIds = new List<long> { 1, 2, 3 };
            DistanceProfileFilter filter = null;
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            distanceTemplateClassDbMock.Setup(e => e.GetFilledDistanceProfileIdsForDistanceClassTemplateId(It.IsAny<long>())).Returns(expectedExcludeIds);
            distanceProfileDbMock.Setup(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(), It.IsAny<DistanceProfileIncludes>()))
                .Callback((DistanceProfileFilter f, DistanceProfileIncludes i) => filter = f)
                .Returns(new List<DistanceProfile>());

            var service = CreateService();

            // act
            service.PopulateDistanceClassesForDistanceTemplate(DistanceTemplateId);

            // assert
            distanceProfileDbMock.Verify(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(), DistanceProfileIncludes.ProfileClassPreference), Times.Once);
            distanceTemplateClassDbMock.Verify(e => e.GetFilledDistanceProfileIdsForDistanceClassTemplateId(DistanceTemplateId), Times.Once);

            Assert.NotNull(filter);
            Assert.True(filter.IsActive);
            Assert.Equal(expectedExcludeIds, filter.ExcludeIds);
        }

        [Fact]
        public void PopulateDistanceClassesForDistanceTemplate_FormTemplateAndProfile_NewDistanceClassIsCreated()
        {
            // arrange
            var distanceProfile = CreateDistanceProfile();
            distanceProfileDbMock.Setup(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(), It.IsAny<DistanceProfileIncludes>()))
                .Returns(new List<DistanceProfile> {distanceProfile});
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(CreateDistanceClassTemplate());

            var newClassForm = new ClassForm();
            classServiceMock.Setup(e => e.GetNewClassForm(It.IsAny<long>(), It.IsAny<ClassCategoryEnum>(), It.IsAny<CurrencyEnum>(), It.IsAny<IPersonHelper>()))
                .Returns(newClassForm);

            var service = CreateService();

            // act
            service.PopulateDistanceClassesForDistanceTemplate(DistanceTemplateId);

            // assert
            Assert.NotNull(newClassForm);
            AssertClassFormForDistanceProfileParams(newClassForm);
            AssertClassFormForDistanceTemplate(newClassForm);
            classServiceMock.Verify(e => e.GetNewClassForm(ProfileId, ClassCategoryEnum.DistanceClass, DistanceClassTemplateService.DistanceClassCurrency, It.IsAny<IPersonHelper>()), Times.Once);
            classServiceMock.Verify(e => e.InsertClass(newClassForm, DistanceClassTemplateService.DistanceClassCurrency, distanceProfile.Profile.ClassPreference, null), Times.Once);
        }

        [Fact]
        public void PopulateDistanceClassesForDistanceTemplate_ForNewlyCreatedClass_RelationWithProfileAndTemplateCreated()
        {
            // arrange
            var distanceProfile = CreateDistanceProfile();
            distanceProfileDbMock.Setup(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(), It.IsAny<DistanceProfileIncludes>()))
                .Returns(new List<DistanceProfile> { distanceProfile });
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(CreateDistanceClassTemplate());
            classServiceMock.Setup(e => e.GetNewClassForm(It.IsAny<long>(), It.IsAny<ClassCategoryEnum>(), It.IsAny<CurrencyEnum>(), It.IsAny<IPersonHelper>()))
                .Returns(new ClassForm());
            classServiceMock.Setup(e => e.InsertClass(It.IsAny<ClassForm>(), It.IsAny<CurrencyEnum>(), It.IsAny<ClassPreference>(), It.IsAny<EnvironmentTypeEnum?>()))
                .Returns(ClassId);

            var service = CreateService();

            // act
            service.PopulateDistanceClassesForDistanceTemplate(DistanceTemplateId);

            // assert
            distanceTemplateClassDbMock.Verify(e => e.InsertDistanceClassTemplateClass(DistanceTemplateId, DistanceProfileId, ClassId), Times.Once);
        }

        #endregion

        #region PropagateChangesToDistanceClasses() tests

        [Fact]
        public void PropagateChangesToDistanceClasses_ForClass_ClassesAreUpdatedByDistanceTemplate()
        {
            // arrange
            var distanceTemplate = CreateDistanceClassTemplate();
            var classIds = new List<long>();
            var classesWithClassForms = new List<ClassWithClassForm>
            {
                new ClassWithClassForm(new Class(), new ClassForm())
            };
            distanceTemplateClassDbMock.Setup(e => e.GetFilledClassIdsByDistanceClassTemplateId(It.IsAny<long>())).Returns(classIds);
            classServiceMock.Setup(e => e.GetClassesWithClassFormsByIds(It.IsAny<List<long>>())).Returns(classesWithClassForms);

            var formToUpdate = (ClassForm)null;
            classServiceMock.Setup(e => e.UpdateClass(It.IsAny<ClassForm>(), It.IsAny<CurrencyEnum>(), It.IsAny<Class>(), It.IsAny<bool>()))
                .Callback((ClassForm f, CurrencyEnum cu, Class cl, bool i) => formToUpdate = f);

            var service = CreateService();

            // act
            service.PropagateChangesToDistanceClasses(distanceTemplate);

            // assert
            distanceTemplateClassDbMock.Verify(e => e.GetFilledClassIdsByDistanceClassTemplateId(DistanceTemplateId), Times.Once);
            classServiceMock.Verify(e => e.GetClassesWithClassFormsByIds(classIds), Times.Once);
            classServiceMock.Verify(
                e => e.UpdateClass(
                    classesWithClassForms[0].ClassForm,
                    DistanceClassTemplateService.DistanceClassCurrency,
                    classesWithClassForms[0].Class, 
                    true),
                Times.Once);

            Assert.NotNull(formToUpdate);
            AssertClassFormForDistanceTemplate(formToUpdate);
        }

        [Fact]
        public void PropagateChangesToDistanceClasses_TemplateWithoutPersons_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceTemplate = CreateDistanceClassTemplate();
            distanceTemplate.DistanceClassTemplatePersons = null;
            var classesWithClassForms = new List<ClassWithClassForm>
            {
                new ClassWithClassForm(new Class(), new ClassForm())
            };

            distanceTemplateClassDbMock.Setup(e => e.GetFilledClassIdsByDistanceClassTemplateId(It.IsAny<long>())).Returns(new List<long>());
            classServiceMock.Setup(e => e.GetClassesWithClassFormsByIds(It.IsAny<List<long>>())).Returns(classesWithClassForms);

            var service = CreateService();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => service.PropagateChangesToDistanceClasses(distanceTemplate));
        }

        #endregion

        #region CompleteDistanceClassTemplate() tests

        [Fact]
        public void CompleteDistanceClassTemplate_ExpectedIncludesPassedWhenTemplatesLoaded()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new DistanceClassTemplate());
            classDbMock.Setup(e => e.GetClassesByIds(It.IsAny<IEnumerable<long>>(), It.IsAny<ClassIncludes>()))
                .Returns(new List<Class>());

            var service = CreateService();

            // act
            service.CompleteDistanceClassTemplate(DistanceTemplateId, CertificateRootPath);

            // assert
            distanceTemplateDbMock.Verify(e => e.GetDistanceClassTemplateById(DistanceTemplateId, DistanceClassTemplateIncludes.DistanceClassTemplateClasses), Times.Once);
        }

        [Fact]
        public void CompleteDistanceClassTemplate_DistanceTemplateWithClasses_ExpectedClassesLoaded()
        {
            // arrange
            var distanceTemplate = new DistanceClassTemplate
            {
                DistanceClassTemplateClasses = new List<DistanceClassTemplateClass>
                {
                    new DistanceClassTemplateClass {ClassId = 1},
                    new DistanceClassTemplateClass {ClassId = 2},
                    new DistanceClassTemplateClass {ClassId = 3}
                }
            };
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(distanceTemplate);
            classDbMock.Setup(e => e.GetClassesByIds(It.IsAny<IEnumerable<long>>(), It.IsAny<ClassIncludes>()))
                .Returns(new List<Class>());

            var service = CreateService();

            // act
            service.CompleteDistanceClassTemplate(DistanceTemplateId, CertificateRootPath);

            // assert
            classDbMock.Verify(e => e.GetClassesByIds(new List<long> { 1, 2, 3 }, ClassIncludes.None), Times.Once);
        }

        [Fact]
        public void CompleteDistanceClassTemplate_ClassInTerminationState_ClassSkippedReturnSuccess()
        {
            // arrange
            var cls = new Class
            {
                ClassId = ClassId,
                IsFinished = true,
            };
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new DistanceClassTemplate());
            classDbMock.Setup(e => e.GetClassesByIds(It.IsAny<IEnumerable<long>>(), It.IsAny<ClassIncludes>()))
                .Returns(new List<Class> { cls });

            var service = CreateService();

            // act
            var result = service.CompleteDistanceClassTemplate(DistanceTemplateId, CertificateRootPath);

            // assert
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { ClassId }, result.SkippedClasses);
            certificateServiceMock.Verify(e => e.GenerateCertificates(It.IsAny<string>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void CompleteDistanceClassTemplate_ClassCanBeProcessed_CertificateIsGeneratedForClass()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new DistanceClassTemplate());
            classDbMock.Setup(e => e.GetClassesByIds(It.IsAny<IEnumerable<long>>(), It.IsAny<ClassIncludes>()))
                .Returns(new List<Class> { new Class { ClassId = ClassId } });
            
            var service = CreateService();

            // act
            service.CompleteDistanceClassTemplate(DistanceTemplateId, CertificateRootPath);

            // assert
            certificateServiceMock.Verify(e => e.GenerateCertificates(CertificateRootPath, ClassId), Times.Once);
        }

        [Fact]
        public void CompleteDistanceClassTemplate_ClassCanBeProcessed_ClassIsCompletedReturnsSuccess()
        {
            // arrange
            var cls = new Class
            {
                ClassId = ClassId
            };
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new DistanceClassTemplate());
            classDbMock.Setup(e => e.GetClassesByIds(It.IsAny<IEnumerable<long>>(), It.IsAny<ClassIncludes>()))
                .Returns(new List<Class> { cls });
            classActionServiceMock.Setup(e => e.CreateClassAction(It.IsAny<Class>(), It.IsAny<ClassActionTypeEnum>()))
                .Returns(ClassActionId);

            var service = CreateService();

            // act
            var result = service.CompleteDistanceClassTemplate(DistanceTemplateId, CertificateRootPath);

            // assert
            Assert.True(result.IsSuccess);
            Assert.Equal(DistanceTemplateId, result.DistanceClassTemplateId);
            Assert.Equal(new[] { ClassId }, result.CompletedClasses);
            classActionServiceMock.Verify(e => e.CreateClassAction(cls, ClassActionTypeEnum.Completion), Times.Once);
            classActionServiceMock.Verify(e => e.ProcessClassAction(ClassActionId), Times.Once);
        }

        [Fact]
        public void CompleteDistanceClassTemplate_CompletionOfClassCausesException_ReturnsFail()
        {
            // assert
            var exception = new Exception();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new DistanceClassTemplate());
            classDbMock.Setup(e => e.GetClassesByIds(It.IsAny<IEnumerable<long>>(), It.IsAny<ClassIncludes>()))
                .Returns(new List<Class> { new Class { ClassId = ClassId } });
            certificateServiceMock.Setup(e => e.GenerateCertificates(It.IsAny<string>(), It.IsAny<long>()))
                .Throws(exception);

            var service = CreateService();

            // act
            var result = service.CompleteDistanceClassTemplate(DistanceTemplateId, CertificateRootPath);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Same(exception, result.Exception);
            Assert.Equal(ClassId, result.CorruptedClassId);
        }

        #endregion

        #region private methods

        private DistanceProfile CreateDistanceProfile()
        {
            return new DistanceProfile
            {
                DistanceProfileId = DistanceProfileId,
                ProfileId = ProfileId,
                PriceListId = 100,
                DistanceCoordinatorId = 101,
                PayPalKeyId = 102,

                Profile = new Profile
                {
                    ClassPreference = new ClassPreference()
                }
            };
        }

        private void AssertClassFormForDistanceProfileParams(ClassForm form)
        {
            Assert.Equal(100, form.PriceListId);
            Assert.Equal(101, form.CoordinatorId);
            Assert.Equal(102, form.PayPalKeyId);
        }

        private DistanceClassTemplate CreateDistanceClassTemplate()
        {
            return new DistanceClassTemplate
            {
                DistanceClassTemplateId = DistanceTemplateId,
                ClassTypeId = ClassTypeEnum.Basic2,
                Location = "Location",
                RegistrationStart = new DateTime(2020, 1, 1),
                RegistrationEnd = new DateTime(2020, 1, 2),
                EventStart = new DateTime(2020, 1, 3),
                EventEnd = new DateTime(2020, 1, 4),
                GuestInstructorId = 103,
                DistanceClassTemplatePersons = new List<DistanceClassTemplatePerson>
                {
                    CreateInstructor(104),
                    CreateInstructor(105)
                }
            };
        }

        private DistanceClassTemplatePerson CreateInstructor(long personId)
        {
            return new DistanceClassTemplatePerson
            {
                RoleTypeId = PersonRoleTypeEnum.Instructor,
                PersonId = personId
            };
        }

        private void AssertClassFormForDistanceTemplate(ClassForm form)
        {
            Assert.Equal(ClassTypeEnum.Basic2, form.ClassTypeId);
            Assert.Equal("Location", form.Location);
            Assert.Equal(new DateTime(2020, 1, 1), form.RegistrationStart);
            Assert.Equal(new DateTime(2020, 1, 2), form.RegistrationEnd);
            Assert.Equal(new DateTime(2020, 1, 3), form.EventStart);
            Assert.Equal(new DateTime(2020, 1, 4), form.EventEnd);
            Assert.Equal(TranslationCode, form.TranslationCode);
            Assert.Equal(103, form.GuestInstructorId);
            Assert.Equal(new List<long> { 104, 105 }, form.InstructorIds);
        }

        private DistanceClassTemplateService CreateService()
        {
            return new DistanceClassTemplateService(
                distanceTemplateClassDbMock.Object,
                distanceTemplateDbMock.Object,
                distanceProfileDbMock.Object,
                languageTranslationProviderMock.Object,
                classServiceMock.Object, 
                classActionServiceMock.Object,
                certificateServiceMock.Object,
                classDbMock.Object);
        }

        #endregion
    }
}
