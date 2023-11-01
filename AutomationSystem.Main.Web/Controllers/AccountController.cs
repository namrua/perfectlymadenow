using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutomationSystem.Shared.Contract.Identities.Data;
using AutomationSystem.Shared.Contract.Localisation.System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PerfectlyMadeInc.WebEx.Authentication;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IAppUserDatabaseLayer _userDbLayer;
        private IAuthentication _authentication;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthentication authentication)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _authentication = authentication;
        }

        // changes language
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangeLanguage(string languageCode, string returnUrl)
        {
            if (languageCode != null)
                IocProvider.Get<ILocalisationService>().SetLanguage(languageCode);
            return RedirectToLocal(returnUrl);
        }



        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public IAppUserDatabaseLayer UserDbLayer
        {
            get => _userDbLayer ?? HttpContext.GetOwinContext().Get<IAppUserDatabaseLayer>();
            private set => _userDbLayer = value;
        }


        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var auth = new AuthenticationService();
            var clientId = "C92ebdee82575df4a62817c095a41e306687d1cafbaee58823a2f25d79bd546bc";
            var responseType = "response_type";
            var redirectUri = "https://google.com/";
            var scope = "spark-admin:broadworks_enterprises_write";
            var state = "set_state_here";
            var authCode = auth.GetAuthenticationCode(clientId, responseType, scope, state, redirectUri);
            // Request a redirect to the external login provider
            //return new ChallengeResult("Google", Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            return null;
        }
            

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var signInResult = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (signInResult)
            {
                // user has user login for google provider
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);        
                    
                // user is not paired yet
                case SignInStatus.Failure:

                    // finds out wheter user is in the DB, otherwise runs login failure
                    var user = await UserDbLayer.GetUserByGoogleAccount(loginInfo.Email);
                    if (user == null)
                        return View("ExternalLoginFailure");
                    
                    // adds login for user
                    var addloginResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (!addloginResult.Succeeded)
                        return View("ExternalLoginFailure");                

                    // signs in user and redirect to returnUrl
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToLocal(returnUrl);
                    
                default:
                    return View("ExternalLoginFailure");
            }
        }
       

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToHome();
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }



        // dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }


        #region private

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }

}