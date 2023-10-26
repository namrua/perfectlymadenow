using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.TimeZones.System;

namespace AutomationSystem.Shared.Core.TimeZones.System
{
    /// <summary>
    /// Provides operation on datetimes and timezones
    /// </summary>
    public class TimeZoneService : ITimeZoneService
    {

        // private fields
        private readonly IEnumDatabaseLayer enumDb;

        // constructor
        public TimeZoneService(IEnumDatabaseLayer enumDb)
        {
            this.enumDb = enumDb;
        }

        // gets utc date time from date time and time zone Id
        public DateTime GetUtcDateTime(DateTime dateTime, TimeZoneEnum timeZoneId)
        {
            // obtains time zone from db
            var timeZone = enumDb.GetItemById(EnumTypeEnum.TimeZone, (int) timeZoneId);
            if (timeZone == null)
                throw new ArgumentException($"There is no time zone with id {timeZoneId}");

            // obtains time zone info and utc date time
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone.Name);
            var result = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZoneInfo);
            return result;
        }

        // gets utc date time from date time and time zone Id
        public DateTime? GetUtcDateTime(DateTime? dateTime, TimeZoneEnum timeZoneId)
        {
            if (!dateTime.HasValue)
                return null;
            return GetUtcDateTime(dateTime.Value, timeZoneId);
        }

    }

}
