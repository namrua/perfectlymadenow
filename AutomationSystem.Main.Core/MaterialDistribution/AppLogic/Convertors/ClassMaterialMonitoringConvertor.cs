using System;
using System.Linq;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors
{
    /// <summary>
    /// Converts class materials monitoring related objects
    /// </summary>
    public class ClassMaterialMonitoringConvertor : IClassMaterialMonitoringConvertor
    {
        // Creates ClassMaterialMonitoringListItem by RecipientId
        public ClassMaterialMonitoringListItem CreateClassMaterialMonitoringListItem(RecipientId recipientId)
        {
            var result = new ClassMaterialMonitoringListItem
            {
                RecipientId = recipientId
            };
            return result;
        }
            
        // Converts ClassMaterialRecipient to ClassMaterialMonitoringListItem
        public ClassMaterialMonitoringListItem ConvertToClassMaterialMonitoringListItem(ClassMaterialRecipient materialRecipient)
        {
            if (materialRecipient.ClassMaterialDownloadLogs == null)
            {
                throw new InvalidOperationException("ClassMaterialDownloadLogs is not included into ClassMaterialRecipient object.");
            }

            var result = new ClassMaterialMonitoringListItem
            {
                ClassMaterialRecipientId = materialRecipient.ClassMaterialRecipientId,
                RecipientId = new RecipientId(materialRecipient.RecipientTypeId, materialRecipient.RecipientId),
                Password = materialRecipient.Password,
                RequestCode = materialRecipient.RequestCode,
                Notified = materialRecipient.Notified,
                TotalDonwnloadCount = materialRecipient.ClassMaterialDownloadLogs.Count(x => x.WasDownloaded)
            };
            return result;
        }
    }
}
