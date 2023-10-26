using System.Collections.Generic;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports
{
    /// <summary>
    /// Class recipient selection for edit
    /// </summary>
    public class ClassRecipientSelectionForEdit
    {
        public ClassRecipientSelectionForm Form { get; set; }
        public List<PersonShortDetail> Recipients { get; set; }

        // constructor
        public ClassRecipientSelectionForEdit()
        {
            Form = new ClassRecipientSelectionForm();
            Recipients = new List<PersonShortDetail>();
        }
    }
}
