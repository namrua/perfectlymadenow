using System.Collections.Generic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Actions
{
    /// <summary>
    /// Class action detail
    /// </summary>
    public class ClassActionDetail
    {
        // public properties
        public ClassState ClassState { get; set; }
        public ClassActionListItem ClassAction { get; set; }        
        public List<EmailTemplateListItem> EmailTemplates { get; set; }
        public bool CanDelete { get; set; }
        public bool CanProcess { get; set; }

        // constructor
        public ClassActionDetail()
        {
            ClassAction = new ClassActionListItem();
            EmailTemplates = new List<EmailTemplateListItem>();
        }
    }
}