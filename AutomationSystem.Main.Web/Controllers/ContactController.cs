using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Contacts.AppLogic;
using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;
using CorabeuControl.Helpers;
using System;
using System.Web.Mvc;
using CorabeuControl.Context;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    [HandleAdminError("ContactController")]
    [AuthorizeEntitle(Entitle.MainContacts)]
    public class ContactController : Controller
    {
        private readonly IContactAdministration contactAdministration = IocProvider.Get<IContactAdministration>();

        public IMessageContainer MessageContainer => new MessageContainer(Session);
        
        public ActionResult Index(ContactListFilter filter, bool search = false)
        {
           
            var model = contactAdministration.GetContactListPageModel(filter, search);
            if (model.Profiles.Count == 0)
            {
                return View("NoProfileMessage");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EnableContact(string email)
        {
            try
            {
                contactAdministration.RemoveContactFromBlackList(email);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
        }

        [HttpPost]
        public ActionResult DisableContact(string email)
        {
            try
            {
                contactAdministration.AddContactToBlackList(email);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
        }

        [HttpPost]
        public ActionResult CreateContactList(ContactListDefinition contactListDefinition)
        {
            try
            {
                var result = contactAdministration.CreateContactList(contactListDefinition);
                return RedirectToAction("Detail", new {id = result});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        [CorabeuContext]
        public ActionResult Detail(long id)
        {
            try
            {
                var result = contactAdministration.GetContactListDetail(id);
                return View(result);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult NotifyContacts(long id)
        {
            try
            {
                contactAdministration.NotifyContacts(id);
                MessageContainer.PushMessage("Contacts were notified");
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public ActionResult UpdateSenderId(ContactListSenderForm form)
        {
            try
            {
                contactAdministration.UpdateSenderId(form);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
        }
    }
}