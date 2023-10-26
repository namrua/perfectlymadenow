using AutomationSystem.Main.Core;
using AutomationSystem.Main.Web.Helpers;
using AutomationSystem.Shared.Core;
using AutomationSystem.Shared.Core.Localisation.AppLogic;
using CorabeuControl.ModelMetadata;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools;
using PerfectlyMadeInc.WebEx;

namespace AutomationSystem.Main.Web
{
    public class ServiceConfig
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDesignToolsServices();
            services.AddSharedServices();
            services.AddWebExServices();
            services.AddMainServices();
            services.AddWebHelpers();
            services.AddSingleton<ICorabeuLocalisationProvider, AppLocalisationProvider>();
        }
    }

}