using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{

    /// <summary>
    /// pairs integration states with misc strategies
    /// </summary>
    public class IntegrationStatePairingProvider : IIntegrationStatePairingProvider
    {

        // pairs integration states by common matching patterns 
        public PairingResult<IntegrationState> Pair(List<IntegrationState> left, List<IntegrationState> right,
            params Func<IntegrationState, IntegrationState>[] matchingPatterns)
        {
            var result = new PairingResult<IntegrationState>();

            // pairs left and right list           
            foreach (var item in left)
            {
                IntegrationState match = null;
                foreach (var matchingPattern in matchingPatterns.Where(x => x != null))
                {
                    match = matchingPattern(item);
                    if (match != null) break;
                }

                // creates pair
                var pair = new Tuple<IntegrationState, IntegrationState>(item, match);
                result.Pairs.Add(pair);
            }

            // gets unassigned items
            result.Unassigned = GetUnassigned(right, result);
            return result;
        }



        // pairs integration states by same entity 
        public PairingResult<IntegrationState> PairByEntity(List<IntegrationState> left, List<IntegrationState> right,
            Func<IntegrationState, IntegrationState> noMatchSubstitution = null)
        {            
            var rightEntityMap = GenerateMap(right, GetEntityKey);
            var result = Pair(left, right, x => GetKeyOrNull(rightEntityMap, GetEntityKey(x)), noMatchSubstitution);
            return result;
        }


        // pairs integration states by attendeeId
        public PairingResult<IntegrationState> PairByAttendeeId(List<IntegrationState> left, List<IntegrationState> right,
            Func<IntegrationState, IntegrationState> noMatchSubstitution = null)
        {                                  
            var rightAttendeeIdMap = GenerateMap(right, GetAttendeeIdKey);           
            var result = Pair(left, right, x => GetKeyOrNull(rightAttendeeIdMap, GetAttendeeIdKey(x)), noMatchSubstitution);          
            return result;
        }
       

        // pairs integration states by attendeeId or email for InSystem states
        public PairingResult<IntegrationState> PairByAttendeeIdOrEmailForInSystem(List<IntegrationState> left, List<IntegrationState> right,
            Func<IntegrationState, IntegrationState> noMatchSubstitution = null)
        {
            var rightAttendeeIdMap = GenerateMap(right, GetAttendeeIdKey);
            var rightEmailMap = GenerateMap(right, GetEmailKey);          
            var result = Pair(left, right, 
                x => GetKeyOrNull(rightAttendeeIdMap, GetAttendeeIdKey(x)), 
                x => x.IntegrationStateTypeId == IntegrationStateTypeEnum.InWebEx ?  GetKeyOrNull(rightEmailMap, GetEmailKey(x)) : null,
                noMatchSubstitution);
            return result;
        }


        #region key generators

        // gets entity key
        public string GetEntityKey(IntegrationState state)
        {
            return $"{state.EntityTypeId}({state.EntityId})";           
        }

        // gets attendee id key
        public string GetAttendeeIdKey(IntegrationState state)
        {
            return (state.AttendeeId ?? 0).ToString();
        }

        // gets email key
        public string GetEmailKey(IntegrationState state)
        {
            return state.Email.ToLower().Trim();
        }

        #endregion


        #region private methods        

        // gets unassigned items
        private List<IntegrationState> GetUnassigned(IEnumerable<IntegrationState> right,
            PairingResult<IntegrationState> pairingResult)
        {
            var assigned = pairingResult.Pairs.Select(x => x.Item2);
            var resultSet = new HashSet<IntegrationState>(right);
            resultSet.ExceptWith(assigned);
            return resultSet.ToList();
        }


        // generate map
        private Dictionary<string, IntegrationState> GenerateMap(List<IntegrationState> items,
            Func<IntegrationState, string> keyCreator)
        {
            var result = new Dictionary<string, IntegrationState>();
            foreach (var item in items)
                result[keyCreator(item)] = item;
            return result;
        }

        // gets integration state from dictionary by key if exists, otherwise returns null
        private IntegrationState GetKeyOrNull(Dictionary<string, IntegrationState> map, string key)
        {
            if (!map.TryGetValue(key, out var result))
                return null;
            return result;
        }              

        #endregion

    }

}
