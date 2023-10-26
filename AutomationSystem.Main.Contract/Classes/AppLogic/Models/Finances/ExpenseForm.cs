using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Full class expense form
    /// </summary>
    public class ExpenseForm
    {
        [HiddenInput]
        [DisplayName("Order No.")]
        public int Order { get; set; }

        [DisplayName("Text")]
        [Required]
        [MaxLength(128)]
        public string Text { get; set; }

        [DisplayName("Type")]
        [PickInputOptions]
        public ClassExpenseTypeEnum ClassExpenseTypeId { get; set; }

        [DisplayName("Value")]
        [Range(0, 1000000)]
        public decimal? Value { get; set; }

        // constructor
        public ExpenseForm()
        {
            ClassExpenseTypeId = ClassExpenseTypeEnum.Custom;
        }
    }
}