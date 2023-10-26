using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Core.Classes.System.Model;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System
{
    public interface IClassService
    {
        ClassForm GetNewClassForm(long profileId, ClassCategoryEnum classCategoryId, CurrencyEnum currencyId, IPersonHelper personHelper);

        List<ClassWithClassForm> GetClassesWithClassFormsByIds(List<long> classIds);

        long InsertClass(ClassForm form, EnvironmentTypeEnum? env);

        long InsertClass(ClassForm form, CurrencyEnum currencyId, ClassPreference classPreference, EnvironmentTypeEnum? env);

        void UpdateClass(ClassForm form, Class originClass, bool isFullyEditable);

        void UpdateClass(ClassForm form, CurrencyEnum currencyId, Class originClass, bool isFullyEditable);
    }
}
