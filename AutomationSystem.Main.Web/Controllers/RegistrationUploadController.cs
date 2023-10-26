using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic;
using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Main.Web.Helpers.BatchUploads;
using CorabeuControl.Helpers;
using System;
using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [AuthorizeEntitle(Entitle.MainClasses)]
    [HandleAdminError("RegistrationUploadController")]
    public class RegistrationUploadController : Controller
    {

        private readonly IRegistrationBatchUploadAdministration batchUploadService = IocProvider.Get<IRegistrationBatchUploadAdministration>();
        private readonly IBatchUploadHelper batchUploadHelper = IocProvider.Get<IBatchUploadHelper>();

        public IMessageContainer MessageContainer => new MessageContainer(Session);

        #region registration batch uploads

        public ActionResult UploadNew(long id)
        {
            try
            {
                var model = batchUploadService.GetNewBatchUploadForEdit(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToIndex();
            }
        }

        [HttpPost]
        public ActionResult UploadNew(RegistrationBatchUploadForm form)
        {
            try
            {
                var batchFile = Request.Files["BatchFile"];
                if (!ModelState.IsValid || batchFile == null || batchFile.ContentLength == 0)
                {
                    var model = batchUploadService.GetBatchUploadForEditByForm(form);
                    ViewBag.TriggerValidation = true;
                    return View(model);
                }

                var result = batchUploadService.UploadBatch(form, batchFile.InputStream, batchFile.FileName);
                if (!result.IsSuccess)
                {
                    MessageContainer.PushErrorMessage(result.ErrorMessage);
                    var model = batchUploadService.GetBatchUploadForEditByForm(form);
                    ViewBag.TriggerValidation = true;
                    return ViewBag(model);
                }

                return RedirectToAction("BatchDetail", new { id = result.BatchUploadId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToIndex();
            }
        }

        public ActionResult BatchDetail(long id)
        {
            try
            {
                var model = batchUploadService.GetBatchUploadDetail(id);
                var view = "BatchDetail";
                switch (model.BatchUpload.BatchUploadStateId)
                {
                    case BatchUploadStateEnum.InValidation:
                        view = "BatchValidation";
                        break;
                    case BatchUploadStateEnum.InMerging:
                        view = "BatchMerge";
                        break;
                }

                return View(view, model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToIndex();
            }
        }

        public ActionResult StudentBatchItemEdit(long id)
        {
            try
            {
                var model = batchUploadService.GetStudentBatchItemForEdit(id);
                return View(model);
            }
            catch(ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToIndex();
            }
        }

        [HttpPost]
        public ActionResult StudentBatchItemEdit(RegistrationStudentForm form, long id)
        {
            if (!ModelState.IsValid)
            {
                var model = batchUploadService.GetStudentBatchItemForEditByForm(form, id);
                return View(model);
            }

            try
            {
                var batchUploadId = batchUploadService.SaveBatchUploadItem(form, id);
                MessageContainer.PushMessage("Registration batch item was saved.");
                return RedirectToAction("BatchDetail", new { id = batchUploadId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToIndex();
            }
        }

        [HttpPost]
        public ActionResult BatchCompleteValidation(long id)
        {
            try
            {
                var batchStatusId = batchUploadService.CompleteValidation(id);
                if (batchStatusId == BatchUploadStateEnum.Complete)
                {
                    MessageContainer.PushMessage("Validation was completed and registration batch was processed.");
                }
                else
                {
                    MessageContainer.PushMessage("Validation of registrations batch upload was completed.");
                }

                return RedirectToAction("BatchDetail", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToIndex();
            }
        }

        [HttpPost]
        public ActionResult BatchCompleteMerge(long id, FormCollection formCollection)
        {
            try
            {
                var operationMap = batchUploadHelper.ExtractBatchUploadsOperationTypes(formCollection);
                batchUploadService.CompleteMerging(id, operationMap);
                MessageContainer.PushMessage("Merge was completed and registration batch was processed.");
                return RedirectToAction("BatchDetail", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToIndex();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("BatchDetail", new { id });
            }
        }

        [HttpPost]
        public ActionResult BatchDiscard(long id)
        {
            try
            {
                var registrationId = batchUploadService.DiscardBatch(id);
                MessageContainer.PushMessage("Registration batch upload was discarded.");

                if (!registrationId.HasValue)
                {
                    return RedirectToIndex();
                }

                return RedirectToAction("Registrations", "Class", new { id = registrationId.Value });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToIndex();
            }

        }

        #endregion

        #region private methods

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index", "Class");
        }

        #endregion
    }
}