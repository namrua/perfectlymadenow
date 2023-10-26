using AutomationSystem.Main.Model;
using CorabeuControl.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.PriceLists.AppLogic;
using AutomationSystem.Main.Contract.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    [HandleAdminError("PriceListController")]
    [AuthorizeEntitle(Entitle.MainPriceLists)]
    public class PriceListController : Controller
    {

        // private components
        private readonly IPriceListService priceListService;

        // gets message container 
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        // constructor
        public PriceListController()
        {
            priceListService = IocProvider.Get<IPriceListService>();
        }

        // gets main page
        public ActionResult Index(List<PriceList> list)
        {
            var result = priceListService.GetPriceListMainPageModel();
            return View(result);
        }

        // creates paypalKey 
        public ActionResult New(PriceListTypeEnum id)
        {
            var model = priceListService.GetNewPriceListForEdit(id);
            return View("Edit", model);
        }

        // creates paypalKey 
        public ActionResult Detail(long id)
        {
            try
            {
                var model = priceListService.GetPriceListDetailById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index");
            }
        }

        // edit paypal key
        public ActionResult Edit(long id)
        {
            try
            {
                var model = priceListService.GetPriceListForEditById(id);
                return View("Edit", model);
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


        // edit paypal key
        [HttpPost]
        public ActionResult Edit(PriceListForm form)
        {
            if (!ModelState.IsValid || !priceListService.ValidatePriceList(form))
            {
                ViewBag.TriggerValidation = true;
                var model = priceListService.GetPriceListForEditByForm(form);
                return View("Edit", model);
            }

            try
            {
                var priceListId = priceListService.SavePriceList(form);
                MessageContainer.PushMessage("Price list was saved.");
                return RedirectToAction("Detail", new { id = priceListId });
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id = form.PriceListId });
            }

            return RedirectToAction("Index");
        }

        // approve price list
        [HttpPost]
        public ActionResult Approve(long id)
        {
            try
            {
                priceListService.ApprovePriceList(id);
                MessageContainer.PushMessage("Price list was approved.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id } );
            }

            return RedirectToAction("Index");
        }

        // discard price list
        [HttpPost]
        public ActionResult Discard(long id)
        {
            try
            {
                priceListService.DiscardPriceList(id);
            MessageContainer.PushMessage("Price list was discarded.");
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id });
            }

            return RedirectToAction("Index");
        }


        // discard price list
        [HttpPost]
        public ActionResult Delete(long id)
        {
            try
            {
                priceListService.DeletePriceList(id);
                MessageContainer.PushMessage("Price list was deleted.");
            }          
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id });
            }

            return RedirectToAction("Index");
        }


    }

}