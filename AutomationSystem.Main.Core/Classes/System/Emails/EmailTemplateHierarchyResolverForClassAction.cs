using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Main.Core.Classes.System.Emails
{
    public class EmailTemplateHierarchyResolverForClassAction : IEmailTemplateHierarchyResolverForEntity
    {
        private readonly IClassDatabaseLayer classDb;

        public EmailTemplateHierarchyResolverForClassAction(IClassDatabaseLayer classDb)
        {
            this.classDb = classDb;
        }

        public EntityTypeEnum? EntityTypeId => EntityTypeEnum.MainClassAction;

        public EmailTemplateEntityHierarchy GetHierarchyForParent(long? entityId)
        {
            var profileId = GetProfileIdByClassActionId(entityId ?? 0);

            var result = new EmailTemplateEntityHierarchy
            {
                CanUseDefault = false
            };
            result.Entities.Add(new EmailTemplateEntityId());
            result.Entities.Add(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, profileId));

            return result;
        }

        public EmailTemplateEntityHierarchy GetHierarchy(long? entityId)
        {
            throw new InvalidOperationException("Email template hierarchy level on ClassAction is not supported.");
        }

        #region private methods

        private long GetProfileIdByClassActionId(long classActionId)
        {
            var classAction = classDb.GetClassActionById(classActionId, ClassActionIncludes.Class);
            if (classAction == null)
            {
                throw new ArgumentException($"There is no class action with id {classActionId}.");
            }

            return classAction.Class.ProfileId;
        }

        #endregion
    }
}
