using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Main.Web.Helpers;
using AutomationSystem.Main.Web.Helpers.HomeWorkflow;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using CorabeuControl.ModelMetadata;
using Resources;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Controller for former student and classes administration
    /// </summary>
    [RequireHttps]  
    [HandleHomeError]
    [UseCorabeuLocalisation(LocalisedTextAttribute.ProviderKey)]
    public class HomeController : Controller
    {
        // private components
        private readonly IHomeService homeService = IocProvider.Get<IHomeService>();           

        // landing page
        public ActionResult Index(string id, EnvironmentTypeEnum? env)
        {
            var model = homeService.GetHomePageModel(env, id ?? ProfileConstants.DefaultMoniker);
            RegistrationPageStyleHelper.SetStyle(model.ProfilePageStyle, ViewBag);
            return View(model);
        }

        // distance classes page
        public ActionResult Distance(string id, EnvironmentTypeEnum? env)
        {
            var model = homeService.GetDistanceClassesPageModel(env, id ?? ProfileConstants.DefaultMoniker);
            RegistrationPageStyleHelper.SetStyle(model.ProfilePageStyle, ViewBag);
            return View(model);
        }

        // registration selection
        public ActionResult SelectRegistration(long id, bool? forWwa, RegistrationTypeEnum? backFromRegistrationTypeId)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByClassId(id), ViewBag);
            var model = homeService.GetClassRegistrationSelectionPageModel(id, forWwa, backFromRegistrationTypeId);
            return View(model);
        }

        public ActionResult SelectRegistrationForDistance(long id)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByClassId(id), ViewBag);
            var model = homeService.GetClassRegistrationSelectionPageModel(id, true, null);
            return RedirectToAction("New", new { id = model.RegistrationTypes.Single().RegistrationTypeId, classId = id });
        }

        #region registration form

        // new registration
        public ActionResult New(RegistrationTypeEnum id, long classId)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByClassId(classId), ViewBag);
            var model = homeService.GetNewRegistrationForEdit(id, classId);
            var controllerInfo = homeService.GetControllerInfoByRegistrationTypeId(model.Form.RegistrationTypeId);
            ViewBag.ActionForSave = controllerInfo.ActionForSave;
            return View(controllerInfo.ViewForForm, model);       
        }

        // edit registration
        public ActionResult Edit(long id)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(id), ViewBag);
            var model = homeService.GetRegistrationForEdit(id);
            var controllerInfo = homeService.GetControllerInfoByRegistrationTypeId(model.Form.RegistrationTypeId);
            ViewBag.ActionForSave = controllerInfo.ActionForSave;
            return View(controllerInfo.ViewForForm, model);            
        }

        // save actions
        [HttpPost]
        public ActionResult StudentEdit(RegistrationStudentForm form)
        {
            return TrySaveRegistration(form);
        }

        // save actions
        [HttpPost]
        public ActionResult ChildEdit(RegistrationChildForm form)
        {
            return TrySaveRegistration(form);
        }

        // save actions
        [HttpPost]
        public ActionResult WwaEdit(RegistrationWwaForm form)
        {
            return TrySaveRegistration(form);
        }

        // tries to saves registration - generic method
        private ActionResult TrySaveRegistration(BaseRegistrationForm form)
        {          
            if (!ModelState.IsValid)
            {
                RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(form.ClassRegistrationId), ViewBag);
                ViewBag.TriggerValidation = true;
                var model = homeService.GetFormRegistrationForEdit(form);
                var controllerInfo = homeService.GetControllerInfoByRegistrationTypeId(model.Form.RegistrationTypeId);
                ViewBag.ActionForSave = controllerInfo.ActionForSave;
                return View(controllerInfo.ViewForForm, model);
            }

            var workflowState = homeService.SaveRegistration(form);
            var workflowHelper = HomeWorkflowHelper.Create(this, workflowState);
            return Redirect(workflowHelper.GetNextAction().Link);
        }

        #endregion

        #region invitations

        // opens new invitation
        public ActionResult Invitation(string request)
        {
            var model = homeService.GetNewRegistrationForEditByInvitationRequest(request);
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByClassId(model.Form.ClassId), ViewBag);
            var controllerInfo = homeService.GetControllerInfoByRegistrationTypeId(model.Form.RegistrationTypeId);
            ViewBag.ActionForSave = controllerInfo.ActionForSave;
            return View(controllerInfo.ViewForForm, model);
        }

        #endregion

        #region reviewing

        // shows former students compatible for reviewed student
        public ActionResult SelectStudent(long id)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(id), ViewBag);
            var model = homeService.GetFormerStudentSelectionPageModel(id);
            return View(model);
        }


        // saves last class
        [HttpPost]
        public ActionResult SaveLastClass(RegistrationLastClassForm form)
        {
            if (!ModelState.IsValid)
            {
                RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(form.ClassRegistrationId), ViewBag);
                ViewBag.TriggerValidation = true;
                var model = homeService.GetFormerStudentSelectionPageModel(form.ClassRegistrationId);
                model.FormLastClass = form;
                return View("SelectStudent", model);
            }

            homeService.SaveRegistrationLastClass(form);
            return RedirectToAction("Confirmation", new { id = form.ClassRegistrationId });
        }


        // assign former student to registration
        [HttpPost]
        public ActionResult SelectStudent(long id, long? formerStudentId)
        {
            homeService.SaveFormerStudentSelection(id, formerStudentId);
            if (!formerStudentId.HasValue || formerStudentId.Value == 0)
                return RedirectToAction("SelectStudent", new {id});
            return RedirectToAction("Confirmation", new { id });
        }

        #endregion

        #region confirmation and agreement


        // shows personal data
        public ActionResult Confirmation(long id)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(id), ViewBag);
            var model = homeService.GetRegistrationConfirmationPageModel(id);           
            return View(model);            
        }


        // shows agreements and form
        public ActionResult Agreement(long id)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(id), ViewBag);
            var model = homeService.GetRegistrationAgreementPageModel(id);
            return View(model);
        }

        // shows agreements and form
        [HttpPost]
        public ActionResult Agreement(RegistrationAgreementForm form)
        {
            var registrationId = form.ClassRegistrationId;
            if (!ModelState.IsValid || !form.AcceptAgreements)
            {
                RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(form.ClassRegistrationId), ViewBag);
                var model = homeService.GetRegistrationAgreementPageModel(registrationId);
                ViewBag.TriggerValidation = true;
                model.Form = form;
                return View(model);
            }
           
            var workflowState = homeService.SaveRegistrationAgreementAcceptationState(form);
            var workflowHelper = HomeWorkflowHelper.Create(this, workflowState);
            return Redirect(workflowHelper.GetNextAction().Link);
        }

        #endregion

        #region payment


        // Payment page
        public ActionResult Payment(long id)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(id), ViewBag);
            var model = homeService.GetRegistrationPaymentPageModel(id);
            return View(model);
        }


        // execute payment page
        [HttpPost]
        public ActionResult ExecutePayment(PaymentExecutionInput input)
        {          
            string errorMessage;
            if (!ModelState.IsValid)
            {
                errorMessage = "Model state of PaymentExecutionInput is not valid.";
            }
            else
            {
                var paymentResult = homeService.ExecutePayment(input);
                if (paymentResult.IsSuccess)
                    return RedirectToAction("Complete", new { classId = paymentResult.ClassId });
                errorMessage = paymentResult.Message;
            }

            // return back payment page
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByRegistrationId(input.RegistrationId), ViewBag);
            var model = homeService.GetRegistrationPaymentPageModel(input.RegistrationId);
            model.PaymentErrorMessage = errorMessage;
            return View("Payment", model);
        }

        #endregion

        #region final

        // completion of registraion
        public ActionResult Complete(long? classId, long? classRegistrationId)
        {
            var style = classId.HasValue
                ? homeService.GetRegistrationPageStyleByClassId(classId)
                : homeService.GetRegistrationPageStyleByRegistrationId(classRegistrationId);
            RegistrationPageStyleHelper.SetStyle(style, ViewBag);
            return View();
        }


        // manual review
        public ActionResult ManualReview(long? classId, long? classRegistrationId)
        {
            var style = classId.HasValue
                ? homeService.GetRegistrationPageStyleByClassId(classId)
                : homeService.GetRegistrationPageStyleByRegistrationId(classRegistrationId);
            RegistrationPageStyleHelper.SetStyle(style, ViewBag);
            return View();
        }


        // error 
        public ActionResult Error(HomeServiceErrorType id = HomeServiceErrorType.GenericError, long? classId = null)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByClassId(classId), ViewBag);

            string message, title;
            switch (id)
            {
                case HomeServiceErrorType.ClassRegistrationClosed:                    
                    title = ErrorTexts.ClassClosedTitleString;
                    message = ErrorTexts.ClassClosedString;
                    break;
                case HomeServiceErrorType.ClassRegistrationNotStarted:                    
                    title = ErrorTexts.ClassNotStartedTitleString;
                    message = ErrorTexts.ClassNotStartedString;
                    break;
                case HomeServiceErrorType.RegistrationComplete:
                    title = ErrorTexts.RegistrationCompleteTitleString;
                    message = ErrorTexts.RegistrationCompleteString;                  
                    break;
                case HomeServiceErrorType.InvalidRegistrationStep:
                    title = ErrorTexts.InvalidStepTitleString;
                    message = ErrorTexts.InvalidStepString;
                    break;
                case HomeServiceErrorType.RegistrationTypeNotAllowed:
                    title = ErrorTexts.RegistrationNotAllowedTitleString;
                    message = ErrorTexts.RegistrationNotAllowedString;
                    break;
                case HomeServiceErrorType.PreRegistrationClosed:
                    title = ErrorTexts.PreRegistrationClosedTitleString;
                    message = ErrorTexts.PreRegistrationClosedString;
                    break;
                case HomeServiceErrorType.InvitationExpired:
                    title = ErrorTexts.InvitationExpiredTitleString;
                    message = ErrorTexts.InvitationExpiredString;
                    break;
                case HomeServiceErrorType.MaterialsNotAvailable:
                    title = ErrorTexts.MaterialsNotAvailableTitleString;
                    message = ErrorTexts.MaterialsNotAvailableString;
                    break;
                case HomeServiceErrorType.InvalidPage:
                    title = ErrorTexts.InvalidPageTitleString;
                    message = ErrorTexts.InvalidPageString;
                    break;

                // default errors
                case HomeServiceErrorType.GenericError:
                default:
                    title = ErrorTexts.GenericTitleString;
                    message = ErrorTexts.GenericString;
                    break;
            }

            // creates model and returns view
            var model = new HomeErrorPageModel
            {
                Title = title,
                Message = new HtmlString(message),
            };
            return View(model);
        }

        #endregion
    }

}
