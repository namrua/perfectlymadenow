using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    /// <summary>
    /// Core email service that sends core emails
    /// </summary>
    public interface ICoreEmailService
    {

        // sends generic test email, returns email id
        long SendTestEmail(EmailTestSendInfo info, bool allowInvalidTemplate = false);


        // sends incident email, returns email id
        long SendIncidentEmail(EmailTypeEnum emailTypeId, int severity, Incident incident);

        // sends job report email, returns email id
        long SendJobReportEmail(EmailTypeEnum emailTypeId, int severity, long jobRunId, Dictionary<string, object> textMap);        

    }

}
