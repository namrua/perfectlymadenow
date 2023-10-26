using System.Linq;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.Classes.Data.Extensions
{
    public static class ClassRemoveInactive
    {
        public static Class RemoveInactiveForClass(Class entity, ClassIncludes includes)
        {
            if (entity == null)
                return null;
            if (includes.HasFlag(ClassIncludes.ClassPersons))
                entity.ClassPersons = entity.ClassPersons.AsQueryable().Active().ToList();
            if (includes.HasFlag(ClassIncludes.ClassActions)
                || includes.HasFlag(ClassIncludes.ClassActionsClassActionType))
                entity.ClassActions = entity.ClassActions.AsQueryable().Active().ToList();
            if (includes.HasFlag(ClassIncludes.ClassBusinessClassExpenses))
                entity.ClassBusiness.ClassExpenses = entity.ClassBusiness.ClassExpenses.AsQueryable().Active().ToList();
            if (includes.HasFlag(ClassIncludes.PriceListPriceListItems))
                entity.PriceList.PriceListItems = entity.PriceList.PriceListItems.AsQueryable().Active().ToList();
            return entity;
        }

    }
}
