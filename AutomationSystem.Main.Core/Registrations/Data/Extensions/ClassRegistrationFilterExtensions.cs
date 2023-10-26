using System;
using System.Linq;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.Registrations.Data.Extensions
{
    /// <summary>
    /// Common queries on ClassRegistration
    /// </summary>
    public static class ClassRegistrationFilterExtensions
    {
        public static IQueryable<ClassRegistration> Filter(this IQueryable<ClassRegistration> query,
            RegistrationFilter filter)
        {
            query = query.Active();
            if (filter?.RegistrationState == null || filter.RegistrationState.Value != RegistrationState.Temporary)
                query = query.Where(x => !x.IsTemporary);
            if (filter == null) return query;

            if (filter.ClassId.HasValue)
                query = query.Where(x => x.ClassId == filter.ClassId.Value);
            if (filter.RegistrationState.HasValue)
            {
                ClassRegistrationStateSet registrationStateSet;
                switch (filter.RegistrationState.Value)
                {
                    case RegistrationState.Temporary:
                        registrationStateSet = ClassRegistrationStateSet.Temporary;
                        break;
                    case RegistrationState.New:
                        registrationStateSet = ClassRegistrationStateSet.New;
                        break;
                    case RegistrationState.Approved:
                        registrationStateSet = ClassRegistrationStateSet.Approved;
                        break;
                    case RegistrationState.Canceled:
                        registrationStateSet = ClassRegistrationStateSet.Canceled;
                        break;
                    default:
                        throw new ArgumentException($"Unknown registration state {filter.RegistrationState.Value}");
                }

                query = query.FilterStateSet(registrationStateSet);
            }

            if (filter.RegistrationTypeIds.Any())
                query = query.Where(x => filter.RegistrationTypeIdsEnum.Contains(x.RegistrationTypeId));
            if (filter.ExcludedRegistrationTypeIds.Any())
                query = query.Where(x => !filter.ExcludedRegistrationTypeIds.Contains(x.RegistrationTypeId));
            if (filter.IsApproved.HasValue)
                query = query.Where(x => filter.IsApproved.Value == x.IsApproved);

            return query;
        }
        
        public static IQueryable<ClassRegistration> FilterStateSet(this IQueryable<ClassRegistration> query,
            ClassRegistrationStateSet stateSet)
        {
            switch (stateSet)
            {

                case ClassRegistrationStateSet.Temporary:
                    query = query.Where(x => x.IsTemporary);
                    break;

                case ClassRegistrationStateSet.New:
                    query = query.Where(x => !x.IsTemporary && !x.IsCanceled && !x.IsApproved);
                    return query;

                case ClassRegistrationStateSet.NewApproved:
                    query = query.Where(x => !x.IsTemporary && !x.IsCanceled);
                    break;

                case ClassRegistrationStateSet.Approved:
                    query = query.Where(x => !x.IsTemporary && !x.IsCanceled && x.IsApproved);
                    break;               

                case ClassRegistrationStateSet.Canceled:
                    query = query.Where(x => !x.IsTemporary && x.IsCanceled);
                    break;
            }

            return query;
        }

        
        public static IQueryable<FormerStudent> FilterForReview(this IQueryable<FormerStudent> query, ClassRegistration registration)
        {
            query = query
                .Where(x => (x.Address.CountryId == registration.StudentAddress.CountryId
                             && x.Address.FirstName.ToLower() == registration.StudentAddress.FirstName.ToLower()
                             && x.Address.LastName.ToLower() == registration.StudentAddress.LastName.ToLower()
                             && x.Address.Street.ToLower() == registration.StudentAddress.Street.ToLower()
                             && x.Address.City.ToLower() == registration.StudentAddress.City.ToLower()
                             && x.Address.ZipCode.ToLower() == registration.StudentAddress.ZipCode.ToLower())
                        || x.Email.ToLower() == registration.StudentEmail.ToLower()
                        || (x.Phone != null && registration.StudentPhone != null && x.Phone == registration.StudentPhone));                
            return query;
        }

    }

}
