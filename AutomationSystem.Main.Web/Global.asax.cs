using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutomationSystem.Main.Web.Helpers;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Core.Localisation.System;
using CorabeuControl.Binding;
using CorabeuControl.Helpers;
using CorabeuControl.ModelMetadata;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        // private feidls
        private ITracer tracer;                

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // initialization
            IocProvider.Get<ILanguagePersistor>().Initialize();
        }

        protected void Application_Start()
        {
            ServiceConfig.RegisterServices(IocProvider.Services);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // sets default culture of text helper
            TextHelper.DefaultCulture = new CultureInfo(LocalisationInfo.DefaultLanguageCode);

            // binds new metadata provider
            ModelMetadataProviders.Current = new CorabeuControlModelMetadataProvider();
            CorabeuLocalisationManager.RegisterProvider<LocalisedTextAttribute>(IocProvider.Get<ICorabeuLocalisationProvider>());

            // binding registration
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalNullableModelBinder());
            ModelBinders.Binders.Add(typeof(List<int>), new JsonListBinder<int>());
            ModelBinders.Binders.Add(typeof(List<long>), new JsonListBinder<long>());

            tracer = IocProvider.Get<ITracerFactory>().CreateTracer<MvcApplication>();
        
            // Enable TLS 1.2
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;          
            ServicePointManager.SecurityProtocol &= ~(SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11);
        }


        // all errors page
        protected void Application_Error(object sender, EventArgs eventArgs)
        {
            var exception = Server.GetLastError();

            tracer?.Error(exception);

            if (ConfigHelper.ProcessErrors)
            {
                Server.ClearError();
                Response.Redirect("/Home/Error");
            }
        }

    }

}
