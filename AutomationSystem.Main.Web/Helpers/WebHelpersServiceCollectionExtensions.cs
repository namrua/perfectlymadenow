using AutomationSystem.Main.Web.Helpers.BatchUploads;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Web.Helpers
{
    public static class WebHelpersServiceCollectionExtensions
    {
        public static IServiceCollection AddWebHelpers(this IServiceCollection services)
        {
            services.AddSingleton<ILayoutHelper, LayoutHelper>();
            services.AddSingleton<IBatchUploadHelper, BatchUploadHelper>();
            return services;
        }
    }

}