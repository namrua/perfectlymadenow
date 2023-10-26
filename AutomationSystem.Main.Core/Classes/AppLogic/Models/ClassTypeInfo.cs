using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Models
{
    /// <summary>
    /// Encapsulates information about ClassType
    /// </summary>
    public class ClassTypeInfo {

        public ClassTypeEnum Id { get; }
        public ClassTypeCategory Category { get; }
        public ClassTypeTopic Topic { get; }
        public ClassTypeShape Shape { get; }

        // constructor
        public ClassTypeInfo(ClassTypeEnum id, ClassTypeCategory category, ClassTypeTopic topic, ClassTypeShape shape)
        {
            Id = id;
            Category = category;
            Topic = topic;
            Shape = shape;
        }

    }
}