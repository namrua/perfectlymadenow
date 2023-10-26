using System.Collections.Generic;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities;
using Entitle = AutomationSystem.Base.Contract.Identities.Models.Entitle;

namespace AutomationSystem.Main.Web.Filters
{
    /// <summary>
    /// Authorizes access to actions by any of the system entitles
    /// </summary>
    public class AuthorizeAnyEntitleAttribute : AuthorizeAttribute
    {

        private readonly HashSet<Entitle> entitles;

        // constructor
        public AuthorizeAnyEntitleAttribute(params Entitle[] entitles)
        {
            this.entitles = new HashSet<Entitle>(entitles);
        }

        // on authorization request
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var identity = filterContext?.HttpContext?.User?.Identity;
            if (identity == null)
                return;

            // checks whether entitle is granted
            var identityResolver = IocProvider.Get<IIdentityResolver>();
            if (!identityResolver.IsAnyEntitleGranted(entitles))
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