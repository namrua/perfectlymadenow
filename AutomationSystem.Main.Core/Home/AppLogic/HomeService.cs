using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Main.Core.Home.AppLogic.Comparers;
using AutomationSystem.Main.Core.Home.AppLogic.Convertors;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Handlers;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Core.Registrations.AppLogic;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Contract.Payment.Integration;
using AutomationSystem.Shared.Contract.Payment.Integration.Models;
using AutomationSystem.Shared.Contract.Preferences.System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutomationSystem.Main.Core.Classes.System;

namespace AutomationSystem.Main.Core.Home.AppLogic
{
    /// <summary>
    /// Provides services for home controller 
    /// </summary>
    public class HomeService : IHomeService
    {
        private readonly IDistanceAndWwaClassComparer comparer;
        private readonly ILocalisationService localisationService;
        private readonly IRegistrationLogicProviderLocalised registrationLogicProvider;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly IPriceListDatabaseLayer priceListDb;
        private readonly IPaymentDatabaseLayer paymentDb;
        private readonly ICorePreferenceProvider corePreference;
        private readonly IFormerDatabaseLayer formerDb;
        private readonly IMainAsyncRequestManager requestManager;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IClassMaterialDistributionHandler materialDistributionHandler;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IMainMapper mainMapper;
        private readonly IIncidentLogger incidentLogger;
        private readonly IPublicPaymentResolver publicPaymentResolver;
        private readonly IHomeWorkflowManager workflowManager;
        private readonly IPayPalBraintreeProviderFactory payPalBraintreeProviderFactory;
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IEmailTypeResolver emailTypeResolver;
        private readonly IRegistrationEmailService registrationEmailService;
        private readonly IWwaRegistrationSplitter registrationSplitter;
        private readonly IHomeConvertor homeConvertor;
        private readonly IHomePaymentConvertor paymentConvertor;
        private readonly IRegistrationLastClassConvertor lastClassConvertor;
        private readonly IFormerFilterForReviewProvider formerFilterForReviewProvider;
        private readonly IClassTypeResolver classTypeResolver;

        // constructor
        public HomeService(
            IDistanceAndWwaClassComparer comparer,
            ILocalisationService localisationService,
            IRegistrationLogicProviderLocalised registrationLogicProvider, 
            IRegistrationDatabaseLayer registrationDb,
            IClassDatabaseLayer classDb,
            IPriceListDatabaseLayer priceListDb,
            IPaymentDatabaseLayer paymentDb,
            ICorePreferenceProvider corePreference,
            IFormerDatabaseLayer formerDb,
            IMainAsyncRequestManager requestManager,
            IPersonDatabaseLayer personDb,
            IClassMaterialDistributionHandler materialDistributionHandler,
            IProfileDatabaseLayer profileDb,
            IMainMapper mainMapper,
            IIncidentLogger incidentLogger,
            IPublicPaymentResolver publicPaymentResolver,
            IHomeWorkflowManager workflowManager,
            IPayPalBraintreeProviderFactory payPalBraintreeProviderFactory,
            IClassOperationChecker classOperationChecker,
            IHomeConvertor homeConvertor,
            IHomePaymentConvertor paymentConvertor,
            IFormerFilterForReviewProvider formerFilterForReviewProvider,
            IRegistrationLastClassConvertor lastClassConvertor,
            IEmailTypeResolver emailTypeResolver,
            IRegistrationEmailService registrationEmailService,
            IWwaRegistrationSplitter registrationSplitter,
            IClassTypeResolver classTypeResolver)
        {
            this.comparer = comparer;
            this.localisationService = localisationService;
            this.registrationLogicProvider = registrationLogicProvider;
            this.registrationDb = registrationDb;
            this.classDb = classDb;
            this.priceListDb = priceListDb;
            this.paymentDb = paymentDb;
            this.corePreference = corePreference;
            this.formerDb = formerDb;
            this.requestManager = requestManager;
            this.personDb = personDb;
            this.materialDistributionHandler = materialDistributionHandler;
            this.profileDb = profileDb;
            this.mainMapper = mainMapper;
            this.incidentLogger = incidentLogger;
            this.publicPaymentResolver = publicPaymentResolver;
            this.workflowManager = workflowManager;
            this.payPalBraintreeProviderFactory = payPalBraintreeProviderFactory;
            this.classOperationChecker = classOperationChecker;
            this.emailTypeResolver = emailTypeResolver;
            this.registrationEmailService = registrationEmailService;
            this.registrationSplitter = registrationSplitter;
            this.homeConvertor = homeConvertor;
            this.paymentConvertor = paymentConvertor;
            this.lastClassConvertor = lastClassConvertor;
            this.formerFilterForReviewProvider = formerFilterForReviewProvider;
            this.classTypeResolver = classTypeResolver;
        }

        // gets home page model        
        public HomePageModel GetHomePageModel(EnvironmentTypeEnum? env, string profileMoniker)
        {
            // gets profile
            var profile = profileDb.GetProfileByMoniker(profileMoniker, ProfileIncludes.ClassPreference);
            if (profile == null)
                throw HomeServiceException.New(HomeServiceErrorType.InvalidPage, "There is no Profile").AddId(profileMoniker: profileMoniker);

            // gets classes and persons
            var filter = new ClassFilter
            {
                ClassState = ClassState.InRegistration,
                ProfileId = profile.ProfileId,
                NoDetachedHomepage = true,
                Env = env
            };
            var classes = classDb.GetClassesByFilter(filter, ClassIncludes.ClassPersons);
            var personHelper = new PersonHelper(personDb.GetMinimizedPersonsByProfileId(profile.ProfileId));
            var result = new HomePageModel
            {
                ProfileMoniker = profileMoniker,
                ProfilePageStyle = mainMapper.Map<RegistrationPageStyle>(profile),
                Classes = classes
                    .Where(x => publicPaymentResolver.IsPublicPaymentAllowedForClass(x))
                    .Select(x => homeConvertor.ConvertToClassPublicDetailWithInstructors(x, personHelper)).ToList()
            };

            result.Classes = registrationSplitter.SplitWwaClasses(result.Classes);
            return result;
        }


        // gets distance classes page model
        public DistanceClassesPageModel GetDistanceClassesPageModel(EnvironmentTypeEnum? env, string profileMoniker)
        {
            // gets profile
            var profile = profileDb.GetProfileByMoniker(profileMoniker, ProfileIncludes.ClassPreference);
            if (profile == null)
                throw HomeServiceException.New(HomeServiceErrorType.InvalidPage, "There is no Profile").AddId(profileMoniker: profileMoniker);

            // gets classes and persons
            var classes = GetClassesWithAllowedWwa(profile.ProfileId, env);
            classes.AddRange(GetDistanceClasses(profile.ProfileId, env));
            var orderedAndFilteredClasses = classes
                .Where(x => publicPaymentResolver.IsPublicPaymentAllowedForClass(x))
                .OrderBy(x => x.ClassCategoryId == ClassCategoryEnum.DistanceClass)
                .ThenBy(x => x.EventStart)
                .Distinct(comparer)
                .ToList();
            var personHelper = new PersonHelper(personDb.GetMinimizedPersonsByProfileId(profile.ProfileId));
            var result = new DistanceClassesPageModel
            {
                ProfilePageStyle = mainMapper.Map<RegistrationPageStyle>(profile),
                Classes = orderedAndFilteredClasses
                    .Select(x => homeConvertor.ConvertToClassPublicDetailWithInstructors(x, personHelper)).ToList()
            };

            return result;
        }


        // gets class registration selection page model       
        public ClassRegistrationSelectionPageModel GetClassRegistrationSelectionPageModel(long classId, bool? forWwa, RegistrationTypeEnum? backFromRegistrationTypeId)
        {                       
            var cls = GetClassById(classId, ClassIncludes.Currency);
            CheckClassForPublicPayment(cls, HomeWorkflowStage.RegistrationTypeSelection);

            var priceListItems = priceListDb.GetPriceListItemsByPriceListId(cls.PriceListId);
            var allowedRegistrationTypes = registrationLogicProvider.RegistrationTypeFeeder.GetAllowedTypesForPublicRegistration(cls);

            var result = new ClassRegistrationSelectionPageModel
            {
                Class = homeConvertor.ConvertToClassPublicDetail(cls),
                CurrencyCode = cls.Currency.Name,
                RegistrationTypes = allowedRegistrationTypes.Select(x => homeConvertor.ConvertToRegistrationTypeListItem(x, priceListItems)).ToList(),
            };

            forWwa = registrationSplitter.ResolveForWwa(forWwa, backFromRegistrationTypeId, cls.ClassCategoryId, cls.IsWwaFormAllowed);
            result.RegistrationTypes = registrationSplitter.FilterRegistrationTypes(result.RegistrationTypes, forWwa);
            return result;        
        }

        // gets registration confirmation page model       
        public RegistrationConfirmationPageModel GetRegistrationConfirmationPageModel(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, HomeWorkflowStage.Confirmation, ClassRegistrationIncludes.AddressesCountry |
                ClassRegistrationIncludes.RegistrationType | ClassRegistrationIncludes.Class | ClassRegistrationIncludes.ClassRegistrationLastClass);
            var formerStudent = registration.FormerStudentId.HasValue 
                ? formerDb.GetFormerStudentById(registration.FormerStudentId.Value, FormerStudentIncludes.AddressCountry | FormerStudentIncludes.FormerClass) : null;
            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registration.RegistrationTypeId);
            var controllerInfo = registrationLogicProvider.GetConvertorByRegistrationTypeId(registration.RegistrationTypeId).ControllerInfo;

            // creates result;
            var result = new RegistrationConfirmationPageModel
            {
                DetailView = controllerInfo.PartialViewDetailForHome,              
                Class = homeConvertor.ConvertToClassPublicDetail(registration.Class),
                Registration = service.GetRegistrationDetail(registration),
                FormerStudent = formerStudent == null ? null : homeConvertor.ConvertToFormerStudentForReviewListItem(formerStudent),
                WorkflowState = workflowManager.GetHomeWorkflowState(registration, HomeWorkflowStage.Confirmation),
                LastClass = registration.ClassRegistrationLastClass == null 
                    ? null : lastClassConvertor.ConvertToRegistrationLastClassDetail(registration.ClassRegistrationLastClass)            
            };
            return result;
        }


        #region registration page style getting

        // gets registration page style by profileId - CAUSES NO EXCEPTIONS!
        public RegistrationPageStyle GetRegistrationPageStyleByProfileId(long profileId)
        {
            try
            {
                var profile = profileDb.GetProfileById(profileId, ProfileIncludes.ClassPreference);
                if (profile == null)
                    return null;

                var result = mainMapper.Map<RegistrationPageStyle>(profile);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }


        // gets registration page style by classId - CAUSES NO EXCEPTIONS!
        public RegistrationPageStyle GetRegistrationPageStyleByClassId(long? classId)
        {
            // WARNING: this method is used also for error pages and therefore it should not cause an exception!
            try
            {
                // gets homepage url by classId 
                if (classId.HasValue)
                {
                    var cls = classDb.GetClassById(classId.Value, ClassIncludes.ClassStyle | ClassIncludes.Profile);      // WARNING: use direct access to DB because GetClassById causes validations!
                    if (cls == null)
                        return null;
                    var result = homeConvertor.ConvertToRegistrationPageStyle(cls.ClassStyle, cls.ClassCategoryId, cls.Profile.Moniker);
                    return result;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // gets registration page style by registrationId - CAUSES NO EXCEPTIONS!
        public RegistrationPageStyle GetRegistrationPageStyleByRegistrationId(long? classRegistrationId)
        {
            // WARNING: this method is used also for error pages and therefore it should not cause an exception!
            try
            {
                // gets homepage url by registrationId
                if (classRegistrationId.HasValue)
                {
                    var reg = registrationDb.GetClassRegistrationById(classRegistrationId.Value, ClassRegistrationIncludes.ClassClassStyle | ClassRegistrationIncludes.ClassProfile);
                    if (reg == null)
                        return null;
                    var result = homeConvertor.ConvertToRegistrationPageStyle(reg.Class.ClassStyle, reg.Class.ClassCategoryId, reg.Class.Profile.Moniker);
                    return result;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion


        #region registration edit

        // gets controller info     
        public RegistrationControllerInfo GetControllerInfoByRegistrationTypeId(RegistrationTypeEnum registrationTypeId)
        {
            var convertor = registrationLogicProvider.GetConvertorByRegistrationTypeId(registrationTypeId);
            var result = convertor.ControllerInfo;
            return result;
        }
       

        // gets new registration for edit       
        public IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEdit(RegistrationTypeEnum registrationTypeId, long classId)
        {
            var cls = GetClassById(classId);
            CheckClassForPublicPayment(cls, HomeWorkflowStage.PersonalDataForm);
            var result = GetNewRegistrationForEditForClass(registrationTypeId, cls);
            return result;
        }

        // gets new registration for edit by invitation request
        public IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEditByInvitationRequest(string invitationRequest)
        {
            // loads and checks invitation
            var invitation = GetClassRegistrationInvitation(requestCode: invitationRequest);
            var cls = GetClassById(invitation.ClassId);

            // calls GetNewRegistrationForEdit and adds request to form
            var result = GetNewRegistrationForEditForClass(invitation.RegistrationTypeId, cls);
            result.Form.InvitationRequest = invitationRequest;
            return result;
        }


        // gets registration for edit by registration id
        public IRegistrationForEdit<BaseRegistrationForm> GetRegistrationForEdit(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, HomeWorkflowStage.PersonalDataForm, ClassRegistrationIncludes.Addresses | ClassRegistrationIncludes.Class);
            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registration.RegistrationTypeId);
            var result = service.GetRegistrationForEdit(registration);
            return result;
        }

        // gets registration for edit based on form       
        public IRegistrationForEdit<BaseRegistrationForm> GetFormRegistrationForEdit(BaseRegistrationForm form)
        {
            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(form.RegistrationTypeId);
            var result = service.GetFormRegistrationForEdit(form);
            return result;
        }

        // saves registration        
        public HomeWorkflowState SaveRegistration(BaseRegistrationForm form)
        {
            // validation
            long? invitationId = null;           
            var newRegistration = form.ClassRegistrationId == 0;
            if (newRegistration)
            {
                // checks class
                var cls = GetClassById(form.ClassId);
                registrationLogicProvider.RegistrationTypeFeeder.CheckRegistrationTypeForPublicRegistration(cls, form.RegistrationTypeId);

                // check registration invitation request
                if (form.InvitationRequest != null)
                {
                    var invitation = GetClassRegistrationInvitation(requestCode: form.InvitationRequest);
                    invitationId = invitation.ClassRegistrationInvitationId;
                }
                else
                {
                    // checks public payment for non-invitation registrations
                    CheckClassForPublicPayment(cls, HomeWorkflowStage.PersonalDataSave);
                }
            }
            else
            {
                GetRegistrationById(form.ClassRegistrationId, HomeWorkflowStage.PersonalDataSave, ClassRegistrationIncludes.Class);
            }

            // saves registration 
            // WARNING ApprovementType cannot be resaved when registration is updated!!! 
            // InvitationApprovement is set only once (part of form from initialization)
            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(form.RegistrationTypeId);
            var registrationId = service.SaveRegistration(form, true, 
                invitationId.HasValue ? ApprovementTypeEnum.InvitationApprovement : ApprovementTypeEnum.None);

            // resets reviewed student
            if (!newRegistration)
            {
                registrationDb.SetFormerStudentToRegistration(registrationId, null, true);
            }

            // assigns registration id to invitation
            if (newRegistration && invitationId.HasValue)
            {
                registrationDb.SetClassRegistrationIdToClassRegistrationInvitation(invitationId.Value, registrationId);
            }

            // loads workflow state
            var currentRegistration = GetRegistrationById(registrationId, HomeWorkflowStage.PersonalDataSave, ClassRegistrationIncludes.Class);
            var result = workflowManager.GetHomeWorkflowState(currentRegistration, HomeWorkflowStage.PersonalDataSave);
            return result;
        }

        #endregion


        #region reviewing

        // gets former student selection page model       
        public FormerStudentSelectionPageModel GetFormerStudentSelectionPageModel(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, HomeWorkflowStage.FormerStudentSelection, ClassRegistrationIncludes.AddressesCountry |
                ClassRegistrationIncludes.RegistrationType | ClassRegistrationIncludes.Class | ClassRegistrationIncludes.ClassRegistrationLastClass);

            var formerStudentFilter = formerFilterForReviewProvider.ResolveFormerStudentFilterForReviewRegistration(
                registration.Created,
                registration.RegistrationTypeId,
                registration.Class.ClassTypeId);
            var formerStudents = formerDb.GetFormerStudentsForReviewing(registration, formerStudentFilter, FormerStudentIncludes.AddressCountry | FormerStudentIncludes.FormerClass);
            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registration.RegistrationTypeId);
            var controllerInfo = registrationLogicProvider.GetConvertorByRegistrationTypeId(registration.RegistrationTypeId).ControllerInfo;

            // creates result;
            var result = new FormerStudentSelectionPageModel
            {
                DetailView = controllerInfo.PartialViewDetailForHome,
                SelectedFormerStudentId = registration.FormerStudentId,
                Class = homeConvertor.ConvertToClassPublicDetail(registration.Class),
                Registration = service.GetRegistrationDetail(registration),
                FormerStudents = formerStudents.Select(homeConvertor.ConvertToFormerStudentForReviewListItem).ToList(),
                FormLastClass = registration.ClassRegistrationLastClass == null 
                    ? new RegistrationLastClassForm { ClassRegistrationId = registrationId } 
                    : lastClassConvertor.ConvertToRegistrationLastClassForm(registration.ClassRegistrationId, registration.ClassRegistrationLastClass)
            };           
            return result;
        }


        // saves registration last class      
        public long SaveRegistrationLastClass(RegistrationLastClassForm form)
        {
            var registration = GetRegistrationById(form.ClassRegistrationId, HomeWorkflowStage.FormerStudentSelection, 
                ClassRegistrationIncludes.Class | ClassRegistrationIncludes.ClassRegistrationLastClass);

            // reset former student
            registrationDb.SetFormerStudentToRegistration(form.ClassRegistrationId, null, true);

            // saves last class
            var dbLastClass = lastClassConvertor.ConvertToClassRegistrationLastClass(form);
            var result = registrationDb.InsertUpdateRegistrationLastClass(registration.ClassRegistrationId, dbLastClass);
            return result;
        }



        // saves former student selection      
        public void SaveFormerStudentSelection(long registrationId, long? formerStudentId)
        {
            var registration = GetRegistrationById(registrationId, HomeWorkflowStage.FormerStudentSelection, ClassRegistrationIncludes.Class);
            var cls = registration.Class;

            // checks that former student exists in correct reviewing class scope
            if (formerStudentId.HasValue)
            {
                var formerStudentFilter = formerFilterForReviewProvider.ResolveFormerStudentFilterForReviewRegistration(
                    registration.Created,
                    registration.RegistrationTypeId,
                    cls.ClassTypeId);
                var formerStudent = formerDb.GetFormerStudentByIdAndFilter(formerStudentId.Value, formerStudentFilter);
                if (formerStudent == null)
                {
                    throw new InvalidOperationException(
                        $"Former student with {formerStudentId.Value} does not exist or it cannot be used for the registration with id {registrationId}.");
                }
            }

            // executes assignment
            registrationDb.SetFormerStudentToRegistration(registrationId, formerStudentId, true);
        }

        #endregion


        #region agreement acceptance

        // gets agreement registration page model
        public RegistrationAgreementPageModel GetRegistrationAgreementPageModel(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, HomeWorkflowStage.Agreement, ClassRegistrationIncludes.Class);
            var result = new RegistrationAgreementPageModel
            {
                Class = homeConvertor.ConvertToClassPublicDetail(registration.Class)             
            };

            // registration agreement form
            result.Form = new RegistrationAgreementForm
            {
                ClassRegistrationId = registrationId,
                AcceptAgreements = registration.AreAgreementsAccepted
            };
            return result;
        }

        // saves registration acceptiation state, determines whether it is possible to proceed with registration
        public HomeWorkflowState SaveRegistrationAgreementAcceptationState(RegistrationAgreementForm form)
        {
            // validation - don't delete!
            GetRegistrationById(form.ClassRegistrationId, HomeWorkflowStage.AgreementAcceptation, ClassRegistrationIncludes.Class);

            // executes operation
            registrationDb.SetRegistrationAgreementAcceptationState(form.ClassRegistrationId, form.AcceptAgreements);
            var registration = GetRegistrationById(form.ClassRegistrationId, HomeWorkflowStage.AgreementAcceptation, ClassRegistrationIncludes.Class);
            var result = workflowManager.GetHomeWorkflowState(registration, HomeWorkflowStage.AgreementAcceptation);

            // process special cases (WARNING: preserve order "invitation first")
            if (result.Properties.HasFlag(HomeWorkflowProperty.AgreementAccepted))
            {
                // is invitation 
                if (result.Properties.HasFlag(HomeWorkflowProperty.IsInvitation))
                {
                    // laods and checks invitation state
                    GetClassRegistrationInvitation(registrationId: form.ClassRegistrationId);

                    // process registration and send emails
                    registrationDb.SetTemporaryRegistrationForApprovement(form.ClassRegistrationId, 
                        ApprovementTypeEnum.InvitationApprovement, RegistrationOperationOption.OnlyForTemporary);
                    registrationEmailService.SendRegistrationEmailByType(
                        emailTypeResolver.ResolveEmailTypeForRegistration(EmailTypeEnum.InvitationFilledIn, registration.RegistrationTypeId),
                        form.ClassRegistrationId, RecipientType.Coordinator, true);
                    return result;
                }

                // needs manual reviewing
                if (result.Properties.HasFlag(HomeWorkflowProperty.NeedManualReviewing))
                {
                    registrationDb.SetTemporaryRegistrationForApprovement(form.ClassRegistrationId, 
                        ApprovementTypeEnum.ManualReview, RegistrationOperationOption.OnlyForTemporary);
                    registrationEmailService.SendRegistrationEmailByType(EmailTypeEnum.ManualVerificationNotification,
                        form.ClassRegistrationId, RecipientType.Student, true);
                    registrationEmailService.SendRegistrationEmailByType(EmailTypeEnum.ManualVerificationRequest,
                        form.ClassRegistrationId, RecipientType.Coordinator, true);
                    return result;
                }
            }           
            return result;
        }

        #endregion


        #region payment

        // get registration payment page model        
        public RegistrationPaymentPageModel GetRegistrationPaymentPageModel(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, HomeWorkflowStage.Payment, ClassRegistrationIncludes.ClassCurrency);
            var priceListItems = priceListDb.GetPriceListItemsByPriceListId(registration.Class.PriceListId);
            var payPalKey = paymentDb.GetPayPalKeyById(registration.Class.PayPalKeyId ?? 0);            

            var result = new RegistrationPaymentPageModel
            {
                ClassRegistrationId = registrationId,
                Class = homeConvertor.ConvertToClassPublicDetail(registration.Class),
                RegistrationType = homeConvertor.ConvertToRegistrationTypeListItem(registration.RegistrationTypeId, priceListItems),
                WorkflowState = workflowManager.GetHomeWorkflowState(registration, HomeWorkflowStage.Payment),
            };

            // check price
            if (!result.RegistrationType.Price.HasValue)
                throw new InvalidOperationException($"Price is not defined for registration type {registration.RegistrationTypeId} and Price list with id {registration.Class.PriceListId}.");

            // initializes paypal
            try
            {
                var payPalProvider = payPalBraintreeProviderFactory.CreatePayPalBraintreeProvider(payPalKey.BraintreeToken);
                result.PayPalTransactionInfo = new PayPalTransactionInfo
                {
                    ClientToken = payPalProvider.GenerateClientToken(),
                    Amount = result.RegistrationType.Price.Value,
                    CurrencyCode = registration.Class.Currency.Name,
                    Environment = payPalKey.Environment,
                    Locale = GetPayPalLocale()
                };
            }
            catch (Exception e)
            {
                throw HomeServiceException.New(HomeServiceErrorType.GenericError, "Braintree problem", e)
                    .AddId(classId: registration.ClassId, classRegistrationId: registrationId);
            }

            return result;
        }

        // executes payment              
        public PaymentResult ExecutePayment(PaymentExecutionInput input)
        {
            var registrationId = input.RegistrationId;
            var registration = GetRegistrationById(registrationId, HomeWorkflowStage.Payment, ClassRegistrationIncludes.Class);
            var priceListItems = priceListDb.GetPriceListItemsByPriceListId(registration.Class.PriceListId);
            var payPalKey = paymentDb.GetPayPalKeyById(registration.Class.PayPalKeyId ?? 0);           
               
            // gets price
            var price = homeConvertor.GetRegistrationPrice(registration.RegistrationTypeId, priceListItems);            

            // executes payment
            PaymentResult result;
            PaymentExecutionSummary paymentExecutionSummary;
            try
            {
                var payPalProvider = payPalBraintreeProviderFactory.CreatePayPalBraintreeProvider(payPalKey.BraintreeToken);
                var listItemInfo = paymentConvertor.GetPaymentListItemInfo(
                    price,
                    registration.RegistrationTypeId,
                    registration.Class.ClassCategoryId,
                    registration.Class.EventStart);
                paymentExecutionSummary = payPalProvider.ExecutePayment(input.Nonce, listItemInfo);
                result = paymentConvertor.ConvertToPaymentResult(paymentExecutionSummary);
                result.ClassId = registration.ClassId;
            }
            catch (Exception e)
            {
                throw HomeServiceException.New(HomeServiceErrorType.GenericError, "Braintree problem", e)
                    .AddId(classId: registration.ClassId, classRegistrationId: registrationId);
            }

            // process operation after payment
            if (result.IsSuccess)
            {
                // saves registraton payment and approves registration
                var payPalRecord = paymentConvertor.ConvertToPayPalRecord(registrationId, paymentExecutionSummary, input.DetailsJson);
                var payPalRecordId = paymentDb.InsertPayPalRecord(payPalRecord);
                var classRegistrationPayment = paymentConvertor.ConvertToClassRegistrationPayment(paymentExecutionSummary, payPalRecordId);
                registrationDb.UpdateClassRegistrationPaymentAndApprove(registrationId, classRegistrationPayment);

                // adds registration to webes
                requestManager.AddIntegrationRequestForClassRegistration(registration, AsyncRequestTypeEnum.AddToOuterSystem, (int) SeverityEnum.High);

                // sends registration confirmation email
                if (classTypeResolver.AreAutomationNotificationsAllowed(registration.Class.ClassCategoryId))
                {
                    registrationEmailService.SendRegistrationEmailByType(
                        emailTypeResolver.ResolveEmailTypeForRegistration(EmailTypeEnum.RegistrationConfirmation, registration.RegistrationTypeId),
                        registrationId, RecipientType.Student, true);
                }

                // if class id distance class, sends WWA student registration notification
                if (registration.Class.ClassCategoryId == ClassCategoryEnum.DistanceClass)
                    registrationEmailService.SendRegistrationEmailByType(EmailTypeEnum.WwaStudentRegistrationNotification, 
                        registrationId, RecipientType.Coordinator, true);

                // distribute materials
                materialDistributionHandler.HandleRegistrationAprovement(registration, true);
            }
            else
            {
                // creates incident
                var incident = IncidentForLog.New(IncidentTypeEnum.PayPalError, $"Paymant by PayPal for registration with id {registrationId} fails.", result.Message);
                incident.Entity(EntityTypeEnum.MainClassRegistration, registrationId);
                incidentLogger.LogIncident(incident);
            }
            return result;        
        }

        #endregion


        #region private methods

        // gets new registration for edit for class and registration type
        private IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEditForClass(RegistrationTypeEnum registrationTypeId, Class cls)
        {
            registrationLogicProvider.RegistrationTypeFeeder
                .CheckRegistrationTypeForPublicRegistration(cls, registrationTypeId);
            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registrationTypeId);
            var result = service.GetNewRegistrationForEdit(registrationTypeId, cls);
            return result;
        }

        // gets class by id (with includes)
        private Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {            
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
                throw HomeServiceException.New(HomeServiceErrorType.InvalidPage, "There is no Class").AddId(classId: classId);

            classOperationChecker.CheckPublicRegistrationForHomeService(result);
            return result;
        }

        // checks whether class supports public payment
        private void CheckClassForPublicPayment(Class cls, HomeWorkflowStage stage)
        {
            if (!publicPaymentResolver.IsPublicPaymentAllowedForClass(cls))
            {
                throw HomeServiceException.New(HomeServiceErrorType.InvalidRegistrationStep, "Public payment not allowed.")
                    .AddId(classId: cls.ClassId, homeWorkflowStage: stage);
            }
        }

        // gets registration
        private ClassRegistration GetRegistrationById(long registrationId, HomeWorkflowStage stage, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {            
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
                throw HomeServiceException.New(HomeServiceErrorType.InvalidPage, "There is no Class registration").AddId(classRegistrationId: registrationId);  
            
            // checks class state
            if (includes.HasFlag(ClassRegistrationIncludes.Class))
                classOperationChecker.CheckPublicRegistrationForHomeService(result.Class);

            // checks registration workflow state
            workflowManager.CheckForWorkflowState(result, stage);

            // checks registration type / pre-reg expiration
            if (includes.HasFlag(ClassRegistrationIncludes.Class))
                registrationLogicProvider.RegistrationTypeFeeder.CheckRegistrationTypeForPublicRegistration(result.Class, result.RegistrationTypeId);

            return result;
        }

        // gets pay pal locale
        private string GetPayPalLocale()
        {
            var supportedLocale = corePreference.GetPayPalPreferences().SupportedLocale;
            var locale = new StringBuilder(CultureInfo.CreateSpecificCulture(localisationService.CurrentCultureInfo.Name).IetfLanguageTag);
            locale[2] = '_';
            var result = locale.ToString();
            return supportedLocale.Contains(result) ? result : "en_US";
        }


        // gets class invitation and checks it
        private ClassRegistrationInvitation GetClassRegistrationInvitation(string requestCode = null, long? registrationId = null)
        {
            ClassRegistrationInvitation result = null;
            if (requestCode != null)
                result = registrationDb.GetClassRegistrationInvitationByRequestCode(requestCode, ClassRegistrationInvitationIncludes.ClassRegistration);
            if (registrationId.HasValue)
                result = registrationDb.GetClassRegistrationInvitationByRegistrationId(registrationId.Value, ClassRegistrationInvitationIncludes.ClassRegistration);

            if (result == null)
                throw HomeServiceException.New(HomeServiceErrorType.InvitationExpired, "Invitation not found")
                    .AddId(token: requestCode, classRegistrationId: registrationId);

            var invitationState = RegistrationInvitationConvertor.GetRegistrationInvitationState(result);
            if (invitationState != ClassInvitationState.New)
                throw HomeServiceException.New(HomeServiceErrorType.InvitationExpired, "Invitation already used")
                    .AddId(token: requestCode, classRegistrationId: registrationId, classId: result.ClassRegistration.ClassId);
            
            return result;
        }

        private List<Class> GetDistanceClasses(long profileId, EnvironmentTypeEnum? env)
        {
            var filter = new ClassFilter
            {
                ClassState = ClassState.InRegistration,
                ProfileId = profileId,
                NoDetachedHomepage = true,
                ClassCategoryId = ClassCategoryEnum.DistanceClass,
                Env = env
            };
            var classes = classDb.GetClassesByFilter(filter, ClassIncludes.ClassPersons);
            return classes;
        }

        private List<Class> GetClassesWithAllowedWwa(long profileId, EnvironmentTypeEnum? env)
        {
            var filter = new ClassFilter
            {
                ClassState = ClassState.InRegistration,
                ProfileId = profileId,
                NoDetachedHomepage = true,
                IsWwaAllowed = true,
                Env = env
            };
            var result = classDb.GetClassesByFilter(filter, ClassIncludes.ClassPersons);
            return result;
        }

        #endregion

    }

}
