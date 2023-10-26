using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Controler for person administration
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("PersonController")]
    public class PersonController : Controller
    {
        // private components 
        private readonly IPersonAdministration personService = IocProvider.Get<IPersonAdministration>();

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        // list of persons
        [AuthorizeEntitle(Entitle.MainPersonsReadOnly)]
        public ActionResult Index(PersonFilter filter, bool search = false)
        {
            var model = personService.GetPersonListPageModel(filter, search);
            return View(model);
        }


        // shows detail of the person
        [AuthorizeEntitle(Entitle.MainPersonsReadOnly)]
        public ActionResult Detail(long id)
        {
            try
            {
                var model = personService.GetPersonDetail(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // creates new person
        [AuthorizeEntitle(Entitle.MainPersons)]
        public ActionResult New(long profileId)
        {
            try
            {
                var model = personService.GetNewPersonForEdit(profileId);
                return View("Edit", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // edits person    
        [AuthorizeEntitle(Entitle.MainPersons)]
        public ActionResult Edit(long id)
        {
            try
            {
                var model = personService.GetPersonForEdit(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // saves person
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainPersons)]
        public ActionResult Edit(PersonForm form)
        {            
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = personService.GetFormPersonForEdit(form);
                return View(model);
            }

            try
            {
                var personId = personService.SavePerson(form);
                MessageContainer.PushMessage("Person was saved.");
                return RedirectToAction("Detail", new {id = personId});
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // deletes person
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainPersons)]
        public ActionResult Delete(long id)
        {           
            personService.DeletePerson(id);
            MessageContainer.PushMessage("Person was deleted.");
            return RedirectToHome();
        }


        #region private methods

        // redirects to person index
        public ActionResult RedirectToHome()
        {
            return RedirectToAction("Index");
        }

        #endregion

    }

}