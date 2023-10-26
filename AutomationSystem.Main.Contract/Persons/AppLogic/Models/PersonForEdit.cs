using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Person for edit
    /// </summary>
    public class PersonForEdit
    {

        // public properties
        public List<IEnumItem> Countries { get; set; }
        public List<PersonRoleType> RoleTypes { get; set; }
        public PersonForm Form { get; set; }

        // construc
        public PersonForEdit()
        {
            Countries = new List<IEnumItem>();
            RoleTypes = new List<PersonRoleType>();
            Form = new PersonForm();
        }

    }

}
