using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Main.Web.Helpers.BatchUploads;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using CorabeuControl.Context;
using CorabeuControl.Helpers;
using System;
using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Controler for former student and classes administration
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("FormerController")]
    public class FormerController : Controller
    {
        // private components
        private readonly IFormerClassAdministration formerClassAdministration = IocProvider.Get<IFormerClassAdministration>();
        private readonly IFormerStudentAdministration formerStudentAdministration = IocProvider.Get<IFormerStudentAdministration>();
        private readonly IFormerStudentBatchUploadAdministration batchUploadService = IocProvider.Get<IFormerStudentBatchUploadAdministration>();
        private readonly IBatchUploadHelper batchUploadHelper = IocProvider.Get<IBatchUploadHelper>();
        
        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);


        #region classes

        // shows list of classes
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClassesReadOnly)]
        public ActionResult Index(FormerClassFilter filter, bool search = false)
        {
            var model = formerClassAdministration.GetFormerClassesForList(filter, search);
            return View(model);
        }

        // shows student detail
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClassesReadOnly)]
        public ActionResult ClassDetail(long id)
        {
            try
            {
                var model = formerClassAdministration.GetFormerClassById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // shows student detail
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClassesReadOnly)]
        public ActionResult ClassStudents(long id)
        {
            try
            {
                var model = formerClassAdministration.GetFormerClassStudents(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // opens new class form
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult ClassNew()
        {
            var model = formerClassAdministration.GetNewFormerClassForEdit();
            return View("ClassEdit", model);
        }

        // edits former class 
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult ClassEdit(long id)
        {
            try
            {
                var model = formerClassAdministration.GetFormerClassForEdit(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // saves former class
        [HttpPost]
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult ClassEdit(FormerClassForm form)
        {
            var isValid = formerClassAdministration.ValidateFormerClassForm(form);
            if (!ModelState.IsValid || !isValid)
            {
                ViewBag.TriggerValidation = true;
                var model = formerClassAdministration.GetFormFormerClassForEdit(form);
                return View(model);
            }

            try
            {
                var id = formerClassAdministration.SaveFormerClass(form);
                MessageContainer.PushMessage("Former class was saved.");

                // back redirection
                IContextManager cm = ContextHelper.GetContextManager(ViewBag);
                var classDetailUrl = Url.Action("ClassDetail", new {id, context = cm.Get() });
                return Redirect(form.FormerClassId == 0 
                    ? classDetailUrl : 
                    cm.GetBackUrl(classDetailUrl));
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // deletes former class
        [HttpPost]
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult ClassDelete(long id)
        {
            try
            {
                formerClassAdministration.DeleteFormerClass(id);
                MessageContainer.PushMessage("Former class was deleted.");

                // back redirection
                IContextManager cm = ContextHelper.GetContextManager(ViewBag);               
                return Redirect(cm.GetBackUrl(Url.Action("Index", new { context = cm.Get() })));               
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("ClassDetail", new { id });
            }
        }

        #endregion


        #region students

        // shows list of students
        [FormerContext(FormerBasePages.Students)]
        [AuthorizeEntitle(Entitle.MainFormerClassesReadOnly)]
        public ActionResult Student(FormerStudentFilter filter, ClassTypeEnum? classTypeId, bool search = false)
        {
            filter.Class.ClassTypeId = filter.Class.ClassTypeId ?? classTypeId;                 // shortcut for picking
            var model = formerStudentAdministration.GetFormerStudentsForList(filter, search);
            return View(model);
        }


        // shows student detail
        [FormerContext(FormerBasePages.Students)]
        [AuthorizeEntitle(Entitle.MainFormerClassesReadOnly)]
        public ActionResult StudentDetail(long id)
        {
            try
            {
                var model = formerStudentAdministration.GetFormerStudentById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Student");
            }
        }


        // creates new former student
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentNew(long formerClassId)
        {
            try
            {
                var model = formerStudentAdministration.GetNewFormerStudentForEdit(formerClassId);
                return View("StudentEdit", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Student");
            }
        }

        // edits former student
        [FormerContext(FormerBasePages.Students)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentEdit(long id)
        {
            try
            {
                var model = formerStudentAdministration.GetFormerStudentForEdit(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Student");
            }
        }

        // saves former student
        [HttpPost]
        [FormerContext(FormerBasePages.Students)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentEdit(FormerStudentForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = formerStudentAdministration.GetFormFormerStudentForEdit(form);
                return View(model);
            }

            try
            {
                var id = formerStudentAdministration.SaveFormerStudent(form);
                MessageContainer.PushMessage("Former student was saved.");

                // back redirection
                IContextManager cm = ContextHelper.GetContextManager(ViewBag);
                var studentDetailUrl = Url.Action("StudentDetail", new { id, context = cm.Get() });
                return Redirect(form.FormerStudentId == 0
                    ? studentDetailUrl :
                    cm.GetBackUrl(studentDetailUrl));                
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Student");
            }            
        }

        // deletes person
        [HttpPost]
        [FormerContext(FormerBasePages.Students)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentDelete(long id)
        {
            try
            {
                formerStudentAdministration.DeleteFormerStudent(id);
                MessageContainer.PushMessage("Former student was deleted.");

                // back redirection
                IContextManager cm = ContextHelper.GetContextManager(ViewBag);                
                return Redirect(cm.GetBackUrl(Url.Action("Student", new { context = cm.Get() })));               
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("StudentDetail", new {id});
            }
        }

        #endregion


        #region batch upload

        // list of users
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentUpload(long id)
        {
            try
            {
                var model = batchUploadService.GetFormerStudentUploadPageModel(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // batch upload edit
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentUploadNew(long id)
        {
            try
            {
                var model = batchUploadService.GetNewBatchUploadForEdit(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // uploading batch file and file info
        [HttpPost]
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentUploadNew(BatchUploadForm form)
        {
            try
            {
                var batchFile = Request.Files["BatchFile"];               
                if (!ModelState.IsValid || batchFile == null || batchFile.ContentLength == 0)
                {
                    var model = batchUploadService.GetFormBatchUploadForEdit(form);
                    ViewBag.TriggerValidation = true;
                    return View(model);
                }                

                // process batch               
                var result = batchUploadService.UploadBatch(form, batchFile.InputStream, batchFile.FileName);
                if (!result.IsSuccess)
                {
                    MessageContainer.PushErrorMessage(result.ErrorMessage);
                    var model = batchUploadService.GetFormBatchUploadForEdit(form);
                    ViewBag.TriggerValidation = true;
                    return View(model);
                }

                // back redirection
                IContextManager cm = ContextHelper.GetContextManager(ViewBag);               
                return RedirectToAction("StudentBatchDetail", new { id = result.BatchUploadId, context = cm.Get()});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }           
        }


        // shows batch detail
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentBatchDetail(long id)
        {
            try
            {
                var model = batchUploadService.GetBatchUploadDetail(id);
                var view = "StudentBatchDetail";
                switch (model.BatchUpload.BatchUploadStateId)
                {                   
                    case BatchUploadStateEnum.InValidation:
                        view = "StudentBatchValidation";
                        break;
                    case BatchUploadStateEnum.InMerging:
                        view = "StudentBatchMerge";
                        break;
                }
                return View(view, model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // opens student batch item for edit
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentBatchItemEdit(long id)
        {
            try
            {
                var model = batchUploadService.GetFormerStudentBatchItemForEdit(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // saves student batch item
        [HttpPost]
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentBatchItemEdit(FormerStudentForm form, long id)
        {
            if (!ModelState.IsValid)
            {
                var model = batchUploadService.GetFormFormerStudentBatchItemForEdit(form, id);
                return View(model);
            }

            try
            {
                var batchUploadId = batchUploadService.SaveBatchUploadItem(form, id);
                MessageContainer.PushMessage("Former student batch item was saved.");
                IContextManager cm = ContextHelper.GetContextManager(ViewBag);
                var redirectUrl = Url.Action("StudentBatchDetail", new { id = batchUploadId });
                return Redirect(cm.GetBackUrl(redirectUrl));
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }        
        }


        // completes batch validation
        [HttpPost]
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentBatchCompleteValidation(long id)
        {
            try
            {
                var batchStatusId = batchUploadService.CompleteValidation(id);
                if (batchStatusId == BatchUploadStateEnum.Complete)
                    MessageContainer.PushMessage("Validation was completed and former student batch was processed.");
                else 
                    MessageContainer.PushMessage("Validation of former students batch upload was completed.");
                IContextManager cm = ContextHelper.GetContextManager(ViewBag);
                return RedirectToAction("StudentBatchDetail", new { id, context = cm.Get() });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // completes batch merging
        [HttpPost]
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentBatchCompleteMerge(long id, FormCollection formCollection)
        {
            IContextManager cm = ContextHelper.GetContextManager(ViewBag);
            try
            {
                var operationMap = batchUploadHelper.ExtractBatchUploadsOperationTypes(formCollection);
                batchUploadService.CompleteMerging(id, operationMap);
                MessageContainer.PushMessage("Merg was completed and former student batch was processed.");                
                return RedirectToAction("StudentBatchDetail", new {id, context = cm.Get() });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("StudentBatchDetail", new { id, context = cm.Get() });
            }
        }


        // discards batch upload
        [HttpPost]
        [FormerContext(FormerBasePages.Classes)]
        [AuthorizeEntitle(Entitle.MainFormerClasses)]
        public ActionResult StudentBatchDiscard(long id)
        {
            try
            {
                var formerClassId = batchUploadService.Discard(id);
                MessageContainer.PushMessage("Former students batch upload was discarded.");

                if (!formerClassId.HasValue)
                    return RedirectToAction("Index");

                IContextManager cm = ContextHelper.GetContextManager(ViewBag);
                var redirectUrl = Url.Action("StudentUpload", new {id = formerClassId.Value });
                return Redirect(cm.GetBackUrl(redirectUrl));
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }
        
        #endregion      

    }

}