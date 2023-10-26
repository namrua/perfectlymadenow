using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads
{
    /// <summary>
    /// Former student
    /// </summary>
    public class FormerStudentBatchItemForEdit
    {

        // public properties
        public List<IEnumItem> Countries { get; set; }
        public long BatchUploadItemId { get; set; }
        public long BatchUploadId { get; set; }
        public Dictionary<string, string> OriginalValues { get; set; }
        public FormerStudentForm Form { get; set; }

        // constructor
        public FormerStudentBatchItemForEdit()
        {
            Countries = new List<IEnumItem>();
            Form = new FormerStudentForm();
            OriginalValues = new Dictionary<string, string>();
        }

    }

}