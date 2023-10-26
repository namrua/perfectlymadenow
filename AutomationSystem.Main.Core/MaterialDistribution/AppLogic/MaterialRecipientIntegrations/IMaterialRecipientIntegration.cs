using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.MaterialRecipientIntegrations
{
    /// <summary>
    /// Provides integration for specified material recipient types
    /// </summary>
    public interface IMaterialRecipientIntegration
    {
        EntityTypeEnum TypeId { get; }

        List<RecipientId> GetAllRecipientIdsForClass(Class cls);

        List<ClassMaterialMonitoringListItem> GetClassMaterialMonitoringListItems(
            Class cls,
            Func<RecipientId, ClassMaterialMonitoringListItem> monitoringListItemCreator);

        MaterialAvailabilityResult ResolveRecipientRestrictions(long recipientId, Class cls);

        long? CheckAndTryGetClassId(long recipientId, out string materialsDisabledMessage);
    }
}
