using System;
using System.Collections.Generic;
using System.Security.Principal;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;

namespace AutomationSystem.Base.Contract.Identities
{

    /// <summary>
    /// Entitle
    /// </summary>
    public class EntitleAccessDeniedException : Exception
    {

        public Entitle Entitle { get; set; }
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }
        public long? UserGroupId { get; set; }
        public EntityTypeEnum? EntityTypeId { get; set; }
        public string EntityType { get; set; }
        public long? EntityId { get; set; }
      

        // constructors
        public EntitleAccessDeniedException(string message, Entitle entitle) : base(message)
        {
            Entitle = entitle;
        }

        // constructors
        public EntitleAccessDeniedException(string message, Entitle entitle, Exception innerException) : base(message, innerException)
        {
            Entitle = entitle;
        }


        // toString 
        public override string ToString()
        {
            var additionalInfo = new List<string>();

            if (UserGroupTypeId.HasValue)
                additionalInfo.Add($"UserGroupTypeId = {UserGroupTypeId}");
            if (UserGroupId.HasValue)
                additionalInfo.Add($"UserGroupId = {UserGroupId}");
            if (EntityTypeId.HasValue)
                additionalInfo.Add($"EntityTypeId = {EntityTypeId}");
            if (!string.IsNullOrEmpty(EntityType))
                additionalInfo.Add($"EntityType = {EntityType}");
            if (EntityId.HasValue)
                additionalInfo.Add($"EntityId = {EntityId}");
            
            var baseResult = base.ToString();
            var result = additionalInfo.Count == 0 ? baseResult : $"{baseResult}\n\nData = {{ {string.Join(",", additionalInfo)} }}";
            return result;
        }


        #region factory

        // new
        public static EntitleAccessDeniedException New(Entitle entitle, IIdentity identity, string note = null,
            Exception innerException = null)
        {
            var message = $"Access of user '{identity.Name}' denied for entitle {entitle}{(string.IsNullOrEmpty(note) ? "" : $" - {note}")}";
            var result = innerException == null
                ? new EntitleAccessDeniedException(message, entitle)
                : new EntitleAccessDeniedException(message, entitle, innerException);
            return result;
        }

        #endregion


        #region extensions

        // adds id informations
        public EntitleAccessDeniedException AddId(UserGroupTypeEnum? userGroupTypeId = null, long? userGroupId = null, 
            EntityTypeEnum ? entityTypeId = null, long? entityId = null, string entityType = null)
        {
            if (userGroupTypeId.HasValue)
                UserGroupTypeId = userGroupTypeId;
            if (userGroupId.HasValue)
                UserGroupId = userGroupId;
            if (entityTypeId.HasValue)
                EntityTypeId = entityTypeId;
            if (!string.IsNullOrEmpty(entityType))
                EntityType = entityType;
            if (entityId.HasValue)
                EntityId = entityId;
            return this;
        }

        #endregion

    }

}
