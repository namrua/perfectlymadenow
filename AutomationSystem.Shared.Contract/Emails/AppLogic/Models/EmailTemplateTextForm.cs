using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    ///  Email template text form
    /// </summary>
    public class EmailTemplateTextForm
    {

        // public properties
        [HiddenInput]
        public long EmailTemplateId { get; set; }

        [Required]
        [MaxLength(256)]
        [DisplayName("Subject")]
        public string Subject { get; set; }

        [Required]
        [MaxLength(4000)]
        [AllowHtml]
        [DisplayName("Body")]
        [TextInputOptions(ControlType = TextControlType.AceTextInput)]
        public string Text { get; set; }

    }

}
