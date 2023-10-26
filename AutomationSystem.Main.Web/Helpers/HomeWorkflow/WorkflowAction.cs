
namespace AutomationSystem.Main.Web.Helpers.HomeWorkflow
{
    /// <summary>
    /// Encapsulates workflow aciton
    /// </summary>
    public class WorkflowAction
    {

        public string Link { get; set; }
        public string Title { get; set; }
        public bool IsDisabled { get; set; }

        #region factory methods

        // creates new workflow action
        public static WorkflowAction New(string link, string title = null)
        {
            var result = new WorkflowAction();
            result.Link = link;
            result.Title = title;
            return result;
        }

        // disables action
        public WorkflowAction Disable()
        {
            IsDisabled = true;
            return this;
        }

        #endregion

    }

}