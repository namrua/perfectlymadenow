using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Emails.Data.Models
{
    /// <summary>
    /// Email template filter
    /// </summary>
    [Bind(Include="")]
    public class EmailTemplateFilter
    {
       
        public LanguageEnum? LanguageId { get; set; }
        public EmailTypeEnum? EmailTypeId { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsSealed { get; set; }
        public EmailTemplateEntityId EmailTemplateEntityId { get; set; }

        // constructor
        public EmailTemplateFilter() { }
        public EmailTemplateFilter(EmailTypeEnum emailType)
        {
            EmailTypeId = emailType;
        }

    }

}
