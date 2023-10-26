using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models
{
    /// <summary>
    /// Empty class page model for empty tab
    /// </summary>
    public class EmptyClassPageModel
    {
        public ClassShortDetail Class { get; set; }
        public string Message { get; set; }
        public string TabItemId { get; set; }

        // constructor
        public EmptyClassPageModel(ClassShortDetail cls, string message, string tabItemId)
        {
            Class = cls;
            Message = message;
            TabItemId = tabItemId;
        }
    }
}