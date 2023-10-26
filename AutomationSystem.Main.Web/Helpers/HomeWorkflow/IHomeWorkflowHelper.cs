
namespace AutomationSystem.Main.Web.Helpers.HomeWorkflow
{
    /// <summary>
    /// Home workflow controller helper
    /// </summary>
    public interface IHomeWorkflowHelper
    {
        // gets next controller action
        WorkflowAction GetNextAction();

        // gets previous controller action
        WorkflowAction GetPreviousAction();
    }

}