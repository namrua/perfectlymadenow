using System.Security.Principal;
using System.Threading;
using AutomationSystem.Shared.Contract.Identities.System;

namespace AutomationSystem.Shared.Core.Identities.System
{
    /// <summary>
    /// Provides current IIdentity
    /// </summary>
    public class IdentityProvider : IIdentityProvider
    {

        private readonly IClaimsIdentityBuilder identityBuilder;


        // constructor
        public IdentityProvider(IClaimsIdentityBuilder identityBuilder)
        {
            this.identityBuilder = identityBuilder;
        }


        // gets current identity
        public IIdentity GetCurrentIdentity()
        {
            var result = Thread.CurrentPrincipal?.Identity;
            return result ?? identityBuilder.BuildDefaultIdentity();
        }

    }

}
