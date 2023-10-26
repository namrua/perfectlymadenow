using AutomationSystem.Main.Core.Classes.AppLogic.Models.Events;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors;
using AutomationSystem.Main.Core.MaterialDistribution.Data;
using AutomationSystem.Shared.Contract.TimeZones.System;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.EventHandlers
{
    public class CreateClassMaterialsEventHandler : IEventHandler<ClassCreatedEvent>
    {
        private readonly IClassMaterialDatabaseLayer materialDb;
        private readonly IClassMaterialConvertor classMaterialConvertor;

        public CreateClassMaterialsEventHandler(
            IClassMaterialDatabaseLayer materialDb,
            ITimeZoneService timeZoneService,
            IClassMaterialConvertor classMaterialConvertor)
        {
            this.materialDb = materialDb;

            this.classMaterialConvertor = classMaterialConvertor;
        }

        public Result HandleEvent(ClassCreatedEvent evnt)
        {
            var classMaterial = classMaterialConvertor.CreateInitialClassMaterial(evnt.ClassId);
            materialDb.InsertClassMaterial(classMaterial);
            return Result.Success($"ClassMaterial added for class with id {evnt.ClassId}");
        }
    }
}
