using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Core.Utilities.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Classes.System;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    public class RegistrationInvitationAdministration : IRegistrationInvitationAdministration
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IRegistrationInvitationConvertor invitationConvertor;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IRegistrationLogicProvider registrationLogicProvider;
        private readonly IEmailTypeResolver emailTypeResolver;
        private readonly IRegistrationEmailService registrationEmailService;
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IClassTypeResolver classTypeResolver;

        public RegistrationInvitationAdministration(
            IEnumDatabaseLayer enumDb,
            IClassDatabaseLayer classDb,
            IIdentityResolver identityResolver,
            IRegistrationInvitationConvertor invitationConvertor,
            IRegistrationDatabaseLayer registrationDb,
            IRegistrationLogicProvider registrationLogicProvider,
            IEmailTypeResolver emailTypeResolver,
            IRegistrationEmailService registrationEmailService,
            IClassOperationChecker classOperationChecker,
            IClassTypeResolver classTypeResolver)
        {
            this.enumDb = enumDb;
            this.classDb = classDb;
            this.identityResolver = identityResolver;
            this.invitationConvertor = invitationConvertor;
            this.registrationDb = registrationDb;
            this.registrationLogicProvider = registrationLogicProvider;
            this.emailTypeResolver = emailTypeResolver;
            this.registrationEmailService = registrationEmailService;
            this.classOperationChecker = classOperationChecker;
            this.classTypeResolver = classTypeResolver;
        }

        public ClassInvitationPageModel GetClassInvitationPageModel(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassType);
            identityResolver.CheckEntitleForClass(cls);

            var classShortDetail = ClassConvertor.ConvertToClassShortDetial(cls);
            var result = new ClassInvitationPageModel
            {
                Class = classShortDetail,
            };

            if (!classTypeResolver.AreInvitationsAllowed(cls.ClassCategoryId))
            {
                result.AreInvitationsDisabled = true;
                result.InvitationDisabledMessage = "Invitations are not available for the class.";
                return result;
            }

            var invitations = registrationDb.GetClassRegistrationInvitations(classId, ClassRegistrationInvitationIncludes.ClassRegistration).ToList();
            result.Invitations = invitations.Select(invitationConvertor.ConvertToClassInvitationItem).ToList();

            var registrationTypesIds = registrationLogicProvider.RegistrationTypeFeeder.GetAllowedTypesForAdministrationRegistration(cls);
            result.RegistrationTypes = enumDb.GetItemsByFilter(EnumTypeEnum.MainRegistrationType).Where(x => registrationTypesIds.Contains((RegistrationTypeEnum)x.Id)).ToList();

            result.CanInvite = classOperationChecker.IsOperationAllowed(ClassOperation.Invitation, classShortDetail.ClassState);
                
            return result;
        }
        
        public ClassInvitationForEdit GetClassInvitationForEdit(RegistrationTypeEnum id, long classId)
        {
            var cls = GetClassById(classId);
            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(ClassOperation.Invitation, cls);

            var result = new ClassInvitationForEdit();
            result.Form.ClassId = classId;
            result.Form.RegistrationTypeId = id;
            result.Languages = GetClassLanguages(cls.OriginLanguageId, cls.TransLanguageId);
            return result;
        }
        
        public ClassInvitationForEdit GetFormInvitationForEdit(ClassInvitationForm form)
        {
            var cls = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(cls);

            var result = new ClassInvitationForEdit
            {
                Form = form,
                Languages = GetClassLanguages(cls.OriginLanguageId, cls.TransLanguageId)
            };
            return result;
        }
        
        public long SaveInvitation(ClassInvitationForm form)
        {
            var toCheck = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(toCheck);
            classOperationChecker.CheckOperation(ClassOperation.Invitation, toCheck);
            
            var invitationDb = invitationConvertor.ConvertToClassRegistrationInvitation(form, RandomStringGenerator.GenerateRequestCode());
            var result = registrationDb.InsertClassRegistrationInvitation(invitationDb);
            
            try
            {
                registrationEmailService.SendClassRegistrationInvitationEmail(result,
                    emailTypeResolver.ResolveEmailTypeForRegistration(EmailTypeEnum.RegistrationInvitation, form.RegistrationTypeId));
            }
            catch (Exception)
            {
                registrationDb.DeleteClassRegistrationInvitation(result);
                throw;
            }
            return result;
        }
        
        public long? DeleteInvitation(long invitationId)
        {
            var toCheck = registrationDb.GetClassRegistrationInvitationById(invitationId, ClassRegistrationInvitationIncludes.Class);
            if (toCheck == null)
                return null;
            identityResolver.CheckEntitleForClass(toCheck.Class);

            registrationDb.DeleteClassRegistrationInvitation(invitationId, RegistrationOperationOption.CheckOperation);
            return toCheck.ClassId;
        }

        #region private methods
        private Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
                throw new ArgumentException($"There is no Class with id {classId}.");
            return result;
        }

        private List<IEnumItem> GetClassLanguages(LanguageEnum originLanguageId, LanguageEnum? transLanguageId)
        {
            var result = new List<IEnumItem>();
            result.Add(enumDb.GetItemById(EnumTypeEnum.Language, (int)originLanguageId));
            if (transLanguageId.HasValue)
                result.Add(enumDb.GetItemById(EnumTypeEnum.Language, (int)transLanguageId.Value));
            return result;
        }
        #endregion
    }
}
