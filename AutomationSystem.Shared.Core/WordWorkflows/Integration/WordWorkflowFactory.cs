using AutomationSystem.Shared.Contract.WordWorkflows.Integration;

namespace AutomationSystem.Shared.Core.WordWorkflows.Integration
{
    public class WordWorkflowFactory : IWordWorkflowFactory
    {
        public IWordWorkflow CreateWordWorkflow()
        {
            return new WordWorkflow();
        }
    }
}
