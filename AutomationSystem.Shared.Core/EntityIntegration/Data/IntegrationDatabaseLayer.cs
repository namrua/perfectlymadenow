using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.EntityIntegration.Data
{
    /// <summary>
    /// Provides data for entities related to integration
    /// </summary>
    public class IntegrationDatabaseLayer : IIntegrationDatabaseLayer
    {

        // gets integration type by id
        public IntegrationType GetIntegrationTypeById(IntegrationTypeEnum integrationTypeId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.IntegrationTypes.FirstOrDefault(x => x.IntegrationTypeId == integrationTypeId);
                return result;
            }
        }

    }

}
    
