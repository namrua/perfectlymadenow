using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.EntityIntegration.Data
{
    /// <summary>
    /// Provides data for entities related to integration
    /// </summary>
    public interface IIntegrationDatabaseLayer
    {

        // gets integration type by id
        IntegrationType GetIntegrationTypeById(IntegrationTypeEnum integrationTypeId);

    }

}
