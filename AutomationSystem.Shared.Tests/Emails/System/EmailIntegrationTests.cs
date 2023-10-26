using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.AppLogic.Convertors;
using AutomationSystem.Shared.Core.Emails.System;
using AutomationSystem.Shared.Model;
using Moq;
using Xunit;

namespace AutomationSystem.Shared.Tests.Emails.System
{
    public class EmailIntegrationTests
    {
        private readonly Mock<IEmailDatabaseLayer> emailDbMock;
        private readonly Mock<IEmailTemplateConvertor> templateConvertorMock;
        private readonly Mock<IEmailConvertor> emailConvertorMock;
        private readonly Mock<IEmailTemplateResolver> emailTemplateResolverMock;

        public EmailIntegrationTests()
        {
            emailDbMock = new Mock<IEmailDatabaseLayer>();
            templateConvertorMock = new Mock<IEmailTemplateConvertor>();
            emailConvertorMock = new Mock<IEmailConvertor>();
            emailTemplateResolverMock = new Mock<IEmailTemplateResolver>();
        }

        #region CloneEmailTemplates() tests

        [Fact]
        public void CloneEmailTemplates_ExpectedIncludesPassToGetValidTemplates()
        {
            // arrange
            var templates = new List<EmailTemplate> { CreateTemplate() };
            emailTemplateResolverMock.Setup(e => e.GetValidTemplates(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<HashSet<LanguageEnum>>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(templates);
            var languages = new HashSet<LanguageEnum> { LanguageEnum.En };
            var integration = CreateIntegration();

            // act
            integration.CloneEmailTemplates(EmailTypeEnum.ConversationCanceled, new EmailTemplateEntityId(), LanguageEnum.En);

            // assert
            emailTemplateResolverMock.Verify(e => e.GetValidTemplates(
                new EmailTemplateEntityId(), EmailTypeEnum.ConversationCanceled, languages, EmailTemplateIncludes.EmailTemplateParameter),
                Times.Once);
        }

        [Fact]
        public void CloneEmailTemplates_NoValidEmailTemplateForLanguageId_ThrowsArgumentException()
        {
            // arrange
            emailTemplateResolverMock.Setup(e => e.GetValidTemplates(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<HashSet<LanguageEnum>>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(new List<EmailTemplate>());
            var integration = CreateIntegration();

            // act & assert
            Assert.Throws<ArgumentException>(() => integration.CloneEmailTemplates(EmailTypeEnum.ConversationCanceled, new EmailTemplateEntityId(), LanguageEnum.En));
        }

        [Fact]
        public void CloneEmailTemplates_ForGivenParameters_ReturnsEmailTemplates()
        {
            // arrange
            var enTemplate = CreateTemplate();
            var csTemplate = CreateTemplate(LanguageEnum.Cs);
            var templates = new List<EmailTemplate> { enTemplate, csTemplate };
            LanguageEnum?[] languageIds = { LanguageEnum.Cs, LanguageEnum.En };
            emailTemplateResolverMock.Setup(e => e.GetValidTemplates(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<HashSet<LanguageEnum>>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(templates);
            templateConvertorMock.SetupSequence(e => e.CloneEmailTemplate(It.IsAny<EmailTemplate>()))
                .Returns(enTemplate)
                .Returns(csTemplate);
            var integration = CreateIntegration();

            // act
            var result = integration.CloneEmailTemplates(EmailTypeEnum.ConversationCanceled, new EmailTemplateEntityId(), languageIds);

            // assert
            Assert.Collection(result,
                item => Assert.Equal(LanguageEnum.En, item.LanguageId),
                item => Assert.Equal(LanguageEnum.Cs, item.LanguageId));
            templateConvertorMock.Verify(e => e.CloneEmailTemplate(enTemplate), Times.Once);
            templateConvertorMock.Verify(e => e.CloneEmailTemplate(csTemplate), Times.Once);
        }

        #endregion

        #region SaveClonedEmailTemplates() tests

        [Fact]
        public void SaveClonedEmailTemplates_EmailTemplatesAndEmailTemplateEntityId_EmailTemplatesAreInsertedIntoDb()
        {
            // arrange
            var emailTemplates = new List<EmailTemplate>
            {
                new EmailTemplate()
            };
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4);
            var integration = CreateIntegration();

            // act
            integration.SaveClonedEmailTemplates(emailTemplates, emailTemplateEntityId);

            // assert
            Assert.Collection(emailTemplates,
                item =>
                {
                    Assert.Equal(emailTemplateEntityId.Id, item.EntityId);
                    Assert.Equal(emailTemplateEntityId.TypeId, item.EntityTypeId);
                });
            emailDbMock.Verify(e => e.InsertEmailTemplates(emailTemplates), Times.Once);
        }

        #endregion

        #region GetEmailTemplateListItemsByEntity() tests

        [Fact]
        public void GetEmailTemplateListItemsByEntity_ExpectedIncludesPassToGetEmailTemplatesByFilter()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            templateConvertorMock.Setup(e => e.ConvertToEmailTemplateListItem(It.IsAny<EmailTemplate>())).Returns(new EmailTemplateListItem());
            var integration = CreateIntegration();

            // act
            integration.GetEmailTemplateListItemsByEntity(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4));

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplatesByFilter(It.Is<EmailTemplateFilter>(x => x.EmailTemplateEntityId.Id == 4), EmailTemplateIncludes.Language), Times.Once);
        }

        [Fact]
        public void GetEmailTemplateListItemsByEntity_EmailTemplateEntityId_FilterIsSet()
        {
            // arrange
            EmailTemplateFilter filter = null;
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>()))
                .Callback(new Action<EmailTemplateFilter, EmailTemplateIncludes>((f, i) => filter = f))
                .Returns(new List<EmailTemplate>());
            templateConvertorMock.Setup(e => e.ConvertToEmailTemplateListItem(It.IsAny<EmailTemplate>())).Returns(new EmailTemplateListItem());
            var integration = CreateIntegration();

            // act
            integration.GetEmailTemplateListItemsByEntity(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 5));

            // assert
            Assert.Equal(EntityTypeEnum.MainProfile, filter.EmailTemplateEntityId.TypeId);
            Assert.Equal(5, filter.EmailTemplateEntityId.Id);
            Assert.Null(filter.IsSealed);
            Assert.True(filter.IsValidated);
            Assert.False(filter.IsDefault);
        }

        [Fact]
        public void GetEmailTemplateListItemsByEntity_EmailTemplateEntityId_ReturnsEmailTemplateListItems()
        {
            // arrange
            var emailTemplates = new List<EmailTemplate>
            {
                new EmailTemplate
                {
                    EmailTemplateId = 5
                }
            };
            var listItem = new EmailTemplateListItem();
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(emailTemplates);
            templateConvertorMock.Setup(e => e.ConvertToEmailTemplateListItem(It.IsAny<EmailTemplate>())).Returns(listItem);
            var integration = CreateIntegration();

            // act
            var result = integration.GetEmailTemplateListItemsByEntity(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 2));

            // assert
            Assert.Collection(result,
                item => Assert.Same(listItem, item));
            templateConvertorMock.Verify(e => e.ConvertToEmailTemplateListItem(It.Is<EmailTemplate>(x => x.EmailTemplateId == 5)), Times.Once);
        }

        #endregion

        #region GetEmailsByEntity() tests

        [Fact]
        public void GetEmailsByEntity_ExpectedIncludesPassToGetEmailsByEntity()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailsByEntity(It.IsAny<EntityTypeEnum>(), It.IsAny<long>(), It.IsAny<EmailIncludes>())).Returns(new List<Email>());
            emailConvertorMock.Setup(e => e.ConvertToEmailListItem(It.IsAny<Email>())).Returns(new EmailListItem());
            var integration = CreateIntegration();

            // act
            integration.GetEmailsByEntity(new EmailEntityId(EntityTypeEnum.MainProfile, 1));

            // assert
            emailDbMock.Verify(e => e.GetEmailsByEntity(EntityTypeEnum.MainProfile, 1, EmailIncludes.EmailTemplateEmailType), Times.Once);
        }

        [Fact]
        public void GetEmailsByEntity_EmailEntityId_ReturnsEmailListItems()
        {
            // arrange
            var emails = new List<Email>
            {
                new Email
                {
                    EmailId = 11
                }
            };
            var listItem = new EmailListItem();
            emailDbMock.Setup(e => e.GetEmailsByEntity(It.IsAny<EntityTypeEnum>(), It.IsAny<long>(), It.IsAny<EmailIncludes>())).Returns(emails);
            emailConvertorMock.Setup(e => e.ConvertToEmailListItem(It.IsAny<Email>())).Returns(listItem);
            var integration = CreateIntegration();

            // act
            var result = integration.GetEmailsByEntity(new EmailEntityId(EntityTypeEnum.MainProfile, 4));

            // assert
            Assert.Collection(result,
                item => Assert.Same(listItem, item));
            emailConvertorMock.Verify(e => e.ConvertToEmailListItem(It.Is<Email>(x => x.EmailId == 11)), Times.Once);
        }

        #endregion

        #region DeleteEmailTemplate() tests

        [Fact]
        public void DeleteEmailTemplate_EmailTemplateId_DeleteEmailTemplate()
        {
            // arrange
            var integration = CreateIntegration();

            // act
            integration.DeleteEmailTemplate(7);

            // assert
            emailDbMock.Verify(e => e.DeleteEmailTemplate(7, EmailOperationOption.CheckNoEmails), Times.Once);
        }

        #endregion

        #region DeleteEmailTemplatesByEntity() tests

        [Fact]
        public void DeleteEmailTemplatesByEntity_EmailEntityId_EmailTemplatesAreDeleted()
        {
            // arrange
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainProfile, 2);
            var integration = CreateIntegration();

            // act
            integration.DeleteEmailTemplatesByEntity(emailEntityId);

            // assert
            emailDbMock.Verify(e => e.DeleteEmailTemplatesByEntity(emailEntityId.TypeId, emailEntityId.Id, EmailOperationOption.CheckNoEmails), Times.Once);
        }

        #endregion

        #region private methods

        private EmailIntegration CreateIntegration()
        {
            return new EmailIntegration(emailDbMock.Object, templateConvertorMock.Object, emailConvertorMock.Object, emailTemplateResolverMock.Object);
        }

        private EmailTemplate CreateTemplate(LanguageEnum languageId = LanguageEnum.En)
        {
            return new EmailTemplate
            {
                LanguageId = languageId
            };
        }

        #endregion
    }
}
