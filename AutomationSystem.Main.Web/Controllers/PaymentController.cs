using CorabeuControl.Helpers;
using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Payment.AppLogic;
using AutomationSystem.Main.Contract.Payment.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    [HandleAdminError("PaymentController")]
    [AuthorizeEntitle(Entitle.CorePayPalKeys)]
    public class PaymentController : Controller
    {

        // private components
        private readonly IMainPaymentAdministration paymentAdministration = IocProvider.Get<IMainPaymentAdministration>();

        // gets message container 
        public IMessageContainer MessageContainer => new MessageContainer(Session);

    
        // main page
        public ActionResult Index(MainPayPalKeyFilter filter, bool search = false)
        {
            var model = paymentAdministration.GetMainPayPalKeyListPageModel(filter, search);
            return View(model);
        }

        // creates paypalKey 
        public ActionResult New(long profileId)
        {
            try
            {
                var model = paymentAdministration.GetNewPayPalKeyForEdit(profileId);
                return View("Edit", model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }


        // edit paypal key
        public ActionResult Edit(long id)
        {
            try
            {
                var model = paymentAdministration.GetPayPalKeyForEditById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }          
        }


        // edit paypal key
        [HttpPost]
        public ActionResult Edit(MainPayPalKeyForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TriggerValidation = true;
                var model = paymentAdministration.GetPayPalKeyForEditByForm(form);
                return View(model);
            }

            try
            {
                paymentAdministration.SavePayPalKey(form);
                MessageContainer.PushMessage("PayPal API Key was saved.");
            }
            catch(ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
            }
            return RedirectToHome();
        }

        // deletes paypal key
        [HttpPost]
        public ActionResult Delete(long id)
        {
            try
            {

                paymentAdministration.DeletePayPalKey(id);
                MessageContainer.PushMessage("PayPal API Key was deleted.");
                return RedirectToHome();
            }
            catch (InvalidOperationException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Detail", new { id });
            } 
        }


        #region private methods

        // redirects to controllers homepage
        private ActionResult RedirectToHome()
        {
            return RedirectToAction("Index");
        }

        #endregion

    }

}