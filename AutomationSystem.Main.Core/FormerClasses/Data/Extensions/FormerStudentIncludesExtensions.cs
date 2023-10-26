using AutomationSystem.Main.Model;
using System.Data.Entity.Infrastructure;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;

namespace AutomationSystem.Main.Core.FormerClasses.Data.Extensions
{
    public static class FormerStudentIncludesExtensions
    {
        public static DbQuery<FormerStudent> AddIncludes(this DbQuery<FormerStudent> query,
           FormerStudentIncludes includes)
        {
            if (includes.HasFlag(FormerStudentIncludes.Address))
            {
                query = query.Include("Address");
            }

            if (includes.HasFlag(FormerStudentIncludes.AddressCountry))
            {
                query = query.Include("Address.Country");
            }

            if (includes.HasFlag(FormerStudentIncludes.FormerClass))
            {
                query = query.Include("FormerClass");
            }

            if (includes.HasFlag(FormerStudentIncludes.FormerClassClassType))
            {
                query = query.Include("FormerClass.ClassType");
            }

            if (includes.HasFlag(FormerStudentIncludes.FormerClassProfile))
            {
                query = query.Include("FormerClass.Profile");
            }

            return query;
        }
    }
}
