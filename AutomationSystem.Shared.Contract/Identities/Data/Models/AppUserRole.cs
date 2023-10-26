using AutomationSystem.Base.Contract.Enums;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutomationSystem.Shared.Contract.Identities.Data.Models
{

    // objects for identity user
    public class AppUserRole : IdentityUserRole<int>
    {

        public UserRoleTypeEnum RoleTypeId { get; set; }

    }

}
