using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Handlers;
using AutomationSystem.Main.Core.Registrations.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.Convertors;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Enums.Data;
using System;
using System.Linq;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using RecipientType = AutomationSystem.Main.Core.Emails.System.Models.RecipientType;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    public class RegistrationAdministration : IRegistrationAdministration
    {

        private const ClassRegistrationIncludes listItemIncludes = ClassRegistrationIncludes.AddressesCountry | 
            ClassRegistrationIncludes.ApprovementType | ClassRegistrationIncludes.RegistrationType | ClassRegistrationIncludes.Class;

        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IRegistrationLogicProvider registrationLogicProvider;   
        private readonly IEmailIntegration emailIntegration;
        private readonly IFormerDatabaseLayer formerDb;
        private readonly IMainAsyncRequestManager requestManager;
        private readonly IClassMaterialDistributionHandler materialDistributionHandler;
        private readonly IIdentityResolver identityResolver;
        private readonly IFormerFilterForReviewProvider formerFilterForReviewProvider;
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IRegistrationOperationChecker registrationOperationChecker;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IEmailTypeResolver emailTypeResolver;
        private readonly IRegistrationEmailService registrationEmailService;
        private readonly IRegistrationCommandService commandService;
        private readonly IBatchUploadService batchUploadService;

        private readonly IBaseRegistrationConvertor baseConvertor;


        public RegistrationAdministration(
            IRegistrationDatabaseLayer registrationDb,
            IClassDatabaseLayer classDb,
            IEnumDatabaseLayer enumDb,
            IRegistrationLogicProvider registrationLogicProvider,
            IEmailIntegration emailIntegration,
            IFormerDatabaseLayer formerDb,
            IMainAsyncRequestManager requestManager,
            IClassMaterialDistributionHandler materialDistributionHandler,
            IIdentityResolver identityResolver,
            IFormerFilterForReviewProvider formerFilterForReviewProvider,
            IClassOperationChecker classOperationChecker,
            IRegistrationOperationChecker registrationOperationChecker,
            IRegistrationTypeResolver registrationTypeResolver,
            IEmailTypeResolver emailTypeResolver,
            IRegistrationEmailService registrationEmailService,
            IRegistrationCommandService commandService,
            IBatchUploadService batchUploadService)
        {
            this.registrationDb = registrationDb;
            this.classDb = classDb;
            this.enumDb = enumDb;
            this.registrationLogicProvider = registrationLogicProvider;
            this.emailIntegration = emailIntegration;
            this.formerDb = formerDb;
            this.requestManager = requestManager;
            this.materialDistributionHandler = materialDistributionHandler;
            this.identityResolver = identityResolver;
            this.formerFilterForReviewProvider = formerFilterForReviewProvider;
            this.classOperationChecker = classOperationChecker;
            this.registrationOperationChecker = registrationOperationChecker;
            this.registrationTypeResolver = registrationTypeResolver;
            this.emailTypeResolver = emailTypeResolver;
            this.registrationEmailService = registrationEmailService;
            this.commandService = commandService;
            this.batchUploadService = batchUploadService;
            
            baseConvertor = registrationLogicProvider.BaseRegistrationConvertor;
        }


        #region views
        
        public ClassRegistrationPageModel GetClassRegistrationPageModel(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassType);
            identityResolver.CheckEntitleForClass(cls);
            var result = new ClassRegistrationPageModel();
            result.Class = ClassConvertor.ConvertToClassShortDetial(cls);
                    
            var allowedTypes = registrationLogicProvider.RegistrationTypeFeeder
                .GetAllowedTypesForAdministrationRegistration(cls);
            result.RegistrationTypes = enumDb.GetItemsByFilter(EnumTypeEnum.MainRegistrationType)
                .Where(x => allowedTypes.Contains((RegistrationTypeEnum) x.Id)).ToList();
            
            var registrationForApproval = registrationDb.GetRegistrationsByStateSet(classId, ClassRegistrationStateSet.New,
                listItemIncludes);
            result.RegistrationsForApprove = registrationForApproval.Select(x => baseConvertor.ConvertToRegistrationListItem(x)).ToList();
            result.CanAddNewRegistration = classOperationChecker.IsOperationAllowed(ClassOperation.AddRegistration, result.Class.ClassState);
            result.CanBatchUpload = result.CanAddNewRegistration
                                    && registrationLogicProvider.RegistrationTypeFeeder.GetAllowedTypesForBatchUploadRegistration(cls, RegistrationFormTypeEnum.Adult).Any();
            result.RegistrationBatchUploads = batchUploadService.GetBatchUploadListItems(EntityTypeEnum.MainClass, classId);
            return result;
        }
        
        public RegistrationDetailPageModel GetRegistrationDetailPageModel(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, listItemIncludes);
            var cls = GetClassById(registration.ClassId, ClassIncludes.ClassType);
            identityResolver.CheckEntitleForClass(cls);

            var result = new RegistrationDetailPageModel();
            result.Registration = baseConvertor.ConvertToRegistrationListItem(registration);
            result.Class = ClassConvertor.ConvertToClassShortDetial(cls);
            result.RegistrationCancelTemplate = GetTemplateListItemByType(registrationId, GetRegistrationCancelationType(registration.RegistrationTypeId));
            result.CanApprove = registrationOperationChecker.IsOperationAllowed(RegistrationOperation.ApproveRegistration, result.FullState);
            result.CanCancel = registrationOperationChecker.IsOperationAllowed(RegistrationOperation.CancelRegistration, result.FullState)
                && result.RegistrationCancelTemplate == null;
            result.CanDelete = registrationOperationChecker.IsOperationAllowed(RegistrationOperation.DeleteRegistration, result.FullState);
            return result;
        }
        
        public RegistrationsForList GetRegistrationsForList(RegistrationFilter filter, bool search)
        {
            var cls = GetClassById(filter.ClassId ?? 0, ClassIncludes.ClassType);
            identityResolver.CheckEntitleForClass(cls);

            var registrationTypes = enumDb.GetItemsByFilter(EnumTypeEnum.MainRegistrationType);
            if (!cls.IsWwaFormAllowed)
                registrationTypes = registrationTypes.Where(x => (RegistrationTypeEnum)x.Id != RegistrationTypeEnum.WWA).ToList();

            var result = new RegistrationsForList(filter);
            result.Class = ClassConvertor.ConvertToClassShortDetial(cls);
            result.RegistratonState = new[] { RegistrationState.New, RegistrationState.Approved, RegistrationState.Canceled }.ToList();
            result.RegistrationTypes.Initialize(registrationTypes, x => x.Id, x => x.Description);
            result.WasSearched = search;
            if (search)
            {
                var registrations = registrationDb.GetRegistrationsByFilter(filter, listItemIncludes);
                result.Items = registrations.Select(baseConvertor.ConvertToRegistrationListItem).ToList();
            }
            return result;
        }
    
        public RegistrationCommunicationPageModel GetRegistrationCommunication(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);

            var result = new RegistrationCommunicationPageModel
            {
                ClassId = registration.ClassId,
                ClassRegistrationId = registrationId,
                Emails = emailIntegration.GetEmailsByEntity(new EmailEntityId(EntityTypeEnum.MainClassRegistration, registrationId))
            };            
            return result;
        }

        #endregion

        #region registration edit
        
        public RegistrationControllerInfo GetControllerInfoByRegistrationTypeId(RegistrationTypeEnum registrationTypeId)
        {   
            // note: no identity check needed here

            var convertor = registrationLogicProvider.GetConvertorByRegistrationTypeId(registrationTypeId);
            var result = convertor.ControllerInfo;
            return result;
        }
        
        public BaseRegistrationDetail GetRegistrationDetailById(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.AddressesCountry | 
                ClassRegistrationIncludes.RegistrationType | ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);

            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registration.RegistrationTypeId);
            var result = service.GetRegistrationDetail(registration);
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEdit(
            RegistrationTypeEnum registrationTypeId, long classId)
        {
            var cls = GetClassById(classId);
            identityResolver.CheckEntitleForClass(cls);

            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registrationTypeId);
            var result = service.GetNewRegistrationForEdit(registrationTypeId, cls);
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetRegistrationForEdit(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Addresses | ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);

            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registration.RegistrationTypeId);
            var result = service.GetRegistrationForEdit(registration);
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetFormRegistrationForEdit(BaseRegistrationForm form)
        {
            var toCheck = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(toCheck);

            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(form.RegistrationTypeId);
            var result = service.GetFormRegistrationForEdit(form);
            return result;
        }
        
        public long SaveRegistration(BaseRegistrationForm form)
        {
            var toCheck = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(toCheck);
            
            var isNewRegistration = form.ClassRegistrationId == 0;
            var oldRegistration = isNewRegistration ? null : GetRegistrationById(form.ClassRegistrationId);
            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(form.RegistrationTypeId);
            var result = service.SaveRegistration(form, false, ApprovementTypeEnum.ManualApprovement);
            
            if (isNewRegistration) return result;
            var registration = GetRegistrationById(form.ClassRegistrationId);
            if (!registration.IsApproved || registration.IsCanceled) return result;
            
            requestManager.AddIntegrationRequestForClassRegistration(registration, AsyncRequestTypeEnum.AddToOuterSystem, (int) SeverityEnum.High);
            
            materialDistributionHandler.HandleRegistrationChange(oldRegistration, registration);
            return result;
        }

        #endregion
        
        #region commands
        
        public void ApproveRegistration(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);
            registrationOperationChecker.CheckOperation(RegistrationOperation.ApproveRegistration, registration);
            
            commandService.ApproveRegistration(registrationId);
        }
        
        public long? DeleteRegistration(long registrationId)
        {
            var toCheck = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(toCheck.Class);

            var classId = registrationDb.DeleteClassRegistration(registrationId, RegistrationOperationOption.CheckOperation);
            return classId;
        }
        
        public void CreateRegistrationCancelationEmailTemplate(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);
            registrationOperationChecker.CheckOperation(RegistrationOperation.CancelRegistration, registration);
            
            var registrationCanceledEmailType = GetRegistrationCancelationType(registration.RegistrationTypeId);
            if (GetTemplateListItemByType(registrationId, registrationCanceledEmailType) != null)
            {
                throw new InvalidOperationException($"There already is cancelation email template for Class registration {registrationId}.");
            }

            var templates = emailIntegration.CloneEmailTemplates(
                registrationCanceledEmailType,
                new EmailTemplateEntityId(EntityTypeEnum.MainProfile, registration.ProfileId), registration.LanguageId);
            emailIntegration.SaveClonedEmailTemplates(templates, new EmailTemplateEntityId(EntityTypeEnum.MainClassRegistration, registrationId));           
        }
        
        public void CancelRegistration(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);
            registrationOperationChecker.CheckOperation(RegistrationOperation.CancelRegistration, registration);
            
            var template = GetTemplateListItemByType(registrationId, GetRegistrationCancelationType(registration.RegistrationTypeId));
            if (template == null)
                throw new InvalidOperationException($"There is no cancelation email template for Class registration {registrationId}.");
            
            registrationDb.CancelClassRegistration(registrationId);

            registrationEmailService.SendRegistrationEmailTemplate(template.EmailTemplateId, true);                        
            
            if (registration.IsApproved)
                requestManager.AddIntegrationRequestForClassRegistration(registration, AsyncRequestTypeEnum.RemoveFromOuterSystem, (int)SeverityEnum.High);
        }
        
        public void CancelRegistrationWithoutNotification(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);
            registrationOperationChecker.CheckOperation(RegistrationOperation.CancelRegistration, registration);
            
            registrationDb.CancelClassRegistration(registrationId);
            
            if (registration.IsApproved)
                requestManager.AddIntegrationRequestForClassRegistration(registration, AsyncRequestTypeEnum.RemoveFromOuterSystem, (int)SeverityEnum.High);
         
            var template = GetTemplateListItemByType(registrationId, GetRegistrationCancelationType(registration.RegistrationTypeId));
            if (template == null)
                throw new InvalidOperationException($"There is no cancelation email template for Class registraton {registrationId}.");
            emailIntegration.DeleteEmailTemplate(template.EmailTemplateId);
        }
        
        public void DiscardCancelation(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);
            registrationOperationChecker.CheckOperation(RegistrationOperation.CancelRegistration, registration);
            
            var template = GetTemplateListItemByType(registrationId, GetRegistrationCancelationType(registration.RegistrationTypeId));
            if (template == null)
                throw new InvalidOperationException($"There is no cancelation email template for Class registraton {registrationId}.");
            
            emailIntegration.DeleteEmailTemplate(template.EmailTemplateId);
        }
        
        public void ManualReview(long registrationId, long formerStudentId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            var cls = registration.Class;
            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(ClassOperation.EditRegistration, cls);
            
            var formerStudentFilter = formerFilterForReviewProvider.ResolveFormerStudentFilterForReviewRegistration(
                registration.Created,
                registration.RegistrationTypeId,
                cls.ClassTypeId);
            var formerStudent = formerDb.GetFormerStudentByIdAndFilter(formerStudentId, formerStudentFilter);
            if (formerStudent == null)
            {
                throw new InvalidOperationException($"Former student with {formerStudentId} does not exist or it cannot be used for the registration with id {registrationId}.");
            }
            
            registrationDb.SetFormerStudentToRegistration(registrationId, formerStudentId, false);
        }

        
        public void SendPaymentRequest(long id)
        {
            var registration = GetRegistrationById(id, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);

            CompleteManualReviewing(registration);

            var fullState = registrationOperationChecker.CheckOperation(RegistrationOperation.ApproveRegistration, registration);

            if (!fullState.IsReviewed.HasValue)
            {
                throw new InvalidOperationException("Payment request is available only for reviewed registrations");
            }

            registrationEmailService.SendRegistrationEmailByType(EmailTypeEnum.PaymentRequest, id, RecipientType.Student);
        }

        #endregion
        
        #region private methods
        
        private Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }
                
            return result;
        }
        
        private ClassRegistration GetRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {           
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            }
               
            return result;
        }    
        
        private EmailTemplateListItem GetTemplateListItemByType(long registrationId, EmailTypeEnum emailTypeId)
        {
            var templates = emailIntegration.GetEmailTemplateListItemsByEntity(new EmailTemplateEntityId(EntityTypeEnum.MainClassRegistration, registrationId), true);
            var result = templates.FirstOrDefault(x => x.EmailTypeId == emailTypeId);
            return result;
        }

        private EmailTypeEnum GetRegistrationCancelationType(RegistrationTypeEnum registrationTypeId)
        {
            var result = emailTypeResolver.ResolveEmailTypeForRegistration(EmailTypeEnum.RegistrationCanceled, registrationTypeId);
            return result;
        }
        
        private void CompleteManualReviewing(ClassRegistration registration)
        {
            if (registration.Class == null)
            {
                throw new InvalidOperationException("Class is not included into ClassRegistration object.");
            }

            if (registrationTypeResolver.NeedsReview(registration.RegistrationTypeId, registration.Class.ClassTypeId)
                && !registration.IsReviewed)
            {
                registrationDb.SetRegistrationToReviewed(registration.ClassRegistrationId);
            }
        }

        #endregion
    }

}
