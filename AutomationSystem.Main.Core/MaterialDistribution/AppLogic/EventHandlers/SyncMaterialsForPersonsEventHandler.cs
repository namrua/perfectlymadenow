using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.EventHandlers
{
    /// <summary>
    /// Synchronizes materials for class persons
    /// </summary>
    public class SyncMaterialsForPersonsEventHandler : IEventHandler<ClassPersonsChangedEvent>
    {
        private readonly IClassMaterialAdministration materialAdministration;
        private readonly IClassMaterialBusinessRules classMaterialBusinessRules;

        public SyncMaterialsForPersonsEventHandler(IClassMaterialAdministration materialAdministration, IClassMaterialBusinessRules classMaterialBusinessRules)
        {
            this.materialAdministration = materialAdministration;
            this.classMaterialBusinessRules = classMaterialBusinessRules;
        }

        public Result HandleEvent(ClassPersonsChangedEvent evnt)
        {
            var roles = classMaterialBusinessRules.GetMaterialSupportingPersonRoles();
            var recipientIds = new HashSet<RecipientId>(
                evnt.AddedPersons
                    .Where(x => roles.Contains(x.RoleTypeId))
                    .Select(x => new RecipientId(EntityTypeEnum.MainPerson, x.PersonId)));

            foreach (var recipientId in recipientIds)
            {
                materialAdministration.DistributeMaterialsToRecipient(evnt.ClassId, recipientId, false);
            }

            return Result.Success($"Materials was distributed to the following recipients: { string.Join(", ", recipientIds)}");
        }
    }
}
