using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Models;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Home.AppLogic
{
    /// <summary>
    /// Home service exceptions
    /// </summary>
    public class HomeServiceException : Exception
    {

        public HomeServiceErrorType Type { get; set; }
        public long? ClassId { get; set; }
        public long? ClassRegistrationId { get; set; }
        public long? ClassRegistrationInvitationId { get; set; }
        public RegistrationTypeEnum? RegistrationTypeId { get; set; }
        public EntityId EntityId { get; set; }
        public HomeWorkflowStage? HomeWorkflowState { get; set; }
        public string Token { get; set; }
        public string ProfileMoniker { get; set; }

        // constructors
        public HomeServiceException(string message, HomeServiceErrorType type) : base(message)
        {
            Type = type;
        }

        // constructors
        public HomeServiceException(string message, HomeServiceErrorType type, Exception innerException) : base(message, innerException)
        {
            Type = type;
        }

        // toString 
        public override string ToString()
        {
            var additionalInfo = new List<string>();

            if (ClassId.HasValue)
            {
                additionalInfo.Add($"ClassId = {ClassId}");
            }

            if (ClassRegistrationId.HasValue)
            {
                additionalInfo.Add($"ClassRegistrationId = {ClassRegistrationId}");
            }

            if (ClassRegistrationInvitationId.HasValue)
            {
                additionalInfo.Add($"ClassRegistrationInvitationId = {ClassRegistrationInvitationId}");
            }

            if (RegistrationTypeId.HasValue)
            {
                additionalInfo.Add($"RegistrationTypeId = {RegistrationTypeId}");
            }

            if (HomeWorkflowState.HasValue)
            {
                additionalInfo.Add($"HomeWorkflowState = {HomeWorkflowState}");
            }

            if (Token != null)
            {
                additionalInfo.Add($"Token = {Token}");
            }

            if (ProfileMoniker != null)
            {
                additionalInfo.Add($"ProfileMoniker = {ProfileMoniker}");
            }

            if (EntityId != null)
            {
                additionalInfo.Add($"EntityId = {EntityId}");
            }

            var baseResult = base.ToString();
            var result = additionalInfo.Count == 0 ? baseResult : $"{baseResult}\n\nData = {{ {string.Join(",", additionalInfo)} }}";
            return result;
        }

        #region factory

        // new
        public static HomeServiceException New(HomeServiceErrorType type, string note = null,
            Exception innerException = null)
        {
            var message = $"Home service causes error with type {type}{(string.IsNullOrEmpty(note) ? "" : $" - {note}")}";
            var result = innerException == null
                ? new HomeServiceException(message, type)
                : new HomeServiceException(message, type, innerException);
            return result;
        }

        // new invalid class state
        public static HomeServiceException NewInvalidClassState(ClassState classState,
            string note = null, Exception innerException = null)
        {
            var registrationErrorType = classState == ClassState.New
                ? HomeServiceErrorType.ClassRegistrationNotStarted
                : HomeServiceErrorType.ClassRegistrationClosed;
            return New(registrationErrorType, note, innerException);
        }

        #endregion

        #region extensions

        // adds id informations
        public HomeServiceException AddId(
            long? classId = null,
            long? classRegistrationId = null,
            long? classRegistrationInvitationId = null, 
            RegistrationTypeEnum? registrationTypeId = null,
            HomeWorkflowStage? homeWorkflowStage = null,
            string token = null,
            string profileMoniker = null,
            EntityId entityId = null)
        {
            if (classId.HasValue)
            {
                ClassId = classId;
            }

            if (classRegistrationId.HasValue)
            {
                ClassRegistrationId = classRegistrationId;
            }

            if (classRegistrationInvitationId.HasValue)
            {
                ClassRegistrationInvitationId = classRegistrationInvitationId;
            }

            if (registrationTypeId.HasValue)
            {
                RegistrationTypeId = registrationTypeId;
            }

            if (homeWorkflowStage.HasValue)
            {
                HomeWorkflowState = homeWorkflowStage;
            }

            if (token != null)
            {
                Token = token;
            }

            if (profileMoniker != null)
            {
                ProfileMoniker = profileMoniker;
            }

            if (entityId != null)
            {
                EntityId = entityId;
            }

            return this;
        }

        #endregion
    }

}
