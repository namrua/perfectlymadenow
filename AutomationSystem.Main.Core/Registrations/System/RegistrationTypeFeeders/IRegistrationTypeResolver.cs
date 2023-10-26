using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;

namespace AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders
{
    public interface IRegistrationTypeResolver
    {
        List<RegistrationTypeEnum> GetRegistrationTypeByClassCategoryId(ClassCategoryEnum classCategoryId);

        List<RegistrationTypeEnum> GetRegistrationTypeByClassCategoryIdAndTopic(ClassCategoryEnum classCategoryId, ClassTypeTopic topic);

        HashSet<RegistrationTypeEnum> GetNewRegistrationTypes();

        RegistrationFormTypeEnum GetRegistrationFormType(RegistrationTypeEnum registrationTypeId);

        bool IsNewRegistration(RegistrationTypeEnum registrationTypeId);

        bool IsReviewedRegistration(RegistrationTypeEnum registrationTypeId);

        bool NeedsReview(RegistrationTypeEnum registrationTypeId, ClassTypeEnum classTypeId);

        bool IsWwaRegistration(RegistrationTypeEnum registrationTypeId);

        string GetRegistrationTypeCode(RegistrationTypeEnum registrationTypeId, ClassCategoryEnum? classCategoryId = null);
    }
}
