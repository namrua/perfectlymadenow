using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Main.Web.Helpers;
using AutomationSystem.Main.Web.Helpers.Materials;
using AutomationSystem.Main.Web.Helpers.Validations;
using AutomationSystem.Main.Web.Helpers.Validations.Models;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Materials controler - public part and administraiton
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("MaterialsController")]
    public class MaterialsController : Controller
    {
        
        // private components
        private readonly IClassMaterialAdministration materialAdministration = IocProvider.Get<IClassMaterialAdministration>();
        private readonly IClassMaterialService materialService = IocProvider.Get<IClassMaterialService>();
        private readonly IHomeService homeService = IocProvider.Get<IHomeService>();

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);


        #region public download pages

        // public page for students
        [AllowAnonymous]
        [HandleHomeError]
        public ActionResult Index(string request)
        {
            var requestInfo = materialService.GetRequestInfo(request);
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByClassId(requestInfo.ClassId), ViewBag);

            if (!requestInfo.LanguageId.HasValue)
            {
                // language not selected yet
                var model = materialService.GetPublicLanguageSelectionPageModel(request, requestInfo.ClassId);
                return View("SelectLanguage", model);
            }
            else
            {
                // materials ready for download
                var model = materialService.GetPublicDownloadPageModel(requestInfo.ClassMaterialId, requestInfo.ClassMaterialRecipientId, requestInfo.LanguageId.Value);
                return View("MaterialDownload", model);
            }
        }

        // sets registration material language
        [AllowAnonymous]
        [HandleHomeError]
        [HttpPost]
        public ActionResult SelectLanguage(string request, LanguageEnum languageId)
        {
            materialService.SetClassRegistrationMaterialLanguage(request, languageId);
            return RedirectToAction("Index", new { request });
        }

        // downloads materials
        [AllowAnonymous]
        [HandleHomeError]
        public ActionResult Download(string request, long id)
        {
            var requestInfo = WebRequestMetadataHelper.GetWebRequestInfo(Request);
            var model = materialService.GetMaterialForDownload(request, id, requestInfo);
            return model.GetFileActionResult();
        }

        #endregion


        #region class materials 

        // materials tab for class
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult Class(long id)
        {
            try
            {
                var model = materialAdministration.GetClassMaterialsPageModel(id);
                if (model.AreMaterialsDisabled)
                    return View("../Class/EmptyTab", new EmptyClassPageModel(model.Class, model.MaterialsDisabledMessage, TabItemId.ClassMaterials));
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }

        }

        // opens class materials form
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult ClassEdit(long id)
        {
            try
            {
                var model = materialAdministration.GetClassMaterialFormByClassId(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // saves class materials
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult ClassEdit(ClassMaterialForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                return View(form);
            }

            try
            {
                materialAdministration.SaveClassMaterial(form);
                MessageContainer.PushMessage("Class material settings was saved.");
                return RedirectToAction("Class", new {id = form.ClassId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        #endregion


        #region class material commands

        // unlocks class materials
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult UnlockClass(long id)
        {
            try
            {
                materialAdministration.UnlockClassMaterials(id);
                materialAdministration.SendMaterialNotification(id);
                MessageContainer.PushMessage("Materials of class were unlocked and approved students were notified.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToAction("Class", new { id });
        }

        // send material notification
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult SendNotification(long id)
        {
            try
            {
                materialAdministration.SendMaterialNotification(id);
                MessageContainer.PushMessage("Approved students were notified.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToAction("Class", new { id });
        }

        // locks class
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult LockClass(long id)
        {
            try
            {
                materialAdministration.LockClassMaterials(id);
                MessageContainer.PushMessage("Materials of class were locked and they are no more available for the students.");
                return RedirectToAction("Class", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        #endregion


        #region material recipients

        // opens registration material tab
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult Registration(long id)
        {
            try
            {
                var model = materialAdministration.GetMaterialRecipientPageModelByRecipientId(new RecipientId(EntityTypeEnum.MainClassRegistration, id));
                return View(model.IsMaterialsDisabled ? "RecipientMessage" : "Recipient", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // opens recipient detail
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult Recipient(long id)
        {
            try
            {
                var model = materialAdministration.GetMaterialRecipientPageModelByMaterialRecipientId(id);
                return View(model.IsMaterialsDisabled ? "RecipientMessage" : "Recipient", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // opens recipient material form
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult RecipientEdit(long id)
        {
            try
            {
                var model = materialAdministration.GetMaterialRecipientForEdit(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // saves recipient materials
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult RecipientEdit(MaterialRecipientForm form)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.TriggerValidation = true;
                    var model = materialAdministration.GetMaterialRecipientForEditByForm(form);
                    return View(model);
                }

                materialAdministration.SaveClassMaterialRecipient(form);
                MessageContainer.PushMessage("Student's material settings was saved.");
                return RedirectToRecipient(new RecipientId(form.RecipientTypeId, form.RecipientId), form.ClassMaterialRecipientId);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // downloads encrypted file for recipient
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult DownloadRecipientMaterial(long materialRecipientId, long materialFileId)
        {
            try
            {
                var fileToDownload = materialAdministration.GetMaterialForDownload(materialRecipientId, materialFileId);
                return fileToDownload.GetFileActionResult();
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        #endregion


        #region material recipients commands

        // locks  materials recipient
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult LockRecipient(long id)
        {
            try
            {
                var recipientId = materialAdministration.LockMaterialRecipient(id);
                MessageContainer.PushMessage("Materials of the student was locked.");
                return RedirectToRecipient(recipientId, id);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // sends notification to recipient
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult SendNotificationToRecipient(long id)
        {
            try
            {
                var recipientId = materialAdministration.SendMaterialNotificationToRecipient(id);
                MessageContainer.PushMessage("Material notification was sent to the student.");
                return RedirectToRecipient(recipientId, id);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // unlocks recipient materials
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult UnlockRecipient(long id)
        {
            try
            {
                var recipientId = materialAdministration.UnlockMaterialRecipient(id);
                MessageContainer.PushMessage("Materials of the student was unlocked.");
                return RedirectToRecipient(recipientId, id);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        #endregion


        #region file administration

        // opens new material file form
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult FileNew(long id)
        {
            try
            {
                var model = materialAdministration.GetNewClassMaterialFileForEdit(id);
                return View("FileEdit", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // opens existing material file form
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult FileEdit(long id)
        {
            try
            {
                var model = materialAdministration.GetClassMaterialFileForEditById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // saves material file
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult FileEdit(ClassMaterialFileForm form)
        {
            // process pdf material
            var pdfMaterial = Request.Files["PdfMaterial"];
            var isPdfMaterialUploaded = pdfMaterial != null && pdfMaterial.ContentLength != 0;

            // validates pdf material properties
            PdfValidationResult pdfValidationResult = new PdfValidationResult();
            if (isPdfMaterialUploaded)
            {
                pdfValidationResult = PdfValidationHelper.ValidatePdf(pdfMaterial.InputStream);
            }
            else
            {
                // there must be uploaded file when file ClassMatrielFileId is created
                if (form.ClassMaterialFileId == 0)
                {
                    pdfValidationResult.IsValid = false;
                    pdfValidationResult.ValidationMessage = "PDF material was not attached.";
                }

            }

            // validates form
            if (!ModelState.IsValid || !pdfValidationResult.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = materialAdministration.GetClassMaterialFileForEditByForm(form);
                if (!pdfValidationResult.IsValid)
                    MessageContainer.PushErrorMessage(pdfValidationResult.ValidationMessage);
                return View(model);
            }

            // save material file
            try
            {
                materialAdministration.SaveClassMaterialFile(form, 
                    isPdfMaterialUploaded ? pdfMaterial.InputStream : null, 
                    isPdfMaterialUploaded ? pdfMaterial.FileName : null);
                MessageContainer.PushMessage("PDF material was saved.");
                return RedirectToAction("Class", new {id = form.ClassId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // deletes material file
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult FileDelete(long id)
        {
            try
            {
                var classId = materialAdministration.DeleteClassMaterialFileAndReturnClassId(id);
                MessageContainer.PushMessage("PDF material was deleted.");
                return RedirectToAction("Class", new {id = classId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        #endregion


        #region monitoring

        // recipient monitoring
        [AuthorizeEntitle(Entitle.MainMaterials)]
        public ActionResult RecipientMonitoring(long id, EntityTypeEnum recipientTypeId)
        {
            try
            {
                var model = materialAdministration.GetClassMaterialMonitoringPageModel(id, recipientTypeId);
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
            return RedirectToAction("Index", "Class");
        }

        #endregion

        #region static methods

        public ActionResult RedirectToRecipient(RecipientId recipientId, long? recipientMaterialId)
        {
            return Redirect(Url.MaterialRecipientUrl(recipientId, recipientMaterialId));
        }

        #endregion
    }

}