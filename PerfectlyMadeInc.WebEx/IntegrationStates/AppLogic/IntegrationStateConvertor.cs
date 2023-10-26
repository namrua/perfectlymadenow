using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.Connectors.Models;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;
using PerfectlyMadeInc.WebEx.IntegrationStates.Data;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.IntegrationStates.AppLogic
{

    /// <summary>
    /// Converts WebEx integration entities
    /// </summary>
    public class IntegrationStateConvertor : IIntegrationStateConvertor
    {

        private readonly IIntegrationDatabaseLayer integrationDb;
        private readonly Lazy<Dictionary<IntegrationStateTypeEnum, IntegrationStateType>> stateTypeMap;

        // constructor
        public IntegrationStateConvertor(IIntegrationDatabaseLayer integrationDb)
        {
            this.integrationDb = integrationDb;

            stateTypeMap = new Lazy<Dictionary<IntegrationStateTypeEnum, IntegrationStateType>>(() =>
                integrationDb.GetIntegrationStateTypes().ToDictionary(x => x.IntegrationStateTypeId, y => y));
        }

        // converts integration state to webex integration status detail
        public IntegrationStateDetail ConvertToIntegrationStateDetail(IntegrationStateDto state)
        {           
            var result = new IntegrationStateDetail
            {
                AtendeeId = state.AttendeeId,
                LastChecked = state.LastChecked,
                IntegrationStateTypeId = state.IntegrationStateTypeId,
                IntegrationStateType = stateTypeMap.Value[state.IntegrationStateTypeId].Description,
                ErrorMessage = state.ErrorMessage,
                
                FirstName = state.FirstName,
                LastName = state.LastName,
                Street = state.Street,
                Street2 = state.Street2,
                City = state.City,
                State = state.State,
                Country = state.Country,
                ZipCode = state.ZipCode,
                Email = state.Email
            };
            return result;
        }

        // converts consistency result to WebEx integration state summary
        public IntegrationStateSummary ConvertToIntegrationStateSummary(ConsistencyResult consistencyResult, string eventName)
        {
            var result = new IntegrationStateSummary
            {
                EventName = eventName,
                Detail = ConvertToIntegrationStateDetail(consistencyResult.WebExState),
                HasError = consistencyResult.HasError,
                IsInconsistent = consistencyResult.IsInconsistent,
                InconsistencyType = consistencyResult.InconsistencyType,
                InconsistentFields = consistencyResult.InconsistentFields
            };
            return result;
        }

        // converts IntegrationStateDto to IntegrationState
        public IntegrationState ConvertToIntegrationState(IntegrationStateDto integrationStateDto)
        {
            var result = new IntegrationState
            {
                IntegrationStateId = integrationStateDto.IntegrationStateId,
                EventId = integrationStateDto.EventId,
                EntityTypeId = integrationStateDto.EntityTypeId,
                EntityId = integrationStateDto.EntityId,
                AttendeeId = integrationStateDto.AttendeeId,
                IntegrationStateTypeId = integrationStateDto.IntegrationStateTypeId,
                LastChecked = integrationStateDto.LastChecked,
                ErrorMessage = integrationStateDto.ErrorMessage,
                FirstName = integrationStateDto.FirstName,
                LastName = integrationStateDto.LastName,
                Street = integrationStateDto.Street,
                Street2 = integrationStateDto.Street2,
                City = integrationStateDto.City,
                State = integrationStateDto.State,
                Country = integrationStateDto.Country,
                ZipCode = integrationStateDto.ZipCode,
                Email = integrationStateDto.Email
            };
            return result;
        }

        // converts IntegrationState to IntegrationStateDto
        public IntegrationStateDto ConvertToIntegrationStateDto(IntegrationState integrationState)
        {
            var result = new IntegrationStateDto
            {
                IntegrationStateId = integrationState.IntegrationStateId,
                EventId = integrationState.EventId,
                EntityTypeId = integrationState.EntityTypeId,
                EntityId = integrationState.EntityId,
                AttendeeId = integrationState.AttendeeId,
                IntegrationStateTypeId = integrationState.IntegrationStateTypeId,
                LastChecked = integrationState.LastChecked,
                ErrorMessage = integrationState.ErrorMessage,
                FirstName = integrationState.FirstName,
                LastName = integrationState.LastName,
                Street = integrationState.Street,
                Street2 = integrationState.Street2,
                City = integrationState.City,
                State = integrationState.State,
                Country = integrationState.Country,
                ZipCode = integrationState.ZipCode,
                Email = integrationState.Email
            };
            return result;
        }

        // fills common columns of WebExIntegration state             
        public void FillIntegrationState(IntegrationState state, long integrationStateId, EntityTypeEnum entityTypeId, long entityId, long eventId)
        {
            state.EntityTypeId = entityTypeId;
            state.EntityId = entityId;
            state.EventId = eventId;
            state.IntegrationStateId = integrationStateId;
        }

        // clones integration state
        public IntegrationState CloneIntegrationState(IntegrationState state, long? attendeeId = null, long? integrationStateId = null)
        {
            var result = new IntegrationState
            {
                IntegrationStateId = integrationStateId ?? state.IntegrationStateId,
                EventId = state.EventId,
                EntityTypeId = state.EntityTypeId,
                EntityId = state.EntityId,
                AttendeeId = attendeeId ?? state.AttendeeId,
                IntegrationStateTypeId = state.IntegrationStateTypeId,
                LastChecked = state.LastChecked,
                ErrorMessage = state.ErrorMessage,
                FirstName = state.FirstName,
                LastName = state.LastName,
                Street = state.Street,
                Street2 = state.Street2,
                City = state.City,
                State = state.State,
                Country = state.Country,
                ZipCode = state.ZipCode,
                Email = state.Email,                
            };
            return result;
        }

    }

}
