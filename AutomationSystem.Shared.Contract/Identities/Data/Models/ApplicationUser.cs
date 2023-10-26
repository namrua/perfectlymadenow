using System.Security.Claims;
using System.Threading.Tasks;
using AutomationSystem.Shared.Contract.Identities.System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutomationSystem.Shared.Contract.Identities.Data.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim>
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, IClaimsIdentityBuilder builder)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // enriches claims by builder
            userIdentity = await builder.EnrichUserIdentityAsync(userIdentity, this);

            // returns user identity
            return userIdentity;
        }
    }

}
