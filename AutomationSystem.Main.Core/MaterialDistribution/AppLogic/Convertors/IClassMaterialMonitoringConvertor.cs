using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors
{
    /// <summary>
    /// Converts class materials monitoring related objects
    /// </summary>
    public interface IClassMaterialMonitoringConvertor
    {
        // Creates ClassMaterialMonitoringListItem by RecipientId
        ClassMaterialMonitoringListItem CreateClassMaterialMonitoringListItem(RecipientId recipientId);

        // Converts ClassMaterialRecipient to ClassMaterialMonitoringListItem
        ClassMaterialMonitoringListItem ConvertToClassMaterialMonitoringListItem(ClassMaterialRecipient materialRecipient);
    }
}
