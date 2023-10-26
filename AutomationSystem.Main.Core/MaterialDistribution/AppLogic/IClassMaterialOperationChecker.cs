using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    public interface IClassMaterialOperationChecker
    {
        ClassMaterialState GetClassMaterialState(Class cls, ClassMaterial material);

        bool IsOperationAllowed(ClassMaterialOperation materialOperation, ClassMaterialState materialState);

        bool IsOperationDisabled(ClassMaterialOperation materialOperation, ClassMaterialState materialState);

        ClassMaterialState CheckOperation(ClassMaterialOperation operation, Class cls, ClassMaterial material);

        void CheckOperation(ClassMaterialOperation operation, ClassMaterialState state, long classId);
    }
}
