using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.AppLogic;
using AutomationSystem.Shared.Core.Emails.AppLogic.Convertors;
using AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation;
using AutomationSystem.Shared.Model;
using Moq;
using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using Xunit;

namespace AutomationSystem.Shared.Tests.Emails.AppLogic
{
    public class EmailTemplateTextAdministrationTests
    {
        private readonly Mock<IEmailDatabaseLayer> emailDbMock;
        private readonly Mock<ICoreEmailService> coreEmailServiceMock;
        private readonly Mock<IEmailConvertor> emailConvertorMock;
        private readonly Mock<IEmailParameterValidatorFactory> paramsValidatorFactoryMock;
        private readonly Mock<IEmailTemplateConvertor> templateConvertorMock;
        private readonly Mock<IEmailTemplateAdministrationCommonLogic> commonLogicMock;
        private readonly Mock<IEmailTemplateResolver> templateResolverMock;
        private readonly Mock<IEmailPermissionResolver> emailPermissionResolverMock;

        public EmailTemplateTextAdministrationTests()
        {
            emailDbMock = new Mock<IEmailDatabaseLayer>();
            coreEmailServiceMock = new Mock<ICoreEmailService>();
            emailConvertorMock = new Mock<IEmailConvertor>();
            paramsValidatorFactoryMock = new Mock<IEmailParameterValidatorFactory>();
            templateConvertorMock = new Mock<IEmailTemplateConvertor>();
            commonLogicMock = new Mock<IEmailTemplateAdministrationCommonLogic>();
            templateResolverMock = new Mock<IEmailTemplateResolver>();
            emailPermissionResolverMock = new Mock<IEmailPermissionResolver>();
        }

        #region GetEmailTemplateTextForEditById() tests

        [Fact]
        public void GetEmailTemplateTextForEditById_NoEmailTemplateInDb_ThrowsArgumentException()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns((EmailTemplate)null);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetEmailTemplateTextForEditById(1));
        }

        [Fact]
        public void GetEmailTemplateTextForEditById_ExpectedIncludesPassToGetEmailTemplateById()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            InitializeAndValidateTextForEditSetup(new List<EmailParameter>(), new EmailParameterValidator("", new List<EmailParameterInfo>()));
            var admin = CreateAdmin();

            // act
            admin.GetEmailTemplateTextForEditById(1);

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplateById(
                1,
                EmailTemplateIncludes.EmailTemplateParameter 
                | EmailTemplateIncludes.EmailType 
                | EmailTemplateIncludes.Language),
                Times.Once);
        }

        [Fact]
        public void GetEmailTemplateTextForEditById_PermissionIsNotGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var template = CreateTemplate();
            var emailTemplateEntityId = new EmailTemplateEntityId(template.EntityTypeId, template.EntityId);
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            emailPermissionResolverMock
                .Setup(e => e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetEmailTemplateTextForEditById(1));
            emailPermissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(emailTemplateEntityId, template.EmailTypeId), Times.Once);
        }

        [Fact]
        public void GetEmailTemplateTextForEditById_EmailTemplateId_ReturnsEmailTemplateTextForEdit()
        {
            // arrange
            var template = CreateTemplate();
            var parameters = new List<EmailParameter>();
            var ids = new List<long> { 3 };
            var paramsValidator = new EmailParameterValidator("", new List<EmailParameterInfo>());
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            InitializeAndValidateTextForEditSetup(parameters, paramsValidator);
            var admin = CreateAdmin();

            // act
            admin.GetEmailTemplateTextForEditById(1);

            // assert
            InitializeAndValidateTextForEditVerify(ids, template, parameters);
        }

        #endregion

        #region GetResetEmailTemplateTextForEdit() tests

       [Fact]
        public void GetResetEmailTemplateTextForEdit_ExpectedIncludesPassToGetEmailTemplateById()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            templateResolverMock.Setup(e => e.GetParentTemplate(It.IsAny<EmailTemplate>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            InitializeAndValidateTextForEditSetup(new List<EmailParameter>(), new EmailParameterValidator("", new List<EmailParameterInfo>()));
            var admin = CreateAdmin();

            // act
            admin.GetResetEmailTemplateTextForEdit(1);

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplateById(
                1,
                EmailTemplateIncludes.EmailTemplateParameter
                | EmailTemplateIncludes.EmailType
                | EmailTemplateIncludes.Language),
                Times.Once);
        }

        [Fact]
        public void GetResetEmailTemplateTextForEdit_PermissionIsNotGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var template = CreateTemplate();
            var emailTemplateEntityId = new EmailTemplateEntityId(template.EntityTypeId, template.EntityId);
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            emailPermissionResolverMock
                .Setup(e => e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetResetEmailTemplateTextForEdit(1));
            templateResolverMock.Verify(e => e.GetParentTemplate(It.IsAny<EmailTemplate>(), EmailTemplateIncludes.None), Times.Never);
            emailPermissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(emailTemplateEntityId, template.EmailTypeId), Times.Once);
        }
        
        [Fact]
        public void GetResetEmailTemplateTextForEdit_ForTemplateInDb_ReturnsEmailTemplateTextForEdit()
        {
            // arrange
            var template = CreateTemplate();
            var parameters = new List<EmailParameter>();
            var ids = new List<long> {3};
            var paramsValidator = new EmailParameterValidator("", new List<EmailParameterInfo>());
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            templateResolverMock.Setup(e => e.GetParentTemplate(It.IsAny<EmailTemplate>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            InitializeAndValidateTextForEditSetup(parameters, paramsValidator);
            var admin = CreateAdmin();

            // act
            admin.GetResetEmailTemplateTextForEdit(1);

            // assert
            InitializeAndValidateTextForEditVerify(ids, template, parameters);
            templateResolverMock.Verify(e => e.GetParentTemplate(template, EmailTemplateIncludes.None), Times.Once);
        }
        #endregion

        #region GetEmailTemplateTextForEditByForm() tests

        [Fact]
        public void GetEmailTemplateTextForEditByForm_ExpectedIncludesPassToGetEmailTemplateById()
        {
            // arrange
            var form = CreateForm();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            templateConvertorMock.Setup(e => e.InitializeEmailTemplateTextForEdit(It.IsAny<EmailTemplate>(), It.IsAny<List<EmailParameter>>())).Returns(new EmailTemplateTextForEdit());
            var admin = CreateAdmin();

            // act
            admin.GetEmailTemplateTextForEditByForm(form, new EmailTemplateValidationResult());

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplateById(
                    1,
                    EmailTemplateIncludes.EmailTemplateParameter
                    | EmailTemplateIncludes.EmailType
                    | EmailTemplateIncludes.Language),
                Times.Once);
        }

        [Fact]
        public void GetEmailTemplateTextForEditByForm_PermissionIsNotGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var template = CreateTemplate();
            var emailTemplateEntityId = new EmailTemplateEntityId(template.EntityTypeId, template.EntityId);
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            emailPermissionResolverMock
                .Setup(e => e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetEmailTemplateTextForEditByForm(new EmailTemplateTextForm(), new EmailTemplateValidationResult()));
            emailPermissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(emailTemplateEntityId, template.EmailTypeId), Times.Once);
        }

        [Fact]
        public void GetEmailTemplateTextForEditByForm_EmailTemplateTextFormAndEmailTemplateValidationResult_ReturnsEmailTemplateTextForEdit()
        {
            // arrange
            var form = CreateForm();
            var template = CreateTemplate();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            templateConvertorMock.Setup(e => e.InitializeEmailTemplateTextForEdit(It.IsAny<EmailTemplate>(), It.IsAny<List<EmailParameter>>())).Returns(new EmailTemplateTextForEdit());
            var admin = CreateAdmin();

            // act
            var result = admin.GetEmailTemplateTextForEditByForm(form, new EmailTemplateValidationResult());

            // assert
            Assert.Same(form, result.Form);
            templateConvertorMock.Verify(e => e.InitializeEmailTemplateTextForEdit(template, null), Times.Once);
        }

        #endregion

        #region SaveEmailTemplateText() tests

        [Fact]
        public void SaveEmailTemplateText_ExpectedIncludesPassToGetEmailTemplateById()
        {
            // arrange
            var form = CreateForm();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            templateConvertorMock.Setup(e => e.ConvertToEmailTemplate(It.IsAny<EmailTemplateTextForm>())).Returns(new EmailTemplate());
            var admin = CreateAdmin();

            // act
            admin.SaveEmailTemplateText(form, true);

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplateById(1,EmailTemplateIncludes.None), Times.Once);
        }

        [Fact]
        public void SaveEmailTemplateText__PermissionIsNotGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var template = CreateTemplate();
            var emailTemplateEntityId = new EmailTemplateEntityId(template.EntityTypeId, template.EntityId);
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            emailPermissionResolverMock
                .Setup(e => e.CheckEmailTemplateIsGranted(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SaveEmailTemplateText(new EmailTemplateTextForm(), true));
            emailDbMock.Verify(e => e.UpdateEmailTemplateText(It.IsAny<EmailTemplate>(), EmailOperationOption.None), Times.Never);
            emailPermissionResolverMock.Verify(e => e.CheckEmailTemplateIsGranted(emailTemplateEntityId, template.EmailTypeId), Times.Once);
        }

        [Fact]
        public void SaveEmailTemplateText_EmailTemplateTextFormAndIsValid_EmailTemplateIsUpdated()
        {
            // arrange
            var template = CreateTemplate();
            var form = CreateForm();
            var dbTemplate = new EmailTemplate();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            templateConvertorMock.Setup(e => e.ConvertToEmailTemplate(It.IsAny<EmailTemplateTextForm>())).Returns(dbTemplate);
            var admin = CreateAdmin();

            // act
            admin.SaveEmailTemplateText(form, true);

            // assert
            templateConvertorMock.Verify(e => e.ConvertToEmailTemplate(form), Times.Once);
            emailDbMock.Verify(e => e.UpdateEmailTemplateText(dbTemplate, EmailOperationOption.None), Times.Once);
        }

        #endregion

        #region GetEmailDetail() tests

        [Fact]
        public void GetEmailDetail_NoEmailInDb_ThrowsArgumentException()
        {
            // arrange
            emailDbMock.Setup(e => e.GetEmailForDetailById(It.IsAny<long>(), It.IsAny<EmailIncludes>())).Returns((Email)null);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetEmailDetail(1));
        }

        [Fact]
        public void GetEmailDetail_NoPermission_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var email = CreateEmail();
            var emailEntityId = new EmailEntityId(email.EntityTypeId.Value, email.EntityId.Value);
            emailDbMock.Setup(e => e.GetEmailForDetailById(It.IsAny<long>(), It.IsAny<EmailIncludes>())).Returns(email);
            emailPermissionResolverMock.Setup(e => e.CheckEmailIsGranted(It.IsAny<EmailEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetEmailDetail(email.EmailId));
            emailPermissionResolverMock.Verify(e => e.CheckEmailIsGranted(emailEntityId, email.EmailTemplate.EmailTypeId), Times.Once);
        }

        [Fact]
        public void GetEmailDetail_ExpectedIncludesPassToGetEmailForDetailById()
        {
            // arrange
            var email = CreateEmail();
            emailDbMock.Setup(e => e.GetEmailForDetailById(It.IsAny<long>(), It.IsAny<EmailIncludes>())).Returns(email);
            emailConvertorMock.Setup(e => e.ConvertToEmailDetail(It.IsAny<Email>())).Returns(new EmailDetail());
            var admin = CreateAdmin();

            // act
            admin.GetEmailDetail(1);

            // assert
            emailDbMock.Verify(e => e.GetEmailForDetailById(1, EmailIncludes.EmailTemplate | EmailIncludes.EntityType), Times.Once);
        }

        [Fact]
        public void GetEmailDetail_EmailId_ReturnsEmailDetail()
        {
            // arrange
            var email = CreateEmail();
            var emailDetail = new EmailDetail();
            emailDbMock.Setup(e => e.GetEmailForDetailById(It.IsAny<long>(), It.IsAny<EmailIncludes>())).Returns(email);
            emailConvertorMock.Setup(e => e.ConvertToEmailDetail(It.IsAny<Email>())).Returns(emailDetail);
            var admin = CreateAdmin();

            // act
            var result = admin.GetEmailDetail(email.EmailId);

            // assert
            Assert.Same(emailDetail, result);
            emailConvertorMock.Verify(e => e.ConvertToEmailDetail(email), Times.Once);
        }

        #endregion

        #region SendTestEmail() tests

        [Fact]
        public void SendTestEmail_ExpectedIncludesPassToGetEmailTemplateById()
        {
            // arrange
            var info = new EmailTestSendInfo("userEmail", 13);
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(new EmailTemplate());
            coreEmailServiceMock.Setup(e => e.SendTestEmail(It.IsAny<EmailTestSendInfo>(), It.IsAny<bool>())).Returns(13);
            var admin = CreateAdmin();

            // act
            admin.SendTestEmail(info);

            // assert
            emailDbMock.Verify(e => e.GetEmailTemplateById(13, EmailTemplateIncludes.None), Times.Once);
        }

        [Fact]
        public void SendTestEmail_NoPermission_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var info = new EmailTestSendInfo("userEmail", 13)
            {
                EmailEntityId = new EmailEntityId(EntityTypeEnum.MainProfile, 4)
            };
            var template = CreateTemplate();
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            emailPermissionResolverMock
                .Setup(e => e.CheckEmailIsGranted(It.IsAny<EmailEntityId>(), It.IsAny<EmailTypeEnum>()))
                .Throws(new EntitleAccessDeniedException("", Entitle.CoreEmailTemplates));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SendTestEmail(info));
            emailPermissionResolverMock.Verify(e => e.CheckEmailIsGranted(new EmailEntityId(info.EmailEntityId.TypeId, info.EmailEntityId.Id),
                template.EmailTypeId), Times.Once);
        }

        [Fact]
        public void SendTestEmail_EmailTestSendInfoAndAllowInvalidTemplate_ReturnsId()
        {
            // arrange
            var template = CreateTemplate();
            var info = new EmailTestSendInfo("userEmail", 13);
            emailDbMock.Setup(e => e.GetEmailTemplateById(It.IsAny<long>(), It.IsAny<EmailTemplateIncludes>())).Returns(template);
            coreEmailServiceMock.Setup(e => e.SendTestEmail(It.IsAny<EmailTestSendInfo>(), It.IsAny<bool>())).Returns(13);
            var admin = CreateAdmin();

            // act
            admin.SendTestEmail(info);

            // assert
            coreEmailServiceMock.Verify(e => e.SendTestEmail(info, false), Times.Once);
        }

        #endregion

        #region private methods

        private EmailTemplateTextAdministration CreateAdmin()
        {
            return new EmailTemplateTextAdministration(
                emailDbMock.Object,
                coreEmailServiceMock.Object,
                templateConvertorMock.Object,
                emailConvertorMock.Object,
                commonLogicMock.Object,
                paramsValidatorFactoryMock.Object,
                templateResolverMock.Object,
                emailPermissionResolverMock.Object);
        }

        private EmailTemplate CreateTemplate()
        {
            return new EmailTemplate
            {
                EmailTemplateParameters = new List<EmailTemplateParameter>
                {
                    new EmailTemplateParameter
                    {
                        EmailParameterId = 3
                    }
                },
                EmailTypeId = EmailTypeEnum.ConversationCanceled,
                EntityId = 12,
                EntityTypeId = EntityTypeEnum.MainProfile
            };
        }

        private EmailTemplateTextForm CreateForm()
        {
            return new EmailTemplateTextForm
            {
                EmailTemplateId = 1
            };
        }

        private void InitializeAndValidateTextForEditSetup(List<EmailParameter> parameters, EmailParameterValidator paramsValidator)
        {
            emailDbMock.Setup(e => e.GetEmailParametersByIds(It.IsAny<IEnumerable<long>>())).Returns(parameters);
            paramsValidatorFactoryMock.Setup(e => e.GetValidatorByEmailTemplate(It.IsAny<EmailTemplate>(), It.IsAny<List<EmailParameter>>())).Returns(paramsValidator);
            templateConvertorMock.Setup(e => e.InitializeEmailTemplateTextForEdit(
                    It.IsAny<EmailTemplate>(),
                    It.IsAny<List<EmailParameter>>()))
                .Returns(new EmailTemplateTextForEdit());
            templateConvertorMock.Setup(e => e.ConvertToEmailTemplateEditForm(It.IsAny<EmailTemplate>())).Returns(new EmailTemplateTextForm());
            commonLogicMock.Setup(e => e.ValidateEmailTemplate(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<EmailParameterValidator>()))
                .Returns(new EmailTemplateValidationResult());
        }

        private void InitializeAndValidateTextForEditVerify(List<long> ids, EmailTemplate template, List<EmailParameter> parameters)
        {
            emailDbMock.Verify(e => e.GetEmailParametersByIds(ids), Times.Once);
            templateConvertorMock.Verify(e => e.InitializeEmailTemplateTextForEdit(template, parameters), Times.Once);
            templateConvertorMock.Verify(e => e.ConvertToEmailTemplateEditForm(template), Times.Once);
        }

        private Email CreateEmail()
        {
            var template = CreateTemplate();
            return new Email
            {
                EmailId = 15,
                EntityId = 3,
                EntityTypeId = EntityTypeEnum.MainProfile,
                EmailTemplate = template
            };
        }

        #endregion
    }
}
