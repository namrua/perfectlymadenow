using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Registrations.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.System;
using System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    /// <summary>
    /// Provides FormerClassFilter for reviewing process
    /// </summary>
    public class FormerFilterForReviewProvider : IFormerFilterForReviewProvider
    {
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IClassTypeResolver classTypeResolver;

        public FormerFilterForReviewProvider(
            IRegistrationTypeResolver registrationTypeResolver,
            IClassTypeResolver classTypeResolver)
        {
            this.registrationTypeResolver = registrationTypeResolver;
            this.classTypeResolver = classTypeResolver;
        }

        public FormerStudentFilter ResolveFormerStudentFilterForReviewRegistration(
            DateTime registrationCreated,
            RegistrationTypeEnum registrationTypeId,
            ClassTypeEnum classTypeId)
        {
            var reviewFormerClassFilter = ResolveReviewFormerClassFilter(registrationCreated, registrationTypeId, classTypeId);

            var result = new FormerStudentFilter
            {
                Class = new FormerClassFilter
                {
                    SynonymousClassTypes = classTypeResolver.GetSynonymousClassTypesForFormerClassFiltering(reviewFormerClassFilter.ClassTypeId),
                    FromDate = reviewFormerClassFilter.FromDate
                }
            };

            return result;
        }

        public ReviewFormerClassFilter ResolveReviewFormerClassFilter(
            DateTime registrationCreated,
            RegistrationTypeEnum registrationTypeId,
            ClassTypeEnum classTypeId)
        {
            var filterType = ResolveFilterType(registrationTypeId, classTypeId);
            switch (filterType)
            {
                case ReviewFormerClassFilterType.DefaultRegistrationFilter:
                    return new ReviewFormerClassFilter
                    {
                        ClassTypeId = classTypeId
                    };

                case ReviewFormerClassFilterType.BasicIINewRegistrationFilter:
                    return new ReviewFormerClassFilter
                    {
                        FromDate = GetDateYearsAgo(registrationCreated, 2)
                    };

                case ReviewFormerClassFilterType.BasicIIReviewRegistrationFilter:
                    return new ReviewFormerClassFilter
                    {
                        ClassTypeId = classTypeId,
                        FromDate = new DateTime(2011, 1, 1)
                    };

                default:
                    throw new ArgumentOutOfRangeException($"Unknown ReviewFormerClassFilterType {filterType}.");
            }
        }

        #region private fields

        private ReviewFormerClassFilterType ResolveFilterType(
            RegistrationTypeEnum registrationTypeId,
            ClassTypeEnum classTypeId)
        {
            var classTypeTopic = classTypeResolver.GetClassTypeInfo(classTypeId).Topic;

            if (classTypeTopic == ClassTypeTopic.Basic2)
            {
                if (registrationTypeResolver.IsNewRegistration(registrationTypeId))
                {
                    return ReviewFormerClassFilterType.BasicIINewRegistrationFilter;
                }

                if (registrationTypeResolver.IsReviewedRegistration(registrationTypeId))
                {
                    return ReviewFormerClassFilterType.BasicIIReviewRegistrationFilter;
                }
            }

            return ReviewFormerClassFilterType.DefaultRegistrationFilter;
        }

        private DateTime GetDateYearsAgo(DateTime dateTime, int yearsAgo)
        {
            var result = dateTime.AddYears(-yearsAgo).Date;
            return result;
        }

        #endregion
    }
}
