using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Model;
using System.Collections.Generic;

namespace AutomationSystem.Shared.Core.Emails.System
{
    /// <summary>
    /// Core email service
    /// </summary>
    public class CoreEmailService : ICoreEmailService
    {

        // private components
        private readonly IEmailServiceHelper emailHelper;
        private readonly IEmailTextResolverFactory emailTextResolverFactory;
        private readonly ICoreEmailParameterResolverFactory coreEmailParameterResolverFactory;
        private readonly ITestEmailParameterResolverFactory testEmailParameterResolverFactory;
        private readonly IEmailTemplateResolver emailTemplateResolver;


        // constructor
        public CoreEmailService(
            IEmailServiceHelper emailHelper,
            IEmailTextResolverFactory emailTextResolverFactory,
            ICoreEmailParameterResolverFactory coreEmailParameterResolverFactory,
            ITestEmailParameterResolverFactory testEmailParameterResolverFactory,
            IEmailTemplateResolver emailTemplateResolver)
        {
            this.emailHelper = emailHelper;
            this.emailTextResolverFactory = emailTextResolverFactory;
            this.coreEmailParameterResolverFactory = coreEmailParameterResolverFactory;
            this.testEmailParameterResolverFactory = testEmailParameterResolverFactory;
            this.emailTemplateResolver = emailTemplateResolver;
        }


        // sends generic test email, returns email id
        public long SendTestEmail(EmailTestSendInfo info, bool allowInvalidTemplate = false)
        {
            // loads template data
            var emailTemplate = emailTemplateResolver.GetEmailTemplateById(info.EmailTemplateId, allowInvalidTemplate);

            // assembles resolvers
            var testParamResolver = testEmailParameterResolverFactory.CreateTestParameterResolverByEntity(info.ParameterEntityId);
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(testParamResolver);           

            // resolves inputs
            var subject = textResolver.GetText(emailTemplate.LanguageId, info.CurrentSubject ?? emailTemplate.Subject);
            var text = textResolver.GetText(emailTemplate.LanguageId, info.CurrentText ?? emailTemplate.Text);
            var recipient = info.UserEmail;
            
            // sends email
            var result = emailHelper.SendEmail(info.EmailTemplateId, info.EmailEntityId, true, subject, text, recipient, null, (int)SeverityEnum.High);
            return result;
        }


        // sends incident email, returns email id
        public long SendIncidentEmail(EmailTypeEnum emailTypeId, int severity, Incident incident)
        {
            // loads data
            var emailTemplate = emailTemplateResolver.GetEmailTemplateByTypeAndLanguage(emailTypeId, canUseDefault: true);           

            // assembles resolvers                   
            var incidentResolver = coreEmailParameterResolverFactory.CreateIncidentParameterResolver();
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(incidentResolver);
            incidentResolver.Bind(incident);

            // resolves inputs          
            var recipient = emailHelper.GetHelpdeskEmail();

            // sends email
            var result = emailHelper.SendEmailForTemplate(emailTemplate, textResolver,
                new EmailEntityId(EntityTypeEnum.CoreIncident, incident.IncidentId),
                recipient, null, severity);
            return result;
        }


        // sends job report email, returns email id
        public long SendJobReportEmail(EmailTypeEnum emailTypeId, int severity, long jobRunId, Dictionary<string, object> textMap)
        {
            // loads data
            var emailTemplate = emailTemplateResolver.GetEmailTemplateByTypeAndLanguage(emailTypeId);

            // assembles resolver
            var textMapResolver = coreEmailParameterResolverFactory.CreateTextMapParameterResolver();
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(textMapResolver);
            textMapResolver.Bind(textMap);

            // resolves inputs          
            var recipient = emailHelper.GetAdminRecipient();

            // sends email
            var result = emailHelper.SendEmailForTemplate(emailTemplate, textResolver,
                new EmailEntityId(EntityTypeEnum.CoreJobRun, jobRunId),
                recipient, null, severity);
            return result;
        }    

    }

}
