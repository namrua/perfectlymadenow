using AutoMapper;
using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;
using AutomationSystem.Main.Core.Contacts.AppLogic.MappingProfiles;
using AutomationSystem.Main.Model;
using System;
using System.Collections.Generic;
using Xunit;
using ContactListItem = AutomationSystem.Main.Contract.Contacts.AppLogic.Models.ContactListItem;

namespace AutomationSystem.Main.Tests.Contacts.AppLogic.MappingProfiles
{
    public class ContactProfileTests
    {
        #region CreateMap<FormerStudent, ContactListItem>() tests

        [Fact]
        public void Map_AddressIsNotIncluded_ThrowsInvalidOperationException()
        {
            // arrange
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<ContactListItem>(new FormerStudent()));
        }

        [Fact]
        public void Map_FormerStudent_ReturnsContactListItem()
        {
            // arrange
            var formerStudent = new FormerStudent
            {
                FormerStudentId = 9,
                Address = new Address
                {
                    FirstName = "FirstName",
                    LastName = "LastName"
                },
                Email = "EmaiL@mail.Com"
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<ContactListItem>(formerStudent);

            // assert
            Assert.Equal(9, result.FormerStudentId);
            Assert.Equal("FirstName LastName", result.Name);
            Assert.Equal("email@mail.com", result.Email);
        }

        #endregion

        #region CreateMap<ContactListDefinition, ContactList>() tests

        [Fact]
        public void Map_ContactListDefinition_ReturnsContactList()
        {
            // arrange
            var contactListItemDefinition = new List<ContactListItemDefinition>
            {
                new ContactListItemDefinition
                {
                    FormerStudentId = 5,
                    Email = "Email@email.com"
                }
            };
            var contactListDefinition = new ContactListDefinition
            {
                ContactListItems = contactListItemDefinition,
                ProfileId = 1
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<ContactList>(contactListDefinition);

            // assert
            Assert.Collection(result.ContactListItems,
                item =>
                {
                    Assert.Equal(5, item.FormerStudentId);
                    Assert.Equal("Email@email.com", item.Email);
                });
            Assert.Equal(1, result.ProfileId);
        }

        #endregion

        #region CreateMap<ContactListItemDefinition, Model.ContactListItem>() tests

        [Fact]
        public void Map_ContactListItemDefinition_ReturnsContactListItem()
        {
            // arrange
            var contactListItemDefinition = new ContactListItemDefinition
            {
                    FormerStudentId = 5,
                    Email = "Email@email.com"
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<Model.ContactListItem>(contactListItemDefinition);

            // assert
            Assert.Equal(5, result.FormerStudentId);
            Assert.Equal("Email@email.com", result.Email);
        }

        #endregion

        #region CreateMap<Model.ContactListItem, ContactListItemDefinition>() tests

        [Fact]
        public void Map_ContactListItem_ReturnsContactListItemDefinition()
        {
            // arrange
            var contactListItem = new Model.ContactListItem
            {
                FormerStudentId = 4,
                Email = "Email@email.com"
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<ContactListItemDefinition>(contactListItem);

            // assert
            Assert.Equal(4, result.FormerStudentId);
            Assert.Equal("Email@email.com", result.Email);
        }

        #endregion

        #region CreateMap<ContactList, ContactListDetail>() tests

        [Fact]
        public void Map_ContactListItemsIsNotIncluded_ThrowsInvalidOperationException()
        {
            // arrange
            var contactList = new ContactList
            {
                ContactListItems = null
            };
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<ContactListDetail>(contactList));
        }

        [Fact]
        public void Map_ContactList_ReturnsContactListDetail()
        {
            // arrange
            var contactListItems = new List<Model.ContactListItem>
            {
                new Model.ContactListItem
                {
                    Email = "Email@email.com"
                }
            };
            var contactList = new ContactList
            {
                IsNotified = true,
                Notified = new DateTime(2021, 12, 21),
                ContactListItems = contactListItems,
                ContactListId = 12,
                SenderId = 5
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<ContactListDetail>(contactList);

            // assert
            Assert.Collection(result.Emails,
                item =>
                {
                    Assert.Equal("Email@email.com", item);
                });
            Assert.Equal(5, result.Form.SenderId);
            Assert.Equal(12, result.Form.ContactListId);
            Assert.True(result.IsNotified);
            Assert.Equal(contactList.Notified, result.Notified);
        }

        #endregion

        #region private methods

        private Mapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ContactProfile());
            });
            return new Mapper(mapperConfiguration);
        }

        #endregion
    }
}
