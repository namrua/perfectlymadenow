using AutomationSystem.Main.Model;
using System.Data.Entity.Infrastructure;
using AutomationSystem.Main.Core.Registrations.Data.Models;

namespace AutomationSystem.Main.Core.Registrations.Data.Extensions
{
    public static class ClassRegistrationIncludeExtensions
    {
        public static DbQuery<ClassRegistration> AddIncludes(this DbQuery<ClassRegistration> query, ClassRegistrationIncludes includes)
        {
            if (includes.HasFlag(ClassRegistrationIncludes.ApprovementType))
                query = query.Include("ApprovementType");
            if (includes.HasFlag(ClassRegistrationIncludes.RegistrationType))
                query = query.Include("RegistrationType");
            if (includes.HasFlag(ClassRegistrationIncludes.Class))
                query = query.Include("Class");
            if (includes.HasFlag(ClassRegistrationIncludes.ClassCurrency))
                query = query.Include("Class.Currency");
            if (includes.HasFlag(ClassRegistrationIncludes.ClassRegistrationPayment))
                query = query.Include("ClassRegistrationPayment");
            if (includes.HasFlag(ClassRegistrationIncludes.ClassRegistrationInvitations))
                query = query.Include("ClassRegistrationInvitations");

            if (includes.HasFlag(ClassRegistrationIncludes.Addresses))
            {
                query = query.Include("StudentAddress");
                query = query.Include("RegistrantAddress");
            }
            if (includes.HasFlag(ClassRegistrationIncludes.AddressesCountry))
            {
                query = query.Include("StudentAddress.Country");
                query = query.Include("RegistrantAddress.Country");
            }
            if (includes.HasFlag(ClassRegistrationIncludes.ClassRegistrationLastClass))
                query = query.Include("ClassRegistrationLastClass");
            if (includes.HasFlag(ClassRegistrationIncludes.ClassRegistrationFiles))
                query = query.Include("ClassRegistrationFiles");
            if (includes.HasFlag(ClassRegistrationIncludes.ClassClassStyle))
                query = query.Include("Class.ClassStyle");
            if (includes.HasFlag(ClassRegistrationIncludes.ClassProfile))
                query = query.Include("Class.Profile");

            return query;
        }

        
        public static DbQuery<ClassRegistrationInvitation> AddIncludes(this DbQuery<ClassRegistrationInvitation> query,
            ClassRegistrationInvitationIncludes includes)
        {
            if (includes.HasFlag(ClassRegistrationInvitationIncludes.ClassRegistration))
                query = query.Include("ClassRegistration");
            if (includes.HasFlag(ClassRegistrationInvitationIncludes.Class))
                query = query.Include("Class");
            return query;
        }
    }
}
