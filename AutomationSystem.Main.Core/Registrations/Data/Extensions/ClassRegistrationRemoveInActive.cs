using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;
using AutomationSystem.Main.Core.Registrations.Data.Models;

namespace AutomationSystem.Main.Core.Registrations.Data.Extensions
{
    public static class ClassRegistrationRemoveInActive
    {
        public static ClassRegistration RemoveInactiveForClassRegistration(ClassRegistration entity, ClassRegistrationIncludes includes)
        {
            if (entity == null)
                return null;
            if (includes.HasFlag(ClassRegistrationIncludes.ClassRegistrationInvitations))
                entity.ClassRegistrationInvitations = entity.ClassRegistrationInvitations.AsQueryable().Active().ToList();
            if (includes.HasFlag(ClassRegistrationIncludes.ClassRegistrationFiles))
                entity.ClassRegistrationFiles = entity.ClassRegistrationFiles.AsQueryable().Active().ToList();
            return entity;
        }
    }
}
