using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models.Events;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Model;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateAdministrationTests
    {
        private readonly Mock<IDistanceClassTemplateDatabaseLayer> distanceTemplateDbMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<IPersonDatabaseLayer> personDbMock;
        private readonly Mock<IDistanceClassTemplateHelper> templateHelperMock;
        private readonly Mock<IDistanceClassTemplateFactory> factoryMock;
        private readonly Mock<IEventDispatcher> eventDispatcherMock;
        private readonly Mock<IDistanceClassTemplateOperationChecker> templateOperationCheckerMock;

        public DistanceClassTemplateAdministrationTests()
        {
            distanceTemplateDbMock = new Mock<IDistanceClassTemplateDatabaseLayer>();
            mainMapperMock = new Mock<IMainMapper>();
            personDbMock = new Mock<IPersonDatabaseLayer>();
            templateHelperMock = new Mock<IDistanceClassTemplateHelper>();
            factoryMock = new Mock<IDistanceClassTemplateFactory>();
            eventDispatcherMock = new Mock<IEventDispatcher>();
            templateOperationCheckerMock = new Mock<IDistanceClassTemplateOperationChecker>();
        }

        #region GetDistanceClassTemplatePageModel() tests
        [Fact]
        public void GetDistanceClassTemplatePageModel_FilterIsNull_ReturnsPageModelWithoutItems()
        {
            // arrange
            var filter = new DistanceClassTemplateFilter();
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplatePageModel(filter, false);

            // assert
            Assert.False(result.WasSearched);
            Assert.NotNull(result.Filter);
            Assert.Collection(result.TemplateStates,
                item => Assert.Equal(DistanceClassTemplateState.New, item),
                item => Assert.Equal(DistanceClassTemplateState.Approved, item),
                item => Assert.Equal(DistanceClassTemplateState.Completed,item));
        }

        [Fact]
        public void GetDistanceClassTemplatePageModel_ExpectedFilterAndIncludesArePassToGetDistanceClassTemplatesByFilter_ReturnsDistanceClassTemplatePageModel()
        {
            // arrange
            var filter = new DistanceClassTemplateFilter();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new List<DistanceClassTemplate>());
            var admin = CreateAdmin();

            // act
            admin.GetDistanceClassTemplatePageModel(filter, true);

            // assert
            distanceTemplateDbMock.Verify(e => e.GetDistanceClassTemplatesByFilter(
                filter,
                DistanceClassTemplateIncludes.ClassType
                | DistanceClassTemplateIncludes.DistanceClassTemplatePersons), Times.Once);
        }

        [Fact]
        public void GetDistanceClassTemplatePageModel_DistanceClassTemplatesAreMappedToListItems_ReturnsDistanceClassPageModel()
        {
            // arrange
            var template = CreateDistanceClassTemplate();
            var templates = new List<DistanceClassTemplate>
            {
                template
            };
            var listItem = new DistanceClassTemplateListItem();
            var ids = new HashSet<long> { 1, 2, 3 };
            var instructors = new List<string> { "FirstInstructor", "SecondInstructor" };
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(templates);
            templateHelperMock.Setup(e => e.GetDistanceClassTemplatePersonsIds(It.IsAny<List<DistanceClassTemplate>>())).Returns(ids);
            personDbMock.Setup(e => e.GetMinimizedPersonsByIds(It.IsAny<List<long>>())).Returns(new List<PersonMinimized>());
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateListItem>(It.IsAny<DistanceClassTemplate>())).Returns(listItem);
            templateHelperMock.Setup(e => e.GetDistanceClassTemplateInstructorsWithGuestInstructor(It.IsAny<DistanceClassTemplate>(), It.IsAny<IPersonHelper>())).Returns(instructors);
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplatePageModel(new DistanceClassTemplateFilter(), true);

            // assert
            Assert.Collection(
                result.Items,
                item => Assert.Collection(
                    item.Instructors,
                    instructor => Assert.Equal("FirstInstructor", instructor),
                    instructor => Assert.Equal("SecondInstructor", instructor)));
            templateHelperMock.Verify(e => e.GetDistanceClassTemplatePersonsIds(templates), Times.Once);
            personDbMock.Verify(e => e.GetMinimizedPersonsByIds(ids), Times.Once);
            mainMapperMock.Verify(e => e.Map<DistanceClassTemplateListItem>(template), Times.Once);
        }
        #endregion

        #region GetDistanceClassTemplateDetailById() tests
        [Fact]
        public void GetDistanceClassTemplateDetailById_IncludesPassToGetDistanceClassTemplateById_ReturnsDistanceClassTemplateDetail()
        {
            // arrange
            var template = CreateDistanceClassTemplate();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateDetail>(It.IsAny<DistanceClassTemplate>())).Returns(new DistanceClassTemplateDetail());
            var admin = CreateAdmin();

            // act
            admin.GetDistanceClassTemplateDetailById(12);

            // assert
            distanceTemplateDbMock.Verify(e => e.GetDistanceClassTemplateById(
                12,
                DistanceClassTemplateIncludes.ClassType
                | DistanceClassTemplateIncludes.DistanceClassTemplatePersons),
                Times.Once);
        }

        [Fact]
        public void GetDistanceClassTemplateDetailById_ForDistanceClassTemplateAndPersonHelper_ReturnsGuestInstructorAndInstructors()
        {
            // arrange
            var template = CreateDistanceClassTemplate();
            var ids = new HashSet<long> { 1, 2, 3 };
            var personsMinimized = new List<PersonMinimized>
            {
                new PersonMinimized
                {
                    FirstName = "First",
                    LastName = "Instructor",
                    PersonId = 1,
                    AssignedRoles = new HashSet<PersonRoleTypeEnum> { PersonRoleTypeEnum.Instructor },
                    Name = "First Instructor"
                },
                new PersonMinimized
                {
                    FirstName = "Second",
                    LastName = "Instructor",
                    PersonId = 2,
                    AssignedRoles = new HashSet<PersonRoleTypeEnum> { PersonRoleTypeEnum.Instructor },
                    Name = "Second Instructor"
                },
                new PersonMinimized
                {
                    FirstName = "Guest",
                    LastName = "Instructor",
                    PersonId = 3,
                    AssignedRoles = new HashSet<PersonRoleTypeEnum> { PersonRoleTypeEnum.GuestInstructor },
                    Name = "Guest Instructor"
                }
            };
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateDetail>(It.IsAny<DistanceClassTemplate>())).Returns(new DistanceClassTemplateDetail());
            templateHelperMock.Setup(e => e.GetDistanceClassTemplatePersonIds(It.IsAny<DistanceClassTemplate>())).Returns(ids);
            personDbMock.Setup(e => e.GetMinimizedPersonsByIds(It.IsAny<HashSet<long>>())).Returns(personsMinimized);
            templateHelperMock.Setup(e => e.GetDistanceClassTemplateInstructors(It.IsAny<DistanceClassTemplate>(), It.IsAny<IPersonHelper>()))
                .Returns(new List<string> { "First Instructor", "Second Instructor" });
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplateDetailById(12);

            // assert
            Assert.Equal("Guest Instructor", result.GuestInstructor);
            Assert.Collection(result.Instructors,
                item => Assert.Equal("First Instructor", item),
                item => Assert.Equal("Second Instructor", item));
            mainMapperMock.Verify(e => e.Map<DistanceClassTemplateDetail>(template), Times.Once);
            templateHelperMock.Verify(e => e.GetDistanceClassTemplatePersonIds(template), Times.Once);
            personDbMock.Verify(e => e.GetMinimizedPersonsByIds(ids), Times.Once);
        }

        [Theory]
        [InlineData(DistanceClassTemplateOperation.Approve, nameof(DistanceClassTemplateDetail.CanApprove))]
        [InlineData(DistanceClassTemplateOperation.Edit, nameof(DistanceClassTemplateDetail.CanEdit))]
        [InlineData(DistanceClassTemplateOperation.Delete, nameof(DistanceClassTemplateDetail.CanDelete))]
        public void GetDistanceClassTemplateDetailById_ForOperation_PropertyIsSetToTrue(
            DistanceClassTemplateOperation operation,
            string propertyName)
        {
            // arrange
            var template = CreateDistanceClassTemplate();
            var detail = new DistanceClassTemplateDetail
            {
                TemplateState = DistanceClassTemplateState.Approved
            };
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateDetail>(It.IsAny<DistanceClassTemplate>())).Returns(detail);
            templateOperationCheckerMock.Setup(e => e.IsOperationAllowed(It.IsAny<DistanceClassTemplateOperation>(), It.IsAny<DistanceClassTemplateState>())).Returns(false);
            templateOperationCheckerMock.Setup(e => e.IsOperationAllowed(operation, It.IsAny<DistanceClassTemplateState>())).Returns(true);
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplateDetailById(1);

            // assert
            var propertyValue = (bool)typeof(DistanceClassTemplateDetail).GetProperty(propertyName).GetValue(result);
            Assert.True(propertyValue);
            templateOperationCheckerMock.Verify(e => e.IsOperationAllowed(operation, DistanceClassTemplateState.Approved), Times.Once);
        }
        #endregion

        #region GetNewDistanceClassTemplateForEdit() tests
        [Fact]
        public void GetNewDistanceClassTemplateForEdit_FactoryIsCalled_ReturnsDistanceClassTemplateForEdit()
        {
            // arrange
            var forEdit = new DistanceClassTemplateForEdit();
            factoryMock.Setup(e => e.CreateDistanceClassTemplateForEdit()).Returns(forEdit);
            var admin = CreateAdmin();

            // act
            var result = admin.GetNewDistanceClassTemplateForEdit();

            // assert
            Assert.Equal(DateTime.Today, result.Form.RegistrationStart);
            Assert.Same(forEdit, result);
            factoryMock.Verify(e => e.CreateDistanceClassTemplateForEdit(), Times.Once);
        }
        #endregion

        #region GetDistanceClassTemplateForEditById() tests
        [Fact]
        public void GetDistanceClassTemplateForEditById_IncludesPassToGetDistanceClassTemplateById_ReturnsDistanceClassTemplate()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            factoryMock.Setup(e => e.CreateDistanceClassTemplateForEdit()).Returns(new DistanceClassTemplateForEdit());
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateForm>(It.IsAny<DistanceClassTemplate>())).Returns(new DistanceClassTemplateForm());
            var admin = CreateAdmin();

            // act
            admin.GetDistanceClassTemplateForEditById(1);

            // assert
            distanceTemplateDbMock.Verify(e => e.GetDistanceClassTemplateById(1, DistanceClassTemplateIncludes.DistanceClassTemplatePersons), Times.Once);
        }

        [Fact]
        public void GetDistanceClassTemplateForEditById_DistanceClassTemplateOperationIsInvalid_ThrowsInvalidOperationException()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            templateOperationCheckerMock.Setup(e => e.CheckOperation(It.IsAny<DistanceClassTemplateOperation>(), It.IsAny<DistanceClassTemplateState>())).Throws(new InvalidOperationException());
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.GetDistanceClassTemplateForEditById(1));
        }

        [Fact]
        public void GetDistanceCLassTemplateForEditById_FactoryIsCalled_ReturnsDistanceClassTemplateForEdit()
        {
            // arrange
            var forEdit = new DistanceClassTemplateForEdit();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            factoryMock.Setup(e => e.CreateDistanceClassTemplateForEdit()).Returns(forEdit);
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateForm>(It.IsAny<DistanceClassTemplate>())).Returns(new DistanceClassTemplateForm());
            var admin = CreateAdmin();

            // act
            var result = admin.GetNewDistanceClassTemplateForEdit();

            // assert
            Assert.Same(forEdit, result);
            factoryMock.Verify(e => e.CreateDistanceClassTemplateForEdit(), Times.Once);
        }

        [Fact]
        public void GetNewDistanceClassTemplateForEdit_DistanceClassTemplate_IsMappedToDistanceClassTemplateForm()
        {
            // arrange
            var template = CreateDistanceClassTemplate();
            var form = new DistanceClassTemplateForm();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            factoryMock.Setup(e => e.CreateDistanceClassTemplateForEdit()).Returns(new DistanceClassTemplateForEdit());
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateForm>(It.IsAny<DistanceClassTemplate>())).Returns(form);
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplateForEditById(1);

            // assert
            Assert.Same(form, result.Form);
            mainMapperMock.Verify(e => e.Map<DistanceClassTemplateForm>(template), Times.Once);
        }
        #endregion

        #region GetDistanceClassTemplateForEditByForm() tests
        [Fact]
        public void GetDistanceClassTemplateForEditByForm_DIstanceClassTemplateForm_ReturnsDistanceClassTemplateForEditWithSetForm()
        {
            // arrange
            var form = CreateDistanceClassTemplateForm();
            var forEdit = new DistanceClassTemplateForEdit();
            factoryMock.Setup(e => e.CreateDistanceClassTemplateForEdit()).Returns(forEdit);
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplateForEditByForm(form);

            // assert
            Assert.Same(form, result.Form);
            Assert.Same(forEdit, result);
            factoryMock.Verify(e => e.CreateDistanceClassTemplateForEdit(), Times.Once);
        }
        #endregion

        #region ValidateDistanceClassTemplateForm() tests
        [Theory]
        [InlineData("10 January 2022", "11 January 2022", "20 January 2022", "21 January 2022", true)]
        [InlineData("10 January 2022", "10 January 2022", "10 January 2022", "10 January 2022", true)]
        [InlineData("11 January 2022", "10 January 2022", "10 January 2022", "10 January 2022", false)]
        [InlineData("10 January 2022", "11 January 2022", "10 January 2022", "10 January 2022", false)]
        [InlineData("10 January 2022", "10 January 2022", "11 January 2022", "10 January 2022", false)]
        public void ValidateDistanceClassTemplateForm_ForGivenDates_ValidationReturnsExpectedResult(string regStart, string regEnd, string eventStart, string eventEnd, bool isValid)
        {
            // arrange
            var form = new DistanceClassTemplateForm
            {
                RegistrationStart = DateTime.Parse(regStart, CultureInfo.InvariantCulture),
                RegistrationEnd = DateTime.Parse(regEnd, CultureInfo.InvariantCulture),
                EventStart = DateTime.Parse(eventStart, CultureInfo.InvariantCulture),
                EventEnd = DateTime.Parse(eventEnd, CultureInfo.InvariantCulture)
            };
            var admin = CreateAdmin();

            // act
            var result = admin.ValidateDistanceClassTemplateForm(form);

            // assert
            Assert.Equal(isValid, result.IsValid);
        }
        #endregion

        #region ApproveDistanceClassTemplate() tests
        [Fact]
        public void ApproveDistanceClassTemplate_DistanceClassTemplateOperationIsInvalid_ThrowsInvalidOperationException()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            templateOperationCheckerMock.Setup(e => e.CheckOperation(It.IsAny<DistanceClassTemplateOperation>(), It.IsAny<DistanceClassTemplateState>())).Throws(new InvalidOperationException());
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.ApproveDistanceClassTemplate(1));
        }

        [Fact]
        public void ApproveDistanceClassTemplate_DistanceClassTemplateId_DistanceClassTemplateIsApprovedAndEventIsRaised()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            distanceTemplateDbMock.Setup(e => e.ApproveDistanceClassTemplate(It.IsAny<long>()));
            eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<DistanceClassTemplateApprovedEvent>()));
            var admin = CreateAdmin();

            // act
            admin.ApproveDistanceClassTemplate(15);

            // assert
            distanceTemplateDbMock.Verify(e => e.ApproveDistanceClassTemplate(15), Times.Once);
            eventDispatcherMock.Verify(e => e.Dispatch(It.Is<DistanceClassTemplateApprovedEvent>(x => x.DistanceClassTemplateId == 15)), Times.Once);
        }
        #endregion

        #region SaveDistanceClassTemplate() tests
        [Fact]
        public void SaveDistanceClassTemplate_DistanceClassTemplateForm_IsMappedToDistanceClassTemplate()
        {
            // arrange
            var form = CreateDistanceClassTemplateForm();
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplate>(It.IsAny<DistanceClassTemplateForm>())).Returns(new DistanceClassTemplate());
            var admin = CreateAdmin();

            // act
            admin.SaveDistanceClassTemplate(form);

            // assert
            mainMapperMock.Verify(e => e.Map<DistanceClassTemplate>(form), Times.Once);
        }

        [Fact]
        public void SaveDistanceClassTemplate_NewDistanceClassTemplate_DistanceClassTemplateIsInsertedToDb()
        {
            // arrange
            var form = CreateDistanceClassTemplateForm();
            var template = CreateDistanceClassTemplate();
            template.DistanceClassTemplateId = 0;
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplate>(It.IsAny<DistanceClassTemplateForm>())).Returns(template);
            distanceTemplateDbMock.Setup(e => e.InsertDistanceClassTemplate(It.IsAny<DistanceClassTemplate>())).Returns(1);
            var admin = CreateAdmin();

            // act
            var result = admin.SaveDistanceClassTemplate(form);

            // assert
            Assert.Equal(1, result);
            distanceTemplateDbMock.Verify(e => e.InsertDistanceClassTemplate(template), Times.Once);
        }

        [Fact]
        public void SaveDistanceClassTemplate_DistanceClassTemplateOperationIsInvalid_ThrowsInvalidOperationException()
        {
            // arrange
            var template = CreateDistanceClassTemplate();
            var form = new DistanceClassTemplateForm
            {
                DistanceClassTemplateId = 1
            };
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplate>(It.IsAny<DistanceClassTemplateForm>())).Returns(template);
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            templateOperationCheckerMock.Setup(e => e.CheckOperation(It.IsAny<DistanceClassTemplateOperation>(), It.IsAny<DistanceClassTemplateState>())).Throws(new InvalidOperationException());
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.SaveDistanceClassTemplate(form));
        }

        [Fact]
        public void SaveDistanceClassTemplate_DistanceClassTemplate_UpdateDistanceClassTemplateInDbAndRaiseEvent()
        {
            // arrange
            var form = CreateDistanceClassTemplateForm();
            var template = CreateDistanceClassTemplate();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplate>(It.IsAny<DistanceClassTemplateForm>())).Returns(template);
            distanceTemplateDbMock.Setup(e => e.UpdateDistanceClassTemplate(It.IsAny<DistanceClassTemplate>()));
            eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<DistanceClassTemplateChangedEvent>()));
            var admin = CreateAdmin();

            // act
            var result = admin.SaveDistanceClassTemplate(form);

            // assert
            Assert.Equal(12, result);
            distanceTemplateDbMock.Verify(e => e.UpdateDistanceClassTemplate(template), Times.Once);
            eventDispatcherMock.Verify(e => e.Dispatch(It.Is<DistanceClassTemplateChangedEvent>(x => x.DistanceClassTemplateId == 12)), Times.Once);
        }
        #endregion

        #region DeleteDistanceClassTemplate() tests
        [Fact]
        public void DeleteDistanceClassTemplate_DistanceClassTemplateOperationIsInvalid_ThrowsInvalidOperationException()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            templateOperationCheckerMock.Setup(e => e.CheckOperation(It.IsAny<DistanceClassTemplateOperation>(), It.IsAny<DistanceClassTemplateState>())).Throws(new InvalidOperationException());
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.GetDistanceClassTemplateForEditById(1));
        }

        [Fact]
        public void DeleteDistanceClassTemplate_DistanceClassTemplateId_DistanceClassTemplateIsDeleted()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            distanceTemplateDbMock.Setup(e => e.DeleteDistanceClassTemplate(It.IsAny<long>()));
            var admin = CreateAdmin();

            // act
            admin.DeleteDistanceClassTemplate(2);

            // assert
            distanceTemplateDbMock.Verify(e => e.DeleteDistanceClassTemplate(2), Times.Once);
        }
        #endregion

        #region private methods
        private DistanceClassTemplateAdministration CreateAdmin()
        {
            return new DistanceClassTemplateAdministration(
                distanceTemplateDbMock.Object,
                mainMapperMock.Object,
                personDbMock.Object,
                templateHelperMock.Object,
                factoryMock.Object,
                eventDispatcherMock.Object,
                templateOperationCheckerMock.Object);
        }

        private DistanceClassTemplate CreateDistanceClassTemplate()
        {
            return new DistanceClassTemplate
            {
                DistanceClassTemplateId = 12,
                IsApproved = true,
                ClassTypeId = ClassTypeEnum.Basic,
                DistanceClassTemplatePersons = new List<DistanceClassTemplatePerson>
                {
                    new DistanceClassTemplatePerson
                    {
                        PersonId = 1,
                        DistanceClassTemplateId = 12,
                        RoleTypeId = PersonRoleTypeEnum.Instructor
                    },
                    new DistanceClassTemplatePerson
                    {
                        PersonId = 2,
                        DistanceClassTemplateId = 12,
                        RoleTypeId = PersonRoleTypeEnum.Instructor
                    }
                },
                GuestInstructorId = 3
            };
        }

        private DistanceClassTemplateForm CreateDistanceClassTemplateForm()
        {
            return new DistanceClassTemplateForm
            {
                ClassTypeId = ClassTypeEnum.Basic,
                EventStart = new DateTime(2021, 12, 1),
                EventEnd = new DateTime(2022, 1, 1),
                DistanceClassTemplateId = 12,
                InstructorIds = new List<long> { 1, 2, 3 },
                GuestInstructorId = 6,
                Location = "Location",
                TranslationCode = 2,
                RegistrationStart = new DateTime(2021, 1, 1),
                RegistrationEnd = new DateTime(2021, 11, 1),
            };
        }
        #endregion
    }
}