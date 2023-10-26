using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System
{
    public interface IClassActionService
    {
        long CreateClassAction(Class cls, ClassActionTypeEnum classActionTypeId);

        void ProcessClassAction(long classActionId);

        void ProcessClassAction(ClassAction classAction);
    }
}
