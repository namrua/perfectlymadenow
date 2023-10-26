using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Core.Contacts.AppLogic;
using AutomationSystem.Main.Core.Contacts.Data;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Tests.Profiles.TestingHelpers;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using ContactListItem = AutomationSystem.Main.Contract.Contacts.AppLogic.Models.ContactListItem;

namespace AutomationSystem.Main.Tests.Contacts.AppLogic
{
    public class ContactProviderTests
    {
        private readonly Mock<IContactDatabaseLayer> contactDbMock;
        private readonly Mock<IFormerDatabaseLayer> formerDbMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IMainMapper> mainMapperMock;

        public ContactProviderTests()
        {
            contactDbMock = new Mock<IContactDatabaseLayer>();
            formerDbMock = new Mock<IFormerDatabaseLayer>();
            identityResolverMock = new Mock<IIdentityResolver>();
            mainMapperMock = new Mock<IMainMapper>();
        }

        #region GetContacts() tests

        [Fact]
        public void GetContacts_NoAccessToEntitle_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(new EntitleAccessDeniedException("", Entitle.MainContacts));
            var provider = CreateProvider();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => provider.GetContacts(new ContactListFilter()));
        }

        [Fact]
        public void GetContacts_ExpectedIncludesPassToGetFormerStudentsByFilter()
        {
            // arrange
            var contactListFilter = new ContactListFilter
            {
                ProfileId = 1
            };
            formerDbMock.Setup(e => e.GetFormerStudentsByFilter(It.IsAny<FormerStudentFilter>(), It.IsAny<FormerStudentIncludes>())).Returns(new List<FormerStudent>());
            contactDbMock.Setup(e => e.GetBlackLists()).Returns(new List<ContactBlackList>());
            var provider = CreateProvider();

            // act
            provider.GetContacts(contactListFilter);

            // assert
            formerDbMock.Verify(e => e.GetFormerStudentsByFilter(It.Is<FormerStudentFilter>(x => x.Class.ProfileId == 1),
                FormerStudentIncludes.FormerClass | FormerStudentIncludes.Address),
                Times.Once);
        }

        [Fact]
        public void GetContacts_OneFormerStudent_IsMappedToContactListItem()
        {
            // arrange
            var contactListFilter = new ContactListFilter
            {
                ProfileId = 1
            };
            var formerStudents = new List<FormerStudent>
            {
                CreateFormerStudent(new DateTime(2021, 1, 1))
            };
            var contactListItem = new ContactListItem();
            formerDbMock.Setup(e => e.GetFormerStudentsByFilter(It.IsAny<FormerStudentFilter>(), It.IsAny<FormerStudentIncludes>())).Returns(formerStudents);
            contactDbMock.Setup(e => e.GetBlackLists()).Returns(new List<ContactBlackList>());
            mainMapperMock.Setup(e => e.Map<ContactListItem>(It.IsAny<FormerStudent>())).Returns(contactListItem);
            var provider = CreateProvider();

            // act
            var result = provider.GetContacts(contactListFilter);

            // assert
            Assert.Collection(result,
                item => Assert.Same(contactListItem, item));
            mainMapperMock.Verify(e => e.Map<ContactListItem>(It.Is<FormerStudent>(x => x.FormerStudentId == 1)), Times.Once);
        }

        [Fact]
        public void GetContacts_TwoStudentsWithSameEmail_StudentWithLatestEventStartIsUsed()
        {
            // arrange
            var contactListFilter = new ContactListFilter
            {
                ProfileId = 1
            };
            var formerStudents = new List<FormerStudent>
            {
                CreateFormerStudent(new DateTime(2021, 1, 1)),
                CreateFormerStudent(new DateTime(2021, 2, 2), 5, "Email@TEST.com")
            };
            var contactListItem = new ContactListItem();
            formerDbMock.Setup(e => e.GetFormerStudentsByFilter(It.IsAny<FormerStudentFilter>(), It.IsAny<FormerStudentIncludes>())).Returns(formerStudents);
            contactDbMock.Setup(e => e.GetBlackLists()).Returns(new List<ContactBlackList>());
            mainMapperMock.Setup(e => e.Map<ContactListItem>(It.IsAny<FormerStudent>())).Returns(contactListItem);
            var provider = CreateProvider();

            // act
            provider.GetContacts(contactListFilter);

            // assert
            mainMapperMock.Verify(e => e.Map<ContactListItem>(It.Is<FormerStudent>(x => x.FormerStudentId == 5)), Times.Once);
        }

        [Fact]
        public void GetContacts_IncludeDisableContacts_ReturnsContactListItemsWithDisabledContacts()
        {
            // arrange
            var contactListFilter = new ContactListFilter
            {
                ProfileId = 1,
                IncludeDisabledContacts = true
            };
            var formerStudents = new List<FormerStudent>
            {
                CreateFormerStudent(new DateTime(2021, 1, 1)),
                CreateFormerStudent(new DateTime(2021, 2, 2), 5, "test@email.com")
            };
            var blackList = new List<ContactBlackList>
            {
                new ContactBlackList
                {
                    Email = "test@email.com"
                }
            };
            var contactListItemFirst = new ContactListItem
            {
                Email = "test@email.com"
            };
            var contactListItemSecond = new ContactListItem
            {
                Email = "email@test.com"
            };
            formerDbMock.Setup(e => e.GetFormerStudentsByFilter(It.IsAny<FormerStudentFilter>(), It.IsAny<FormerStudentIncludes>())).Returns(formerStudents);
            contactDbMock.Setup(e => e.GetBlackLists()).Returns(blackList);
            mainMapperMock.SetupSequence(e => e.Map<ContactListItem>(It.IsAny<FormerStudent>()))
                .Returns(contactListItemFirst)
                .Returns(contactListItemSecond);
            var provider = CreateProvider();

            // act
            var result = provider.GetContacts(contactListFilter);

            // assert
            Assert.Collection(result,
                item => Assert.Equal("test@email.com", item.Email),
                item => Assert.Equal("email@test.com", item.Email));
        }

        [Fact]
        public void GetContacts_IncludeDisabledContactsIsFalse_ReturnsOnlyEnableContacts()
        {
            // arrange
            var contactListFilter = new ContactListFilter
            {
                ProfileId = 1
            };
            var formerStudents = new List<FormerStudent>
            {
                CreateFormerStudent(new DateTime(2021, 1, 1)),
                CreateFormerStudent(new DateTime(2021, 2, 2), 5, "test@email.com")
            };
            var blackList = new List<ContactBlackList>
            {
                new ContactBlackList
                {
                    Email = "test@email.com"
                }
            };
            var contactListItemFirst = new ContactListItem
            {
                Email = "test@email.com"
            };
            var contactListItemSecond = new ContactListItem
            {
                Email = "email@test.com"
            };
            formerDbMock.Setup(e => e.GetFormerStudentsByFilter(It.IsAny<FormerStudentFilter>(), It.IsAny<FormerStudentIncludes>())).Returns(formerStudents);
            contactDbMock.Setup(e => e.GetBlackLists()).Returns(blackList);
            mainMapperMock.SetupSequence(e => e.Map<ContactListItem>(It.IsAny<FormerStudent>()))
                .Returns(contactListItemFirst)
                .Returns(contactListItemSecond);
            var provider = CreateProvider();

            // act
            var result = provider.GetContacts(contactListFilter);

            // assert
            Assert.Collection(result,
                item => Assert.Equal("email@test.com", item.Email));
        }

        #endregion

        #region RemoveBlackListedItems() tests

        [Fact]
        public void RemoveBlackListedItems_ContactLisItemsDefinitionWithBlackListedItems_ReturnsContactListItemDefinitionWithoutBlackListedItems()
        {
            // arrange
            var blackList = new List<ContactBlackList>
            {
                new ContactBlackList
                {
                    Email = "blackOne@email.com"
                },
                new ContactBlackList
                {
                    Email = "blackTwo@email.com"
                }
            };
            contactDbMock.Setup(e => e.GetBlackLists()).Returns(blackList);
            var contactListItemDef = new List<ContactListItemDefinition>
            {
                new ContactListItemDefinition
                {
                    Email = "first@email.com"
                },
                new ContactListItemDefinition
                {
                    Email = "blackOne@email.com"
                },
                new ContactListItemDefinition
                {
                    Email = "blackTwo@email.com"
                }
            };
            var provider = CreateProvider();

            // act
            var result = provider.RemoveBlackListedItems(contactListItemDef);

            // assert
            Assert.Collection(result,
                item => Assert.Equal("first@email.com", item.Email));
        }

        #endregion

        #region private methods

        private ContactProvider CreateProvider()
        {
            return new ContactProvider(contactDbMock.Object, formerDbMock.Object, identityResolverMock.Object, mainMapperMock.Object);
        }

        private FormerStudent CreateFormerStudent(DateTime eventStart, long formerStudentId = 1, string email = "email@test.com")
        {
            return new FormerStudent
            {
                Address = new Address
                {
                    FirstName = "FirstName",
                    LastName = "LastName"
                },
                FormerStudentId = formerStudentId,
                Email = email,
                FormerClass = new FormerClass
                {
                    EventStart = eventStart,
                    ProfileId = 1
                }
            };
        }
        #endregion
    }
}
