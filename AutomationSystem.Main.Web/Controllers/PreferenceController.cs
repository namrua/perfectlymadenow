using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Preferences.AppLogic;
using AutomationSystem.Main.Contract.Preferences.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Controler for preferences administration
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("PreferenceController")]
    public class PreferenceController : Controller
    {
        
        // private components 
        private readonly IPreferenceAdministration preferenceAdministration = IocProvider.Get<IPreferenceAdministration>();

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        // editation of localis
        [AuthorizeEntitle(Entitle.CorePreferences)]
        public ActionResult Localisation()
        {
            var model = preferenceAdministration.GetLanguagePreferencesForEdit();
            return View(model);
        }


        // saves languages information
        [HttpPost]
        [AuthorizeEntitle(Entitle.CorePreferences)]
        public ActionResult Localisation(LanguagePreferencesForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = preferenceAdministration.GetFormLanguagePreferencesForEdit(form);
                return View("Localisation", model);
            }

            preferenceAdministration.SaveLanguagePreferences(form);
            MessageContainer.PushMessage("Localisation preferences was saved.");
            return RedirectToAction("Localisation", "Preference");
        }



        // editation of email
        [AuthorizeEntitle(Entitle.CorePreferences)]
        public ActionResult Email()
        {
            var model = preferenceAdministration.GetEmailPreferencesForEdit();
            return View(model);
        }


        // saves email information
        [HttpPost]
        [AuthorizeEntitle(Entitle.CorePreferences)]
        public ActionResult Email(EmailPreferencesForm model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                return View("Email", model);
            }
            preferenceAdministration.SaveEmailPreferences(model);
            MessageContainer.PushMessage("Email preferences was saved.");
            return RedirectToAction("Email", "Preference");
        }


        // preference of IZI LLC
        [AuthorizeEntitle(Entitle.MainPreferences)]
        public ActionResult Izi()
        {
            var model = preferenceAdministration.GetIziPreferenceForEdit();
            return View(model);
        }

        // saves IZI LLC preferences
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainPreferences)]
        public ActionResult Izi(IziPreferenceForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = preferenceAdministration.GetFormIziPreferenceForEdit(form);
                return View(model);
            }

            preferenceAdministration.SaveIziPreferences(form);
            MessageContainer.PushMessage("IZI LLC preferences was saved.");
            return RedirectToAction("Izi");
        }

    }

}