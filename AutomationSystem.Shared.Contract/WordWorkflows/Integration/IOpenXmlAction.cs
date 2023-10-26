using DocumentFormat.OpenXml.Packaging;

namespace AutomationSystem.Shared.Contract.WordWorkflows.Integration
{
    /// <summary>
    /// Provides Open xml action
    /// </summary>
    public interface IOpenXmlAction
    {

        // executes action
        void Execute(WordprocessingDocument document);

    }

}
