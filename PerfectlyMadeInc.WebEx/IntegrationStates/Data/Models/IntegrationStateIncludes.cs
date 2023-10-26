using System;

namespace PerfectlyMadeInc.WebEx.IntegrationStates.Data.Models
{
    /// <summary>
    /// Determines integration state includes
    /// </summary>
    [Flags]
    public enum IntegrationStateIncludes
    {
        None = 0x00,
        Event = 0x01,
    }

}
