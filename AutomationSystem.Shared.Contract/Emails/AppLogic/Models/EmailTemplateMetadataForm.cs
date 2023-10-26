using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Email template metadata form
    /// </summary>
    public class EmailTemplateMetadataForm
    {

        // hiddens
        [HiddenInput]
        public long ParentEmailTemplateId { get; set; }
        [HiddenInput]
        public long EmailTemplateId { get; set; }
        [HiddenInput]
        public EmailTypeEnum EmailTypeId { get; set; }
        [HiddenInput]
        public EntityTypeEnum? EntityTypeId { get; set; }
        [HiddenInput]
        public long? EntityId { get; set; }
        [HiddenInput]
        public LanguageEnum LanguageId { get; set; }

        
        [MaxLength(4000)]
        [TextInputOptions(TextAreaRows = 4)]
        [DisplayName("Instruction note to fill an email")]
        public string FillingNote { get; set; }

        [DisplayName("Is required")]
        public long[] RequiredParameters { get; set; }

        // constructor
        public EmailTemplateMetadataForm()
        {
            RequiredParameters = new long[0];
        }

    }

}
