using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.Emails.System
{
    /// <summary>
    /// Email type resolver
    /// </summary>
    public class EmailTypeResolver : IEmailTypeResolver
    {

        private readonly Dictionary<EmailTypeEnum, EmailTypeEnum> wwaEmailTypeMap;
        private readonly HashSet<EmailTypeEnum> wwaEmailTypes;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly HashSet<EmailTypeEnum> profileEmailTypes;
        private readonly HashSet<EmailTypeEnum> profileWwaEmailTypes;

        // constructor
        public EmailTypeResolver(IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationTypeResolver = registrationTypeResolver;

            // WWA EmailType mapping
            wwaEmailTypeMap = new Dictionary<EmailTypeEnum, EmailTypeEnum>
            {
                { EmailTypeEnum.RegistrationConfirmation, EmailTypeEnum.WwaRegistrationConfirmation },
                { EmailTypeEnum.RegistrationInvitation, EmailTypeEnum.WwaRegistrationInvitation },
                { EmailTypeEnum.ConversationChanged, EmailTypeEnum.WwaConversationChanged },
                { EmailTypeEnum.ConversationCanceled, EmailTypeEnum.WwaConversationCanceled },
                { EmailTypeEnum.ConversationCompleted, EmailTypeEnum.WwaConversationCompleted },
                { EmailTypeEnum.RegistrationCanceled, EmailTypeEnum.WwaRegistrationCanceled },
                { EmailTypeEnum.InvitationFilledIn, EmailTypeEnum.WwaInvitationFilledIn },
            };

            // WWA email types
            wwaEmailTypes = new HashSet<EmailTypeEnum>
            {
                EmailTypeEnum.WwaRegistrationConfirmation,
                EmailTypeEnum.WwaRegistrationInvitation,
                EmailTypeEnum.WwaConversationChanged,
                EmailTypeEnum.WwaConversationCanceled,
                EmailTypeEnum.WwaConversationCompleted,
                EmailTypeEnum.WwaRegistrationCanceled,
                EmailTypeEnum.WwaInvitationFilledIn,
                EmailTypeEnum.WwaStudentRegistrationNotification,
            };

            profileEmailTypes = new HashSet<EmailTypeEnum>
            {
                EmailTypeEnum.RegistrationConfirmation,
                EmailTypeEnum.RegistrationInvitation,
                EmailTypeEnum.ManualVerificationNotification,
                EmailTypeEnum.ConversationChanged,
                EmailTypeEnum.ConversationCanceled,
                EmailTypeEnum.ConversationCompleted,
                EmailTypeEnum.RegistrationCanceled,
                EmailTypeEnum.PaymentRequest,
                EmailTypeEnum.MaterialsNotification,
                EmailTypeEnum.WwaConversationCanceled,
                EmailTypeEnum.WwaConversationChanged,
                EmailTypeEnum.WwaConversationCompleted,
                EmailTypeEnum.WwaRegistrationCanceled,
                EmailTypeEnum.WwaRegistrationConfirmation,
                EmailTypeEnum.WwaRegistrationInvitation
            };

            profileWwaEmailTypes = new HashSet<EmailTypeEnum>
            {
                EmailTypeEnum.WwaConversationCanceled,
                EmailTypeEnum.WwaConversationChanged,
                EmailTypeEnum.WwaConversationCompleted,
                EmailTypeEnum.WwaRegistrationCanceled,
                EmailTypeEnum.WwaRegistrationConfirmation,
                EmailTypeEnum.WwaRegistrationInvitation
            };
        }


        // resolve email type
        public EmailTypeEnum ResolveEmailTypeForRegistration(EmailTypeEnum emailTypeId, RegistrationTypeEnum registrationTypeId)
        {
            // determines whether registration is wwa, if no returns origin email type id
            var isWwa = registrationTypeResolver.IsWwaRegistration(registrationTypeId);
            if (!isWwa) return emailTypeId;

            // try to obtain mapped type
            if (!wwaEmailTypeMap.TryGetValue(emailTypeId, out var mappedEmailTypeId))
                return emailTypeId;
            return mappedEmailTypeId;
        }


        // gets wwa counterpart for email type, if it is not exists, returns origin value
        public EmailTypeEnum GetWwaEmailTypeFrom(EmailTypeEnum emailTypeId)
        {
            // try to obtain mapped type
            if (!wwaEmailTypeMap.TryGetValue(emailTypeId, out var mappedEmailTypeId))
                return emailTypeId;
            return mappedEmailTypeId;
        }


        // determines whether email type is related to WWA
        public bool IsWwaEmailType(EmailTypeEnum emailTypeId)
        {
            var result = wwaEmailTypes.Contains(emailTypeId);
            return result;
        }
        public HashSet<EmailTypeEnum> GetEmailTypesForProfile(bool onlyWwaEmailTypes)
        {
            HashSet<EmailTypeEnum> result;
            if (onlyWwaEmailTypes)
            {
                result = new HashSet<EmailTypeEnum>(profileWwaEmailTypes);
            }
            else
            {
                result = new HashSet<EmailTypeEnum>(profileEmailTypes);
            }

            return result;
        }
    }
}
