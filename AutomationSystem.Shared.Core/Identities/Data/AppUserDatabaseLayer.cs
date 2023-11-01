using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutomationSystem.Shared.Contract.Identities.Data;
using AutomationSystem.Shared.Contract.Identities.Data.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace AutomationSystem.Shared.Core.Identities.Data
{
    /// <summary>
    /// Provides db connection for UserStore
    /// </summary>
    public class AppUserDatabaseLayer : IAppUserDatabaseLayer
    {

        // private fields
        private readonly CoreEntities context;


        // constructor
        public AppUserDatabaseLayer()
        {
            context = new CoreEntities();            
        }

        // dispose
        public void Dispose()
        {
            context?.Dispose();
        }

        // factory method
        public static IAppUserDatabaseLayer Create(IdentityFactoryOptions<IAppUserDatabaseLayer> options,
            IOwinContext context)
        {
            return new AppUserDatabaseLayer();
        }


        #region IAppUserDatabaseLayer
        
        // gets user by google account
        public async Task<ApplicationUser> GetUserByGoogleAccount(string googleAccount)
        {
            var user = await context.Users.Include("UserLogins").Include("UserRoleAssignments").Active()
                .FirstOrDefaultAsync(x => x.Active && x.GoogleAccount.ToLower() == googleAccount.ToLower());
            if (user == null) return null;

            var result = ConvertToApplicationUser(user);
            return result;
        }

        #endregion

        public async Task<UserAccount> GetWebexUserAccount(int? accountId)
        {
            //default by first account for full authorization
            accountId = accountId ?? 1;
            var user = await context.UserAccounts
                .FirstOrDefaultAsync(x => x.IsActive.HasValue && x.IsActive.Value && x.AccountId == accountId);
            if (user == null) return null;
            return user;
        }

        #region IUserStore

        //UserManager.CreateAsync
        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            //throw new NotImplementedException();
            return Task.FromResult(0);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        // finds user by id
        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            var user = await context.Users.Include("UserLogins").Include("UserRoleAssignments").Active().FirstOrDefaultAsync(x => x.Active && x.UserId == userId);
            if (user == null) return null;

            var result = ConvertToApplicationUser(user);
            return result;
        }


        // finds user by user name   
        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = await context.Users.Include("UserLogins").Include("UserRoleAssignments").Active()
                .FirstOrDefaultAsync(x => x.Active && x.Name.ToLower() == userName.ToLower());
            if (user == null) return null;

            var result = ConvertToApplicationUser(user);
            return result;
        }

        #endregion


        #region IUserLoginStore (dědí od IUserStore)

        // adds new user login
        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            var userLogin = new UserLogin();
            userLogin.UserId = user.Id;
            userLogin.LoginProvider = login.LoginProvider;
            userLogin.ProviderKey = login.ProviderKey;            
            context.UserLogins.Add(userLogin);
            await context.SaveChangesAsync();            
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }


        // SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false)
        // UserManager.AddLoginAsync(user.Id, info.Login);
        // finds user by login info
        public async Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {            
            var userLogin = await context.UserLogins.Include("User.UserRoleAssignments").Active()
                .FirstOrDefaultAsync(x => x.LoginProvider == login.LoginProvider 
                                          && x.ProviderKey == login.ProviderKey 
                                          && !x.User.Deleted && x.User.Active);
            if (userLogin == null) return null;

            var result = ConvertToApplicationUser(userLogin.User);
            return result;
        }

        #endregion

       
        #region IUserLockoutStore

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        //SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(false);
        }

        // UserManager.CreateAsync(user) 4
        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.FromResult(0);
        }

        #endregion


        #region IUserTwoFactorStore

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        //SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(false);
        }

        #endregion


        #region IUserEmailStore

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            throw new NotImplementedException();
        }

        // UserManager.CreateAsync(user);      2 
        public Task<string> GetEmailAsync(ApplicationUser user)
        {            
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }


        // UserManager.CreateAsync(user) 3
        // finds user by email
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = await context.Users.Include("UserLogins").Include("UserRoleAssignments").Active()
                .FirstOrDefaultAsync(x => x.Active && x.GoogleAccount.ToLower() == email.ToLower());
            if (user == null) return null;

            var result = ConvertToApplicationUser(user);
            return result;
        }

        #endregion


        #region private 
        

        // converts db user to application user
        private ApplicationUser ConvertToApplicationUser(User user)
        {
            var result = new ApplicationUser();
            result.Id = user.UserId;
            result.Email = user.GoogleAccount;
            result.UserName = user.Name;
            foreach (var login in user.UserLogins.AsQueryable().Active())
            {
                var appUserLogin = new AppUserLogin();
                appUserLogin.UserId = user.UserId;
                appUserLogin.LoginProvider = login.LoginProvider;
                appUserLogin.ProviderKey = login.ProviderKey;               
                result.Logins.Add(appUserLogin);
            }
            
            // sets roles
            foreach (var role in user.UserRoleAssignments.Where(x => !x.Deleted))
            {
                var appRole = new AppUserRole
                {
                    UserId = result.Id,
                    RoleId = (int)role.UserRoleTypeId,
                    RoleTypeId = role.UserRoleTypeId
                };
                result.Roles.Add(appRole);
            }

            result.EmailConfirmed = false;
            result.PasswordHash = null;            
            result.PhoneNumber = null;
            result.PhoneNumberConfirmed = false;
            result.TwoFactorEnabled = false;
            result.LockoutEnabled = true;
            result.LockoutEndDateUtc = null;
            result.AccessFailedCount = 0;            
            return result;
        }

        #endregion

    }

}
