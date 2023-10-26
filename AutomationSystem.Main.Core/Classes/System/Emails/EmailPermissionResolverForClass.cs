using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Classes.System.Emails
{
    public class EmailPermissionResolverForClass : IEmailPermissionResolverForEntity
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IIdentityResolver identityResolver;

        public List<EntityTypeEnum?> SupportedEntityTypeIds => new List<EntityTypeEnum?> {EntityTypeEnum.MainClass, EntityTypeEnum.MainClassAction};

        public EmailPermissionResolverForClass(IClassDatabaseLayer classDb, IIdentityResolver identityResolver)
        {
            this.classDb = classDb;
            this.identityResolver = identityResolver;
        }

        public bool IsGrantedForEmail(EmailEntityId emailEntityId, EmailTypeEnum emailTypeId)
        {
            return IsGranted(emailEntityId.TypeId, emailEntityId.Id);
        }

        public bool IsGrantedForEmailTemplate(EmailTemplateEntityId emailTemplateEntityId, EmailTypeEnum emailTypeId)
        {
            return IsGranted(emailTemplateEntityId.TypeId, emailTemplateEntityId.Id);
        }

        #region private methods

        private Class GetClassByEntityTypeAndId(EntityTypeEnum? entityTypeId, long? entityId)
        {
            switch (entityTypeId)
            {
                case EntityTypeEnum.MainClass:
                    var cls = classDb.GetClassById(entityId ?? 0);
                    return cls;

                case EntityTypeEnum.MainClassAction:
                    var classAction = classDb.GetClassActionById(entityId ?? 0, ClassActionIncludes.Class);
                    return classAction.Class;

                default:
                    return null;
            }
        }
        
        private bool IsGranted(EntityTypeEnum? entityTypeId, long? entityId)
        {
            var cls = GetClassByEntityTypeAndId(entityTypeId, entityId);
            if (cls == null)
            {
                return false;
            }

            var result = identityResolver.IsEntitleGrantedForClass(cls);
            return result;
        }

        #endregion
    }
}
