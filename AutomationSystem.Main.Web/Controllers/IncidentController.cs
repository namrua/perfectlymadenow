using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Shared.Contract.Incidents.AppLogic;
using AutomationSystem.Shared.Contract.Incidents.AppLogic.Models;
using CorabeuControl.Context;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Provides incident administration
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("IncidentController")]
    [AuthorizeEntitle(Entitle.CoreIncidents)]
    public class IncidentController : Controller
    {

        // private components
        private readonly IIncidentAdministration incidentAdministration = IocProvider.Get<IIncidentAdministration>();        

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);


        // shows list of incidents
        [CorabeuContext]
        public ActionResult Index(IncidentFilter filter, bool search = false)
        {
            IContextManager cm = ContextHelper.GetContextManager(ViewBag);
            var model = incidentAdministration.GetIncidentsByFilter(filter, search);
            return View(model);
        }

        // show incident detail
        [CorabeuContext]
        public ActionResult Detail(long id)
        {
            try
            {
                var model = incidentAdministration.GetIncidentById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // resolves incident
        [HttpPost]
        [CorabeuContext]
        public ActionResult Resolve(long id)
        {
            try
            {
                incidentAdministration.ResolveIncident(id);
                MessageContainer.PushMessage("Incident was resolved.");

                IContextManager cm = ContextHelper.GetContextManager(ViewBag);
                return Redirect(cm.GetBackUrl(Url.Action("Detail", new { id })));
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // reports incident
        [HttpPost]
        [CorabeuContext]
        public ActionResult Report(long id)
        {
            try
            {
                incidentAdministration.ReportIncident(id);
                MessageContainer.PushMessage("Incident was reported.");

                IContextManager cm = ContextHelper.GetContextManager(ViewBag);
                return Redirect(cm.GetBackUrl(Url.Action("Detail", new { id })));
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

    }

}