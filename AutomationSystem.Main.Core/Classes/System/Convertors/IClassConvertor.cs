using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System.Convertors
{
    /// <summary>
    /// Converts class objects
    /// </summary>
    public interface IClassConvertor
    {
        // merges class filter and profile filter
        ClassFilter MergeProfileFilterToClassFilter(ClassFilter classFilter, ProfileFilter profileFilter, HashSet<ClassCategoryEnum> grantedClassCategories);

        // initializes class for edit
        ClassForEdit InitializeClassForEdit(long profileId, ClassCategoryEnum classCategoryId, CurrencyEnum currencyId, long? currentPriceListId = null, 
            long? currentPayPalKeyId = null, long? currentIntegrationCode = null, ClassValidationResult classValidationResult = null);

        // converts class to class form
        ClassForm ConvertToClassForm(Class cls);

        // converts class to class list item
        ClassListItem ConvertToClassListItem(Class cls);

        // converts class to class detail
        ClassDetail ConvertToClassDetail(Class cls);

        // converts class form to class
        Class ConvertToClass(ClassForm form);       
        
        // gets no integration IntegrationCode
        long GetNoIntegrationCode();
    }
}
