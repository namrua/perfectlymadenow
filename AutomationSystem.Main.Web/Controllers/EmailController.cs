using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Extensions;
using CorabeuControl.Context;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Provides email administration and support    
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("EmailController")]
    public class EmailController : Controller
    {

        // private components
        private readonly IEmailTemplateAdministration emailTemplateAdministration = IocProvider.Get<IEmailTemplateAdministration>();
        private readonly IEmailTemplateTextAdministration emailTemplateTextAdministration = IocProvider.Get<IEmailTemplateTextAdministration>();

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);


        #region lists and detail

        // list of email types
        [AuthorizeEntitle(Entitle.CoreEmailTemplates)]
        public ActionResult Index()
        {
            var model = emailTemplateAdministration.GetSystemEmailTypeSummary();
            return View(model);

        }


        // list of email templates by type
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult List(EmailTypeEnum id, EntityTypeEnum? entityTypeId, long? entityId)
        {
            try
            {
                var model = emailTemplateAdministration.GetEmailTemplateList(id, new EmailTemplateEntityId(entityTypeId, entityId));
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // detail of email template
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult Detail(long id)
        {
            try
            {
                var model = emailTemplateAdministration.GetEmailTemplateDetail(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        #endregion


        #region commands

        // send test email
        [HttpPost]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult SendTestEmail(long id)
        {
            try
            {
                var emailInfo = new EmailTestSendInfo(User.Identity.GetEmail(), id);
                emailTemplateTextAdministration.SendTestEmail(emailInfo);
                MessageContainer.PushMessage("Test email was sent.");
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
                return RedirectToAction("Detail", new { id });
            }
        }


        // resets email template
        [HttpPost]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult ResetEmailTemplate(long id)
        {
            try
            {
                emailTemplateAdministration.ResetEmailTemplate(id);
                MessageContainer.PushMessage("Email template was reset to default values.");
                return RedirectToAction("Detail", new {id});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        #endregion


        #region metadata editation

        // new email metadata
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult New(EmailTypeEnum emailTypeId, LanguageEnum langId, EntityTypeEnum? entityTypeId, long? entityId)
        {
            try
            {
                var model = emailTemplateAdministration.GetNewEmailTemplateMetadataForEdit(emailTypeId, langId, new EmailTemplateEntityId(entityTypeId, entityId));
                return View("EditMetadata", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("List", new { id = emailTypeId, entityTypeId, entityId });
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("List", new { id = emailTypeId, entityTypeId, entityId });
            }
        }


        // editataion of metadata of existing template
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult EditMetadata(long id)
        {
            try
            {
                var model = emailTemplateAdministration.GetEmailTemplateMetadataForEditById(id);
                return View("EditMetadata", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // saves email metadata
        [HttpPost]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult EditMetadata(EmailTemplateMetadataForm form)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var model = emailTemplateAdministration.GetEmailTemplateMetadataForEditByForm(form);
                    ViewBag.TriggerValidation = true;
                    return View("EditMetadata", model);
                }

                var id = emailTemplateAdministration.SaveEmailTemplateMetadata(form);
                MessageContainer.PushMessage("Metadata of email template was saved.");
                if (form.EmailTemplateId == 0)
                    return RedirectToAction("EditText", new { id });
                else 
                    return RedirectToAction("Detail", new { id });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("List", new { id = form.EmailTypeId, entityTypeId = form.EntityTypeId, entityId = form.EntityId });
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("List", new { id = form.EmailTypeId, entityTypeId = form.EntityTypeId, entityId = form.EntityId });
            }
        }


        #endregion


        #region text editation

        // edits email texts
        [EmailContext]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult EditText(long id)
        {
            IContextManager cm = ContextHelper.GetContextManager(ViewBag);
            try
            {
                var model = emailTemplateTextAdministration.GetEmailTemplateTextForEditById(id);
                model.Context = cm.GetCustomContext<EmailTemplateTextContext>();
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.ToString());
                return RedirectToAction("Index");
            }
        }


        // saves email texts
        [HttpPost]
        [EmailContext]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult EditText(EmailTemplateTextForm form)
        {
            IContextManager cm = ContextHelper.GetContextManager(ViewBag);
            try
            {
                // validates and saves email texts
                var validationResult = emailTemplateTextAdministration.ValidateEmailTemplateText(form);
                if (!validationResult.IsValid || !ModelState.IsValid)
                {
                    var model = emailTemplateTextAdministration.GetEmailTemplateTextForEditByForm(form, validationResult);
                    model.Context = cm.GetCustomContext<EmailTemplateTextContext>();
                    return View("EditText", model);
                }

                emailTemplateTextAdministration.SaveEmailTemplateText(form, true);
                MessageContainer.PushMessage("Texts of email template was saved.");

                // returns back               
                return Redirect(cm.GetBackUrl(Url.Action("Detail", new {id = form.EmailTemplateId})));
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // validates email template text
        [HttpPost]
        [EmailContext]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult ValidateText(EmailTemplateTextForm form)
        {
            IContextManager cm = ContextHelper.GetContextManager(ViewBag);
            try
            {
                // validates result
                var validationResult = emailTemplateTextAdministration.ValidateEmailTemplateText(form);
                if (!validationResult.IsValid || !ModelState.IsValid)
                    MessageContainer.PushErrorMessage("Texts of email template are not valid.");
                else
                    MessageContainer.PushMessage("Texts of email template are valid.");

                // gets model
                var model = emailTemplateTextAdministration.GetEmailTemplateTextForEditByForm(form, validationResult);
                model.Context = cm.GetCustomContext<EmailTemplateTextContext>();
                return View("EditText", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // sends test email
        [HttpPost]
        [EmailContext]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult SendTestText(EmailTemplateTextForm form)
        {
            IContextManager cm = ContextHelper.GetContextManager(ViewBag);
            var context = cm.GetCustomContext<EmailTemplateTextContext>();
            try
            {
                var validationResult = emailTemplateTextAdministration.ValidateEmailTemplateText(form);
                if (!validationResult.IsValid || !ModelState.IsValid)
                    MessageContainer.PushErrorMessage("Test email cannot be sent. Email text is not valid.");
                else
                {
                    var emailInfo = new EmailTestSendInfo(User.Identity.GetEmail(), form.EmailTemplateId)
                    {
                        CurrentSubject = form.Subject,
                        CurrentText = form.Text,
                        EmailEntityId = new EmailEntityId(
                            context.TestEmailEntityTypeId ?? EntityTypeEnum.CoreEmail,
                            context.TestEmailEntityId ?? 0),
                        ParameterEntityId = new EmailEntityId(
                            context.ParameterEntityTypeId ?? EntityTypeEnum.CoreEmail,
                            context.ParameterEntityId ?? 0)
                    };
                    emailTemplateTextAdministration.SendTestEmail(emailInfo, allowInvalidTemplate: true);
                    MessageContainer.PushMessage("Test email was sent.");
                }
                var model = emailTemplateTextAdministration.GetEmailTemplateTextForEditByForm(form, validationResult);
                model.Context = context;
                return View("EditText", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }



        // resets email text
        [HttpPost]
        [EmailContext]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult ResetEmailText(long id)
        {
            IContextManager cm = ContextHelper.GetContextManager(ViewBag);
            try
            {
                var model = emailTemplateTextAdministration.GetResetEmailTemplateTextForEdit(id);
                model.Context = cm.GetCustomContext<EmailTemplateTextContext>();
                MessageContainer.PushMessage("Texts of email template was reset to default values.");
                return View("EditText", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        #endregion


        #region other email integration

        // email detail
        [EmailContext]
        [AuthorizeEntitle(Entitle.CoreEmailTemplatesIntegration)]
        public ActionResult EmailDetail(long id)
        {
            try
            {
                var model = emailTemplateTextAdministration.GetEmailDetail(id);
                return View(model);
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