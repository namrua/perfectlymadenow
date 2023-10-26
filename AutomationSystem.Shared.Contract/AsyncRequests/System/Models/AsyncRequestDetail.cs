using System;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.AsyncRequests.System.Models
{
    /// <summary>
    /// Asynchronous request detail
    /// </summary>
    public class AsyncRequestDetail
    {

        [DisplayName("ID")]
        public long AsyncRequestId { get; set; }

        [DisplayName("Type code")]
        public AsyncRequestTypeEnum AsyncRequestTypeId { get; set; }

        [DisplayName("Type")]
        public string AsyncRequestType { get; set; }

        [DisplayName("State code")]
        public ProcessingStateEnum ProcessingStateId { get; set; }

        [DisplayName("State")]
        public string ProcessingState { get; set; }

        [DisplayName("Planned")]
        public DateTime Planned { get; set; }

        [DisplayName("Started")]
        public DateTime? Started { get; set; }

        [DisplayName("Finished")]
        public DateTime? Finished { get; set; }
                
    }
}
