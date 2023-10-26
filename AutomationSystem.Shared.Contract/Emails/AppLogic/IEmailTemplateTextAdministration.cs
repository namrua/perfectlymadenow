using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic
{
    /// <summary>
    /// Provides email template text administration 
    /// The service is intended for integration with other web pages and contains integration with IDM
    /// </summary>
    public interface IEmailTemplateTextAdministration
    {

        #region email template text editing

        // get email text for editation of existing template
        EmailTemplateTextForEdit GetEmailTemplateTextForEditById(long emailTemplateId);

        // gets reset email template text for edit
        EmailTemplateTextForEdit GetResetEmailTemplateTextForEdit(long emailTemplateId);

        // get email text for editation by form
        EmailTemplateTextForEdit GetEmailTemplateTextForEditByForm(EmailTemplateTextForm form, EmailTemplateValidationResult validation);

        // validates email text
        EmailTemplateValidationResult ValidateEmailTemplateText(EmailTemplateTextForm form);

        // saves email template text, return true whether email template is valid
        void SaveEmailTemplateText(EmailTemplateTextForm form, bool isValidated);

        #endregion


        #region email

        // gets email detail
        EmailDetail GetEmailDetail(long emailId);

        // sends generic test email, returns email id
        long SendTestEmail(EmailTestSendInfo info, bool allowInvalidTemplate = false);

        #endregion

    }

}
