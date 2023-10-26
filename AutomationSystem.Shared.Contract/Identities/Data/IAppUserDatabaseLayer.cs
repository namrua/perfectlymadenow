using System.Threading.Tasks;
using AutomationSystem.Shared.Contract.Identities.Data.Models;
using Microsoft.AspNet.Identity;

namespace AutomationSystem.Shared.Contract.Identities.Data
{
    /// <summary>
    /// Provides db connection for UserStore
    /// </summary>
    public interface IAppUserDatabaseLayer : 
        IUserLoginStore<ApplicationUser, int>,
        IUserLockoutStore<ApplicationUser, int>, 
        IUserTwoFactorStore<ApplicationUser, int>,
        IUserEmailStore<ApplicationUser, int>
    {

        // gets user by google account
        Task<ApplicationUser> GetUserByGoogleAccount(string googleAccount);

    }    

}
