using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Certificates;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Web.Filters;
using CorabeuControl.Context;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Class controler
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("RegistrationController")]
    public class RegistrationController : Controller
    {
   
        private readonly IRegistrationAdministration registrationAdministration = IocProvider.Get<IRegistrationAdministration>();
        private readonly IRegistrationDocumentAdministration documentAdministration = IocProvider.Get<IRegistrationDocumentAdministration>();
        private readonly IRegistrationIntegrationAdministration integrationAdministration = IocProvider.Get<IRegistrationIntegrationAdministration>();
        private readonly IRegistrationInvitationAdministration invitationAdministration = IocProvider.Get<IRegistrationInvitationAdministration>();
        private readonly IRegistrationPaymentAdministration paymentAdministration = IocProvider.Get<IRegistrationPaymentAdministration>();
        private readonly IRegistrationPersonalDataAdministration personalDataAdministration = IocProvider.Get<IRegistrationPersonalDataAdministration>();
        private readonly IRegistrationReviewAdministration reviewAdministration = IocProvider.Get<IRegistrationReviewAdministration>();
        

        private readonly Lazy<string> certificateRootPath;
        
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        public RegistrationController()
        {
            certificateRootPath = new Lazy<string>(() => Server.MapPath(CertificateConstants.CertificateRootPath));
        }

        #region detail and commands
        
        [CorabeuContext]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Detail(long id)
        {
            try
            {
                var model = registrationAdministration.GetRegistrationDetailPageModel(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Approve(long id)
        {
            try
            {
                registrationAdministration.ApproveRegistration(id);
                MessageContainer.PushMessage("Registration was approved.");
                return RedirectToAction("Detail", new {id});

            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }
        
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult SendPayment(long id)
        {
            try
            {
                registrationAdministration.SendPaymentRequest(id);
                MessageContainer.PushMessage("Payment request was sent.");
                return RedirectToAction("Detail", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }
        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult PrepareCancel(long id)
        {
            try
            {
                registrationAdministration.CreateRegistrationCancelationEmailTemplate(id);
                MessageContainer.PushMessage("Registration cancelation was initialized. Please, modify and send cancelation email to complete cancelation.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);                
            }
            return RedirectToAction("Detail", new { id });
        }

        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Cancel(long id)
        {
            try
            {
                registrationAdministration.CancelRegistration(id);
                MessageContainer.PushMessage("Cancelation email was send to the student and registration was canceled.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToAction("Detail", new { id });
        }
        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult CancelWithoutNotification(long id)
        {
            try
            {
                registrationAdministration.CancelRegistrationWithoutNotification(id);
                MessageContainer.PushMessage("Registration was canceled.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToAction("Detail", new { id });
        }

        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult DiscardCancelation(long id)
        {
            try
            {
                registrationAdministration.DiscardCancelation(id);
                MessageContainer.PushMessage("Cancelation email was discarded.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToAction("Detail", new { id });
        }


        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Delete(long id)
        {
            try
            {
                var classId = registrationAdministration.DeleteRegistration(id);
                MessageContainer.PushMessage("Registration was deleted.");
                return classId.HasValue
                    ? RedirectToAction("Students", "Class", new {id = classId.Value})
                    : RedirectToAction("Index", "Class");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        #endregion


        #region editing and personal data
        
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult PersonalData(long id)
        {
            try
            {                
                var model = personalDataAdministration.GetRegistrationDetailById(id);
                var controllerInfo = personalDataAdministration.GetControllerInfoByRegistrationTypeId(model.RegistrationTypeId);
                return View(controllerInfo.ViewForDetail, model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }
        
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult New(RegistrationTypeEnum id, long classId)
        {
            try
            {                                
                var model = personalDataAdministration.GetNewRegistrationForEdit(id, classId);
                var controllerInfo = personalDataAdministration.GetControllerInfoByRegistrationTypeId(model.Form.RegistrationTypeId);
                ViewBag.ActionForSave = controllerInfo.ActionForSave;
                return View(controllerInfo.ViewForForm, model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }
        
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Edit(long id)
        {
            try
            {              
                var model = personalDataAdministration.GetRegistrationForEdit(id);
                var controllerInfo = personalDataAdministration.GetControllerInfoByRegistrationTypeId(model.Form.RegistrationTypeId);
                ViewBag.ActionForSave = controllerInfo.ActionForSave;
                return View(controllerInfo.ViewForForm, model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }
        
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult StudentEdit(RegistrationStudentForm form)
        {
            return TrySaveRegistration(form);
        }
        
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult ChildEdit(RegistrationChildForm form)
        {
            return TrySaveRegistration(form);
        }
        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult WwaEdit(RegistrationWwaForm form)
        {
            return TrySaveRegistration(form);
        }

        #endregion


        #region payment
        
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Payment(long id)
        {
            try
            {
                var model = paymentAdministration.GetRegistrationPaymentByRegistrationId(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }
        
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult PaymentEdit(long id)
        {
            try
            {
                var model = paymentAdministration.GetRegistrationPaymentForEditById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }
        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult PaymentEdit(RegistrationPaymentForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var result = paymentAdministration.GetRegistrationPaymentForEditByForm(form);
                return View(result);
            }

            try
            {
                paymentAdministration.SaveRegistrationPayment(form);
                MessageContainer.PushMessage("Registration’s payment was saved.");
                return RedirectToAction("Payment", new {id = form.ClassRegistrationId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        #endregion


        #region manual review
        
        [CorabeuContext]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult ManualReview(long id)
        {
            try
            {
                var model = reviewAdministration.GetRegistrationManualReviewPageModel(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }
        
        [CorabeuContext]
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult ManualReview(long id, long formerStudentId)
        {
            try
            {
                registrationAdministration.ManualReview(id, formerStudentId);
                MessageContainer.PushMessage("Former student was assigned to the registration.");
                return RedirectToAction("ManualReview", new {id});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);                
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);               
            }
            IContextManager manager = ContextHelper.GetContextManager(ViewBag);
            return Redirect(manager.GetBackUrl(Url.Action("Index", "Class")));
        }

        #endregion


        #region integration
        
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult Integration(long id)
        {
            try
            {
                var model = integrationAdministration.GetRegistrationIntegrationPageModel(id);                
                return View(model.IsIntegrationDisabled ? "IntegrationMessage" : "Integration", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }            
        }
        
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public async Task<ActionResult> UpdateIntegrationState(long id)
        {
            try
            {
                var model = await Task.Run(() => integrationAdministration.ExecuteIntegrationRequest(id, AsyncRequestTypeEnum.UpdateOuterSystemState));               
                return PartialView("_IntegrationStatesPartial", model);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
        }
        
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public async Task<ActionResult> SyncWebExState(long id)
        {
            try
            {
                var model = await Task.Run(() => integrationAdministration.ExecuteIntegrationRequest(id, AsyncRequestTypeEnum.SyncOuterSystemState));
                return PartialView("_IntegrationStatesPartial", model);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
        }
        
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public async Task<ActionResult> SendWebExInvitation(long id)
        {
            try
            {
                var model = await Task.Run(() => integrationAdministration.ExecuteIntegrationRequest(id, AsyncRequestTypeEnum.SendOuterSystemInvitation));
                return PartialView("_IntegrationStatesPartial", model);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
        }

        #endregion


        #region communication
        
        [CorabeuContext]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Communication(long id)
        {
            try
            {
                var model = registrationAdministration.GetRegistrationCommunication(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        #endregion


        #region documents
        
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Documents(long id)
        {
            try
            {
                var model = documentAdministration.GetRegistrationDocumentsPageModel(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult GenerateCertificate(long id)
        {
            try
            {
                documentAdministration.GenerateCertificate(certificateRootPath.Value, id);
                MessageContainer.PushMessage("Certificate was generated.");
                return RedirectToAction("Documents", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }       

        #endregion


        #region private methods
        
        private ActionResult TrySaveRegistration(BaseRegistrationForm form)
        {                                  
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.TriggerValidation = true;
                    var model = personalDataAdministration.GetFormRegistrationForEdit(form);
                    var controllerInfo = personalDataAdministration.GetControllerInfoByRegistrationTypeId(model.Form.RegistrationTypeId);
                    ViewBag.ActionForSave = controllerInfo.ActionForSave;
                    return View(controllerInfo.ViewForForm, model);
                }

                var id = personalDataAdministration.SaveRegistration(form);
                MessageContainer.PushMessage("Registration was saved.");
                return form.ClassRegistrationId == 0 
                    ? RedirectToAction("Detail", new { id })
                    : RedirectToAction("PersonalData", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        #endregion

    }

}