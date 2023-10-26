using System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Web
{
    public static class IocProvider
    {
        private static readonly object lockObject = new object();
        private static readonly IServiceCollection serviceCollection;

        private static IServiceProvider serviceProvider;

        public static IServiceCollection Services => GetServices();

        static IocProvider()
        {
            serviceCollection = new ServiceCollection();
        }

        public static T Get<T>()
        {
            return GetProvider().GetService<T>();
        }

        #region private methods

        private static IServiceCollection GetServices()
        {
            lock (lockObject)
            {
                if (serviceProvider != null)
                {
                    throw new InvalidOperationException("Cannot access IServiceCollection because ServiceProvider is already created.");
                }

                return serviceCollection;
            }
        }

        private static IServiceProvider GetProvider()
        {
            lock (lockObject)
            {
                return serviceProvider ?? (serviceProvider = serviceCollection.BuildServiceProvider());
            }
        }

        #endregion
    }

}