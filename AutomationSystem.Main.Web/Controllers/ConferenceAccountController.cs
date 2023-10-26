using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Integration.AppLogic;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts;
using AutomationSystem.Main.Web.Filters;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    [HandleAdminError("ConferenceAccountController")]
    [AuthorizeEntitle(Entitle.WebExAccounts)]
    public class ConferenceAccountController : Controller
    {

        // private components
        private readonly IMainAccountAdministration accountAdministration = IocProvider.Get<IMainAccountAdministration>();

        // gets message container 
        public IMessageContainer MessageContainer => new MessageContainer(Session);


        // main page
        public ActionResult Index(MainAccountFilter filter, bool search = false)
        {
            var model = accountAdministration.GetMainAccountListPageModel(filter, search);
            return View(model);
        }

        // creates conference account
        public ActionResult New(long profileId)
        {
            try
            {
                var model = accountAdministration.GetNewAccountForm(profileId);
                return View("Edit", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // edit conference account
        public ActionResult Edit(long id)
        {
            try
            {
                var model = accountAdministration.GetAccountFormById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // edit conference account
        [HttpPost]
        public ActionResult Edit(MainAccountForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                return View(form);
            }

            try
            {
                accountAdministration.SaveAccount(form);
                MessageContainer.PushMessage("WebEx account was saved.");
            }
            catch(ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToHome();

        }

        // deletes conference account
        [HttpPost]
        public ActionResult Delete(long id)
        {
            try
            {
                accountAdministration.DeleteAccount(id);
                MessageContainer.PushMessage("WebEx account was deleted.");
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        #region private methods

        // redirects to controler's homepage
        private ActionResult RedirectToHome()
        {
            return RedirectToAction("Index");
        }

        #endregion

    }

}