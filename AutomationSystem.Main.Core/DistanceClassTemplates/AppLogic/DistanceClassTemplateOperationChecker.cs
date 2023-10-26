using System;
using System.Collections.Generic;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateOperationChecker : IDistanceClassTemplateOperationChecker
    {
        private readonly IDictionary<DistanceClassTemplateOperation, HashSet<DistanceClassTemplateState>> operationMap;

        public DistanceClassTemplateOperationChecker()
        {
            operationMap = new Dictionary<DistanceClassTemplateOperation, HashSet<DistanceClassTemplateState>>
            {
                {
                    DistanceClassTemplateOperation.Edit,
                    new HashSet<DistanceClassTemplateState>
                    {
                        DistanceClassTemplateState.New,
                        DistanceClassTemplateState.Approved
                    }
                },
                {
                    DistanceClassTemplateOperation.Approve,
                    new HashSet<DistanceClassTemplateState>
                    {
                        DistanceClassTemplateState.New
                    }
                },
                {
                    DistanceClassTemplateOperation.Complete,
                    new HashSet<DistanceClassTemplateState>
                    {
                        DistanceClassTemplateState.Approved
                    }
                },
                {
                    DistanceClassTemplateOperation.Delete,
                    new HashSet<DistanceClassTemplateState>
                    {
                        DistanceClassTemplateState.New
                    }
                }
            };
        }

        public void CheckOperation(DistanceClassTemplateOperation operation, DistanceClassTemplateState state)
        {
            if (!IsOperationAllowed(operation, state))
            {
                throw new InvalidOperationException($"Operation {operation} is not allowed in distance class template state {state}.");
            }
        }

        public bool IsOperationAllowed(DistanceClassTemplateOperation operation, DistanceClassTemplateState state)
        {
            if (!operationMap.TryGetValue(operation, out var allowedStates))
            {
                throw new ArgumentException($"Unknown distance class template operation {operation}.");
            }

            return allowedStates.Contains(state);
        }
    }
}
