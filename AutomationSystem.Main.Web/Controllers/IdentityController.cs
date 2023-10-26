using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Shared.Contract.Identities.AppLogic;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Models;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Controler for identity administration
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("IdentityController")]
    [AuthorizeAnyEntitle(Entitle.CoreUserAccounts, Entitle.CoreUserAccountsRestricted)]
    public class IdentityController : Controller
    {

        // private components 
        private readonly IIdentityAdministration identityAdmin = IocProvider.Get<IIdentityAdministration>();

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);
       

        // list of users
        public ActionResult Index()
        {
            var model = identityAdmin.GetUsers();
            return View(model);
        }

        // creates new user
        public ActionResult New()
        {
            var model = identityAdmin.GetNewUserForEdit();
            return View("Edit", model);
        }

        // edits person
        public ActionResult Edit(int id)
        {
            try
            {
                var model = identityAdmin.GetUserForEdit(id);
                return View("Edit", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // saves person
        [HttpPost]
        public ActionResult Edit(UserForm form)
        {
            var validationSummary = identityAdmin.ValidateUserForm(form);
            if (!ModelState.IsValid || !validationSummary.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = identityAdmin.GetFormUserForEdit(form, validationSummary);
                return View("Edit", model);
            }

            try
            {
                identityAdmin.SaveUser(form);
                MessageContainer.PushMessage("User was saved.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToAction("Index");
        }

        // deletes person
        [HttpPost]
        public ActionResult Delete(int id)
        {
            identityAdmin.DeleteUser(id);
            MessageContainer.PushMessage("User was deleted.");
            return RedirectToAction("Index");
        }

    }

}