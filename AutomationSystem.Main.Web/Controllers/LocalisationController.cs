using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Main.Web.Models.Sessions;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using AutomationSystem.Shared.Contract.Localisation.AppLogic.Models;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Controler for localisation administration
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("LocalisationController")]
    [AuthorizeEntitle(Entitle.CoreLocalisation)]
    public class LocalisationController : Controller
    {

        // private components 
        private readonly ILocalisationAdministration localisationAdmin = IocProvider.Get<ILocalisationAdministration>();

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);



        // session access
        public LocalisationSession LocalisationSession
        {
            get
            {               
                if (Session == null) return new LocalisationSession();
                var result = Session["LocalisationController.LocalisationSession"] as LocalisationSession ?? new LocalisationSession();               
                Session["LocalisationController.LocalisationSession"] = result;
                return result;
            }
        }


        #region app localisation

        // list of languages
        public ActionResult Index()
        {
            var model = localisationAdmin.GetLanguageLocalisationSummary();
            ClearSession();
            return View(model);
        }


        // list of app localised items by language
        public ActionResult List(LanguageEnum langId)
        {
            try
            {
                var model = localisationAdmin.GetAppLocalisationList(langId);
                UpdateAppSession(model.Items.Select(x => x.OriginAppLocalisationId));
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }
       

        // editing of app localisation
        public ActionResult Edit(long id, LanguageEnum langId)
        {
            try
            {
                var model = localisationAdmin.GetAppLocalisatonForEdit(id, langId);
                SetAppsOrderIds(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // saves app localisation
        [HttpPost]
        public ActionResult Edit(AppLocalisationForm form)
        {
            try
            {
                var validationResult = localisationAdmin.ValidateAppLocalisation(form);               
                if (!validationResult.IsValid || !ModelState.IsValid)
                {
                    var model = localisationAdmin.GetFormAppLocalisationForEdit(form, validationResult);
                    SetAppsOrderIds(form.AppLocalisationOriginId);
                    return View(model);
                }

                // saves app localisation and reloads edit form
                localisationAdmin.SaveAppLocalisation(form);
                MessageContainer.PushMessage("Application localisation was saved.");
                var newModel = localisationAdmin.GetAppLocalisatonForEdit(form.AppLocalisationOriginId, form.LanguageId);
                SetAppsOrderIds(form.AppLocalisationOriginId);
                return View(newModel);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // validates app localisation
        [HttpPost]
        public ActionResult Validate(AppLocalisationForm form)
        {
            try
            {
                var validationResult = localisationAdmin.ValidateAppLocalisation(form);
                if (!validationResult.IsValid || !ModelState.IsValid)
                    MessageContainer.PushErrorMessage("Application localisation text is not valid.");
                else
                    MessageContainer.PushMessage("Application localisation text is valid.");
                var model = localisationAdmin.GetFormAppLocalisationForEdit(form, validationResult);
                SetAppsOrderIds(form.AppLocalisationOriginId);
                return View("Edit", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        #endregion


        #region enum localisation

        // enum type list
        public ActionResult EnumTypeList(LanguageEnum langId)
        {
            try
            {
                var model = localisationAdmin.GetEnumTypeList(langId);
                ClearSession();
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // enum item list
        public ActionResult EnumList(LanguageEnum langId, EnumTypeEnum typeId)
        {
            try
            {
                var model = localisationAdmin.GetEnumLocalisationList(langId, typeId);
                UpdateEnumSession(model.Items.Select(x => x.ItemId));
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // editing of enum item
        public ActionResult EnumEdit(int id, LanguageEnum langId, EnumTypeEnum typeId)
        {
            try
            {
                var model = localisationAdmin.GetEnumLocalisationForEdit(langId, typeId, id);
                SetEnumOrderIds(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }


        // saves enum item
        [HttpPost]
        public ActionResult EnumEdit(EnumLocalisationForm form)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var model = localisationAdmin.GetFormEnumLocalisationForEdit(form);
                    SetEnumOrderIds(form.ItemId);
                    return View(model);
                }

                // saves enum item
                localisationAdmin.SaveEnumLocalisation(form);
                MessageContainer.PushMessage("Enum localisation item was saved.");
                var newModel = localisationAdmin.GetEnumLocalisationForEdit(form.LanguageId, form.EnumTypeId, form.ItemId);
                SetEnumOrderIds(form.ItemId);
                return View(newModel);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        #endregion


        #region session operations

        // clears session
        private void ClearSession()
        {
            LocalisationSession.AppLocalisationOrder.Clear();
            LocalisationSession.EnumLocalisationOrder.Clear();
        }

        // updates app session
        private void UpdateAppSession(IEnumerable<long> originAppLocalisationIds)
        {
            LocalisationSession.AppLocalisationOrder = originAppLocalisationIds.ToList();
        }

        // sets predecesor and successor ids
        private void SetAppsOrderIds(long originId)
        {
            // sets predecessor and successor
            var order = LocalisationSession.AppLocalisationOrder;
            var indexOfOrigin = order.IndexOf(originId);
            if (indexOfOrigin > 0)
                ViewBag.PredecessorId = order[indexOfOrigin - 1];
            if (indexOfOrigin >= 0 && indexOfOrigin < order.Count - 1)
                ViewBag.SuccessorId = order[indexOfOrigin + 1];
        }

        // updates app session
        private void UpdateEnumSession(IEnumerable<int> originAppLocalisationIds)
        {
            LocalisationSession.EnumLocalisationOrder = originAppLocalisationIds.ToList();
        }

        // sets predecesor and successor ids
        private void SetEnumOrderIds(int itemId)
        {
            // sets predecessor and successor
            var order = LocalisationSession.EnumLocalisationOrder;
            var indexOfOrigin = order.IndexOf(itemId);
            if (indexOfOrigin > 0)
                ViewBag.PredecessorId = order[indexOfOrigin - 1];
            if (indexOfOrigin >= 0 && indexOfOrigin < order.Count - 1)
                ViewBag.SuccessorId = order[indexOfOrigin + 1];
        }

        #endregion

    }

}