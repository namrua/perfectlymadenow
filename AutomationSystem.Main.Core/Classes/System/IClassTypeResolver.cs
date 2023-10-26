using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Model;
using AutomationSystem.Main.Model;
using System.Collections.Generic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Core.Classes.System
{
    public interface IClassTypeResolver
    {
        #region common methods

        ClassDomainInfo GetClassDomainInfo(Class cls);

        ClassTypeInfo GetClassTypeInfo(ClassTypeEnum classTypeId);

        HashSet<ClassTypeEnum> GetClassTypesByClassCategoryId(ClassCategoryEnum classCategoryId);

        HashSet<ClassTypeEnum> GetAllowedClassTypesForFormerClasses();

        HashSet<ClassTypeEnum> GetAllowedClassTypesForFormerClassFiltering();

        HashSet<ClassTypeEnum> GetSynonymousClassTypesForFormerClassFiltering(ClassTypeEnum? classTypeId);

        ClassTypeEnum? NormalizeClassTypeSynonymForFormerClassFiltering(ClassTypeEnum? classTypeId);

        #endregion

        #region business specific methods

        bool AreMaterialsAllowed(ClassCategoryEnum classCategoryId);

        bool AreCertificatesAllowed(ClassCategoryEnum classCategoryId);

        bool AreCertificatesAllowedForClassPersons(ClassCategoryEnum classCategoryId);

        bool IsSupervisedByMasterCoordinator(ClassCategoryEnum classCategoryId);

        bool AreFinancialFormsAllowed(ClassCategoryEnum classCategoryId);

        bool AreReportsAllowed(ClassCategoryEnum classCategoryId);

        bool ShowClassBehaviorSettings(ClassCategoryEnum classCategoryId);

        bool IsPropagationToFormerClassesAllowed(ClassCategoryEnum classCategoryId);

        bool IsCurrencyAllowed(ClassCategoryEnum classCategoryId);

        bool AreInvitationsAllowed(ClassCategoryEnum classCategoryId);

        bool AreStyleAndBehaviorAllowed(ClassCategoryEnum classCategoryId);

        bool AreAutomationNotificationsAllowed(ClassCategoryEnum classCategoryId);

        ClassFormConfiguration GetClassFormConfigurationByClassCategoryId(ClassCategoryEnum classCategoryId);

        #endregion
    }
}
