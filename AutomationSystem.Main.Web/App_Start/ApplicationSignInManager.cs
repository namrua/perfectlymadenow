using System.Security.Claims;
using System.Threading.Tasks;
using AutomationSystem.Shared.Contract.Identities.Data.Models;
using AutomationSystem.Shared.Contract.Identities.System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace AutomationSystem.Main.Web
{
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>
    {

        private readonly IClaimsIdentityBuilder builder = IocProvider.Get<IClaimsIdentityBuilder>();

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }      


        // claims identity is created here
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager, builder);
        }


        // factory method
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

    }

}