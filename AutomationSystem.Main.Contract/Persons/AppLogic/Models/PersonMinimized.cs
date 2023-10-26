using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Determines persons minimized object
    /// </summary>
    public class PersonMinimized
    {

        // empty person - empty design pattern
        public static PersonMinimized Empty { get; } = new PersonMinimized { FirstName = "empty", LastName = "person" };


        // public properties
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public HashSet<PersonRoleTypeEnum> AssignedRoles { get; set; }
        public HashSet<PersonRoleTypeEnum> DefaultRoles { get; set; }

        // gets full name
        public string Name { get; set; }

        // constructor
        public PersonMinimized()
        {
            AssignedRoles = new HashSet<PersonRoleTypeEnum>();
            DefaultRoles = new HashSet<PersonRoleTypeEnum>();
        }

    }

}
