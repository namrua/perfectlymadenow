using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// List of email templates
    /// </summary>
    public class EmailTemplateList
    {

        // public properties
        public EmailType Type { get; set; }
        public List<IEnumItem> LanguagesWithoutTemplate { get; set; }
        public List<EmailTemplateListItem> Items { get; set; }
        public EmailTemplateEntityId EmailTemplateEntityId { get; set; }

        // constructor
        public EmailTemplateList()
        {
            Items = new List<EmailTemplateListItem>();
            LanguagesWithoutTemplate = new List<IEnumItem>();
        }

    }

}
