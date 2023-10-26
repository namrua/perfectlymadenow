using System.Linq;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.MaterialDistribution.Data.Extensions
{
    public static class ClassMaterialActiveExtensions
    {
        // selects active ClassMaterial entities 
        public static IQueryable<ClassMaterial> ActiveInActiveClass(this IQueryable<ClassMaterial> query)
        {
            return query.Active().Where(x => !x.Class.Deleted);
        }
    }
}
