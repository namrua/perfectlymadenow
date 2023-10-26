using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations;
using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Main.Web.Helpers;
using AutomationSystem.Main.Web.Helpers.Validations;
using AutomationSystem.Main.Web.Helpers.Validations.Models;
using CorabeuControl.Context;
using CorabeuControl.Helpers;
using System;
using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Class controler
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("ClassController")]
    public class ClassController : Controller
    {
        // private components
        private readonly IClassActionAdministration classActionAdministration = IocProvider.Get<IClassActionAdministration>();
        private readonly IClassAdministration classAdministration = IocProvider.Get<IClassAdministration>();
        private readonly IClassCertificateAdministration classCertificateAdministration = IocProvider.Get<IClassCertificateAdministration>();
        private readonly IClassFinanceAdministration classFinanceAdministration = IocProvider.Get<IClassFinanceAdministration>();
        private readonly IClassReportAdministration classReportAdministration = IocProvider.Get<IClassReportAdministration>();
        private readonly IClassStyleAdministration classStyleAdministration = IocProvider.Get<IClassStyleAdministration>();
        private readonly IRegistrationAdministration registrationAdministration = IocProvider.Get<IRegistrationAdministration>();
        private readonly IRegistrationInvitationAdministration registrationInvitationAdministration = IocProvider.Get<IRegistrationInvitationAdministration>();

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        // private fields
        private readonly Lazy<string> rootPath;

        // constructor
        public ClassController()
        {
            rootPath = new Lazy<string>(() => Server.MapPath(ReportConstants.ReportRootPath));
        }

        #region editing

        // shows list of classes
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Index(ClassFilter filter, bool search = false)
        {            
            if (filter.Env.HasValue)
                search = true;
            var model = classAdministration.GetClassesForList(filter, search);
            return View(model);
        }

        // shows detail of class
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Detail(long id)
        {
            try
            {
                var model = classAdministration.GetClassDetailById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // opens new class form
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult New(long profileId, ClassCategoryEnum category, EnvironmentTypeEnum? env)
        {
            var model = classAdministration.GetNewClassForEdit(profileId, category);
            model.Env = env;
            return View("Edit", model);
        }


        // opens class form
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Edit(long id)
        {
            try
            {
                var model = classAdministration.GetClassForEditById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // saves class
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Edit(ClassForm form, EnvironmentTypeEnum? env)
        {
            var validationResult = classAdministration.ValidateClassForm(form);
            if (!ModelState.IsValid || !validationResult.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = classAdministration.GetFormClassForEdit(form, validationResult);
                model.Env = env;
                return View(model);
            }

            try
            {
                var id = classAdministration.SaveClass(form, env);
                MessageContainer.PushMessage("Class was saved.");
                return RedirectToAction("Detail", new {id});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // deletes class
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Delete(long id)
        {
            try
            {
                classAdministration.DeleteClass(id);
                MessageContainer.PushMessage("Class was deleted.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);                
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);               
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region actions

        // views class action list
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Actions(long id)
        {
            try
            {
                var model = classActionAdministration.GetClassActionPageModel(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // views conversation action detail
        [CorabeuContext]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult ActionDetail(long id)
        {
            try
            {
                var model = classActionAdministration.GetClassActionDetail(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // creates new action
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult CreateAction(ClassActionTypeEnum id, long classId)
        {
            try
            {              
                var classActionId = classActionAdministration.CreateClassAction(classId, id);
                MessageContainer.PushMessage("Action was created.");
                return RedirectToAction("ActionDetail", new { id = classActionId });

            }            
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Actions", new { id = classId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // processes class action
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult ProcessAction(long id)
        {
            try
            {
                classActionAdministration.ProcessClassAction(id);
                MessageContainer.PushMessage("Action was processed.");
                return RedirectToAction("ActionDetail", new { id });
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("ActionDetail", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // delete class action
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult DeleteAction(long id)
        {
            try
            {
                var classId = classActionAdministration.DeleteClassAction(id);
                if (!classId.HasValue) return RedirectToAction("Index");
                MessageContainer.PushMessage("Action was deleted.");
                return RedirectToAction("Actions", new { id = classId });

            }           
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region registrations

        // gets registraiton tab
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Registrations(long id)
        {
            try
            {
                var model = registrationAdministration.GetClassRegistrationPageModel(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region students

        // gets students tab
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Students(long id, RegistrationFilter filter, bool search = false)
        {
            // sets defaults if no searching
            filter.ClassId = id;
            if (!search)
            {
                filter.RegistrationState = RegistrationState.Approved;
            }

            try
            {
                var model = registrationAdministration.GetRegistrationsForList(filter, search);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }            
        }

        #endregion

        #region invitations

        // get invitations tab
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Invitations(long id)
        {
            try
            {
                var model = registrationInvitationAdministration.GetClassInvitationPageModel(id);
                return model.AreInvitationsDisabled 
                    ? View("EmptyTab", new EmptyClassPageModel(model.Class, model.InvitationDisabledMessage, TabItemId.ClassInvitations)) 
                    : View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // creates invitation 
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult InvitationNew(RegistrationTypeEnum id, long classId)
        {
            var model = registrationInvitationAdministration.GetClassInvitationForEdit(id, classId);
            return View("InvitationEdit", model);
        }

        // saves invitation
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult InvitationEdit(ClassInvitationForm form)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = registrationInvitationAdministration.GetFormInvitationForEdit(form);
                return View("InvitationEdit", model);
            }
            try
            {
                var id = registrationInvitationAdministration.SaveInvitation(form);
                MessageContainer.PushMessage("Class invitation was sent and saved.");
                return RedirectToAction("Invitations", new { id = form.ClassId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // deletes invitation
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult InvitationDelete(long id)
        {
            var classId = registrationInvitationAdministration.DeleteInvitation(id);
            MessageContainer.PushMessage("Class invitation was deleted.");
            return classId.HasValue ? RedirectToAction("Invitations", new { id = classId}) : RedirectToAction("Index");
        }

        #endregion


        #region certificates

        // gets students tab
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Certificates(long id)
        {
            try
            {
                var model = classCertificateAdministration.GetClassCertificatesPageModel(id);
                return model.AreCertificatesDisabled 
                    ? View("EmptyTab", new EmptyClassPageModel(model.Class, model.CertificatesDisabledMessage, TabItemId.ClassCertificates))
                    : View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // generate certificates
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult GenerateCertificates(long id)
        {
            try
            {
                classCertificateAdministration.GenerateCertificates(id);
                MessageContainer.PushMessage("Generating of class certificates was started. Refresh page to view current state in the Document generating section.");
                return RedirectToAction("Certificates", "Class", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        #endregion

        #region report setting

        // opens report settings form
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult ReportSettingEdit(long id)
        {
            try
            {
                var model = classReportAdministration.GetClassReportSettingForEditByClassId(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        // saves report settings
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult ReportSettingEdit(ClassReportSettingForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = classReportAdministration.GetClassReportSettingForEditByForm(form);
                return View(model);
            }

            try
            {
                classReportAdministration.SaveClassReportSetting(form);
                MessageContainer.PushMessage("Report settings was saved.");
                return RedirectToAction("Reports", new { id = form.ClassId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        #endregion

        #region reports

        // gets students tab
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult Reports(long id)
        {
            try
            {
                var model = classReportAdministration.GetClassReportsPageModel(id);
                return model.AreReportsDisabled 
                    ? View("EmptyTab", new EmptyClassPageModel(model.Class, model.ReportsDisabledMessage, TabItemId.ClassReports)) 
                    : View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // generate certificates
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult GenerateFinancialForms(long id)
        {
            try
            {
                classReportAdministration.GenerateFinancialForms(id);
                MessageContainer.PushMessage("Generating of class financial forms was started. Refresh page to view current state in the Document generating section.");
                return RedirectToAction("Reports", "Class", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        // selects recipient for class communication
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult SelectRecipients(long id, ClassCommunicationType type)
        {
            try
            {
                var model = classReportAdministration.GetClassRecipientSelectionForEdit(id, type);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }


        // send message to all selected recipients
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult SendToRecipients(ClassRecipientSelectionForm form)
        {
            try
            {
                classReportAdministration.SendMessageToRecipients(form);
                MessageContainer.PushMessage("Emails were sent to all recipients.");
                return RedirectToAction("Reports", "Class", new {id = form.ClassId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }


        // send registration list to master coordinator
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult SendRegistrationListToMasterCoordinator(long id)
        {
            try
            {
                classReportAdministration.SendRegistrationListToMasterCoordinator(rootPath.Value, id);
                MessageContainer.PushMessage("Registration list was send to Master coordinator.");
                return RedirectToAction("Reports", new{ id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        #endregion

        #region styles & behavior

        // shows style and behavior tab
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult Style(long id)
        {
            try
            {
                var model = classStyleAdministration.GetClassStylePageModel(id);
                return model.AreStylesAndBehaviorDisabled
                    ? View("EmptyTab", new EmptyClassPageModel(model.Class, model.StylesAndBehaviorDisabledMessage, TabItemId.ClassStyle))
                    : View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        // opens class style form
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult StyleEdit(long id)
        {
            try
            {
                var model = classStyleAdministration.GetClassStyleForEditByClassId(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        // saves class style form
        [HttpPost]
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult StyleEdit(ClassStyleForm form)
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
                var model = classStyleAdministration.GetClassStyleForEditByForm(form);
                if (!imageValidationResult.IsValid)
                    MessageContainer.PushErrorMessage(imageValidationResult.ValidationMessage);
                return View(model);
            }

            // saves style
            try
            {
                classStyleAdministration.SaveClassStyle(form, 
                   isPictureUploaded ? headerPicture.InputStream : null, 
                   isPictureUploaded ? headerPicture.FileName : null);
               MessageContainer.PushMessage("Style and behavior of the class was saved.");
               return RedirectToAction("Style", new {id = form.ClassId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        #endregion

        #region business

        // shows finance tab
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult Business(long id)
        {
            try
            {
                var model = classFinanceAdministration.GetClassFinancePageModel(id);
                return model.AreFinanceDisabled 
                    ? View("EmptyTab", new EmptyClassPageModel(model.Class, model.FinanceDisabledMessage, TabItemId.ClassFinance))
                    : View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        // opens form for finance editing (class business)
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult BusinessEdit(long id)
        {
            try
            {
                var model = classFinanceAdministration.GetClassBusinessForEditByClassId(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        // saves finance (class business)
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult BusinessEdit(ClassBusinessForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = classFinanceAdministration.GetFormClassBusinessForEdit(form);
                return View(model);
            }

            try
            {
                classFinanceAdministration.SaveClassBusiness(form);
                MessageContainer.PushMessage("Finance setting was saved.");
                return RedirectToAction("Business", new {id = form.ClassId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }


        // opens form for expense layout editing
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult ExpensesLayoutEdit(long id)
        {
            try
            {
                var mode = classFinanceAdministration.GetExpensesLayoutForEditByClassId(id);
                return View(mode);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }

        // saves expense layout
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult ExpensesLayoutEdit(ExpensesLayoutForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = classFinanceAdministration.GetFormExpensesLayoutForEdit(form);
                return View(model);
            }

            try
            {
                classFinanceAdministration.SaveExpensesLayout(form);
                MessageContainer.PushMessage("Layout of expenses was saved.");
                return RedirectToAction("Business", new {id = form.EntityId});
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