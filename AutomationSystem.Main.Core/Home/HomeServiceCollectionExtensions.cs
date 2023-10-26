using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Core.Home.AppLogic;
using AutomationSystem.Main.Core.Home.AppLogic.Comparers;
using AutomationSystem.Main.Core.Home.AppLogic.Convertors;
using AutomationSystem.Main.Core.Home.System.Incidents;
using AutomationSystem.Shared.Contract.Incidents.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.Home
{
    public static class HomeServiceCollectionExtensions
    {
        public static IServiceCollection AddHomeServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IHomeService, HomeService>();
            services.AddSingleton<IHomeWorkflowManager, HomeWorkflowManager>();
            services.AddSingleton<IPreviewService, PreviewService>();
            services.AddSingleton<IPublicPaymentResolver, PublicPaymentResolver>();
            services.AddSingleton<IWwaRegistrationSplitter, WwaRegistrationSplitter>();

            // app logic - comparers
            services.AddSingleton<IDistanceAndWwaClassComparer, DistanceAndWwaClassComparer>();

            // app logic - convertors
            services.AddSingleton<IHomeConvertor, HomeConvertor>();
            services.AddSingleton<IHomePaymentConvertor, HomePaymentConvertor>();

            // system - incidents
            services.AddSingleton<IIncidentHandler, MissingClassIdInUrlIncidentHandler>();
            
            return services;
        }
    }
}
