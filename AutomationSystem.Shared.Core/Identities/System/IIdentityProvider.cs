using System.Security.Principal;

namespace AutomationSystem.Shared.Core.Identities.System
{
    /// <summary>
    /// Provides current IIdentity
    /// </summary>
    public interface IIdentityProvider
    {

        // gets current identity
        IIdentity GetCurrentIdentity();

    }

}
