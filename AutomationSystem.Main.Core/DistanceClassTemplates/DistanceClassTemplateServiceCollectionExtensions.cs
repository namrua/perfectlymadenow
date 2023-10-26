using System;
using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.AsyncRequestExecutors;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventHandlers;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models.Events;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.Models.Events;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using AutomationSystem.Main.Core.DistanceClassTemplates.System.JobRunExecutors;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventCheckers;

namespace AutomationSystem.Main.Core.DistanceClassTemplates
{
    public static class DistanceClassTemplateServiceCollectionExtensions
    {
        public static IServiceCollection AddDistanceClassTemplateServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IDistanceClassTemplateAdministration, DistanceClassTemplateAdministration>();
            services.AddSingleton<IDistanceClassTemplateCompletionAdministration, DistanceClassTemplateCompletionAdministration>();
            services.AddSingleton<IDistanceClassTemplateHelper, DistanceClassTemplateHelper>();
            services.AddSingleton<IDistanceClassTemplateFactory, DistanceClassTemplateFactory>();
            services.AddSingleton<IDistanceClassTemplateOperationChecker, DistanceClassTemplateOperationChecker>();

            // app logic - event checkers
            services.AddTransient<IEventChecker<PersonDeletingEvent>, PersonToDeleteHasNoDistanceClassTemplate>();

            // app logic - event handlers
            services.AddTransient<IEventHandler<DistanceProfileStatusChangedEvent>, PopulateDistanceClassesForProfileEventHandler>();
            services.AddTransient<IEventHandler<DistanceClassTemplateApprovedEvent>, CreateDistanceClassesForApprovedTemplateEventHandler>();
            services.AddTransient<IEventHandler<DistanceClassTemplateChangedEvent>, ChangesPropagatedToDistanceClassesEventHandler>();

            // app logic - async requests
            services.AddSingleton<IAsyncRequestExecutorFactory, CompleteDistanceClassesForTemplateAsyncRequestExecutorFactory>();

            // data
            services.AddSingleton<IDistanceClassTemplateDatabaseLayer, DistanceClassTemplateDatabaseLayer>();
            services.AddSingleton<IDistanceClassTemplateClassDatabaseLayer, DistanceClassTemplateClassDatabaseLayer>();

            // system
            services.AddSingleton<IDistanceClassTemplateService, DistanceClassTemplateService>();

            // system - job executors
            services.AddSingleton<IJobRunExecutorFactory, DistanceCompletionJobExecutorFactory>();

            return services;
        }

        public static List<Profile> CreateProfiles(IServiceProvider provider)
        {
            return new List<Profile>
            {
                new DistanceClassTemplateProfile(provider.GetService<ILanguageTranslationProvider>(), provider.GetService<IDistanceClassTemplateHelper>())
            };
        }
    }
}
