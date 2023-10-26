using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class ClassOperationChecker : IClassOperationChecker
    {
        private readonly IDictionary<ClassOperation, HashSet<ClassState>> operationStateMap;

        public ClassOperationChecker()
        {
            operationStateMap = new Dictionary<ClassOperation, HashSet<ClassState>>
            {
                {
                    ClassOperation.AddRegistration,
                    new HashSet<ClassState>
                    {
                        ClassState.InRegistration, ClassState.AfterRegistration
                    }
                },
                {
                    ClassOperation.CreateActionCancelation,
                    new HashSet<ClassState>
                    {
                        ClassState.InRegistration, ClassState.AfterRegistration
                    }
                },
                {
                    ClassOperation.CreateActionChange,
                    new HashSet<ClassState>
                    {
                        ClassState.InRegistration, ClassState.AfterRegistration
                    }
                },
                {
                    ClassOperation.CreateActionCompletion,
                    new HashSet<ClassState>
                    {
                        ClassState.AfterRegistration
                    }
                },
                {
                    ClassOperation.DeleteClass,
                    new HashSet<ClassState>
                    {
                        ClassState.New, ClassState.InRegistration, ClassState.AfterRegistration
                    }
                },
                {
                    ClassOperation.EditClass,
                    new HashSet<ClassState>
                    {
                        ClassState.New, ClassState.InRegistration, ClassState.AfterRegistration
                    }
                },
                {
                    ClassOperation.EditPayment,
                    new HashSet<ClassState>
                    {
                        ClassState.InRegistration, ClassState.AfterRegistration, ClassState.Completed
                    }
                },
                {
                    ClassOperation.EditRegistration,
                    new HashSet<ClassState>
                    {
                        ClassState.InRegistration, ClassState.AfterRegistration, ClassState.Completed
                    }
                },
                {
                    ClassOperation.FullEditClass,
                    new HashSet<ClassState>
                    {
                        ClassState.New
                    }
                },
                {
                    ClassOperation.Invitation,
                    new HashSet<ClassState>
                    {
                        ClassState.InRegistration
                    }
                },
                {
                    ClassOperation.MaterialDistribution,
                    new HashSet<ClassState>
                    {
                        ClassState.InRegistration, ClassState.AfterRegistration
                    }
                },
                {
                    ClassOperation.PublicRegistration,
                    new HashSet<ClassState>
                    {
                        ClassState.InRegistration
                    }
                }
            };
        }


        public ClassState CheckOperation(ClassOperation operation, Class cls)
        {
            var result = ClassConvertor.GetClassState(cls);
            CheckOperation(operation, result, cls.ClassId);
            return result;
        }

        public void CheckOperation(ClassOperation operation, ClassState state, long classId)
        {
            if (IsOperationDisabled(operation, state))
            {
                throw new InvalidOperationException($"Operation {operation} is not allowed for Class with id {classId} and state {state}.");
            }
        }

        public void CheckPublicRegistrationForHomeService(Class cls)
        {
            var classState = ClassConvertor.GetClassState(cls);
            if (IsOperationDisabled(ClassOperation.PublicRegistration, classState))
            {
                throw HomeServiceException.NewInvalidClassState(classState).AddId(classId: cls.ClassId);
            }
        }

        public ClassOperation ConvertToClassOperation(ClassActionTypeEnum classActionTypeId)
        {
            switch (classActionTypeId)
            {
                case ClassActionTypeEnum.Completion:
                    return ClassOperation.CreateActionCompletion;

                case ClassActionTypeEnum.Cancelation:
                    return ClassOperation.CreateActionCancelation;

                case ClassActionTypeEnum.Change:
                case ClassActionTypeEnum.WwaChange:
                    return ClassOperation.CreateActionChange;

                default:
                    throw new ArgumentOutOfRangeException(nameof(classActionTypeId), classActionTypeId, null);
            }
        }

        public bool IsOperationAllowed(ClassOperation classOperation, ClassState classState)
        {
            if (!operationStateMap.TryGetValue(classOperation, out var allowedStates))
            {
                throw new ArgumentOutOfRangeException(nameof(classOperation));
            }
            return allowedStates.Contains(classState);
        }

        public bool IsOperationDisabled(ClassOperation classOperation, ClassState classState)
        {
            var isAllowed = IsOperationAllowed(classOperation, classState);
            return !isAllowed;
        }
    }
}
