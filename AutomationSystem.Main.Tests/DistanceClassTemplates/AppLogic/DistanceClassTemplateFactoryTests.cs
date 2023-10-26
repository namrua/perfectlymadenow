using System.Collections.Generic;
using Xunit;
using Moq;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateFactoryTests
    {
        private readonly Mock<IPersonDatabaseLayer> personDbMock;
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;
        private readonly Mock<ILanguageTranslationProvider> languageProviderMock;
        private readonly Mock<IClassTypeResolver> classTypeResolverMock;

        public DistanceClassTemplateFactoryTests()
        {
            personDbMock = new Mock<IPersonDatabaseLayer>();
            enumDbMock = new Mock<IEnumDatabaseLayer>();
            languageProviderMock = new Mock<ILanguageTranslationProvider>();
            classTypeResolverMock = new Mock<IClassTypeResolver>();
        }

        #region CreateDistanceClassTemplateForEdit() tests
        [Fact]
        public void CreateDistanceClassTemplateForEdit_ForClassTypeInDb_ReturnsFilteredClassTypes()
        {
            // arrange
            var classTypes = CreateClassTypes();
            var allowedClassTypes = new HashSet<ClassTypeEnum> {ClassTypeEnum.Basic, ClassTypeEnum.Basic2};
            enumDbMock.Setup(x => x.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), It.IsAny<EnumItemFilter>())).Returns(classTypes);
            classTypeResolverMock.Setup(e => e.GetClassTypesByClassCategoryId(It.IsAny<ClassCategoryEnum>())).Returns(allowedClassTypes);
            var factory = CreateFactory();

            // act
            var result = factory.CreateDistanceClassTemplateForEdit();

            // assert
            Assert.Collection(result.ClassTypes,
                item => Assert.Equal((int)ClassTypeEnum.Basic, item.Id),
                item => Assert.Equal((int)ClassTypeEnum.Basic2, item.Id));
            enumDbMock.Verify(x => x.GetItemsByFilter(EnumTypeEnum.MainClassType, null), Times.Once);
        }

        [Fact]
        public void CreateDistanceClassTemplateForEdit_ForPersonsInDb_ReturnsPersons()
        {
            // arrange
            var persons = CreatePersonsMinimized();
            enumDbMock.Setup(x => x.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), It.IsAny<EnumItemFilter>())).Returns(new List<IEnumItem>());
            personDbMock.Setup(x => x.GetMinimizedPersonsByProfileId(It.IsAny<long?>())).Returns(persons);
            var factory = CreateFactory();

            // act
            var result = factory.CreateDistanceClassTemplateForEdit();

            // assert
            Assert.Equal("firstName lastName", result.Persons.GetPersonNameById(1));
            Assert.Equal("secondFirstName secondLastName", result.Persons.GetPersonNameById(2));
            personDbMock.Verify(x => x.GetMinimizedPersonsByProfileId(null), Times.Once);
        }

        [Fact]
        public void CreateDistanceClassTemplateForEdit_GetTranslationOptionsIsCalled_ReturnsTranslations()
        {
            // arrange
            var translations = CreateDropDownItems();
            enumDbMock.Setup(x => x.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), It.IsAny<EnumItemFilter>())).Returns(new List<IEnumItem>());
            personDbMock.Setup(x => x.GetMinimizedPersonsByProfileId(It.IsAny<long?>())).Returns(new List<PersonMinimized>());
            languageProviderMock.Setup(x => x.GetTranslationOptions()).Returns(translations);
            var factory = CreateFactory();

            // act
            var result = factory.CreateDistanceClassTemplateForEdit();

            // assert
            Assert.Collection(result.Translations,
                item =>
                {
                    Assert.Equal("1", item.Id);
                    Assert.Equal("English", item.ExtensionText);
                    Assert.Equal("En", item.Text);
                },
                item =>
                {
                    Assert.Equal("2", item.Id);
                    Assert.Equal("Czech", item.ExtensionText);
                    Assert.Equal("Cs", item.Text);
                });
            languageProviderMock.Verify(x => x.GetTranslationOptions(), Times.Once);
        }
        #endregion

        #region private methods
        private DistanceClassTemplateFactory CreateFactory()
        {
            return new DistanceClassTemplateFactory(
                personDbMock.Object,
                enumDbMock.Object,
                languageProviderMock.Object,
                classTypeResolverMock.Object);
        }

        private List<IEnumItem> CreateClassTypes()
        {
            return new List<IEnumItem>
            {
                new EnumItem
                {
                    Id = (int)ClassTypeEnum.Basic
                },
                new EnumItem
                {
                    Id = (int)ClassTypeEnum.Basic2
                },
                new EnumItem
                {
                    Id = (int)ClassTypeEnum.LectureBasic
                }
            };
        }

        private List<PersonMinimized> CreatePersonsMinimized()
        {
            return new List<PersonMinimized>()
            {
                new PersonMinimized
                {
                    PersonId = 1,
                    Name = "firstName lastName"
                },
                new PersonMinimized
                {
                    PersonId = 2,
                    Name = "secondFirstName secondLastName"
                }
            };
        }

        private List<DropDownItem> CreateDropDownItems()
        {
            var items = new List<DropDownItem>();
            items.Add(DropDownItem.Item(1, "En", "English"));
            items.Add(DropDownItem.Item(2, "Cs", "Czech"));
            return items;
        }
        #endregion
    }
}
