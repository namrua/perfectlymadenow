using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic;
using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;
using CorabeuControl.Helpers;
using System;
using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    [HandleAdminError("DistanceProfileController")]
    [AuthorizeEntitle(Entitle.MainDistanceProfile)]
    public class DistanceProfileController : Controller
    {

        // private components
        private readonly IDistanceProfileAdministration distanceProfileAdministration;
        
        // gets message container 
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        // constructor
        public DistanceProfileController()
        {
            distanceProfileAdministration = IocProvider.Get<IDistanceProfileAdministration>();
        }

        // gets main page
        public ActionResult Index()
        {
            var model = distanceProfileAdministration.GetDistanceProfilePageModel();
            return View(model);
        }

        // shows distance profile detail
        public ActionResult Detail(long id)
        {
            try
            {
                var model = distanceProfileAdministration.GetDistanceProfileDetailById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // gets new distance profile form
        public ActionResult New(long profileId)
        {
            try
            {
                var model = distanceProfileAdministration.GetNewDistanceProfileForEdit(profileId);
                return View("edit", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // opens distance profile form
        public ActionResult Edit (long id)
        {
            try
            {
                var model = distanceProfileAdministration.GetDistanceProfileForEditById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // activate distance profile
        [HttpPost]
        public ActionResult Activate(long id)
        {
            try
            {
                distanceProfileAdministration.ActivateDistanceProfile(id);
                MessageContainer.PushMessage("Distance profile was activated. Missing distance classes was created.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id });
            }

            return RedirectToAction("Detail", new { id });
        }

        // deactivate distance profile
        [HttpPost]
        public ActionResult Deactivate(long id)
        {
            try
            {
                distanceProfileAdministration.DeactivateDistanceProfile(id);
                MessageContainer.PushMessage("Distance profile was deactivated.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id });
            }

            return RedirectToAction("Detail", new { id });
        }

        // saves distance profile
        [HttpPost]
        public ActionResult Edit(DistanceProfileForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = distanceProfileAdministration.GetDistanceProfileForEditByForm(form);
                return View(model);
            }
            try
            {
                var id = distanceProfileAdministration.SaveDistanceProfile(form);
                MessageContainer.PushMessage("Distance profile was saved.");
                return RedirectToAction("Detail", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // delete distance profile
        [HttpPost]
        public ActionResult Delete (long id)
        {
            distanceProfileAdministration.DeleteDistanceProfile(id);
            MessageContainer.PushMessage("Distance profile was deleted.");
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