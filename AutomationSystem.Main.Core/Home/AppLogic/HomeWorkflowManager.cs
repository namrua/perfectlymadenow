using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Home.AppLogic
{

    /// <summary>
    ///  Manages home workflow 
    /// </summary>
    public class HomeWorkflowManager : IHomeWorkflowManager
    {
        private readonly IPublicPaymentResolver publicPaymentResolver;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public HomeWorkflowManager(IPublicPaymentResolver publicPaymentResolver, IRegistrationTypeResolver registrationTypeResolver)
        {
            this.publicPaymentResolver = publicPaymentResolver;
            this.registrationTypeResolver = registrationTypeResolver;
        }

        // converts registration to workflow stage and checks all properties for stage
        public HomeWorkflowState GetHomeWorkflowState(ClassRegistration registration, HomeWorkflowStage stage, HomeWorkflowProperty propertyInfo = HomeWorkflowProperty.None)
        {
            var result = InitialzeHomeWorkflowState(registration, stage, propertyInfo);
            switch (stage)
            {
                case HomeWorkflowStage.PersonalDataSave:
                case HomeWorkflowStage.Confirmation:
                case HomeWorkflowStage.AgreementAcceptation:
                case HomeWorkflowStage.Payment:
                    break;

                default:
                    throw new NotImplementedException($"Stage {stage} is not supported by home workflow manager.");
            }
            return result;
        }


        // checks registration workflow state
        public void CheckForWorkflowState(ClassRegistration registration, HomeWorkflowStage stage)
        {
            var workflowState = InitialzeHomeWorkflowState(registration, stage);

            // check for completnes
            if (registration.IsApproved || registration.IsCanceled)
                throw HomeServiceException.New(HomeServiceErrorType.RegistrationComplete)
                    .AddId(classRegistrationId: registration.ClassRegistrationId, classId: registration.ClassId);

            // for classes where public payment is not allowed, only invitations are allowed
            if (!workflowState.Properties.HasFlag(HomeWorkflowProperty.PublicPaymentAllowed)
                && !workflowState.Properties.HasFlag(HomeWorkflowProperty.IsInvitation))
            {
                throw HomeServiceException.New(HomeServiceErrorType.InvalidRegistrationStep, "Public payment not allowed.")
                    .AddId(classRegistrationId: registration.ClassRegistrationId, homeWorkflowStage: stage,
                        classId: registration.ClassId);
            }
            
            // payment vs. non-payment
            if (stage == HomeWorkflowStage.Payment)
            {
                // payment stage - not accepted
                if (!workflowState.Properties.HasFlag(HomeWorkflowProperty.AgreementAccepted))
                    throw HomeServiceException.New(HomeServiceErrorType.InvalidRegistrationStep, "Not accepted when payed.")
                        .AddId(classRegistrationId: registration.ClassRegistrationId, homeWorkflowStage: stage, classId: registration.ClassId);

                // payment stage - needs reviewing
                if (workflowState.Properties.HasFlag(HomeWorkflowProperty.NeedManualReviewing))
                    throw HomeServiceException.New(HomeServiceErrorType.InvalidRegistrationStep, "Not reviewed when payed.")
                        .AddId(classRegistrationId: registration.ClassRegistrationId, homeWorkflowStage: stage, classId: registration.ClassId);

                // payment staget - invitation
                if (workflowState.Properties.HasFlag(HomeWorkflowProperty.IsInvitation))
                    throw HomeServiceException.New(HomeServiceErrorType.InvalidRegistrationStep, "Is invitation.")
                        .AddId(classRegistrationId: registration.ClassRegistrationId, homeWorkflowStage: stage, classId: registration.ClassId);
            }
            else
            {
                // non-payment stage - payment only
                if (workflowState.Properties.HasFlag(HomeWorkflowProperty.PaymentOnly))
                    throw HomeServiceException.New(HomeServiceErrorType.InvalidRegistrationStep, "Only payment allowed")
                        .AddId(classRegistrationId: registration.ClassRegistrationId, homeWorkflowStage: stage, classId: registration.ClassId);
            }
            
            // review vs. non-review
            if (stage == HomeWorkflowStage.FormerStudentSelection)
            {
                // review stage
                if (!workflowState.Properties.HasFlag(HomeWorkflowProperty.IsReviewRegistration))
                    throw HomeServiceException.New(HomeServiceErrorType.InvalidRegistrationStep, "Not reviewed registration")
                        .AddId(classRegistrationId: registration.ClassRegistrationId, homeWorkflowStage: stage, classId: registration.ClassId);
            }
            else
            {
                // non-review stage
            }

            // confirmation and agreement stage
            if (stage == HomeWorkflowStage.Agreement || stage == HomeWorkflowStage.AgreementAcceptation || stage == HomeWorkflowStage.Confirmation)
            {
                if (workflowState.Properties.HasFlag(HomeWorkflowProperty.IsReviewRegistration) &&
                    registration.FormerStudentId == null && registration.ClassRegistrationLastClassId == null)
                {
                    throw HomeServiceException.New(HomeServiceErrorType.InvalidRegistrationStep, "No former student or last class filled")
                        .AddId(classRegistrationId: registration.ClassRegistrationId, homeWorkflowStage: stage, classId: registration.ClassId);
                }
            }

        }

        #region private methods

        // initializes Home workflow state
        private HomeWorkflowState InitialzeHomeWorkflowState(ClassRegistration registration, HomeWorkflowStage stage,
            HomeWorkflowProperty propertyInfo = HomeWorkflowProperty.None)
        {
            if (registration.Class == null)
            {
                throw new InvalidOperationException("Class is not included into ClassRegistration object.");
            }

            var needsReview = registrationTypeResolver.NeedsReview(registration.RegistrationTypeId, registration.Class.ClassTypeId);
            var result = new HomeWorkflowState
            {
                ClassRegistrationId = registration.ClassRegistrationId,
                Stage = stage,
                Type = needsReview ? HomeWorkflowType.ReviewedRegistration : HomeWorkflowType.NormalRegistration,
                Properties = propertyInfo,
            };

            if (needsReview)
            {
                result.Properties |= HomeWorkflowProperty.IsReviewRegistration;
            }

            if (needsReview && !registration.IsReviewed)
            {
                result.Properties |= HomeWorkflowProperty.NeedManualReviewing;
            }

            if (registration.AreAgreementsAccepted)
            {
                result.Properties |= HomeWorkflowProperty.AgreementAccepted;
            }

            if (!registration.IsTemporary && registration.ApprovementTypeId == ApprovementTypeEnum.ManualReview)
            {
                result.Properties |= HomeWorkflowProperty.PaymentOnly;
            }

            if (registration.ApprovementTypeId == ApprovementTypeEnum.InvitationApprovement)
            {
                result.Properties |= HomeWorkflowProperty.IsInvitation;
            }

            if (publicPaymentResolver.IsPublicPaymentAllowedForClass(registration.Class))
            {
                result.Properties |= HomeWorkflowProperty.PublicPaymentAllowed;
            }

            return result;
        }

        #endregion

    }

}
