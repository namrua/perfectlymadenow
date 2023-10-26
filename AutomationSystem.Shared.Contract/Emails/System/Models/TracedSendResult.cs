using System;
using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Emails.System.Models
{
    /// <summary>
    /// Encapsulates information about traced sending of batch emails
    /// </summary>
    public class TracedSendResult<T>
    {

        public bool IsSuccessful { get; set; } = true;
        public Exception Exception { get; set; }

        // tracks processed emails and related entities
        public List<Tuple<long, T>> ProcessedEmailIdEntityIdPairs { get; set; } = new List<Tuple<long, T>>();

    }

}
