using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Web.Helpers.EmailTemplates
{
    public static class UrlHelperEmailTemplatesExtensions
    {
        public static string EmailTemplateUrl(this UrlHelper helper, EmailTemplateEntityId emailTemplateEntityId)
        {
            if (emailTemplateEntityId.TypeId == EntityTypeEnum.MainProfile)
            {
                return helper.Action("EmailTemplates", "Profile", new { emailTemplateEntityId.Id });
            }

            return helper.Action("Index", "Email");
        }
    }
}