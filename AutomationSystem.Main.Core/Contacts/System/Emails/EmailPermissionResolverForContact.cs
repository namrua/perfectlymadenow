using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Core.Contacts.Data;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Contacts.System.Emails
{
    public class EmailPermissionResolverForContact : IEmailPermissionResolverForEntity
    {
        private readonly IContactDatabaseLayer contactDb;
        private readonly IIdentityResolver identityResolver;

        public List<EntityTypeEnum?> SupportedEntityTypeIds => new List<EntityTypeEnum?> { EntityTypeEnum.MainContactList };

        public EmailPermissionResolverForContact(
            IContactDatabaseLayer contactDb,
            IIdentityResolver identityResolver)
        {
            this.contactDb = contactDb;
            this.identityResolver = identityResolver;
        }

        public bool IsGrantedForEmail(EmailEntityId emailEntityId, EmailTypeEnum emailTypeId)
        {
            return IsGranted(emailEntityId.Id);
        }

        public bool IsGrantedForEmailTemplate(EmailTemplateEntityId emailTemplateEntityId, EmailTypeEnum emailTypeId)
        {
            return IsGranted(emailTemplateEntityId.Id);
        }

        #region private methods

        private bool IsGranted(long? entityId)
        {
            var contactList = contactDb.GetContactListById(entityId ?? 0);
            if (contactList == null)
            {
                return false;
            }

            identityResolver.CheckEntitleForProfileId(Entitle.MainContacts, contactList.ProfileId);

            return true;
        }

        #endregion
    }
}
