using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.MappingProfiles;
using AutomationSystem.Main.Model;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic.MappingProfiles
{
    public class DistanceClassTemplateProfileTests
    {
        private readonly Mock<ILanguageTranslationProvider> languageProviderMock;
        private readonly Mock<IDistanceClassTemplateHelper> templateHelperMock;

        public DistanceClassTemplateProfileTests()
        {
            languageProviderMock = new Mock<ILanguageTranslationProvider>();
            templateHelperMock = new Mock<IDistanceClassTemplateHelper>();
        }

        #region CreateMap<DistanceClassTemplate, DistanceClassTemplateListItem>() tests
        [Fact]
        public void Map_ClassTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var template = CreateTemplate();
            template.ClassType = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceClassTemplateListItem>(template));
        }

        [Fact]
        public void Map_DistanceClassTemplate_ReturnsDistanceClassTemplateListItem()
        {
            // arrange
            var template = CreateTemplate();
            templateHelperMock.Setup(e => e.GetDistanceClassTemplateState(It.IsAny<DistanceClassTemplate>())).Returns(DistanceClassTemplateState.Approved);
            var mapper = CreateMapper();

            // act
            var item = mapper.Map<DistanceClassTemplateListItem>(template);

            // assert
            Assert.Equal(1, item.DistanceClassTemplateId);
            Assert.Equal(ClassTypeEnum.Basic, item.ClassTypeId);
            Assert.Equal("En", item.OriginLanguage);
            Assert.Equal("Cs", item.TransLanguage);
            Assert.Equal("Location", item.Location);
            Assert.Equal(new DateTime(2021, 04, 01), item.EventStart);
            Assert.Equal(new DateTime(2021, 05, 01), item.EventEnd);
            Assert.Equal(new DateTime(2021, 01, 01), item.RegistrationStart);
            Assert.Equal(new DateTime(2021, 02, 01), item.RegistrationEnd);
            Assert.Equal("April 01 & May 01, Location, Basic", item.Title);
            Assert.Equal(DistanceClassTemplateState.Approved, item.TemplateState);
            templateHelperMock.Verify(e => e.GetDistanceClassTemplateState(template), Times.Once);
        }
        #endregion

        #region CreateMap<DistanceClassTemplate, DistanceClassTemplateDetail>() tests
        [Fact]
        public void MapDetail_ClassTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var template = CreateTemplate();
            template.ClassType = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceClassTemplateDetail>(template));
        }

        [Fact]
        public void Map_DistanceClassTemplate_ReturnsDistanceClassTemplateDetail()
        {
            // arrange
            var template = CreateTemplate();
            templateHelperMock.Setup(e => e.GetDistanceClassTemplateState(It.IsAny<DistanceClassTemplate>())).Returns(DistanceClassTemplateState.Approved);
            var mapper = CreateMapper();

            // act
            var detail = mapper.Map<DistanceClassTemplateDetail>(template);

            // assert
            Assert.Equal(1, detail.DistanceClassTemplateId);
            Assert.Equal(ClassTypeEnum.Basic, detail.ClassTypeId);
            Assert.Equal("En", detail.OriginLanguage);
            Assert.Equal("Cs", detail.TransLanguage);
            Assert.Equal("Location", detail.Location);
            Assert.Equal(new DateTime(2021, 04, 01), detail.EventStart);
            Assert.Equal(new DateTime(2021, 05, 01), detail.EventEnd);
            Assert.Equal(new DateTime(2021, 01, 01), detail.RegistrationStart);
            Assert.Equal(new DateTime(2021, 02, 01), detail.RegistrationEnd);
            Assert.Equal("April 01 & May 01, Location, Basic", detail.Title);
            Assert.Equal(DistanceClassTemplateState.Approved, detail.TemplateState);
            Assert.Equal("Basic", detail.ClassType);
            Assert.Null(detail.GuestInstructor);
            templateHelperMock.Verify(e => e.GetDistanceClassTemplateState(template), Times.Once);
        }
        #endregion

        #region CreateMap<DistanceClassTemplate, DistanceClassTemplateForm>() tests
        [Fact]
        public void Map_DistancClassTemplatePersonsIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var template = CreateTemplate();
            template.DistanceClassTemplatePersons = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceClassTemplateForm>(template));
        }

        [Fact]
        public void Map_DistanceClassTemplate_ReturnsDistanceClassTemplateForm()
        {
            // arrange
            var template = CreateTemplate();
            languageProviderMock.Setup(x => x.GetTranslationCode(It.IsAny<LanguageEnum>(), It.IsAny<LanguageEnum?>())).Returns(2001);
            var mapper = CreateMapper();

            // act
            var form = mapper.Map<DistanceClassTemplateForm>(template);

            // assert
            Assert.Equal(1, form.DistanceClassTemplateId);
            Assert.Equal(ClassTypeEnum.Basic, form.ClassTypeId);
            Assert.Null(form.GuestInstructorId);
            Assert.Equal(new DateTime(2021, 04, 01), form.EventStart);
            Assert.Equal(new DateTime(2021, 05, 01), form.EventEnd);
            Assert.Equal(new DateTime(2021, 01, 01), form.RegistrationStart);
            Assert.Equal(new DateTime(2021, 02, 01), form.RegistrationEnd);
            Assert.Equal("Location", form.Location);
            Assert.Equal(2001, form.TranslationCode);
            Assert.Collection(form.InstructorIds,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item));

            languageProviderMock.Verify(x => x.GetTranslationCode(LanguageEnum.En, LanguageEnum.Cs), Times.Once);
        }
        #endregion

        #region CreateMap<DistanceClassTemplateForm, DistanceClassTemplate>() tests
        [Fact]
        public void Map_DistanceClassTemplateForm_ReturnsDistanceClassTemplate()
        {
            // arrange
            var form = new DistanceClassTemplateForm
            {
                ClassTypeId = ClassTypeEnum.Basic,
                DistanceClassTemplateId = 12,
                EventStart = new DateTime(2021, 04, 01),
                EventEnd = new DateTime(2021, 05, 01),
                RegistrationStart = new DateTime(2021, 01, 01),
                RegistrationEnd = new DateTime(2021, 02, 01),
                GuestInstructorId = 5,
                Location = "Location",
                TranslationCode = 2001,
                InstructorIds = new List<long> { 4, 7 }
            };
            languageProviderMock.Setup(x => x.GetOriginalLanguageId(It.IsAny<long>())).Returns(LanguageEnum.En);
            languageProviderMock.Setup(x => x.GetTranslationLanguageId(It.IsAny<long>())).Returns(LanguageEnum.Cs);
            var mapper = CreateMapper();

            // act
            var template = mapper.Map<DistanceClassTemplate>(form);

            // assert
            Assert.Equal(ClassTypeEnum.Basic, template.ClassTypeId);
            Assert.Equal(12, template.DistanceClassTemplateId);
            Assert.Equal(new DateTime(2021, 04, 01), template.EventStart);
            Assert.Equal(new DateTime(2021, 05, 01), template.EventEnd);
            Assert.Equal(new DateTime(2021, 01, 01), template.RegistrationStart);
            Assert.Equal(new DateTime(2021, 02, 01), template.RegistrationEnd);
            Assert.Equal(5, template.GuestInstructorId);
            Assert.Equal("Location", template.Location);
            Assert.Equal(LanguageEnum.En, template.OriginLanguageId);
            Assert.Equal(LanguageEnum.Cs, template.TransLanguageId);
            Assert.Collection(template.DistanceClassTemplatePersons,
                item =>
                {
                    Assert.Equal(4, item.PersonId);
                    Assert.Equal(12, item.DistanceClassTemplateId);
                    Assert.Equal(PersonRoleTypeEnum.Instructor, item.RoleTypeId);
                },
                item =>
                {
                    Assert.Equal(7, item.PersonId);
                    Assert.Equal(12, item.DistanceClassTemplateId);
                    Assert.Equal(PersonRoleTypeEnum.Instructor, item.RoleTypeId);
                });

            languageProviderMock.Verify(x => x.GetOriginalLanguageId(2001), Times.Once);
            languageProviderMock.Verify(x => x.GetTranslationLanguageId(2001), Times.Once);
        }
        #endregion
        
        #region CreateMap<DistanceClassTemplate, DistanceClassTemplateCompletionForm>() tests
        [Fact]
        public void Map_DistanceClassTemplate_ReturnsDistanceClassTemplateCompletionForm()
        {
            // arrange
            var template = CreateTemplate();
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<DistanceClassTemplateCompletionForm>(template);

            // assert
            Assert.Equal(1, result.DistanceClassTemplateId);
            Assert.Equal(new DateTime(2022, 1, 1), result.AutomationCompleteTime);
        }
        #endregion

        #region CreateMap<DistanceClassTemplate, DistanceClassTemplateCompletionShortDetail>() tests
        [Fact]
        public void Map_ClassTypeIsMissing_ThrowsInvalidOperationException()
        {
            // arrange
            var template = CreateTemplate();
            template.ClassType = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException > (() => mapper.Map<DistanceClassTemplateCompletionShortDetail>(template));
        }

        [Fact]
        public void Map_DistanceClassTemplate_ReturnsDistanceClassTemplateComletionShortDetail()
        {
            // arrange
            templateHelperMock.Setup(e => e.GetDistanceClassTemplateState(It.IsAny<DistanceClassTemplate>())).Returns(DistanceClassTemplateState.Approved);
            var template = CreateTemplate();
            var mapper = CreateMapper();

            // act
            var detail = mapper.Map<DistanceClassTemplateCompletionShortDetail>(template);

            // assert
            Assert.Equal(1, detail.DistanceClassTemplateId);
            Assert.Equal("April 01 & May 01, Location, Basic", detail.Title);
            Assert.Equal(DistanceClassTemplateState.Approved, detail.TemplateState);
            templateHelperMock.Verify(e => e.GetDistanceClassTemplateState(template), Times.Once);
        }
        #endregion

        #region private methods
        private Mapper CreateMapper()
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DistanceClassTemplateProfile(languageProviderMock.Object, templateHelperMock.Object));
            });
            return new Mapper(mapperCfg);
        }

        private DistanceClassTemplate CreateTemplate()
        {
            return new DistanceClassTemplate
            {
                DistanceClassTemplateId = 1,
                ClassTypeId = ClassTypeEnum.Basic,
                OriginLanguageId = LanguageEnum.En,
                TransLanguageId = LanguageEnum.Cs,
                Location = "Location",
                EventStart = new DateTime(2021, 04, 01),
                EventEnd = new DateTime(2021, 05, 01),
                RegistrationStart = new DateTime(2021, 01, 01),
                RegistrationEnd = new DateTime(2021, 02, 01),
                ClassType = new ClassType
                {
                    ClassTypeId = ClassTypeEnum.Basic,
                    Description = "Basic"
                },
                IsApproved = true,
                IsCompleted = false,
                DistanceClassTemplatePersons = new List<DistanceClassTemplatePerson>
                {
                    new DistanceClassTemplatePerson
                    {
                        PersonId = 1,
                        RoleTypeId = PersonRoleTypeEnum.Instructor,
                        DistanceClassTemplateId = 1
                    },
                    new DistanceClassTemplatePerson
                    {
                        PersonId = 2,
                        RoleTypeId = PersonRoleTypeEnum.Instructor,
                        DistanceClassTemplateId = 1
                    }
                },
                AutomationCompleteTime = new DateTime(2022, 1, 1),
                GuestInstructor = new Person()
            };
        }
        #endregion
    }
}
