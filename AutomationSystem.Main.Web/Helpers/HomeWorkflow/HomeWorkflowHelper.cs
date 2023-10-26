using System;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Web.Controllers;

namespace AutomationSystem.Main.Web.Helpers.HomeWorkflow
{
    /// <summary>
    /// Homew workflow controller helper
    /// </summary>
    public class HomeWorkflowHelper : IHomeWorkflowHelper
    {
        public const string ViewBagKey = "HomeWorkflowHelper";

        private readonly HomeController c;
        private readonly HomeWorkflowState state;


        // constructor
        public HomeWorkflowHelper(HomeController controller, HomeWorkflowState state)
        {
            this.c = controller;
            this.state = state;
        }

        // gets next controller action
        public WorkflowAction GetNextAction()
        {
            var result = new WorkflowAction();
            switch (state.Stage)
            {
                // action after saving of form data
                case HomeWorkflowStage.PersonalDataSave:
                    if (state.Type == HomeWorkflowType.ReviewedRegistration)
                    {                       
                        result.Link = c.Url.Action("SelectStudent", "Home", new {id = state.ClassRegistrationId });
                        break;
                    }
                    result.Link = c.Url.Action("Confirmation", "Home", new {id = state.ClassRegistrationId});
                    break;

                // action after agreement acceptation
                case HomeWorkflowStage.AgreementAcceptation:
                    if (!state.Properties.HasFlag(HomeWorkflowProperty.AgreementAccepted))
                    {
                        result.Link = c.Url.Action("Agreement", "Home", new {id = state.ClassRegistrationId});
                        break;
                    }
                    if (state.Properties.HasFlag(HomeWorkflowProperty.IsInvitation))
                    {
                        result.Link = c.Url.Action("Complete", "Home", new { classRegistrationId = state.ClassRegistrationId });
                        break;
                    }
                    var manualReviewing = state.Properties.HasFlag(HomeWorkflowProperty.NeedManualReviewing);
                    result.Link = manualReviewing
                        ? c.Url.Action("ManualReview", "Home", new { classRegistrationId = state.ClassRegistrationId })
                        : c.Url.Action("Payment", "Home", new {id = state.ClassRegistrationId});
                    break;

                default:
                    throw new NotImplementedException("Stage is not supported by HomeWorkflowHelper.GetNewAction().");
            }        
            return result;
        }

        // gets previous controller action
        public WorkflowAction GetPreviousAction()
        {
            switch (state.Stage)
            {
                default:
                    throw new NotImplementedException("Stage is not supported by HomeWorkflowHelper.GetPreviousAction().");
            }
        }

        #region static members

        // creates workflow helper by state
        public static IHomeWorkflowHelper Create(HomeController controller, HomeWorkflowState state)
        {
            var result = new HomeWorkflowHelper(controller, state);
            return result;
        }

        // gets workflow helper from viewbag
        public static IHomeWorkflowHelper GetHomeWorkflowHelper(dynamic viewBag)
        {
            var result = viewBag[ViewBagKey];
            if (result == null)
                throw new InvalidOperationException("Home workflow helper is not initialized.");
            return result;
        }

        #endregion
    }

}