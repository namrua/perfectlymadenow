using System;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.TimeZones.System
{
    /// <summary>
    /// Provides operation on datetimes and timezones
    /// </summary>
    public interface ITimeZoneService
    {   

        // gets utc date time from date time and time zone Id
        DateTime GetUtcDateTime(DateTime dateTime, TimeZoneEnum timeZoneId);

        // gets utc date time from date time and time zone Id
        DateTime? GetUtcDateTime(DateTime? dateTime, TimeZoneEnum timeZoneId);

    }

}
