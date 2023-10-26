namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Encapsulates home workflow full state
    /// </summary>
    public class HomeWorkflowState
    {
        public long ClassRegistrationId { get; set; }
        public HomeWorkflowType Type { get; set; }
        public HomeWorkflowStage Stage { get; set; }
        public HomeWorkflowProperty Properties { get; set; }
    }
}