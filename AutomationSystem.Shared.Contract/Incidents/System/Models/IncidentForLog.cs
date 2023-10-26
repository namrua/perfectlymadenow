using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Models;

namespace AutomationSystem.Shared.Contract.Incidents.System.Models
{
    /// <summary>
    /// Encapsulates record about incident, that can be logged
    /// </summary>
    public class IncidentForLog
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public bool CanBeReported { get; set; }
        public EntityTypeEnum? EntityTypeId { get; set; }
        public long? EntityId { get; set; }
        public DateTime Occurred { get; set; }
        public IncidentTypeEnum IncidentTypeId { get; set; }
        public List<IncidentForLog> Children { get; set; }
        public string IpAddress { get; set; }
        public string RequestUrl { get; set; }

        public IncidentForLog()
        {
            Children = new List<IncidentForLog>();
            Occurred = DateTime.Now;
            CanBeReported = true;
        }

        #region factory methods

        public static IncidentForLog New(IncidentTypeEnum type, string message, string description = null)
        {            
            var result = new IncidentForLog
            {
                IncidentTypeId = type,
                Message = message ?? "no-message",
                Description = description,
            };
            return result;
        }

        public static IncidentForLog New(IncidentTypeEnum type, Exception exception)
        {            
            var result = new IncidentForLog
            {
                IncidentTypeId = type,
                Message = exception?.Message ?? "no-message",
                Description = exception?.ToString(),
            };
            return result;
        }

        public static IncidentForLog New(IncidentTypeEnum type, string message, Exception exception)
        {            
            var result = new IncidentForLog
            {
                IncidentTypeId = type,
                Message = message ?? "no-message",
                Description = exception?.ToString(),
            };
            return result;
        }

        #endregion

        #region extensions

        public IncidentForLog Entity(EntityTypeEnum? entityType, long? entityId)
        {
            EntityId = entityId;
            EntityTypeId = entityType;
            return this;
        }

        public IncidentForLog Entity(EntityId entityId)
        {
            EntityId = entityId?.Id;
            EntityTypeId = entityId?.TypeId;
            return this;
        }

        public IncidentForLog NonReportable()
        {
            CanBeReported = false;
            return this;
        }

        public IncidentForLog AddChild(IncidentForLog incidentChild)
        {
            if (incidentChild == null) return this;
            incidentChild.CanBeReported = false;
            Children.Add(incidentChild);
            return this;
        }

        public IncidentForLog AddChildrenCollection(IEnumerable<IncidentForLog> incidentChildren)
        {
            if (incidentChildren == null) return this;
            foreach (var child in incidentChildren)
                AddChild(child);
            return this;
        }

        public IncidentForLog AddChildren(params IncidentForLog[] incidentChildren)
        {
            AddChildrenCollection(incidentChildren);
            return this;
        }

        public IncidentForLog AddRequestInfo(string ipAddress, string requestUrl)
        {
            IpAddress = ipAddress;
            RequestUrl = requestUrl;
            return this;
        }

        #endregion
    }
}
