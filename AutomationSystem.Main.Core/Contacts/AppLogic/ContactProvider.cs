using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Core.Contacts.Data;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using System.Collections.Generic;
using System.Linq;
using ContactListItem = AutomationSystem.Main.Contract.Contacts.AppLogic.Models.ContactListItem;

namespace AutomationSystem.Main.Core.Contacts.AppLogic
{
    public class ContactProvider : IContactProvider
    {
        private readonly IContactDatabaseLayer contactDb;
        private readonly IFormerDatabaseLayer formerDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainMapper mainMapper;

        public ContactProvider(
            IContactDatabaseLayer contactDb,
            IFormerDatabaseLayer formerDb,
            IIdentityResolver identityResolver,
            IMainMapper mainMapper)
        {
            this.contactDb = contactDb;
            this.formerDb = formerDb;
            this.identityResolver = identityResolver;
            this.mainMapper = mainMapper;
        }

        public List<ContactListItem> GetContacts(ContactListFilter filter)
        {
            identityResolver.CheckEntitleForProfileId(Entitle.MainContacts, filter.ProfileId);
            var formerStudentFilter = new FormerStudentFilter
            {
                Class = new FormerClassFilter
                {
                    ProfileId = filter.ProfileId
                }
            };
            var formerStudents = formerDb.GetFormerStudentsByFilter(formerStudentFilter, FormerStudentIncludes.Address | FormerStudentIncludes.FormerClass);
            var blackList = contactDb.GetBlackLists();
            var blockedEmails = new HashSet<string>(blackList.Select(x => x.Email));

            var result = formerStudents
                .GroupBy(x => MainTextHelper.GetNormalizedEmail(x.Email))
                .Select(g => g.OrderByDescending(x => x.FormerClass.EventStart).First())
                .Select(mainMapper.Map<ContactListItem>)
                .Select(x => SetDisableContact(x, blockedEmails.Contains(x.Email)))
                .Where(x => filter.IncludeDisabledContacts || !x.IsDisabled)
                .ToList();

            return result;
        }

        public List<ContactListItemDefinition> RemoveBlackListedItems(List<ContactListItemDefinition> contactListDefinitions)
        {
            var blackList = contactDb.GetBlackLists();
            var blackListEmails = new HashSet<string>(blackList.Select(x => x.Email));
            var result = contactListDefinitions.Where(x => !blackListEmails.Contains(x.Email)).ToList();
            return result;
        }

        #region private methods
        
        private ContactListItem SetDisableContact(ContactListItem item, bool onBlackList)
        {
            item.IsDisabled = onBlackList;
            return item;
        }

        #endregion
    }
}
