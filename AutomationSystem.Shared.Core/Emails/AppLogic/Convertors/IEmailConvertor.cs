using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.Convertors
{
    /// <summary>
    /// Converts emails objects
    /// </summary>
    public interface IEmailConvertor
    {

        // convets email to email detail
        EmailDetail ConvertToEmailDetail(Email email);

        // converts email to email list item
        EmailListItem ConvertToEmailListItem(Email email);

    }

}
