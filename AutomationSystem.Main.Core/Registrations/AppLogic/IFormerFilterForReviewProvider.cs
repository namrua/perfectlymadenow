using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    /// <summary>
    /// Provides FormerClassFilter for reviewing process
    /// </summary>
    public interface IFormerFilterForReviewProvider
    {
        FormerStudentFilter ResolveFormerStudentFilterForReviewRegistration(
            DateTime registrationCreated,
            RegistrationTypeEnum registrationTypeId,
            ClassTypeEnum classTypeId);

        ReviewFormerClassFilter ResolveReviewFormerClassFilter(
            DateTime registrationCreated,
            RegistrationTypeEnum registrationTypeId,
            ClassTypeEnum classTypeId);
    }
}
