using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Class expenses form
    /// </summary>
    public class ClassCustomExpenseForm
    {
        [HiddenInput]
        [DisplayName("Order No.")]
        public int Order { get; set; }

        [DisplayName("Expense text")]
        [Required]
        [MaxLength(128)]
        public string Text { get; set; }

        [DisplayName("Expense value")]
        [Range(0, 1000000)]
        [TextInputOptions(RightAddonText = LocalisationInfo.DefaultCurrencyCode)]
        public decimal? Value { get; set; }
    }
}