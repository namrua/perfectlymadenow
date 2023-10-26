using System;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Determines home workflow binar property
    /// </summary>
    [Flags]
    public enum HomeWorkflowProperty
    {
        None = 0,        
        IsReviewRegistration = 1 << 0,                  // is review registration
        AgreementAccepted = 1 << 1,                     // agreement accepted
        NeedManualReviewing = 1 << 2,                   // needs manual registration
        PaymentOnly = 1 << 3,                           // determines that registration can be only payed
        IsInvitation = 1 << 4,                          // determines whether registration is invitation
        PublicPaymentAllowed = 1 << 5,                  // determines whether public payment is allowed
    }
}