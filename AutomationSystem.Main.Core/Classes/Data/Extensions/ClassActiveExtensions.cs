using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;

namespace AutomationSystem.Main.Core.Classes.Data.Extensions
{
    public static class ClassActiveExtensions
    {
        public static IQueryable<ClassPerson> ActiveInActiveClass (this IQueryable<ClassPerson> query)
        {
            return query.Active().Where(x => !x.Class.Deleted);
        }
    }
}
