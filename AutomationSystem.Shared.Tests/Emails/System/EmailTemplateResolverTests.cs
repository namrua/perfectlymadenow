using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using AutomationSystem.Shared.Core.Emails.System;
using AutomationSystem.Shared.Model;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Shared.Tests.Emails.System
{
    public class EmailTemplateResolverTests
    {
        private readonly Mock<IEmailTemplateHierarchyResolver> resolverMock;
        private readonly Mock<IEmailDatabaseLayer> emailDbMock;

        public EmailTemplateResolverTests()
        {
            resolverMock = new Mock<IEmailTemplateHierarchyResolver>();
            emailDbMock = new Mock<IEmailDatabaseLayer>();
        }

        #region GetParentTemplate(EmailTemplateEntityId, LanguageEnum, EmailTypeEnum, EmailTemplateIncludes) tests

        [Fact]
        public void GetParentTemplate_NoParentEmailTemplate_ThrowsInvalidOperationException()
        {
            // arrange
            var entityHierarchy = new EmailTemplateEntityHierarchy
            {
                CanUseDefault = true
            };
            var emailTemplateEntityId = new EmailTemplateEntityId();
            resolverMock.Setup(e => e.GetHierarchyForParent(It.IsAny<EmailTemplateEntityId>())).Returns(entityHierarchy);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            emailDbMock.Setup(e => e.GetDefaultEmailTemplateByType(It.IsAny<EmailTypeEnum>(), It.IsAny<EmailTemplateIncludes>())).Returns((EmailTemplate)null);
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => resolver.GetParentTemplate(emailTemplateEntityId, LanguageEnum.En, EmailTypeEnum.ClassDailyReports));
            resolverMock.Verify(e => e.GetHierarchyForParent(emailTemplateEntityId), Times.Once);
            emailDbMock.Verify(e => e.GetDefaultEmailTemplateByType(EmailTypeEnum.ClassDailyReports, EmailTemplateIncludes.None), Times.Once);
        }

        [Fact]
        public void GetParentTemplate_ForEmailTemplateInDb_ReturnsEmailTemplate()
        {
            // arrange
            var entityHierarchy = new EmailTemplateEntityHierarchy
            {
                Entities = new List<EmailTemplateEntityId>
                {
                    new EmailTemplateEntityId()
                }
            };
            var template = new EmailTemplate();
            var templates = new List<EmailTemplate>{ template };
            resolverMock.Setup(e => e.GetHierarchyForParent(It.IsAny<EmailTemplateEntityId>())).Returns(entityHierarchy);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(templates);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetParentTemplate(new EmailTemplateEntityId(), LanguageEnum.En, EmailTypeEnum.ClassDailyReports);

            // assert
            Assert.Same(template, result);
        }

        [Fact]
        public void GetParentTemplate_NoEmailTemplateInDb_ReturnsDefaultEmailTemplate()
        {
            // arrange
            var entityHierarchy = new EmailTemplateEntityHierarchy
            {
                CanUseDefault = true
            };
            var defaultTemplate = new EmailTemplate();
            resolverMock.Setup(e => e.GetHierarchyForParent(It.IsAny<EmailTemplateEntityId>())).Returns(entityHierarchy);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            emailDbMock.Setup(e => e.GetDefaultEmailTemplateByType(It.IsAny<EmailTypeEnum>(), It.IsAny<EmailTemplateIncludes>())).Returns(defaultTemplate);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetParentTemplate(new EmailTemplateEntityId(), LanguageEnum.En, EmailTypeEnum.ClassDailyReports);

            // assert
            Assert.Same(defaultTemplate, result);
        }

        #endregion

        #region GetParentTemplate(EmailTemplate, EmailTemplateIncludes) tests

        [Fact]
        public void GetParentTemplate_EmailTemplate_ReturnsEmailTemplate()
        {
            // arrange
            var template = new EmailTemplate
            {
                EntityTypeId = EntityTypeEnum.MainProfile,
                EntityId = 4,
                LanguageId = LanguageEnum.En,
                EmailTypeId = EmailTypeEnum.ConversationCanceled
            };
            var entityHierarchy = new EmailTemplateEntityHierarchy
            {
                Entities = new List<EmailTemplateEntityId>
                {
                    new EmailTemplateEntityId()
                }
            };
            var templates = new List<EmailTemplate> { template };
            resolverMock.Setup(e => e.GetHierarchyForParent(It.IsAny<EmailTemplateEntityId>())).Returns(entityHierarchy);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(templates);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetParentTemplate(template);

            // assert
            Assert.Equal(LanguageEnum.En, result.LanguageId);
            Assert.Equal(EmailTypeEnum.ConversationCanceled, result.EmailTypeId);
        }

        [Fact]
        public void GetParentTemplate_NoTemplateInDb_ReturnsDefaultEmailTemplate()
        {
            // arrange
            var entityHierarchy = new EmailTemplateEntityHierarchy
            {
                CanUseDefault = true
            };
            var defaultTemplate = new EmailTemplate();
            resolverMock.Setup(e => e.GetHierarchyForParent(It.IsAny<EmailTemplateEntityId>())).Returns(entityHierarchy);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            emailDbMock.Setup(e => e.GetDefaultEmailTemplateByType(It.IsAny<EmailTypeEnum>(), It.IsAny<EmailTemplateIncludes>())).Returns(defaultTemplate);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetParentTemplate(new EmailTemplate());

            // assert
            Assert.Same(defaultTemplate, result);
        }

        #endregion

        #region GetValidTemplates() tests

        [Fact]
        public void GetValidTemplates_ForGivenParameters_ReturnsValidEmailTemplates()
        {
            // arrange
            var templates = new List<EmailTemplate> { CreateTemplate() };
            resolverMock.Setup(e => e.GetHierarchy(It.IsAny<EmailTemplateEntityId>())).Returns(CreateHierarchy());
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(templates);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetValidTemplates(
                new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4),
                EmailTypeEnum.ConversationCanceled,
                new HashSet<LanguageEnum> { LanguageEnum.En });

            // assert
            Assert.Collection(result,
                item =>
                {
                    Assert.Equal(LanguageEnum.En, item.LanguageId);
                });
            resolverMock.Verify(e => e.GetHierarchy(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4)), Times.Once);
            emailDbMock.Verify(e => e.GetEmailTemplatesByFilter(
                It.Is<EmailTemplateFilter>(x =>
                    x.EmailTypeId == EmailTypeEnum.ConversationCanceled
                    && x.IsDefault == false
                    && x.IsValidated == true),
                EmailTemplateIncludes.None),
                Times.Once);
        }

        [Fact]
        public void GetValidTemplates_TemplatesWithDifferentLanguageId_TemplatesAreFilteredByLanguageIds()
        {
            // arrange
            var templates = new List<EmailTemplate> { CreateTemplate(), CreateTemplate(LanguageEnum.Cs) };
            resolverMock.Setup(e => e.GetHierarchy(It.IsAny<EmailTemplateEntityId>())).Returns(CreateHierarchy());
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(templates);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetValidTemplates(
                new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4),
                EmailTypeEnum.ConversationCanceled,
                new HashSet<LanguageEnum> { LanguageEnum.En });

            // assert
            Assert.Collection(result,
                item => Assert.Equal(LanguageEnum.En, item.LanguageId));
            emailDbMock.Verify(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), EmailTemplateIncludes.None), Times.Once);
        }

        [Fact]
        public void GetValidTemplates_TemplatesWithSameLanguageId_SelectFirstTemplate()
        {
            // arrange
            var profileTemplates = new List<EmailTemplate> { CreateTemplate(LanguageEnum.En, 14) };
            var secondTemplates = new List<EmailTemplate> { CreateTemplate(), CreateTemplate(LanguageEnum.Cs) };
            resolverMock.Setup(e => e.GetHierarchy(It.IsAny<EmailTemplateEntityId>())).Returns(CreateHierarchy());
            emailDbMock.SetupSequence(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>()))
                .Returns(profileTemplates)
                .Returns(secondTemplates);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetValidTemplates(
                new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4),
                EmailTypeEnum.ConversationCanceled,
                new HashSet<LanguageEnum> { LanguageEnum.En, LanguageEnum.Cs });

            // assert
            Assert.Collection(result,
                item =>
                {
                    Assert.Equal(LanguageEnum.En, item.LanguageId);
                    Assert.Equal(14, item.EmailTemplateId);
                },
                item => Assert.Equal(LanguageEnum.Cs, item.LanguageId));
            emailDbMock.Verify(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), EmailTemplateIncludes.None), Times.Exactly(2));
        }
        #endregion

        #region GetEmailTemplateByEmailTemplateEntityId() tests

        [Fact]
        public void GetEmailTemplateByEmailTemplateEntityId_ExpectedIncludesPassToGetEmailTemplateByEmailTemplateEntityId()
        {
            // arrange
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainContactList, 4);
            emailDbMock.Setup(e => e.GetEmailTemplateByEmailTemplateEntityId(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            var resolver = CreateResolver();

            // act
            resolver.GetEmailTemplateByEmailTemplateEntityId(emailTemplateEntityId, EmailTemplateIncludes.EmailType);

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplateByEmailTemplateEntityId(emailTemplateEntityId, EmailTemplateIncludes.EmailType), Times.Once);
        }

        [Fact]
        public void GetEmailTemplateByEmailTemplateEntityId_NoEmailTemplateInDb_ThrowsArgumentException()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailTemplateByEmailTemplateEntityId(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTemplateIncludes>())).Returns((EmailTemplate)null);
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<ArgumentException>(() => resolver.GetEmailTemplateByEmailTemplateEntityId(new EmailTemplateEntityId()));
        }

        [Fact]
        public void GetEmailTemplateByEmailTemplateEntityId_EmailTemplateEntityId_ReturnsEmailTemplate()
        {
            // arrange
            var template = CreateTemplate();
            emailDbMock.Setup(e => e.GetEmailTemplateByEmailTemplateEntityId(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetEmailTemplateByEmailTemplateEntityId(new EmailTemplateEntityId(EntityTypeEnum.MainContactList, 3));

            // assert
            Assert.Same(template, result);
        }

        #endregion

        #region GetValidTemplate() tests

        [Fact]
        public void GetValidTemplate_ExpectedIncludesPassToGetEmailTemplatesByFilter()
        {
            // arrange
            resolverMock.Setup(e => e.GetHierarchy(It.IsAny<EmailTemplateEntityId>())).Returns(CreateHierarchy());
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>{ CreateTemplate() });
            var resolver = CreateResolver();

            // act
            resolver.GetValidTemplate(EmailTypeEnum.RegistrationConfirmation, LanguageEnum.En, new EmailTemplateEntityId());

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), EmailTemplateIncludes.None), Times.Once);

        }

        [Fact]
        public void GetValidTemplate_NoEmailTemplateWithEmailTypeId_ThrowsArgumentException()
        {
            
            // arrange
            resolverMock.Setup(e => e.GetHierarchy(It.IsAny<EmailTemplateEntityId>())).Returns(CreateHierarchy());
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<ArgumentException>(() => resolver.GetValidTemplate(EmailTypeEnum.RegistrationConfirmation, LanguageEnum.En, new EmailTemplateEntityId()));
        }

        [Fact]
        public void GetValidTemplate_ForGivenParameters_ReturnsEmailTemplate()
        {
            // arrange
            var template = CreateTemplate();
            resolverMock.Setup(e => e.GetHierarchy(It.IsAny<EmailTemplateEntityId>())).Returns(CreateHierarchy());
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate> { template });
            var resolver = CreateResolver();

            // act
            var result = resolver.GetValidTemplate(EmailTypeEnum.RegistrationConfirmation, LanguageEnum.En, new EmailTemplateEntityId());

            // assert
            Assert.Equal(1, result.EmailTemplateId);
            Assert.Equal(LanguageEnum.En, result.LanguageId);
            Assert.Same(template, result);
        }

        #endregion

        #region private methods

        private EmailTemplateResolver CreateResolver()
        {
            return new EmailTemplateResolver(resolverMock.Object, emailDbMock.Object);
        }

        private EmailTemplate CreateTemplate(LanguageEnum languageId = LanguageEnum.En, long emailTemplateId = 1)
        {
            return new EmailTemplate
            {
                LanguageId = languageId,
                EmailTemplateId = emailTemplateId
            };
        }

        private EmailTemplateEntityHierarchy CreateHierarchy()
        {
            var hierarchy = new EmailTemplateEntityHierarchy();
            hierarchy.Entities.Add(new EmailTemplateEntityId());
            hierarchy.Entities.Add(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4));

            return hierarchy;
        }

        #endregion
    }
}
