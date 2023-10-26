using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Actions;

namespace AutomationSystem.Main.Contract.Classes.AppLogic
{
    public interface IClassActionAdministration
    {
        ClassActionPageModel GetClassActionPageModel(long classId);

        ClassActionDetail GetClassActionDetail(long classActionId);
        
        long CreateClassAction(long classId, ClassActionTypeEnum classActionTypeId);

        void ProcessClassAction(long classActionId);

        long? DeleteClassAction(long classActionId);
    }
}
