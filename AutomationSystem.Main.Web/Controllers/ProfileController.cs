using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences;
using AutomationSystem.Main.Contract.Profiles.AppLogic;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Main.Web.Helpers.Validations;
using AutomationSystem.Main.Web.Helpers.Validations.Models;
using CorabeuControl.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    [HandleAdminError("ProfileController")]
    [AuthorizeEntitle(Entitle.MainProfiles)]
    public class ProfileController : Controller
    {

        // private components
        private readonly IProfileAdministration profileAdministration = IocProvider.Get<IProfileAdministration>();

        private readonly IProfileEmailAdministration profileEmailAdministration = IocProvider.Get<IProfileEmailAdministration>();

        private readonly IProfileUsersAdministration profileUsersAdministration = IocProvider.Get<IProfileUsersAdministration>();

        private readonly IClassPreferenceAdministration classPreferenceAdministration = IocProvider.Get<IClassPreferenceAdministration>();

        private readonly IEmailTemplateAdministration emailTemplateAdministration = IocProvider.Get<IEmailTemplateAdministration>();

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        #region profiles

        // views profile list
        public ActionResult Index()
        {
            var model = profileAdministration.GetProfiles();
            return View(model);
        }


        // views profile detail
        public ActionResult Detail(long id)
        {
            try
            {
                var model = profileAdministration.GetProfileDetail(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // opens form for new profile
        public ActionResult New()
        {
            var model = profileAdministration.GetNewProfileForm();
            return View("Edit", model);
        }

        // opens existing profile for edit
        public ActionResult Edit(long id)
        {
            try
            {
                var model = profileAdministration.GetProfileFormById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // saves profile
        [HttpPost]
        public ActionResult Edit(ProfileForm form)
        {
            var profileValidation = profileAdministration.ValidateProfileForm(form);
            if (!ModelState.IsValid || !profileValidation.IsValid)
            {
                ViewBag.TriggerValidation = true;
                form = profileAdministration.GetProfileFormByFormAndValidation(form, profileValidation);
                return View(form);
            }

            try
            {
                var profileId = profileAdministration.SaveProfile(form, out var updateIdentityClaims);
                MessageContainer.PushMessage("Profile was saved.");
                if (updateIdentityClaims)
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Detail", new {id = profileId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // deletes profile
        [HttpPost]
        public ActionResult Delete(long id)
        {
            try
            {
                profileAdministration.DeleteProfile(id);
                MessageContainer.PushMessage("Profile was deleted.");
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id });
            }
        }

        #endregion


        #region class preference

        // shows class preferences detail tab
        public ActionResult ClassPreference(long id)
        {
            try
            {
                var model = classPreferenceAdministration.GetClassPreferenceDetail(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // opens class preference form
        public ActionResult ClassPreferenceEdit(long id)
        {
            try
            {
                var model = classPreferenceAdministration.GetClassPreferenceForEditByProfileId(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // saves class preferences
        [HttpPost]
        public ActionResult ClassPreferenceEdit(ClassPreferenceForm form)
        {
            // process picture
            var headerPicture = Request.Files["HeaderPicture"];
            var isPictureUploaded = headerPicture != null && headerPicture.ContentLength != 0;

            // validates picture properties
            var imageValidationResult = isPictureUploaded
                ? ImageValidationHelper.ValidateJpgSize(headerPicture.InputStream, 520, 200)
                : new ImageValidationResult();

            // validates form
            if (!ModelState.IsValid || !imageValidationResult.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = classPreferenceAdministration.GetClassPreferenceForEditByForm(form);
                if (!imageValidationResult.IsValid)
                    MessageContainer.PushErrorMessage(imageValidationResult.ValidationMessage);
                return View(model);
            }

            // saves preferences
            try
            {
                classPreferenceAdministration.SaveClassPreference(form, 
                    isPictureUploaded ? headerPicture.InputStream : null, 
                    isPictureUploaded ? headerPicture.FileName : null);
                MessageContainer.PushMessage("Class preferences of the profile was saved.");
                return RedirectToAction("ClassPreference", new {id = form.ProfileId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }

        }


        // opens form for expense layout editing
        public ActionResult ExpensesLayoutEdit(long id)
        {
            try
            {
                var mode = classPreferenceAdministration.GetExpenseLayoutForEditByProfileId(id);
                return View(mode);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // saves expense layout
        [HttpPost]
        public ActionResult ExpensesLayoutEdit(ExpensesLayoutForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = classPreferenceAdministration.GetExpenseLayoutForEditByForm(form);
                return View(model);
            }

            try
            {
                classPreferenceAdministration.SaveExpenseLayout(form);
                MessageContainer.PushMessage("Layout of expenses was saved.");
                return RedirectToAction("ClassPreference", new { id = form.EntityId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        #endregion


        #region profile users

        // shows profile's users tab
        public ActionResult Users(long id)
        {
            try
            {
                var model = profileUsersAdministration.GetProfileUsersPageModel(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // adds new user to profile
        [HttpPost]
        public ActionResult AddUser(long profileId, int userId)
        {
            try
            {
                profileUsersAdministration.AddUserToProfile(profileId, userId);
                return RedirectToAction("Users", new {id = profileId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // removes user from profile
        [HttpPost]
        public ActionResult RemoveUser(long profileId, int userId)
        {
            try
            {
                profileUsersAdministration.RemoveUserFromProfile(profileId, userId);
                return RedirectToAction("Users", new { id = profileId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        #endregion

        #region email templates
        public ActionResult EmailTemplates(long id)
        {
            try
            {
                var model = profileEmailAdministration.GetEmailTypeSummaryByProfileId(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }
        #endregion

        #region private methods

        // redirects to controllers home url
        private ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Profile");
        }

        #endregion

    }

}