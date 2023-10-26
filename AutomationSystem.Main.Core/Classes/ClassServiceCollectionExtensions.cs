using AutoMapper;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.AsyncRequestExecutors;
using AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.JobRunExecutors;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Classes
{
    public static class ClassServiceCollectionExtensions
    {
        public static IServiceCollection AddClassServices(this IServiceCollection services)
        {
            // app logic - administration
            services.AddSingleton<IClassActionAdministration, ClassActionAdministration>();
            services.AddSingleton<IClassAdministration, ClassAdministration>();
            services.AddSingleton<IClassCertificateAdministration, ClassCertificateAdministration>();
            services.AddSingleton<IClassFinanceAdministration, ClassFinanceAdministration>();
            services.AddSingleton<IClassOperationChecker, ClassOperationChecker>();
            services.AddSingleton<IClassReportAdministration, ClassReportAdministration>();
            services.AddSingleton<IClassStyleAdministration, ClassStyleAdministration>();
            services.AddSingleton<IClassPreferenceAdministration, ClassPreferenceAdministration>();

            // app logic - async request executors
            services.AddSingleton<IAsyncRequestExecutorFactory, DocumentAsyncExecutorFactory>();

            // app logic - event checkers
            services.AddTransient<IEventChecker<PayPalAccountDeletingEvent>, PayPalAccountToDeleteHasNoClassEventChecker>();
            services.AddTransient<IEventChecker<PersonDeletingEvent>, PersonToDeleteHasNoClassEventChecker>();
            services.AddTransient<IEventChecker<PriceListDeletingEvent>, PriceListToDeleteHasNoClassEventChecker>();
            services.AddTransient<IEventChecker<ProfileDeletingEvent>, ProfileToDeleteHasNoClassEventChecker>();

            // app logic - factories
            services.AddSingleton<IClassEventFactory, ClassEventFactory>();
            services.AddSingleton<IClassExpenseFactory, ClassExpenseFactory>();
            services.AddSingleton<IClassReportSettingFactory, ClassReportSettingFactory>();
            services.AddSingleton<IClassPreferenceFactory, ClassPreferenceFactory>();

            // app logic - providers
            services.AddSingleton<ILanguageTranslationProvider, LanguageTranslationProvider>();

            // data
            services.AddSingleton<IClassDatabaseLayer, ClassDatabaseLayer>();
            services.AddSingleton<IClassPreferenceDatabaseLayer, ClassPreferenceDatabaseLayer>();

            // system
            services.AddSingleton<IClassActionService, ClassActionService>();
            services.AddSingleton<IClassService, ClassService>();
            services.AddSingleton<IClassEmailParameterResolverFactory, ClassEmailParameterResolverFactory>();
            services.AddSingleton<IEmailEntityParameterResolverFactoryForEntity, EmailEntityParameterResolverFactoryForClass>();
            services.AddSingleton<IEmailTemplateHierarchyResolverForEntity, EmailTemplateHierarchyResolverForClassAction>();
            services.AddSingleton<IClassEmailService, ClassEmailService>();
            services.AddSingleton<IClassTypeResolver, ClassTypeResolver>();

            // system - convertors
            services.AddTransient<IClassActionConvertor, ClassActionConvertor>();
            services.AddTransient<IClassConvertor, ClassConvertor>();
            services.AddTransient<IClassBusinessConvertor, ClassBusinessConvertor>();
            services.AddTransient<IClassStyleConvertor, ClassStyleConvertor>();

            // system - email permission resolvers
            services.AddSingleton<IEmailPermissionResolverForEntity, EmailPermissionResolverForClass>();

            // system - job run executors
            services.AddSingleton<IJobRunExecutorFactory, ActiveClassReportJobExecutorFactory>();

            return services;
        }

        public static List<Profile> CreateProfiles(IServiceProvider serviceProvider)
        {
            return new List<Profile>
            {
                new ClassExpenseProfile(serviceProvider.GetService<IClassDatabaseLayer>()),
                new ClassReportSettingProfile(),
                new ClassPreferenceProfile(serviceProvider.GetService<IClassDatabaseLayer>())
            };
        }
    }
}
