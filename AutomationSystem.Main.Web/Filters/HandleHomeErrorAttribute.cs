using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Web.Filters
{
    /// <summary>
    /// Handles errors of Home controller
    /// </summary>
    public class HandleHomeErrorAttribute : HandleErrorAttribute
    {

        // private constants
        private readonly IIncidentLogger incidentLogger;
        private readonly ITracer tracer;
        private readonly HashSet<HomeServiceErrorType> supportedErrorTypes;
        private readonly string controllerName;        

        // constructor
        public HandleHomeErrorAttribute(string controllerName = "Home")
        {
            this.controllerName = controllerName;
            incidentLogger = IocProvider.Get<IIncidentLogger>();
            tracer = IocProvider.Get<ITracerFactory>().CreateTracer<HandleHomeErrorAttribute>($"{controllerName}Controller");


            supportedErrorTypes = new HashSet<HomeServiceErrorType>(new [] {
                HomeServiceErrorType.ClassRegistrationClosed,
                HomeServiceErrorType.ClassRegistrationNotStarted,
                HomeServiceErrorType.RegistrationComplete,
                HomeServiceErrorType.InvalidRegistrationStep,
                HomeServiceErrorType.PreRegistrationClosed,
                HomeServiceErrorType.RegistrationTypeNotAllowed,
                HomeServiceErrorType.InvitationExpired,
                HomeServiceErrorType.InvalidPage,
                HomeServiceErrorType.MaterialsNotAvailable,
            });
        }

        // error event handler
        public override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            HomeServiceErrorType resultErrorType;
            long? classId = null;

            // know exception is processed
            if (exception is HomeServiceException homeException && supportedErrorTypes.Contains(homeException.Type))
            {
                tracer.Error(exception);
                resultErrorType = homeException.Type;
                classId = homeException.ClassId;
            }
            else
            {
                // unknown or unsupported exception is processed
                tracer.Error(exception, "Home pages causes unexpected exception.");
                try
                {
                    var request = filterContext.HttpContext?.Request;
                    var incident = IncidentForLog
                        .New(IncidentTypeEnum.WebRenderingError, exception)
                        .AddRequestInfo(request?.UserHostAddress, request?.Url?.ToString());
                    incidentLogger.LogIncident(incident);
                }
                catch (Exception e)
                {
                    tracer.Error(e, "Creating of incident causes error");
                }    
                resultErrorType = HomeServiceErrorType.GenericError;
            }

            // redirects to
            filterContext.ExceptionHandled = true;
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            var url = urlHelper.Action("Error", controllerName, new { id = resultErrorType, classId });
            filterContext.Result = new RedirectResult(url);           
        }

    }    

}