using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Shared.Contract.Jobs.AppLogic;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    [HandleAdminError("JobController")]
    [AuthorizeEntitle(Entitle.CoreJobs)]
    public class JobController : Controller
    {
        private readonly IJobAdministration jobAdministration = IocProvider.Get<IJobAdministration>();

        public IMessageContainer MessageContainer => new MessageContainer(Session);

        public ActionResult Index()
        {
            var model = jobAdministration.GetJobListItems();
            return View(model);
        }

        [HttpPost]
        public ActionResult ScheduleNow(long id)
        {
            try
            {
                jobAdministration.ScheduleJobRun(id, DateTime.Now);
                MessageContainer.PushMessage("Job run was scheduled.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToHome();
        }

        #region private methods

        private ActionResult RedirectToHome()
        {
            return RedirectToAction("Index");
        }

        #endregion
    }
}