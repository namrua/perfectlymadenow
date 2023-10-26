using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;
using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    public class ClassMaterialOperationChecker : IClassMaterialOperationChecker
    {
        private readonly IClassMaterialBusinessRules classMaterialBusinessRules;
        private readonly IDictionary<ClassMaterialOperation, HashSet<ClassMaterialState>> operationStateMap;

        public ClassMaterialOperationChecker(IClassMaterialBusinessRules classMaterialBusinessRules)
        {
            this.classMaterialBusinessRules = classMaterialBusinessRules;


            operationStateMap = new Dictionary<ClassMaterialOperation, HashSet<ClassMaterialState>>
            {
                {
                    ClassMaterialOperation.InitializeMaterialRecipient,
                    new HashSet<ClassMaterialState>
                    {
                        ClassMaterialState.Locked, ClassMaterialState.LockedByAutolock, ClassMaterialState.LockedByEndOfClass, ClassMaterialState.Unlocked
                    }
                },
                {
                    ClassMaterialOperation.Lock,
                    new HashSet<ClassMaterialState>
                    {
                        ClassMaterialState.Unlocked
                    }
                },
                {
                    ClassMaterialOperation.LockRecipient,
                    new HashSet<ClassMaterialState>
                    {
                        ClassMaterialState.Locked, ClassMaterialState.LockedByAutolock, ClassMaterialState.Unlocked
                    }
                },
                {
                    ClassMaterialOperation.SendNotification,
                    new HashSet<ClassMaterialState>
                    {
                        ClassMaterialState.Unlocked
                    }
                },
                {
                    ClassMaterialOperation.SendNotificationToRecipient,
                    new HashSet<ClassMaterialState>
                    {
                        ClassMaterialState.Unlocked
                    }
                },
                {
                    ClassMaterialOperation.Unlock,
                    new HashSet<ClassMaterialState>
                    {
                        ClassMaterialState.PreparingStage, ClassMaterialState.Locked
                    }
                },
                {
                    ClassMaterialOperation.UnlockRecipient,
                    new HashSet<ClassMaterialState>
                    {
                        ClassMaterialState.Locked, ClassMaterialState.LockedByAutolock, ClassMaterialState.Unlocked
                    }
                }
            };
        }

        public ClassMaterialState GetClassMaterialState(Class cls, ClassMaterial material)
        {
            var utcNow = DateTime.UtcNow;

            if (classMaterialBusinessRules.IsLockedByClassEndDate(cls.EventEndUtc, utcNow))
            {
                return ClassMaterialState.LockedByEndOfClass;
            }

            if (material.AutomationLockTimeUtc.HasValue && material.AutomationLockTimeUtc.Value < utcNow)
            {
                return ClassMaterialState.LockedByAutolock;
            }

            if (material.IsLocked)
            {
                return ClassMaterialState.Locked;
            }

            if (material.IsUnlocked)
            {
                return ClassMaterialState.Unlocked;
            }

            return ClassMaterialState.PreparingStage;
        }
        
        public bool IsOperationAllowed(ClassMaterialOperation materialOperation, ClassMaterialState materialState)
        {
            if (!operationStateMap.TryGetValue(materialOperation, out var allowedStates))
                throw new ArgumentOutOfRangeException(nameof(materialOperation));
            return allowedStates.Contains(materialState);
        }
        
        public bool IsOperationDisabled(ClassMaterialOperation materialOperation, ClassMaterialState materialState)
        {
            var isAllowed = IsOperationAllowed(materialOperation, materialState);
            return !isAllowed;
        }
        
        public ClassMaterialState CheckOperation(ClassMaterialOperation operation, Class cls, ClassMaterial material)
        {
            var result = GetClassMaterialState(cls, material);
            CheckOperation(operation, result, cls.ClassId);
            return result;
        }
        
        public void CheckOperation(ClassMaterialOperation operation, ClassMaterialState state, long classId)
        {
            if (IsOperationDisabled(operation, state))
                throw new InvalidOperationException($"Operaton {operation} is not allowed for materials with state {state} related to class with id {classId}.");
        }
    }
}
