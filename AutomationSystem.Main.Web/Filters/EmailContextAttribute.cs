using System.Web.Mvc;
using AutomationSystem.Main.Web.Helpers;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using CorabeuControl.Context;
using Resources;

namespace AutomationSystem.Main.Web.Filters
{
    /// <summary>
    /// Initializes context manager and email context
    /// </summary>
    public class EmailContextAttribute : CorabeuContextAttribute
    {                

        // creates and initializes context manager
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var viewBag = filterContext.Controller.ViewBag;
            IContextManager contextManager = ContextHelper.GetContextManager(viewBag);

            // initializes object
            var emailContext = contextManager.GetCustomContext<EmailTemplateTextContext>();
            if (emailContext == null)
            {
                emailContext = new EmailTemplateTextContext();
                emailContext.AddTitleAndMenuItem(CommonResources.MenuCoreEmailTemplates, MenuItemId.CoreEmailTemplates);               
                contextManager.SetCustomContext(emailContext);
            }
            viewBag.EmailContext = emailContext;
            viewBag.Title = emailContext.Title;
            viewBag.ActiveMainMenuItemId = emailContext.ActiveMainMenuItemId;           
        }

    }

}