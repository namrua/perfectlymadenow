using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.Connectors.Models;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;
using PerfectlyMadeInc.WebEx.IntegrationStates.AppLogic;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{

    /// <summary>
    /// WebEx consistency resolver
    /// </summary>
    public class ConsistencyResolver : IConsistencyResolver
    {
        private readonly IIntegrationStateConvertor integrationStateConvertor;

        public ConsistencyResolver(IIntegrationStateConvertor integrationStateConvertor)
        {
            this.integrationStateConvertor = integrationStateConvertor;
        }


        // compares two integration states
        public ConsistencyResult Compare(IntegrationState systemState, IntegrationState webExState)
        {
            if(systemState == null) throw new ArgumentNullException(nameof(systemState));
            if(webExState == null) throw new ArgumentNullException(nameof(webExState));

            // creates result
            var result = new ConsistencyResult
            {
                EntityId = systemState.EntityId != 0 ? (long?)systemState.EntityId : null,
                SystemState = integrationStateConvertor.ConvertToIntegrationStateDto(systemState),
                WebExState = integrationStateConvertor.ConvertToIntegrationStateDto(webExState),
                HasError = webExState.IntegrationStateTypeId == IntegrationStateTypeEnum.Error,
                ErrorMessage = webExState.ErrorMessage                        
            };

            // resolves inconsistencies
            if (!result.HasError)
            {
                result.InconsistencyType = ResolveInconsistencyType(result);
                result.IsInconsistent = result.InconsistencyType != InconsistencyType.None;
            }                

            // resolves operation
            result.OperationType = ResolveOperationType(result);
            return result;
        }


        #region private fields

        // resolves inconsistency type (sets inconsistent fields)
        private InconsistencyType ResolveInconsistencyType(ConsistencyResult consistencyResult)
        {
            // registration should be in webex but it does not
            if (consistencyResult.SystemState.IntegrationStateTypeId == IntegrationStateTypeEnum.InWebEx
                && consistencyResult.WebExState.IntegrationStateTypeId == IntegrationStateTypeEnum.NotInWebEx)
            {
                return InconsistencyType.NotInWebEx;
            }

            // registration is in the webex but it is not in the system
            if (consistencyResult.SystemState.IntegrationStateTypeId == IntegrationStateTypeEnum.NotInWebEx
                && consistencyResult.WebExState.IntegrationStateTypeId == IntegrationStateTypeEnum.InWebEx)
            {
                return InconsistencyType.NotInSystem;
            }

            // check for data consistency
            if (consistencyResult.SystemState.IntegrationStateTypeId == IntegrationStateTypeEnum.InWebEx
                && consistencyResult.WebExState.IntegrationStateTypeId == IntegrationStateTypeEnum.InWebEx)
            {
                consistencyResult.InconsistentFields = GetInconsistentFields(consistencyResult.SystemState, consistencyResult.WebExState);
                if (consistencyResult.InconsistentFields.Count > 0)
                {
                   
                    return InconsistencyType.InconsistentData;
                }
            }
            return InconsistencyType.None;
        }

        // resolvers operation type by consistency result
        private SyncOperationType ResolveOperationType(ConsistencyResult consistencyResult)
        {
            // executes adding operation if needed
            if (consistencyResult.InconsistencyType == InconsistencyType.NotInWebEx
                || consistencyResult.InconsistencyType == InconsistencyType.InconsistentData
                || consistencyResult.HasError && consistencyResult.SystemState.IntegrationStateTypeId == IntegrationStateTypeEnum.InWebEx)
            {
                return SyncOperationType.Save;
            }

            // executes removing operation if needed
            if (consistencyResult.InconsistencyType == InconsistencyType.NotInSystem)
            {
                return SyncOperationType.Remove;
            }

            return SyncOperationType.None;
        }

        // checks data consistency
        private List<InconsistentField> GetInconsistentFields(IntegrationStateDto systemState, IntegrationStateDto webExState)
        {
            var result = new List<InconsistentField>();
            IsDifferent(result, "First name", systemState.FirstName, webExState.FirstName);
            IsDifferent(result, "Last name", systemState.LastName, webExState.LastName);
            IsDifferent(result, "Address line 1", systemState.Street, webExState.Street);
            IsDifferent(result, "Address line 2", systemState.Street2, webExState.Street2);
            IsDifferent(result, "City", systemState.City, webExState.City);
            IsDifferent(result, "State", systemState.State, webExState.State);
            IsDifferent(result, "Country", systemState.Country, webExState.Country);
            IsDifferent(result, "Zip code", systemState.ZipCode, webExState.ZipCode);
            IsDifferent(result, "Email", systemState.Email, webExState.Email, ignoreCase: true);            
            return result;
        }

        // field comparer
        private bool IsDifferent(List<InconsistentField> inconsistentFields, string fieldName, string systemValue, string webExValue, bool ignoreCase = false)
        {
            if (ignoreCase)
            {
                systemValue = systemValue.ToLower();
                webExValue = webExValue.ToLower();
            }
            var result = systemValue != webExValue;
            if (result)
                inconsistentFields.Add(new InconsistentField(fieldName, systemValue, webExValue));
            return result;
        }

        #endregion

    }

}
