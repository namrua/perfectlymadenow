using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Core.Emails.AppLogic;
using AutomationSystem.Shared.Model;
using Moq;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.AppLogic.Convertors;
using AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation;
using Xunit;

namespace AutomationSystem.Shared.Tests.Emails.AppLogic
{
    public class EmailTemplateAdministrationTests
    {
        private readonly Mock<ILocalisationService> localisationServiceMock;
        private readonly Mock<IEmailDatabaseLayer> emailDbMock;
        private readonly Mock<IEmailTemplateConvertor> convertorMock;
        private readonly Mock<IEmailParameterValidatorFactory> paramsValidatorFactoryMock;
        private readonly Mock<IEmailTemplateAdministrationCommonLogic> commonLogicMock;
        private readonly Mock<IEmailTemplateResolver> templateResolverMock;
        private readonly Mock<IEmailPermissionResolver> permissionResolverMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;

        public EmailTemplateAdministrationTests()
        {
            localisationServiceMock = new Mock<ILocalisationService>();
            emailDbMock = new Mock<IEmailDatabaseLayer>();
            convertorMock = new Mock<IEmailTemplateConvertor>();
            paramsValidatorFactoryMock = new Mock<IEmailParameterValidatorFactory>();
            commonLogicMock = new Mock<IEmailTemplateAdministrationCommonLogic>();
            templateResolverMock = new Mock<IEmailTemplateResolver>();
            permissionResolverMock = new Mock<IEmailPermissionResolver>();
            identityResolverMock = new Mock<IIdentityResolver>();
        }

        #region GetEmailTypeSummary() tests

        [Fact]
        public void GetEmailTypeSummary_AllowedEmailTypesNotNull_ReturnsAllowedEmailTypes()
        {
            // arrange
            var emailTypes = new List<EmailType>
            {
                new EmailType
                {
                    EmailTypeId = EmailTypeEnum.ConversationCanceled
                },
                new EmailType
                {
                    EmailTypeId = EmailTypeEnum.RegistrationConfirmation
                },
                new EmailType
                {
                    EmailTypeId = EmailTypeEnum.WwaRegistrationConfirmation
                },
                new EmailType
                {
                    EmailTypeId = EmailTypeEnum.RegistrationCanceled
                },
            };
            emailDbMock.Setup(e => e.GetEmailTypes()).Returns(emailTypes);
            var allowedTypes = new HashSet<EmailTypeEnum> {EmailTypeEnum.ConversationCanceled, EmailTypeEnum.ConversationCompleted};
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            var admin = CreateAdmin();

            // act
            var result = admin.GetEmailTypeSummary(new EmailTemplateEntityId(), allowedTypes);

            // assert
            Assert.Collection(result.Items,
                item => Assert.Equal(EmailTypeEnum.ConversationCanceled, item.EmailTypeId));
        }

        [Fact]
        public void GetEmailTypeSummary_EmailTemplateFilter_FilterIsSetAndCalled()
        {
            // arrange
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 12);
            emailDbMock.Setup(e => e.GetEmailTypes()).Returns(new List<EmailType>());
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            var admin = CreateAdmin();

            // act
            admin.GetEmailTypeSummary(emailTemplateEntityId);

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplatesByFilter(It.Is<EmailTemplateFilter>(x => x.IsDefault.Equals(true)), EmailTemplateIncludes.None), Times.Once);
            emailDbMock.Verify(e => e.GetEmailTemplatesByFilter(It.Is<EmailTemplateFilter>(x =>
                x.IsDefault.Equals(false) && x.EmailTemplateEntityId == emailTemplateEntityId),
                EmailTemplateIncludes.None),
                Times.Once);
        }

        [Fact]
        public void GetEmailTypeSummary_EmailTemplateEntityId_ReturnsEmailTypeSummary()
        {
            // arrange
            var emailTypes = CreateEmailTypes();
            var emailTemplates = CreateTemplates();
            var defaultTemplates = CreateDefaultTemplates();
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 11);
            emailDbMock.Setup(e => e.GetEmailTypes()).Returns(emailTypes);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.Is<EmailTemplateFilter>(x => x.IsDefault == true), It.IsAny<EmailTemplateIncludes>())).Returns(defaultTemplates);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.Is<EmailTemplateFilter>(x => x.IsDefault == false), It.IsAny<EmailTemplateIncludes>())).Returns(emailTemplates);
            var admin = CreateAdmin();

            // act
            var result = admin.GetEmailTypeSummary(emailTemplateEntityId);

            // assert
            Assert.Same(emailTemplateEntityId, result.EmailTemplateEntityId);
            Assert.Collection(result.Items,
                item =>
                {
                    Assert.Equal(EmailTypeEnum.ConversationChanged, item.EmailTypeId);
                    Assert.Equal("ConversationChanged", item.EmailType);
                    Assert.Equal(2, item.EmailCount);
                    Assert.Equal(1, item.ValidEmailCount);
                    Assert.False(item.IsAllValid);
                    Assert.False(item.IsLocalisable);
                },
                item =>
                {
                    Assert.Equal(EmailTypeEnum.ConversationCompleted, item.EmailTypeId);
                    Assert.Equal("ConversationCompleted", item.EmailType);
                    Assert.Equal(0, item.EmailCount);
                    Assert.True(item.IsAllValid);
                    Assert.True(item.IsLocalisable);
                });
        }

        #endregion

        #region GetSystemEmailTypeSummary() tests

        [Fact]
        public void GetSystemEmailTypeSummary_ReturnsEmailTypeSummary()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailTypes()).Returns(new List<EmailType> { new EmailType{ EmailTypeId = EmailTypeEnum.CoreIncidentWarning } });
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            var admin = CreateAdmin();

            // act
            var result = admin.GetSystemEmailTypeSummary();

            // assert
            Assert.Collection(result.Items,
                item => Assert.Equal(EmailTypeEnum.CoreIncidentWarning, item.EmailTypeId));
            Assert.True(result.EmailTemplateEntityId.IsNull);
        }

        #endregion

        #region GetEmailTemplateList() tests

        [Fact]
        public void GetEmailTemplateList_NoPermissionIsGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var templateEntityId = CreateEmailTemplateEntityId();
            permissionResolverMock.Setup(e =>
                    e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetEmailTemplateList(EmailTypeEnum.ConversationCanceled, templateEntityId));
            permissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(templateEntityId, EmailTypeEnum.ConversationCanceled), Times.Once);
        }

        #endregion

        #region GetEmailTemplateDetail() tests

        [Fact]
        public void GetEmailTemplateDetail_NoPermissionIsGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var template = CreateTemplate();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            permissionResolverMock.Setup(e =>
                    e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetEmailTemplateDetail(1));
            permissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(
                It.Is<EmailTemplateEntityId>(x => x.Id == template.EntityId),
                EmailTypeEnum.ConversationCanceled),
                Times.Once);
        }

        #endregion

        #region ResetEmailTemplate() tests

        [Fact]
        public void ResetEmailTemplate_ForEmailTemplateId_EmailTemplateIsReset()
        {
            // arrange
            var template = new EmailTemplate();
            var parentTemplate = new EmailTemplate();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            templateResolverMock.Setup(e => e.GetParentTemplate(It.IsAny<EmailTemplate>(), It.IsAny<EmailTemplateIncludes>())).Returns(parentTemplate);
            commonLogicMock.Setup(e => e.ValidateEmailTemplate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEmailParameterValidator>())).Returns(new EmailTemplateValidationResult());
            var admin = CreateAdmin();

            // act
            admin.ResetEmailTemplate(3);

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplateById(3, EmailTemplateIncludes.EmailTemplateParameter), Times.Once);
            templateResolverMock.Verify(e => e.GetParentTemplate(template, EmailTemplateIncludes.EmailTemplateParameter), Times.Once);
            convertorMock.Verify(e => e.ResetEmailTemplate(template, parentTemplate), Times.Once);
            emailDbMock.Verify(e => e.UpdateEmailTemplate(template, EmailOperationOption.None), Times.Once);
        }

        [Fact]
        public void ResetEmailTemplate_TemplateIsValidAfterReset_IsValidatedIsUpdated()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            templateResolverMock.Setup(e => e.GetParentTemplate(It.IsAny<EmailTemplate>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            commonLogicMock.Setup(e => e.ValidateEmailTemplate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEmailParameterValidator>()))
                .Returns(new EmailTemplateValidationResult
                {
                    IsValid = true
                });
            var admin = CreateAdmin();

            // act
            admin.ResetEmailTemplate(3);

            // assert
            emailDbMock.Verify(e => e.UpdateEmailTemplate(It.Is<EmailTemplate>(x => x.IsValidated), EmailOperationOption.None), Times.Once());
        }

        [Fact]
        public void ResetEmailTemplate_NoPermissionIsGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var template = CreateTemplate();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            permissionResolverMock.Setup(e =>
                    e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.ResetEmailTemplate(2));
            permissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(
                    It.Is<EmailTemplateEntityId>(x => x.Id == template.EntityId),
                    EmailTypeEnum.ConversationCanceled),
                Times.Once);
        }

        #endregion

        #region GetNewEmailTemplateMetadataForEdit() tests

        [Fact]
        public void GetNewEmailTemplateMetadataForEdit_NoPermissionIsGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var templateEntityId = CreateEmailTemplateEntityId();
            permissionResolverMock.Setup(e =>
                    e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetNewEmailTemplateMetadataForEdit(EmailTypeEnum.ConversationCanceled, LanguageEnum.Cs, templateEntityId));
            permissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(templateEntityId, EmailTypeEnum.ConversationCanceled), Times.Once);
        }

        [Fact]
        public void GetNewEmailTemplateMetadataForEdit_EmailTemplateIsNotLocalisableAndLanguageIsNotDefault_ThrowsArgumentException()
        {
            // arrange
            var template = new EmailTemplate
            {
                IsLocalisable = false
            };
            templateResolverMock.Setup(e => e.GetParentTemplate(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<LanguageEnum>(),
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(template);
            emailDbMock.Setup(e => e.GetDefaultEmailTemplateByType(It.IsAny<EmailTypeEnum>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetNewEmailTemplateMetadataForEdit(EmailTypeEnum.ConversationChanged, LanguageEnum.Cs, new EmailTemplateEntityId()));
        }

        [Fact]
        public void GetNewEmailTemplateMetadataForEdit_LanguageIsNotSupported_ThrowsArgumentException()
        {
            // arrange
            var template = new EmailTemplate
            {
                IsLocalisable = true
            };
            var languagesWithEmail = new HashSet<LanguageEnum> { LanguageEnum.En };
            templateResolverMock.Setup(e => e.GetParentTemplate(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<LanguageEnum>(),
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(template);
            emailDbMock.Setup(e => e.GetDefaultEmailTemplateByType(It.IsAny<EmailTypeEnum>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            emailDbMock.Setup(e => e.GetLanguageIdsForEmailType(It.IsAny<EmailTemplateFilter>())).Returns(languagesWithEmail);
            localisationServiceMock.Setup(e => e.GetAllLanguages()).Returns(new List<IEnumItem>());
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetNewEmailTemplateMetadataForEdit(EmailTypeEnum.ConversationCanceled, LanguageEnum.Cs, new EmailTemplateEntityId()));
            emailDbMock.Verify(e => e.GetLanguageIdsForEmailType(It.Is<EmailTemplateFilter>(x => x.EmailTypeId == EmailTypeEnum.ConversationCanceled)), Times.Once());
            localisationServiceMock.Verify(e => e.GetAllLanguages(), Times.Once);
        }

        [Fact]
        public void GetNewEmailTemplateMetadataForEdit_LanguageWithEmailTemplate_ThrowsArgumentException()
        {
            // arrange
            var template = new EmailTemplate
            {
                IsLocalisable = false
            };
            var languagesWithEmail = new HashSet<LanguageEnum> { LanguageEnum.En };
            var items = new List<IEnumItem>
            {
                new EnumItem(1, "English", "English")
            };
            templateResolverMock.Setup(e => e.GetParentTemplate(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<LanguageEnum>(),
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(template);
            emailDbMock.Setup(e => e.GetDefaultEmailTemplateByType(It.IsAny<EmailTypeEnum>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            emailDbMock.Setup(e => e.GetLanguageIdsForEmailType(It.IsAny<EmailTemplateFilter>())).Returns(languagesWithEmail);
            localisationServiceMock.Setup(e => e.GetAllLanguages()).Returns(items);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetNewEmailTemplateMetadataForEdit(EmailTypeEnum.ConversationChanged, LanguageEnum.En, new EmailTemplateEntityId()));
        }

        [Fact]
        public void GetNewEmailTemplateMetadataForEdit_AllParametersAreSet_ReturnsEmailTemplateMetadataForEdit()
        {
            // arrange
            var template = new EmailTemplate();
            var form = new EmailTemplateMetadataForm
            {
                EmailTypeId = EmailTypeEnum.ConversationCanceled
            };
            var forEdit = new EmailTemplateMetadataForEdit();
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 3);
            var items = new List<IEnumItem>
            {
                new EnumItem(1, "English", "English")
            };
            templateResolverMock.Setup(e => e.GetParentTemplate(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<LanguageEnum>(),
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(template);
            emailDbMock.Setup(e => e.GetDefaultEmailTemplateByType(It.IsAny<EmailTypeEnum>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            emailDbMock.Setup(e => e.GetLanguageIdsForEmailType(It.IsAny<EmailTemplateFilter>())).Returns(new HashSet<LanguageEnum>());
            localisationServiceMock.Setup(e => e.GetAllLanguages()).Returns(items);
            convertorMock.Setup(e => e.InitializeEmailTemplateMetadataForEdit(It.IsAny<EmailTemplate>(), It.IsAny<bool>())).Returns(forEdit);
            convertorMock.Setup(e => e.ConvertToEmailTemplateMetadataForm(It.IsAny<EmailTemplate>(), It.IsAny<bool>())).Returns(form);
            var admin = CreateAdmin();

            // act
            var result = admin.GetNewEmailTemplateMetadataForEdit(EmailTypeEnum.ConversationCanceled, LanguageEnum.En, emailTemplateEntityId);

            // assert
            Assert.Equal(3, result.Form.EntityId);
            Assert.Equal(1, result.Language.Id);
            Assert.Equal(EmailTypeEnum.ConversationCanceled, result.Form.EmailTypeId);
            Assert.Same(form, result.Form);
            Assert.Same(forEdit, result);
            convertorMock.Verify(e => e.InitializeEmailTemplateMetadataForEdit(template, true), Times.Once);
            convertorMock.Verify(e => e.ConvertToEmailTemplateMetadataForm(template, true), Times.Once);
        }

        #endregion

        #region GetEmailTemplateMetadataForEditById() tests
        
        [Fact]
        public void GetEmailTemplateMetadataForEditById_NoEmailTemplateIdInDb_ThrowsArgumentException()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns((EmailTemplate)null);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetEmailTemplateMetadataForEditById(1));
            emailDbMock.Verify(e => e.GetEmailTemplateById(
                1,
                EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language),
                Times.Once);
        }

        [Fact]
        public void GetEmailTemplateMetadataForEditById_NoPermissionIsGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var template = CreateTemplate();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>()))
                .Returns(template);
            permissionResolverMock.Setup(e =>
                    e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetEmailTemplateMetadataForEditById(2));
            permissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(
                    It.Is<EmailTemplateEntityId>(x => x.Id == template.EntityId),
                    EmailTypeEnum.ConversationCanceled),
                Times.Once);
        }

        [Fact]
        public void GetEmailTemplateMetadataForEditById_EmailTypeId_ReturnsDefaultTemplate()
        {
            // arrange
            var template = new EmailTemplate
            {
                EmailTypeId = EmailTypeEnum.ClassDailyReports
            };
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            templateResolverMock.Setup(e => e.GetParentTemplate(It.IsAny<EmailTemplate>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            convertorMock.Setup(e => e.InitializeEmailTemplateMetadataForEdit(It.IsAny<EmailTemplate>(), It.IsAny<bool>())).Returns(new EmailTemplateMetadataForEdit());
            convertorMock.Setup(e => e.ConvertToEmailTemplateMetadataForm(It.IsAny<EmailTemplate>(), It.IsAny<bool>())).Returns(new EmailTemplateMetadataForm());
            var admin = CreateAdmin();

            // act
            admin.GetEmailTemplateMetadataForEditById(1);

            // assert
            templateResolverMock.Verify(e => e.GetParentTemplate(template, EmailTemplateIncludes.None), Times.Once);
        }

        [Fact]
        public void GetEmailTemplateMetadataForEditById_EmailTemplateId_ReturnsEmailTemplateMetadataForEdit()
        {
            // arrange
            var template = new EmailTemplate
            {
                EntityId = 1,
                EntityTypeId = EntityTypeEnum.MainProfile,
                EmailTemplateId = 10,
                EmailTypeId = EmailTypeEnum.ConversationCanceled
            };
            var parentTemplate = new EmailTemplate
            {
                EmailTemplateId = 10,
                EmailTypeId = EmailTypeEnum.ConversationCanceled
            };
            var form = new EmailTemplateMetadataForm
            {
                EntityId = 4,
                EmailTypeId = EmailTypeEnum.ConversationCanceled,
                ParentEmailTemplateId = 10
            };
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            templateResolverMock.Setup(e => e.GetParentTemplate(It.IsAny<EmailTemplate>(), It.IsAny<EmailTemplateIncludes>())).Returns(parentTemplate);
            convertorMock.Setup(e => e.InitializeEmailTemplateMetadataForEdit(It.IsAny<EmailTemplate>(), It.IsAny<bool>())).Returns(new EmailTemplateMetadataForEdit());
            convertorMock.Setup(e => e.ConvertToEmailTemplateMetadataForm(It.IsAny<EmailTemplate>(), It.IsAny<bool>())).Returns(form);
            var admin = CreateAdmin();

            // act
            var result = admin.GetEmailTemplateMetadataForEditById(10);

            // assert
            Assert.Equal(4, result.Form.EntityId);
            Assert.Equal(10, result.Form.ParentEmailTemplateId);
            Assert.Equal(EmailTypeEnum.ConversationCanceled, result.Form.EmailTypeId);
            convertorMock.Verify(e => e.InitializeEmailTemplateMetadataForEdit(template, false), Times.Once);
            convertorMock.Verify(e => e.ConvertToEmailTemplateMetadataForm(template, false), Times.Once);
        }

        #endregion

        #region GetEmailTemplateMetadataForEditByForm() tests

        [Fact]
        public void GetEmailTemplateMetadataForEditByForm_NoPermissionIsGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var form = new EmailTemplateMetadataForm
            {
                EntityId = 2,
                EntityTypeId = EntityTypeEnum.MainProfile,
                EmailTypeId = EmailTypeEnum.ClassFinalReports
            };
            permissionResolverMock.Setup(e =>
                    e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetEmailTemplateMetadataForEditByForm(form));
            permissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(
                It.Is<EmailTemplateEntityId>(x => x.Id == form.EntityId),
                EmailTypeEnum.ClassFinalReports),
                Times.Once);
        }

        [Fact]
        public void GetEmailTemplateMetadataForEditByForm_NewForm_ReturnsEmailTemplateMetadataForEdit()
        {
            // arrange
            var template = new EmailTemplate
            {
                IsLocalisable = true
            };
            var form = new EmailTemplateMetadataForm
            {
                EmailTemplateId = 0,
                EmailTypeId = EmailTypeEnum.ClassDailyReports,
                EntityId = 3,
                EntityTypeId = EntityTypeEnum.MainProfile,
                LanguageId = LanguageEnum.En
            };
            var items = new List<IEnumItem>
            {
                new EnumItem(1, "English", "English")
            };
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            convertorMock.Setup(e => e.InitializeEmailTemplateMetadataForEdit(It.IsAny<EmailTemplate>(), It.IsAny<bool>())).Returns(new EmailTemplateMetadataForEdit());
            localisationServiceMock.Setup(e => e.GetAllLanguages()).Returns(items);
            emailDbMock.Setup(e => e.GetLanguageIdsForEmailType(It.IsAny<EmailTemplateFilter>())).Returns(new HashSet<LanguageEnum>());
            var admin = CreateAdmin();

            // act
            var result = admin.GetEmailTemplateMetadataForEditByForm(form);

            // assert
            Assert.Same(form, result.Form);
            convertorMock.Verify(e => e.InitializeEmailTemplateMetadataForEdit(template, true), Times.Once);
            emailDbMock.Verify(e => e.GetEmailTemplateById(
                0,
                EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language),
                Times.Never);
        }

        [Fact]
        public void GetEmailTemplateMetadataForEditByForm_EmailTemplateMetadataForm_ReturnsEmailTemplateMetadataForEdit()
        {
            // arrange
            var template = new EmailTemplate();
            var form = new EmailTemplateMetadataForm
            {
                EmailTemplateId = 15
            };
            var items = new List<IEnumItem>
            {
                new EnumItem(1, "English", "English")
            };
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            templateResolverMock.Setup(e => e.GetParentTemplate(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<LanguageEnum>(),
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(template);
            convertorMock.Setup(e => e.InitializeEmailTemplateMetadataForEdit(It.IsAny<EmailTemplate>(), It.IsAny<bool>())).Returns(new EmailTemplateMetadataForEdit());
            localisationServiceMock.Setup(e => e.GetAllLanguages()).Returns(items);
            emailDbMock.Setup(e => e.GetLanguageIdsForEmailType(It.IsAny<EmailTemplateFilter>())).Returns(new HashSet<LanguageEnum>());
            var admin = CreateAdmin();

            // act
            var result = admin.GetEmailTemplateMetadataForEditByForm(form);

            // assert
            Assert.Same(form, result.Form);
            emailDbMock.Verify(e => e.GetEmailTemplateById(
                    15,
                    EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language),
                Times.Once);
            convertorMock.Verify(e => e.InitializeEmailTemplateMetadataForEdit(template, false), Times.Once);
        }

        #endregion

        #region SaveEmailTemplateMetadata() tests

        [Fact]
        public void SaveEmailTemplateMetadata_NoPermissionIsGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var form = new EmailTemplateMetadataForm
            {
                EntityId = 2,
                EntityTypeId = EntityTypeEnum.MainProfile,
                EmailTypeId = EmailTypeEnum.ClassFinalReports
            };
            permissionResolverMock.Setup(e =>
                    e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SaveEmailTemplateMetadata(form));
            permissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(
                It.Is<EmailTemplateEntityId>(x => x.Id == form.EntityId),
                EmailTypeEnum.ClassFinalReports),
                Times.Once);
        }

        [Fact]
        public void SaveEmailTemplateMetadata_TemplateCannotBeLocalisedAndLanguageIsNotDefault_ThrowsArgumentException()
        {
            // arrange
            var parentTemplate = new EmailTemplate
            {
                IsLocalisable = false
            };
            var form = new EmailTemplateMetadataForm
            {
                LanguageId = LanguageEnum.Cs
            };
            var defaultParameters = new List<EmailParameter>();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(parentTemplate);
            emailDbMock.Setup(e => e.GetEmailParametersByIds(It.IsAny<List<long>>())).Returns(defaultParameters);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.SaveEmailTemplateMetadata(form));
        }

        [Fact]
        public void SaveEmailTemplateMetadata_EmailTemplateAlreadyExist_ThrowsInvalidOperationException()
        {
            // arrange
            var parentTemplate = new EmailTemplate
            {
                IsLocalisable = true
            };
            var templates = CreateDefaultTemplates();
            var defaultParameters = new List<EmailParameter>();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(parentTemplate);
            emailDbMock.Setup(e => e.GetEmailParametersByIds(It.IsAny<List<long>>())).Returns(defaultParameters);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(templates);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.SaveEmailTemplateMetadata(new EmailTemplateMetadataForm()));
        }

        [Fact]
        public void SaveEmailTemplateMetadata_NoEmailTemplateInDb_ThrowsArgumentException()
        {
            // arrange
            var form = new EmailTemplateMetadataForm
            {
                ParentEmailTemplateId = 5,
                EmailTemplateId = 2,
                EmailTypeId = EmailTypeEnum.ClassFinalReports,
                LanguageId = LanguageEnum.Cs
            };
            var parentTemplate = new EmailTemplate
            {
                IsLocalisable = true
            };
            emailDbMock.SetupSequence(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>()))
                .Returns(parentTemplate)
                .Returns((EmailTemplate)null);
            emailDbMock.Setup(e => e.GetEmailParametersByIds(It.IsAny<List<long>>())).Returns(new List<EmailParameter>());
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.SaveEmailTemplateMetadata(form));
        }

        [Fact]
        public void SaveEmailTemplateMetadata_NewEmailTemplateMetadataForm_EmailTemplateMetadataIsInsertedIntoDb()
        {
            // arrange
            var form = new EmailTemplateMetadataForm
            {
                ParentEmailTemplateId = 5,
                EmailTemplateId = 0,
                EmailTypeId = EmailTypeEnum.ClassFinalReports,
                LanguageId = LanguageEnum.Cs
            };
            var parentTemplate = new EmailTemplate
            {
                IsLocalisable = true
            };
            var defaultParameters = new List<EmailParameter>();
            var dbTemplate = new EmailTemplate();
            var validator = new EmailParameterValidator("", new List<EmailParameterInfo>());
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(parentTemplate);
            emailDbMock.Setup(e => e.GetEmailParametersByIds(It.IsAny<List<long>>())).Returns(defaultParameters);
            emailDbMock.Setup(e => e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>())).Returns(new List<EmailTemplate>());
            convertorMock.Setup(e => e.ConvertToEmailTemplate(
                    It.IsAny<EmailTemplateMetadataForm>(),
                    It.IsAny<EmailTemplate>(),
                    It.IsAny<List<EmailParameter>>(),
                    It.IsAny<EmailTemplate>()))
                .Returns(dbTemplate);
            paramsValidatorFactoryMock.Setup(e => e.GetValidatorByEmailTemplate(It.IsAny<EmailTemplate>(), It.IsAny<List<EmailParameter>>())).Returns(validator);
            commonLogicMock.Setup(e => e.ValidateEmailTemplate(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<EmailParameterValidator>()))
                .Returns(new EmailTemplateValidationResult());
            var admin = CreateAdmin();

            // act
            admin.SaveEmailTemplateMetadata(form);

            // assert
            commonLogicMock.Verify(e => e.ValidateEmailTemplate(dbTemplate.Subject, dbTemplate.Text, validator), Times.Once);
            emailDbMock.Verify(e => e.InsertEmailTemplate(dbTemplate), Times.Once);
            emailDbMock.Verify(e => e.UpdateEmailTemplate(dbTemplate, EmailOperationOption.None), Times.Never);
        }

        [Fact]
        public void SaveEmailTemplateMetadata_EmailTemplateMetadataForm_EmailTemplateMetadataIsUpdated()
        {
            // arrange
            var form = new EmailTemplateMetadataForm
            {
                ParentEmailTemplateId = 5,
                EmailTemplateId = 1,
                EmailTypeId = EmailTypeEnum.ClassFinalReports,
                LanguageId = LanguageEnum.Cs
            };
            var parentTemplate = new EmailTemplate
            {
                IsLocalisable = true
            };
            var defaultParameters = new List<EmailParameter>();
            var dbTemplate = new EmailTemplate
            {
                EmailTemplateId = 1
            };
            var validator = new EmailParameterValidator("", new List<EmailParameterInfo>());
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(parentTemplate);
            emailDbMock.Setup(e => e.GetEmailParametersByIds(It.IsAny<List<long>>())).Returns(defaultParameters);
            emailDbMock.Setup(e =>
                    e.GetEmailTemplatesByFilter(It.IsAny<EmailTemplateFilter>(), It.IsAny<EmailTemplateIncludes>()))
                .Returns(new List<EmailTemplate>());
            convertorMock.Setup(e => e.ConvertToEmailTemplate(
                    It.IsAny<EmailTemplateMetadataForm>(),
                    It.IsAny<EmailTemplate>(),
                    It.IsAny<List<EmailParameter>>(),
                    It.IsAny<EmailTemplate>()))
                .Returns(dbTemplate);
            paramsValidatorFactoryMock.Setup(e =>
                    e.GetValidatorByEmailTemplate(It.IsAny<EmailTemplate>(), It.IsAny<List<EmailParameter>>()))
                .Returns(validator);
            commonLogicMock.Setup(e => e.ValidateEmailTemplate(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<EmailParameterValidator>()))
                .Returns(new EmailTemplateValidationResult());
            var admin = CreateAdmin();

            // act
            admin.SaveEmailTemplateMetadata(form);

            // assert
            emailDbMock.Verify(e => e.InsertEmailTemplate(dbTemplate), Times.Never);
            emailDbMock.Verify(e => e.UpdateEmailTemplateMetadata(dbTemplate, EmailOperationOption.None), Times.Once);
        }

        #endregion

        #region private methods

        private EmailTemplateAdministration CreateAdmin()
        {
            return new EmailTemplateAdministration(
                localisationServiceMock.Object, 
                emailDbMock.Object,
                convertorMock.Object,
                paramsValidatorFactoryMock.Object,
                commonLogicMock.Object,
                templateResolverMock.Object,
                permissionResolverMock.Object);
        }

        private EmailTemplate CreateTemplate()
        {
            return new EmailTemplate
            {
                EntityId = 5,
                EntityTypeId = EntityTypeEnum.MainProfile,
                EmailTypeId = EmailTypeEnum.ConversationCanceled
            };
        }

        private EmailTemplateEntityId CreateEmailTemplateEntityId()
        {
            return new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 3);
        }

        private List<EmailType> CreateEmailTypes()
        {
            return new List<EmailType>
            {
                new EmailType
                {
                    EmailTypeId = EmailTypeEnum.ConversationChanged,
                    Description = "ConversationChanged"
                },
                new EmailType
                {
                    EmailTypeId = EmailTypeEnum.ConversationCompleted,
                    Description = "ConversationCompleted"
                }
            };
        }

        private List<EmailTemplate> CreateTemplates()
        {
            return new List<EmailTemplate>
            {
                new EmailTemplate
                {
                    EmailTypeId = EmailTypeEnum.ConversationChanged,
                    IsDefault = false,
                    IsLocalisable = true,
                    IsValidated = true
                },
                new EmailTemplate
                {
                    EmailTypeId = EmailTypeEnum.ConversationChanged,
                    IsDefault = false,
                    IsLocalisable = true,
                    IsValidated = false
                }
            };
        }

        private List<EmailTemplate> CreateDefaultTemplates()
        {
            return new List<EmailTemplate>
            {
                new EmailTemplate
                {
                    IsDefault = true,
                    IsLocalisable = true,
                    IsValidated = false,
                    EmailTypeId = EmailTypeEnum.ConversationCompleted
                }
            };
        }

        #endregion
    }
}
