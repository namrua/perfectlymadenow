using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Model;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateHelperTests
    {
        private readonly Mock<IPersonHelper> personHelperMock;

        public DistanceClassTemplateHelperTests()
        {
            personHelperMock = new Mock<IPersonHelper>();
        }

        #region GetDistanceClassTemplatePersonIds() tests
        [Fact]
        public void GetDistanceClassTemplatePersonIds_DistanceClassTemplatePersonsIsNull_ThrowsInvalidOpertaionException()
        {
            // arrange
            var template = CreateTemplate();
            template.DistanceClassTemplatePersons = null;
            var templateHelper = CreateTemplateHelper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => templateHelper.GetDistanceClassTemplatePersonIds(template));
        }

        [Fact]
        public void GetDistanceClassTemplatePersonIds_DistanceClassTemplateWithoutGuestInstructor_ReturnsDistanceClassTemplatePersonIds()
        {
            // arrange
            var template = CreateTemplate();
            template.GuestInstructorId = null;
            var templateHelper = CreateTemplateHelper();

            // act
            var result = templateHelper.GetDistanceClassTemplatePersonIds(template);

            // assert
            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item));
        }

        [Fact]
        public void GetDistanceClassTemplatePersonIds_DistanceClassTemplateWithGuestInstructor_ReturnsDistanceClassTemplatePersonIds()
        {
            // arrange
            var template = CreateTemplate();
            var templateHelper = CreateTemplateHelper();

            // act
            var result = templateHelper.GetDistanceClassTemplatePersonIds(template);

            // assert
            Assert.Collection(result,
                item => Assert.Equal(6, item),
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item));
        }
        #endregion

        #region GetDistanceClassTemplatePersonsIds() tests
        [Fact]
        public void GetDistanceClassTemplatePersonsIds_DistanceClassTemplatePersonsIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var templates = new List<DistanceClassTemplate>
            {
                new DistanceClassTemplate
                {
                    DistanceClassTemplatePersons = null
                }
            };
            var templateHelper = CreateTemplateHelper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => templateHelper.GetDistanceClassTemplatePersonsIds(templates));
        }

        [Fact]
        public void GetDistanceClassTemplatePersonsIds_DistanceClassTemplates_ReturnsPersonsIds()
        {
            // arrange
            var templates = CreateTemplates();
            var templateHelper = CreateTemplateHelper();

            // act
            var result = templateHelper.GetDistanceClassTemplatePersonsIds(templates);

            // assert
            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(11, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(4, item));
        }
        #endregion

        #region GetDistanceClassTemplateInstructorsWithGuestInstructor() tests
        [Fact]
        public void GetDistanceClassTemplateInstructorsWithGuestInstructor_DistanceClassTemplatePersonsIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var template = CreateTemplate();
            template.DistanceClassTemplatePersons = null;
            var templateHelper = CreateTemplateHelper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => templateHelper.GetDistanceClassTemplateInstructorsWithGuestInstructor(template, personHelperMock.Object));
        }

        [Fact]
        public void GetDistanceClassTemplateInstructorsWithGuestInstructor__TemplateWithGuestInstructorAndInstructorDuplicities_ReturnsInstructors()
        {
            // arrange
            var template = new DistanceClassTemplate
            {
                GuestInstructorId = 15,
                DistanceClassTemplatePersons = new List<DistanceClassTemplatePerson>
                {
                    new DistanceClassTemplatePerson
                    {
                        RoleTypeId = PersonRoleTypeEnum.Instructor,
                        PersonId = 15

                    },
                    new DistanceClassTemplatePerson
                    {
                        RoleTypeId = PersonRoleTypeEnum.Instructor,
                        PersonId = 2
                    },
                    new DistanceClassTemplatePerson
                    {
                        RoleTypeId = PersonRoleTypeEnum.Instructor,
                        PersonId = 2
                    }
                }
            };
            var templateHelper = CreateTemplateHelper();
            personHelperMock.Setup(e => e.GetPersonNameById(15)).Returns("firstName");
            personHelperMock.Setup(e => e.GetPersonNameById(2)).Returns("secondName");

            // act
            var result = templateHelper.GetDistanceClassTemplateInstructorsWithGuestInstructor(template, personHelperMock.Object);

            // assert
            Assert.Collection(result,
                item => Assert.Equal("firstName", item),
                item => Assert.Equal("secondName", item));
        }
        #endregion

        #region GetDistanceClassTemplateInstructors() tests
        [Fact]
        public void GetDistanceClassTemplateInstructors_DistanceClassTemplatePersonsIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var template = CreateTemplate();
            template.DistanceClassTemplatePersons = null;
            var templateHelper = CreateTemplateHelper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => templateHelper.GetDistanceClassTemplateInstructors(template, personHelperMock.Object));
        }

        [Fact]
        public void GetDistanceClassTemplateInstructors_TemplateWithInstructorDuplicities_ReturnsInstructors()
        {
            // arrange
            var template = new DistanceClassTemplate
            {
                DistanceClassTemplatePersons = new List<DistanceClassTemplatePerson>
                {
                    new DistanceClassTemplatePerson
                    {
                        RoleTypeId = PersonRoleTypeEnum.Instructor,
                        PersonId = 2
                    },
                    new DistanceClassTemplatePerson
                    {
                        RoleTypeId = PersonRoleTypeEnum.Instructor,
                        PersonId = 2
                    }
                }
            };
            var templateHelper = CreateTemplateHelper();
            personHelperMock.Setup(e => e.GetPersonNameById(2)).Returns("firstName");

            // act
            var result = templateHelper.GetDistanceClassTemplateInstructors(template, personHelperMock.Object);

            // assert
            Assert.Collection(result,
                item => Assert.Equal("firstName", item));
        }
        #endregion

        #region GetDistanceClassTemplateState() tests
        [Theory]
        [InlineData(true, true, DistanceClassTemplateState.Completed)]
        [InlineData(true, false, DistanceClassTemplateState.Completed)]
        [InlineData(false, true, DistanceClassTemplateState.Approved)]
        [InlineData(false, false, DistanceClassTemplateState.New)]
        public void GetDistanceClassTemplateState_ForDistanceClassTemplateState_ReturnsExpectedDistanceClassTemplateState(
            bool isCompleted,
            bool isApproved,
            DistanceClassTemplateState expectedState)
        {
            var template = CreateTemplate();
            template.IsCompleted = isCompleted;
            template.IsApproved = isApproved;
            var templateHelper = CreateTemplateHelper();

            // act
            var result = templateHelper.GetDistanceClassTemplateState(template);

            // assert
            Assert.Equal(expectedState, result);
        }
        #endregion

        #region private methods
        private DistanceClassTemplateHelper CreateTemplateHelper()
        {
            return new DistanceClassTemplateHelper();
        }

        private DistanceClassTemplate CreateTemplate()
        {
            return new DistanceClassTemplate
            {
                DistanceClassTemplatePersons = new List<DistanceClassTemplatePerson>
                {
                    new DistanceClassTemplatePerson
                    {
                        PersonId = 1,
                        RoleTypeId = PersonRoleTypeEnum.Instructor,
                        
                    },
                    new DistanceClassTemplatePerson
                    {
                        PersonId = 2,
                        RoleTypeId = PersonRoleTypeEnum.ApprovedStaff
                    }
                },
                GuestInstructorId = 6
            };
        }

        private List<DistanceClassTemplate> CreateTemplates()
        {
            return new List<DistanceClassTemplate>
            {
                new DistanceClassTemplate
                {
                    DistanceClassTemplatePersons = new List<DistanceClassTemplatePerson>
                    {
                        new DistanceClassTemplatePerson
                        {
                            PersonId = 1,
                            RoleTypeId = PersonRoleTypeEnum.Instructor
                        },
                        new DistanceClassTemplatePerson
                        {
                            PersonId = 2,
                            RoleTypeId = PersonRoleTypeEnum.DistanceDoordinator
                        }
                    }
                },
                new DistanceClassTemplate
                {
                    DistanceClassTemplatePersons = new List<DistanceClassTemplatePerson>
                    {
                        new DistanceClassTemplatePerson
                        {
                            PersonId = 3,
                            RoleTypeId = PersonRoleTypeEnum.Instructor
                        },
                        new DistanceClassTemplatePerson
                        {
                            PersonId = 4,
                            RoleTypeId = PersonRoleTypeEnum.DistanceDoordinator
                        }
                    },
                    GuestInstructorId = 11
                }
            };
        }
        #endregion
    }
}
