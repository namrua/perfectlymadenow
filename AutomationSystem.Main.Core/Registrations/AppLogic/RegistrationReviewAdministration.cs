using System;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    public class RegistrationReviewAdministration : IRegistrationReviewAdministration
    {
        private readonly IFormerDatabaseLayer formerDb;
        private readonly IFormerFilterForReviewProvider formerFilterForRewievProvider;
        private readonly IFormerStudentConvertor formerStudentConvertor;
        private readonly IIdentityResolver identityResolver;
        private readonly IRegistrationLastClassConvertor lastClassConvertor;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IRegistrationTypeResolver registrationTypeResolver;


        // constructor
        public RegistrationReviewAdministration(
            IFormerDatabaseLayer formerDb,
            IFormerFilterForReviewProvider formerFilterForRewievProvider,
            IFormerStudentConvertor formerStudentConvertor,
            IIdentityResolver identityResolver,
            IRegistrationLastClassConvertor lastClassConvertor,
            IRegistrationDatabaseLayer registrationDb,
            IClassOperationChecker classOperationChecker,
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.formerDb = formerDb;
            this.formerFilterForRewievProvider = formerFilterForRewievProvider;
            this.formerStudentConvertor = formerStudentConvertor;
            this.identityResolver = identityResolver;
            this.lastClassConvertor = lastClassConvertor;
            this.registrationDb = registrationDb;
            this.classOperationChecker = classOperationChecker;
            this.registrationTypeResolver = registrationTypeResolver;
        }

        public RegistrationManualReviewPageModel GetRegistrationManualReviewPageModel(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class | ClassRegistrationIncludes.ClassRegistrationLastClass);
            identityResolver.CheckEntitleForClass(registration.Class);

            var result = new RegistrationManualReviewPageModel
            {
                NeedsReview = registrationTypeResolver.NeedsReview(registration.RegistrationTypeId, registration.Class.ClassTypeId),
                IsReviewed = registration.IsReviewed,
                ClassId = registration.ClassId,
                ClassRegistrationId = registrationId,
                ClassState = ClassConvertor.GetClassState(registration.Class),
                ReviewFormerClassFilter = formerFilterForRewievProvider.ResolveReviewFormerClassFilter(
                    registration.Created,
                    registration.RegistrationTypeId,
                    registration.Class.ClassTypeId),
                LastClassDetail = registration.ClassRegistrationLastClass == null ? null
                    : lastClassConvertor.ConvertToRegistrationLastClassDetail(registration.ClassRegistrationLastClass),

                CanPick = identityResolver.IsEntitleGranted(Entitle.MainFormerClassesReadOnly)
            };
            result.CanPickOperation = classOperationChecker.IsOperationAllowed(ClassOperation.EditRegistration, result.ClassState);

            if (!result.NeedsReview)
            {
                return result;
            }
            
            if (registration.FormerStudentId.HasValue)
            {
                var formerStudent = formerDb.GetFormerStudentById(registration.FormerStudentId.Value,
                    FormerStudentIncludes.AddressCountry | FormerStudentIncludes.FormerClassClassType | FormerStudentIncludes.FormerClassProfile);
                if (formerStudent == null)
                {
                    throw new ArgumentException(
                        $"There is no Former student with id {registration.FormerStudentId.Value}.");
                }

                result.FormerStudentDetail = formerStudentConvertor.ConvertToFormerStudentDetail(formerStudent, true);
            }
            return result;
        }

        #region private methods
        private ClassRegistration GetRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            return result;
        }
        #endregion
    }
}
