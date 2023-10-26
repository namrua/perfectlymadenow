using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Person role detail
    /// </summary>
    public class PersonRoleDetail
    {

        [DisplayName("Role code")]
        public PersonRoleTypeEnum RoleTypeId { get; set; }

        [DisplayName("Role")]
        public string RoleType { get; set; }

        [DisplayName("Is default")]
        public bool IsDefault { get; set; }

    }

}
