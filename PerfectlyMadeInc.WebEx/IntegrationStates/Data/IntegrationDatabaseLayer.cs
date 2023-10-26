using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.IntegrationStates.Data.Models;
using PerfectlyMadeInc.WebEx.IntegrationStates.Data.Queries;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Model.Queries;

namespace PerfectlyMadeInc.WebEx.IntegrationStates.Data
{
    /// <summary>
    /// Provides integration database layer
    /// </summary>
    public class IntegrationDatabaseLayer : IIntegrationDatabaseLayer
    {


        // gets WebEx integration state types
        public List<IntegrationStateType> GetIntegrationStateTypes()
        {
            using (var context = new WebExEntities())
            {
                var result = context.IntegrationStateTypes.ToList();
                return result;
            }
        }


        // gets list of webex integration state by entity id 
        public List<IntegrationState> GetIntegrationStatesByEntityId(EntityTypeEnum entityTypeId, long entityId,
            IntegrationStateIncludes includes = IntegrationStateIncludes.None)
        {
            using (var context = new WebExEntities())
            {
                var result = context.IntegrationStates.AddIncludes(includes).Active().Where(x =>
                    x.EntityId == entityId && x.EntityTypeId == entityTypeId).ToList();
                return result;
            }
        }


        // gets all integration states by eventId
        public List<IntegrationState> GetIntegrationStatesByEventId(long eventId,
            IntegrationStateIncludes includes = IntegrationStateIncludes.None)
        {
            using (var context = new WebExEntities())
            {
                var result = context.IntegrationStates.AddIncludes(includes).Active()
                    .Where(x => x.EventId == eventId).ToList();
                return result;
            }
        }


        // get WebEx integration state
        public IntegrationState GetIntegrationStateByEntityId(EntityTypeEnum entityTypeId, long entityId, long eventId,
            IntegrationStateIncludes includes = IntegrationStateIncludes.None)
        {
            using (var context = new WebExEntities())
            {
                var result = context.IntegrationStates.AddIncludes(includes).Active().FirstOrDefault(x => x.EventId == eventId
                    && x.EntityId == entityId && x.EntityTypeId == entityTypeId);
                return result;
            }
        }

        // insert WebEx integration state
        public long InsertIntegrationState(IntegrationState state)
        {
            using (var context = new WebExEntities())
            {
                context.IntegrationStates.Add(state);
                context.SaveChanges();
                return state.IntegrationStateId;
            }
        }

        // update WebEx integration state
        public void UpdateIntegrationState(IntegrationState state)
        {
            using (var context = new WebExEntities())
            {
                var toUpdate = context.IntegrationStates.Active()
                    .FirstOrDefault(x => x.IntegrationStateId == state.IntegrationStateId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no IntegrationState with id {state.IntegrationStateId}.");

                toUpdate.AttendeeId = state.AttendeeId;
                toUpdate.IntegrationStateTypeId = state.IntegrationStateTypeId;
                toUpdate.ErrorMessage = state.ErrorMessage;
                toUpdate.LastChecked = state.LastChecked;
                toUpdate.FirstName = state.FirstName;
                toUpdate.LastName = state.LastName;
                toUpdate.Street = state.Street;
                toUpdate.Street2 = state.Street2;
                toUpdate.City = state.City;
                toUpdate.State = state.State;
                toUpdate.Country = state.Country;
                toUpdate.ZipCode = state.ZipCode;
                toUpdate.Email = state.Email;

                context.SaveChanges();
            }
        }

    }

}
