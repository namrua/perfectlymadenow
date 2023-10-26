using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Emails.System.Models
{
    public class EmailTemplateEntityHierarchy
    {
        public List<EmailTemplateEntityId> Entities { get; set; } = new List<EmailTemplateEntityId>();

        public bool CanUseDefault { get; set; }
    }
}
