using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Model;
using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailTemplateResolver
    {
        EmailTemplate GetParentTemplate(
            EmailTemplateEntityId emailTemplateEntityId,
            LanguageEnum languageId,
            EmailTypeEnum emailTypeId,
            EmailTemplateIncludes includes = EmailTemplateIncludes.None);

        EmailTemplate GetParentTemplate(EmailTemplate template, EmailTemplateIncludes includes = EmailTemplateIncludes.None);

        List<EmailTemplate> GetValidTemplates(
            EmailTemplateEntityId emailTemplateEntityId,
            EmailTypeEnum emailTypeId,
            HashSet<LanguageEnum> languageIds,
            EmailTemplateIncludes includes = EmailTemplateIncludes.None);

        EmailTemplate GetValidTemplate(
            EmailTypeEnum emailTypeId,
            LanguageEnum languageId,
            EmailTemplateEntityId emailTemplateEntityId,
            EmailTemplateIncludes includes = EmailTemplateIncludes.None);


        EmailTemplate GetEmailTemplateByEmailTemplateEntityId(EmailTemplateEntityId emailTemplateEntityId, EmailTemplateIncludes includes = EmailTemplateIncludes.None);

        EmailTemplate GetEmailTemplateById(long id, bool allowInvalidTemplate = false);
        
        EmailTemplate GetEmailTemplateByTypeAndLanguage(EmailTypeEnum emailTypeId,
            LanguageEnum language = LocalisationInfo.DefaultLanguage, bool canUseDefault = false);
    }
}
