using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutomationSystem.Shared.Contract.Identities.Data.Models;
using AutomationSystem.Shared.Contract.Identities.System;
using AutomationSystem.Shared.Contract.Identities.System.Models;
using Newtonsoft.Json;

namespace AutomationSystem.Shared.Core.Identities.System
{
    /// <summary>
    /// Builds ClaimsIdentities
    /// </summary>
    public class ClaimsIdentityBuilder : IClaimsIdentityBuilder
    {

        public readonly List<IUserGroupMembershipProvider> membershipProviders;

        public ClaimsIdentityBuilder(IEnumerable<IUserGroupMembershipProvider> membershipProviders)
        {
            this.membershipProviders = membershipProviders.ToList();
        }

        // enriches ClaimsIdentity by ApplicationUser and other extensions (UserGroups)
        public async Task<ClaimsIdentity> EnrichUserIdentityAsync(ClaimsIdentity userIdentity, ApplicationUser user)
        {
            // adds custom user claims            
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            // adds roles
            foreach (var role in user.Roles)
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, role.RoleId.ToString()));

            // adds user's groups membership
            foreach (var membershipProvider in membershipProviders)
            {
                // todo: #paraller processing?
                var userGroupMembership = await membershipProvider.GetUserGroupMembershipAsync(user.Id);
                var serializedMembership = SerializeUserGroupMembershipToString(userGroupMembership);
                userIdentity.AddClaim(new Claim(CoreIdentityConstants.UserGroupMembershipClaimType, serializedMembership));
            }
            
            return userIdentity;
        }

        // builds default identity
        public ClaimsIdentity BuildDefaultIdentity()
        {
            var userIdentity = new ClaimsIdentity();

            // adds custom user claims     
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, CoreIdentityConstants.DefaultIdForClaims.ToString()));
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, CoreIdentityConstants.DefaultNameForClaims));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, CoreIdentityConstants.DefaultEmailForClaims));

            // no user privileges are granted! prevents misusing - system services should not use identity checking

            return userIdentity;
        }

        #region private methods

        // serializes user group membership
        private string SerializeUserGroupMembershipToString(UserGroupMembership membership)
        {
            var result = JsonConvert.SerializeObject(membership);
            return result;
        }

        #endregion

    }

}
