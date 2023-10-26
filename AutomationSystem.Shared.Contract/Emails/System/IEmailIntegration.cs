using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    /// <summary>
    /// Integrates email templates administration to other pages
    /// </summary>
    public interface IEmailIntegration
    {

        // return clones of email template for given email type and laguages (executes check)
        List<EmailTemplate> CloneEmailTemplates(EmailTypeEnum emailTypeId, EmailTemplateEntityId emailTemplateEntityId, params LanguageEnum?[] languageIds);

        // saves clones of email templates for given entity type id and entity id
        void SaveClonedEmailTemplates(List<EmailTemplate> emailTemplates, EmailTemplateEntityId emailTemplateEntityId);
   

        // gets email templates list item for given entity
        List<EmailTemplateListItem> GetEmailTemplateListItemsByEntity(EmailTemplateEntityId emailTemplateEntityId, bool onlyNotSealed = false);

        // gets emails for given entity
        List<EmailListItem> GetEmailsByEntity(EmailEntityId emailEntityId);

        // delete email template
        void DeleteEmailTemplate(long emailTemplateId);

        // delete email templates by entity id
        void DeleteEmailTemplatesByEntity(EmailEntityId emailEntityId);

    }

}
