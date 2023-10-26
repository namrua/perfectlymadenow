using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion;
using AutomationSystem.Main.Web.Filters;
using CorabeuControl.Helpers;
using System;
using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Distance class template controller
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("DistanceClassTemplateController")]
    [AuthorizeEntitle(Entitle.MainDistanceClassTemplate)]
    public class DistanceClassTemplateController : Controller
    {
        private readonly IDistanceClassTemplateAdministration templateAdministration;
        private readonly IDistanceClassTemplateCompletionAdministration completionAdministration;

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        public DistanceClassTemplateController()
        {
            templateAdministration = IocProvider.Get<IDistanceClassTemplateAdministration>();
            completionAdministration = IocProvider.Get<IDistanceClassTemplateCompletionAdministration>();
        }

        // gets DistanceClassTemplate
        public ActionResult Index(DistanceClassTemplateFilter filter, bool search = false)
        {
            var model = templateAdministration.GetDistanceClassTemplatePageModel(filter, search);
            return View(model);
        }

        #region detail
        // gets DistanceClassTemplateDetail
        public ActionResult Detail(long id)
        {
            try
            {
                var model = templateAdministration.GetDistanceClassTemplateDetailById(id);
                return View(model);
            }
            catch(ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // gets new DistanceClassTemplate
        public ActionResult New()
        {
            var model = templateAdministration.GetNewDistanceClassTemplateForEdit();
            return View("Edit", model);
        }

        // gets DistanceClassTemplateForEdit
        public ActionResult Edit(long id)
        {
            try
            {
                var model = templateAdministration.GetDistanceClassTemplateForEditById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id });
            }
        }

        // saves DistanceClassTemplate
        [HttpPost]
        public ActionResult Edit(DistanceClassTemplateForm form)
        {
            var validationResult = templateAdministration.ValidateDistanceClassTemplateForm(form);
            if (!ModelState.IsValid && !validationResult.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = templateAdministration.GetDistanceClassTemplateForEditByForm(form);
                return View(model);
            }

            try
            {
                var id = templateAdministration.SaveDistanceClassTemplate(form);
                MessageContainer.PushMessage("Distance class template was saved.");
                return RedirectToAction("Detail", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { form.DistanceClassTemplateId });
            }
        }

        // approves DistanceClassTemplate
        [HttpPost]
        public ActionResult Approve(long id)
        {
            try
            {
                templateAdministration.ApproveDistanceClassTemplate(id);
                MessageContainer.PushMessage("Distance class template was approved. Distance classses were created.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id });
            }

            return RedirectToAction("Detail", new { id });
        }
        
        // deletes DistanceClassTemplate
        [HttpPost]
        public ActionResult Delete(long id)
        {
            templateAdministration.DeleteDistanceClassTemplate(id);
            MessageContainer.PushMessage("Distance class template was deleted.");
            return RedirectToHome();
        }
        #endregion

        #region completion
        // gets DistanceClassTemplateCompletionPageModel
        public ActionResult Completion(long id)
        {
            try
            {
                var model = completionAdministration.GetDistanceClassTemplateCompletionPageModel(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Completion", new { id });
            }
        }

        // completes distanceClassTemplate
        [HttpPost]
        public ActionResult Complete(long id)
        {
            try
            {
                completionAdministration.CompleteDistanceClassTemplate(id);
                MessageContainer.PushMessage("Distance class template completion has started. Refresh page to see current status.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Completion", new { id });
            }

            return RedirectToAction("Completion", new { id });
        }

        // gets DistanceClassTemplateForm
        public ActionResult CompletionSettings(long id)
        {
            try
            {
                var model = completionAdministration.GetDistanceClassTemplateCompletionFormById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        [HttpPost]
        public ActionResult CompletionSettings(DistanceClassTemplateCompletionForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            try
            {
                completionAdministration.SaveDistanceClassTemplateCompletionSettings(form);
                MessageContainer.PushMessage("Completion settings were saved.");
                return RedirectToAction("Completion", new { id = form.DistanceClassTemplateId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }
        #endregion

        #region private methods
        private ActionResult RedirectToHome()
        {
            return RedirectToAction("Index");
        }
        #endregion
    }
}