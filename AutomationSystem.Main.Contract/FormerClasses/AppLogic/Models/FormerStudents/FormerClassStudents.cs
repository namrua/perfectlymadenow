using System.Collections.Generic;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents
{
    public class FormerClassStudents
    {

        public FormerClassListItem Class { get; set; }
        public List<FormerStudentListItem> Students { get; set; }

        public bool CanInsert { get; set; }


        // constructor
        public FormerClassStudents()
        {
            Class = new FormerClassListItem();
            Students = new List<FormerStudentListItem>();
        }

    }
}
