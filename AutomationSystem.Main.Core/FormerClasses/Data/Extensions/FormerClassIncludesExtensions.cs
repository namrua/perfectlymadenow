using AutomationSystem.Main.Model;
using System.Data.Entity.Infrastructure;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;

namespace AutomationSystem.Main.Core.FormerClasses.Data.Extensions
{
    public static class FormerClassIncludesExtensions
    {
        public static DbQuery<FormerClass> AddIncludes(this DbQuery<FormerClass> query,
            FormerClassIncludes includes)
        {
            if (includes.HasFlag(FormerClassIncludes.ClassType))
            {
                query = query.Include("ClassType");
            }

            if (includes.HasFlag(FormerClassIncludes.FormerStudent))
            {
                query = query.Include("FormerStudents");
            }

            if (includes.HasFlag(FormerClassIncludes.FormerStudentAddress))
            {
                query = query.Include("FormerStudents.Address");
            }

            if (includes.HasFlag(FormerClassIncludes.FormerStudentAddressCountry))
            {
                query = query.Include("FormerStudents.Address.Country");
            }

            if (includes.HasFlag(FormerClassIncludes.Profile))
            {
                query = query.Include("Profile");
            }
            
            return query;
        }
    }
}
