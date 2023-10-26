using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic
{
    /// <summary>
    /// Provides email template administration
    /// The service is NOT intended for integration with other web pages
    /// </summary>
    public interface IEmailTemplateAdministration
    {

        #region lists and details

        // gets email type summary
        EmailTypeSummary GetEmailTypeSummary(EmailTemplateEntityId emailTemplateEntityId, HashSet<EmailTypeEnum> allowedEmailTypes = null);

        EmailTypeSummary GetSystemEmailTypeSummary();

        // gets list of email templates
        EmailTemplateList GetEmailTemplateList(EmailTypeEnum emailTypeId, EmailTemplateEntityId emailTemplateEntityId);

        // gets email template detail
        EmailTemplateDetail GetEmailTemplateDetail(long emailTemplateId);

        // resets email template
        void ResetEmailTemplate(long emailTemplateId);

        #endregion


        #region email template metadata editing

        // get metadata for new template editation
        EmailTemplateMetadataForEdit GetNewEmailTemplateMetadataForEdit(EmailTypeEnum emailTypeId, LanguageEnum languageId, EmailTemplateEntityId emailTemplateEntityId);

        // get metadata for editation of existing template
        EmailTemplateMetadataForEdit GetEmailTemplateMetadataForEditById(long emailTemplateId);

        // gets metadata for editation by form
        EmailTemplateMetadataForEdit GetEmailTemplateMetadataForEditByForm(EmailTemplateMetadataForm form);

        // updates new template
        long SaveEmailTemplateMetadata(EmailTemplateMetadataForm form);

        #endregion

    }

}
