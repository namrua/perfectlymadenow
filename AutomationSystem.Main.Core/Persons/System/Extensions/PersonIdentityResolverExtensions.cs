using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Persons.System.Extensions
{
    public static class PersonIdentityResolverExtensions
    {
        public static bool IsEntitleGrantedForPerson(this IIdentityResolver identityResolver, Entitle entitle, Person person)
        {
            return identityResolver.IsEntitleGrantedForUserGroup(entitle, UserGroupTypeEnum.MainProfile, person.ProfileId);
        }
        
        public static void CheckEntitleForPerson(this IIdentityResolver identityResolver, Entitle entitle, Person person)
        {
            identityResolver.CheckEntitleForUserGroup(entitle, UserGroupTypeEnum.MainProfile, person.ProfileId,
                EntityTypeEnum.MainPerson, person.PersonId);
        }
    }
}
