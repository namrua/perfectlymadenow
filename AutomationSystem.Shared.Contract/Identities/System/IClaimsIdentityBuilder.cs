using System.Security.Claims;
using System.Threading.Tasks;
using AutomationSystem.Shared.Contract.Identities.Data.Models;

namespace AutomationSystem.Shared.Contract.Identities.System
{
    /// <summary>
    /// Builds ClaimsIdentities
    /// </summary>
    public interface IClaimsIdentityBuilder
    {
        // enriches ClaimsIdentity by ApplicationUser and other extensions (UserGroups)
        Task<ClaimsIdentity> EnrichUserIdentityAsync(ClaimsIdentity userIdentity, ApplicationUser user);

        // builds default identity
        ClaimsIdentity BuildDefaultIdentity();
    }

}
