using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Models.Events
{
    /// <summary>
    /// Person id and role
    /// </summary>
    public class PersonIdAndRole
    {
        public PersonRoleTypeEnum RoleTypeId { get; }
        public long PersonId { get; }

        public PersonIdAndRole(PersonRoleTypeEnum roleTypeId, long personId)
        {
            RoleTypeId = roleTypeId;
            PersonId = personId;
        }

        public override string ToString()
        {
            return $"RoleTypeId: {RoleTypeId}; PersonId: {PersonId}";
        }
    }
}
