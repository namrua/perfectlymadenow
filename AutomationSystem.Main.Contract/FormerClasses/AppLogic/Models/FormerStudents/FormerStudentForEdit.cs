using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents
{
    /// <summary>
    /// Former student
    /// </summary>
    public class FormerStudentForEdit
    {

        // public properties
        public List<IEnumItem> Countries { get; set; }
        public FormerStudentForm Form { get; set; }

        // constructor
        public FormerStudentForEdit()
        {
            Countries = new List<IEnumItem>();
            Form = new FormerStudentForm();
        }

    }

}
