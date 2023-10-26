using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents
{
    /// <summary>
    /// Encapsulates list of former students with filter
    /// </summary>
    public class FormerStudentsForList
    {

        public FormerStudentFilter Filter { get; set; }
        public List<IEnumItem> ClassTypes { get; set; }
        public List<IEnumItem> Countries { get; set; }
        public List<FormerStudentListItem> Items { get; set; }
        public bool WasSearched { get; set; }                           // determines whether searching was executed

        // constructo
        public FormerStudentsForList(FormerStudentFilter filter)
        {
            Filter = filter ?? new FormerStudentFilter();
            ClassTypes = new List<IEnumItem>();
            Countries = new List<IEnumItem>();
            Items = new List<FormerStudentListItem>();
        }

    }

}
