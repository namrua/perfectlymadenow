using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;

namespace AutomationSystem.Main.Core.Integration.AppLogic.Convertors
{
    /// <summary>
    /// Converts program related objects to and from main objects
    /// </summary>
    public class MainProgramConvertor : IMainProgramConvertor
    {

        // converts MainProgramFilter to ProgramFilter
        public ProgramFilter ConvertToProgramFilter(MainProgramFilter filter, ProfileFilter profileFilter)
        {
            filter = filter ?? new MainProgramFilter();
            var result = new ProgramFilter
            {
                IncludeUsed = filter.IncludeUsed,
                UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                UserGroupId = filter.ProfileId,
                UserGroupIds = profileFilter.ProfileIds
            };
            return result;
        }

        // converts ProgramListItem to MainProgramListItem
        public MainProgramListItem ConvertToMainProgramListItem(ProgramListItem item, Dictionary<long, Profile> profileMap)
        {
            var result = new MainProgramListItem
            {
                Id = item.Id,
                Name = item.Name,
                AccountName = item.AccountName
            };

            // fills profile properties
            if (profileMap.TryGetValue(CheckAndGetProfileId(item.UserGroupTypeId, item.UserGroupId), out var profile))
            {
                result.Profile = profile.Name;
            }
            return result;
        }

        // converts ProgramDetail to MainProgramDetail
        public MainProgramDetail ConvertToMainProgramDetail(ProgramDetail detail, Profile profile)
        {
            CheckAndGetProfileId(detail.UserGroupTypeId, detail.UserGroupId, profile.ProfileId);
            var result = new MainProgramDetail
            {
                Id = detail.Id,
                Name = detail.Name,
                ProgramUrl = detail.ProgramUrl,
                IsUsed = detail.IsUsed,
                AccountName = detail.AccountName,
                Profile = profile.Name,
                Events = detail.Events
            };
            return result;
        }


        #region private methods

        // checks user group and gets profile id
        public long CheckAndGetProfileId(UserGroupTypeEnum? typeId, long? entityId, long? profileId = null)
        {
            if (typeId != UserGroupTypeEnum.MainProfile || !entityId.HasValue || (profileId.HasValue && entityId != profileId))
                throw new ArgumentException($"User group '{typeId}-{entityId}' does not refer to Profile.");
            return entityId.Value;
        }

        #endregion

    }

}
