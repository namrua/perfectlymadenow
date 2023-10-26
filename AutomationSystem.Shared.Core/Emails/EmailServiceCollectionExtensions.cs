using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.AppLogic;
using AutomationSystem.Shared.Core.Emails.AppLogic.Convertors;
using AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation;
using AutomationSystem.Shared.Core.Emails.Data;
using AutomationSystem.Shared.Core.Emails.System;
using AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors;
using AutomationSystem.Shared.Core.Emails.System.EmailPermissionResolvers;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Emails
{
    public static class EmailServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IEmailIntegration, EmailIntegration>();
            services.AddSingleton<IEmailTemplateAdministration, EmailTemplateAdministration>();
            services.AddSingleton<IEmailTemplateAdministrationCommonLogic, EmailTemplateAdministrationCommonLogic>();
            services.AddSingleton<IEmailTemplateTextAdministration, EmailTemplateTextAdministration>();

            // app logic - convertors
            services.AddSingleton<IEmailConvertor, EmailConvertor>();
            services.AddSingleton<IEmailTemplateConvertor, EmailTemplateConvertor>();

            // app logic -  parameter validation
            services.AddSingleton<IEmailParameterValidatorFactory, EmailParameterValidatorFactory>();

            // app logic - permission resolver
            services.AddSingleton<IEmailPermissionResolver, EmailPermissionResolver>();
            services.AddSingleton<IEmailPermissionResolverForEntity, EmailPermissionResolverForAdmin>();

            // data 
            services.AddSingleton<IEmailDatabaseLayer, EmailDatabaseLayer>();

            // system
            services.AddSingleton<ICoreEmailParameterResolverFactory, CoreEmailParameterResolverFactory>();
            services.AddSingleton<ICoreEmailService, CoreEmailService>();
            services.AddSingleton<IEmailAttachmentProviderFactory, EmailAttachmentProviderFactory>();
            services.AddSingleton<IEmailServiceHelper, EmailServiceHelper>();
            services.AddSingleton<IEmailTextResolverFactory, EmailTextResolverFactory>();
            services.AddSingleton<IEmailTemplateResolver, EmailTemplateResolver>();
            services.AddSingleton<IEmailTemplateHierarchyResolver, EmailTemplateHierarchyResolver>();
            services.AddSingleton<IEmailTemplateHierarchyResolverForEntity, EmailTemplateHierarchyResolverForSystem>();
            services.AddSingleton<IEmailEntityParameterResolverFactory, EmailEntityParameterResolverFactory>();
            services.AddSingleton<ITestEmailParameterResolverFactory, TestEmailParameterResolverFactory>();

            // system - async request executors
            services.AddSingleton<IAsyncRequestExecutorFactory, EmailSenderExecutorFactory>();

            return services;
        }
    }
}