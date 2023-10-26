using System;

namespace AutomationSystem.Shared.Core.AsyncRequests.Data.Models
{
    /// <summary>
    /// AsyncRequest includes
    /// </summary>
    [Flags]
    public enum AsyncRequestIncludes
    {
        None = 0x00,
        AsyncRequestType = 0x01,
        ProcessingState = 0x02
    }

}
