using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public interface IClassOperationChecker
    {
        ClassOperation ConvertToClassOperation(ClassActionTypeEnum classActionTypeId);

        bool IsOperationAllowed(ClassOperation classOperation, ClassState classState);

        bool IsOperationDisabled(ClassOperation classOperation, ClassState classState);

        ClassState CheckOperation(ClassOperation operation, Class cls);

        void CheckOperation(ClassOperation operation, ClassState state, long classId);

        void CheckPublicRegistrationForHomeService(Class cls);
    }
}
