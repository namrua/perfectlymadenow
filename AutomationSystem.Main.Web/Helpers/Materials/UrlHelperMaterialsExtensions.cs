using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;

namespace AutomationSystem.Main.Web.Helpers.Materials
{
    /// <summary>
    /// Extends UrlHelper for materials
    /// </summary>
    public static class UrlHelperMaterialsExtensions
    {
        public static string MaterialRecipientUrl(this UrlHelper helper, RecipientId recipientId, long? recipientMaterialId)
        {
            if (recipientId.TypeId == EntityTypeEnum.MainClassRegistration)
            {
                return helper.Action("Registration", "Materials", new { id = recipientId.Id });
            }

            if (!recipientMaterialId.HasValue)
            {
                return helper.Action("Index", "Class");
            }

            return helper.Action("Recipient", "Materials", new { id = recipientMaterialId });
        }
    }

}