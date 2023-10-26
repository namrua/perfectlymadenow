using System;

namespace AutomationSystem.Main.Core.Classes.Data.Models
{
    /// <summary>
    /// Determines class includes
    /// </summary>
    [Flags]
    public enum ClassIncludes
    {
        None = 0,   
        ClassType = 1 << 0,
        TimeZone = 1 << 1,
        ClassPersons = 1 << 2,       
        PriceList = 1 << 3,
        PriceListPriceListItems = 1 << 4,
        ClassActions = 1 << 5,
        ClassActionsClassActionType = 1 << 6,
        ClassBusiness = 1 << 7,
        ClassBusinessClassExpenses = 1 << 8,
        ClassReportSetting= 1 << 9,
        ClassStyle = 1 << 10,
        Profile = 1 << 11,
        Currency = 1 << 12,
        LocationInfo = 1 << 13,
    }
}