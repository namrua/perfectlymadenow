using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Class business form
    /// </summary>
    public class ClassBusinessForm
    {
        [HiddenInput]
        public long ClassId { get; set; }
     
        [DisplayName("Approved budget")]
        [Range(0, 1000000)]
        public decimal? ApprovedBudget { get; set; }

        [DisplayName("Reimbursement for printing")]
        [Range(0, 1000000)]
        public decimal? PrintReimbursement { get; set; }

        [DisplayName("Associated lecture")]
        [PickInputOptions(NoItemText = "no lecture", Placeholder = "select lecture")]
        public long? AssociatedLectureId { get; set; }

        [DisplayName("Custom expenses")]
        public List<ClassCustomExpenseForm> CustomExpenses { get; set; } = new List<ClassCustomExpenseForm>();
    }
}