using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Home.AppLogic
{
    /// <summary>
    /// Manages home workflow 
    /// </summary>
    public interface IHomeWorkflowManager
    {
        // converts registration to workflow stage and checks all properties for stage
        HomeWorkflowState GetHomeWorkflowState(ClassRegistration registration, HomeWorkflowStage stage, HomeWorkflowProperty propertyInfo = HomeWorkflowProperty.None);

        // checks registration workflow state
        void CheckForWorkflowState(ClassRegistration registration, HomeWorkflowStage stage);
    }
}