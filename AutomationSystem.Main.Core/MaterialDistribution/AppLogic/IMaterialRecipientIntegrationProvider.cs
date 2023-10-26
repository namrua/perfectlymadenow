using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.MaterialRecipientIntegrations;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    /// <summary>
    /// Provides material recipient integration
    /// </summary>
    public interface IMaterialRecipientIntegrationProvider
    {
        List<IMaterialRecipientIntegration> GetAllRecipientIntegratons();

        IMaterialRecipientIntegration GetRecipientIntegrationByTypeId(EntityTypeEnum recipientTypeId);
    }
}
