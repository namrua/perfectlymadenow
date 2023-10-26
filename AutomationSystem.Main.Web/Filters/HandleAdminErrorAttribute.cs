using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Web.Helpers;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Web.Filters
{
    /// <summary>
    /// Handles unexpected errors of administration controllers
    /// </summary>
    public class HandleAdminErrorAttribute : HandleErrorAttribute
    {

        private readonly ITracer tracer;
        private readonly IIncidentLogger incidentLogger;
        private readonly IncidentTypeEnum incidentType;

        // constructor
        public HandleAdminErrorAttribute(string controllerName, IncidentTypeEnum incidentType = IncidentTypeEnum.WebRenderingError)
        {
            tracer = IocProvider.Get<ITracerFactory>().CreateTracer<HandleAdminErrorAttribute>(controllerName);
            incidentLogger = IocProvider.Get<IIncidentLogger>();

            this.incidentType = incidentType;
        }

        // error event handler
        public override void OnException(ExceptionContext filterContext)
        {
            // gets exception
            var exception = filterContext.Exception;
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            tracer.Error(exception, "Administration pages causes exception.");

            // ignores handled exceptions
            if (filterContext.ExceptionHandled)
                return;

            // checks for access denied exception
            if (exception is EntitleAccessDeniedException)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new RedirectResult(urlHelper.Action("AccessDenied", "Administration"));
                return;
            }

            // ignores logging incedints on development environment
            if (!ConfigHelper.ProcessErrors)
                return;

            // creates incident
            long? incidentId = null;           
            try
            {
                var request = filterContext.HttpContext?.Request;
                var incident = IncidentForLog
                    .New(incidentType, exception)
                    .AddRequestInfo(request?.UserHostAddress, request?.Url?.ToString());
                incidentLogger.LogIncident(incident);
            }
            catch (Exception e)
            {
                tracer.Error(e, "Creating of incident causes error.");
            }

            // redirect to
            filterContext.ExceptionHandled = true;
            var url = urlHelper.Action("Error", "Administration", new { id = incidentId });
            filterContext.Result = new RedirectResult(url);
        }

    }

}