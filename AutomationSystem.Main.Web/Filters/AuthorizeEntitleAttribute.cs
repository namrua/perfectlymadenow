using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities;
using Entitle = AutomationSystem.Base.Contract.Identities.Models.Entitle;

namespace AutomationSystem.Main.Web.Filters
{
    // todo: make abstract Authorize attribute and refactor when extending

    /// <summary>
    /// Authorizes access to actions by the system entitle (optimized for one entitle)
    /// </summary>
    public class AuthorizeEntitleAttribute : AuthorizeAttribute
    {

        private readonly Entitle entitle;

        // constructor
        public AuthorizeEntitleAttribute(Entitle entitle)
        {
            this.entitle = entitle;
        }

        // on authorization request
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var identity = filterContext?.HttpContext?.User?.Identity;
            if (identity == null)
                return;

            // checks whether entitle is granted
            var identityResolver = IocProvider.Get<IIdentityResolver>();
            if (!identityResolver.IsEntitleGranted(entitle))
                HandleUnauthorizedRequest(filterContext);
        }


        // on authentication challange
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                var url = urlHelper.Action("AccessDenied", "Administration");
                filterContext.Result = new RedirectResult(url);
            }
        }

    }

}