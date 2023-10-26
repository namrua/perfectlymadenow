using System.Data.Entity.Infrastructure;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.Data.Extensions
{
    public static class ClassIncludeExtensions
    {
        public static DbQuery<Class> AddIncludes(this DbQuery<Class> query, ClassIncludes includes)
        {
            if (includes.HasFlag(ClassIncludes.ClassType))
                query = query.Include("ClassType");
            if (includes.HasFlag(ClassIncludes.TimeZone))
                query = query.Include("TimeZone");
            if (includes.HasFlag(ClassIncludes.ClassPersons))
                query = query.Include("ClassPersons");
            if (includes.HasFlag(ClassIncludes.PriceList))
                query = query.Include("PriceList");
            if (includes.HasFlag(ClassIncludes.PriceListPriceListItems))
                query = query.Include("PriceList.PriceListItems");
            if (includes.HasFlag(ClassIncludes.ClassActions))
                query = query.Include("ClassActions");
            if (includes.HasFlag(ClassIncludes.ClassActionsClassActionType))
                query = query.Include("ClassActions.ClassActionType");
            if (includes.HasFlag(ClassIncludes.ClassBusiness))
                query = query.Include("ClassBusiness");
            if (includes.HasFlag(ClassIncludes.ClassBusinessClassExpenses))
                query = query.Include("ClassBusiness.ClassExpenses");
            if (includes.HasFlag(ClassIncludes.ClassReportSetting))
                query = query.Include("ClassReportSetting");
            if (includes.HasFlag(ClassIncludes.ClassStyle))
                query = query.Include("ClassStyle");
            if (includes.HasFlag(ClassIncludes.Profile))
                query = query.Include("Profile");
            if (includes.HasFlag(ClassIncludes.Currency))
                query = query.Include("Currency");

            return query;
        }

        public static DbQuery<ClassAction> AddIncludes(this DbQuery<ClassAction> query, ClassActionIncludes includes)
        {
            if (includes.HasFlag(ClassActionIncludes.Class))
                query = query.Include("Class");
            if (includes.HasFlag(ClassActionIncludes.ClassActionType))
                query = query.Include("ClassActionType");
            if (includes.HasFlag(ClassActionIncludes.ClassClassStyle))
                query = query.Include("Class.ClassStyle");
            return query;
        }
    }
}
