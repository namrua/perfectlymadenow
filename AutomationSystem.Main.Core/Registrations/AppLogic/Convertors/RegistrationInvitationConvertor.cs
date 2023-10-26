using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.Convertors
{
    /// <summary>
    /// Registration invitation convertor
    /// </summary>
    public class RegistrationInvitationConvertor : IRegistrationInvitationConvertor
    {
        private readonly IEnumDatabaseLayer enumDb;
        
        public RegistrationInvitationConvertor(IEnumDatabaseLayer enumDb)
        {
            this.enumDb = enumDb;
        }
        
        public ClassInvitationItem ConvertToClassInvitationItem(ClassRegistrationInvitation invitation)
        {
            var result = new ClassInvitationItem
            {
                ClassRegistrationInvitationId = invitation.ClassRegistrationInvitationId,
                ClassRegistrationId = invitation.ClassRegistrationId,
                Email = invitation.Email,
                Language = enumDb.GetItemById(EnumTypeEnum.Language, (int)invitation.LanguageId).Description,
                State = GetRegistrationInvitationState(invitation)
            };

            return result;
        }
        
        public ClassRegistrationInvitation ConvertToClassRegistrationInvitation(ClassInvitationForm form, string requestCode)
        {
            var result = new ClassRegistrationInvitation
            {
                ClassRegistrationInvitationId = form.ClassRegistrationInvitationId,
                ClassId = form.ClassId,
                ClassRegistrationId = form.ClassRegistrationId,
                Email = form.Email,
                LanguageId = form.LanguageId ?? 0,
                RegistrationTypeId = form.RegistrationTypeId,
                RequestCode = requestCode
            };
            return result;
        }

        #region  private methods
        
        public static ClassInvitationState GetRegistrationInvitationState(ClassRegistrationInvitation invitation)
        {
            if (invitation.ClassRegistration == null && invitation.ClassRegistrationId.HasValue)
                throw new InvalidOperationException("ClassRegistration is not included into ClassRegistrationInvitation object.");

            var registration = invitation.ClassRegistration;
            if (registration == null || registration.IsTemporary)
                return ClassInvitationState.New;            
            if (registration.IsCanceled)
                return ClassInvitationState.Canceled;
            if (registration.IsApproved)
                return ClassInvitationState.Approved;
            return ClassInvitationState.Filed;
        }

        #endregion

    }
}
