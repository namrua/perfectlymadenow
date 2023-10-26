using AutomationSystem.Main.Core.Certificates.System;
using AutomationSystem.Main.Core.Certificates.System.Convertors;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.Certificates
{
    public static class CertificateServiceCollectionExtensions
    {
        public static IServiceCollection AddCertificateServices(this IServiceCollection services)
        {
            // system
            services.AddSingleton<ICertificateService, CertificateService>();
            services.AddSingleton<ICertificateDocumentCreator, CertificateDocumentCreator>();

            // system convertors
            services.AddSingleton<ICertificateConvertor, CertificateConvertor>();

            return services;
        }
    }
}
