using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Identities.System.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace AutomationSystem.Shared.Contract.Identities.AppLogic.Extensions
{
    /// <summary>
    /// Extensions of IIdentity and 
    /// </summary>
    public static class IdentityExtensions
    {

        // gets user ID from identity claim
        public static int GetId(this IIdentity identity)
        {
            return identity.GetUserId<int>();
        }


        // gets email address from identity claim
        public static string GetEmail(this IIdentity identity)
        {
            var claimsIdentity = ConvertToClaimsIdentity(identity);
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            if (claim == null)
                throw new InvalidOperationException($"There is no claim with type {ClaimTypes.Email}");
            return claim.Value;
        }


        // gets all roles of user
        public static HashSet<UserRoleTypeEnum> GetRoles(this IIdentity identity)
        {
            var claimsIdentity = ConvertToClaimsIdentity(identity);
            var result = new HashSet<UserRoleTypeEnum>();
            foreach (var roleClaim in claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role))
            {
                if (int.TryParse(roleClaim.Value, out var roleTypeId))
                    result.Add((UserRoleTypeEnum) roleTypeId);
            }
            return result;
        }

        // gets all user group memberships
        public static List<UserGroupMembership> GetUserGrpouMemberships(this IIdentity identity)
        {
            var claimsIdentity = ConvertToClaimsIdentity(identity);
            var result = new List<UserGroupMembership>();
            foreach (var mambershipClaim in claimsIdentity.Claims.Where(x => x.Type == CoreIdentityConstants.UserGroupMembershipClaimType))
            {
                var membership = SerializeUserGroupMembershipToString(mambershipClaim.Value);
                if (membership != null)
                    result.Add(membership);
            }
            return result;
        }


        #region private methods

        // converts IIdentity to claims identity, executes type check
        private static ClaimsIdentity ConvertToClaimsIdentity(IIdentity identity)
        {
            if (!(identity is ClaimsIdentity claimIdentity))
                throw new InvalidOperationException($"{typeof(ClaimsIdentity)} was expected but identity is {identity.GetType()}.");
            return claimIdentity;
        }


        // serializes user group membership
        private static UserGroupMembership SerializeUserGroupMembershipToString(string membership)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<UserGroupMembership>(membership);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

    }

}