using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base
{
    /// <summary>
    /// Encapsulates full registration state
    /// </summary>
    public class RegistrationFullState
    {

        public ClassState ClassState { get; set; }
        public RegistrationState RegistrationState { get; set; }
        public ApprovementTypeEnum ApprovementTypeId { get; set; }
        public bool? IsReviewed { get; set; }


        #region factory methods

        // creates new registration full state
        public static RegistrationFullState New(ClassState classState, RegistrationState registrationState,
            ApprovementTypeEnum approvementTypeId, bool? isReviewd)
        {
            var result = new RegistrationFullState
            {
                ClassState = classState,
                RegistrationState = registrationState,
                ApprovementTypeId = approvementTypeId,
                IsReviewed = isReviewd
            };
            return result;
        }

        #endregion

    }

}
