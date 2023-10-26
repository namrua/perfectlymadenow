using System.Web.Mvc;
using AutomationSystem.Main.Web.Helpers;
using CorabeuControl.Context;
using Resources;

namespace AutomationSystem.Main.Web.Filters
{
    /// <summary>
    /// Initializes context manager and former context
    /// </summary>
    public class FormerContextAttribute : CorabeuContextAttribute
    {

        // private fields
        private readonly FormerBasePages basePages;        

        // constructor
        public FormerContextAttribute(FormerBasePages basePages)
        {
            this.basePages = basePages;
        }


        // creates and initializes context manager
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var viewBag = filterContext.Controller.ViewBag;
            IContextManager contextManager = ContextHelper.GetContextManager(viewBag);

            // initializes object
            var formerContext = contextManager.GetCustomContext<FormerContext>();
            if (formerContext == null)
            {
                formerContext = new FormerContext(basePages);
                contextManager.SetCustomContext(formerContext);
            }
            viewBag.FormerContext = formerContext;
            switch (formerContext.Base)
            {
                case FormerBasePages.Classes:
                    viewBag.Title = CommonResources.MenuCoordinatorFormerClasses;
                    viewBag.ActiveMainMenuItemId = MenuItemId.CoordinatorFormerClasses;
                    break;

                case FormerBasePages.Students:
                    viewBag.Title = CommonResources.MenuCoordinatorFormerStudents;
                    viewBag.ActiveMainMenuItemId = MenuItemId.CoordinatorFormerStudents;
                    break;

                case FormerBasePages.Registrations:
                    viewBag.Title = CommonResources.TitleCoordinatedRegistrations;
                    viewBag.ActiveMainMenuItemId = MenuItemId.CoordinatorClasses;                  
                    break;
            }
        }

    }

}