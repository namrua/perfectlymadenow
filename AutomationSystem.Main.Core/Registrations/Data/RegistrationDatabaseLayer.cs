using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.AppLogic;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Core.Registrations.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.Data.Extensions;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.Registrations.Data
{

    /// <summary>
    /// Provides registration database layer
    /// </summary>
    public class RegistrationDatabaseLayer : IRegistrationDatabaseLayer
    {

        private readonly IAddressConvertor addressConvertor;
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IRegistrationOperationChecker registrationOperationChecker;


        public RegistrationDatabaseLayer(
            IAddressConvertor addressConvertor,
            IClassOperationChecker classOperationChecker,
            IRegistrationOperationChecker registrationOperationChecker)
        {
            this.addressConvertor = addressConvertor;
            this.classOperationChecker = classOperationChecker;
            this.registrationOperationChecker = registrationOperationChecker;
        }

        
        public List<ClassRegistration> GetRegistrationsByStateSet(long classId, ClassRegistrationStateSet stateSet,
            ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrations.AddIncludes(includes).Active().Where(x => x.ClassId == classId)
                    .FilterStateSet(stateSet).ToList();
                result = result.Select(x => ClassRegistrationRemoveInActive.RemoveInactiveForClassRegistration(x, includes)).ToList();
                return result;
            }
        }
        
        public List<ClassRegistration> GetRegistrationsByFilter(RegistrationFilter filter,
            ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrations.AddIncludes(includes).Filter(filter).ToList();
                result = result.Select(x => ClassRegistrationRemoveInActive.RemoveInactiveForClassRegistration(x, includes)).ToList();
                return result;
            }
        }
        
        public List<ClassRegistration> GetRegistrationsByIds(IEnumerable<long> registrationIds,
            ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrations.AddIncludes(includes).Active()
                    .Where(x => registrationIds.Contains(x.ClassRegistrationId)).ToList();
                result = result.Select(x => ClassRegistrationRemoveInActive.RemoveInactiveForClassRegistration(x, includes)).ToList();
                return result;
            }
        }

        
        public ClassRegistration GetClassRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrations.AddIncludes(includes).Active().FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                result = ClassRegistrationRemoveInActive.RemoveInactiveForClassRegistration(result, includes);
                return result;
            }
        }


        
        public long InsertClassRegistration(ClassRegistration classRegistration)
        {
            using (var context = new MainEntities())
            {
                context.ClassRegistrations.Add(classRegistration);
                context.SaveChanges();
                return classRegistration.ClassRegistrationId;
            }
        }
        
        public void UpdateClassRegistration(ClassRegistration classRegistration,
            RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassRegistrations.AddIncludes(ClassRegistrationIncludes.Addresses).Active()
                    .FirstOrDefault(x => x.ClassRegistrationId == classRegistration.ClassRegistrationId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Class registration with id {classRegistration.ClassRegistrationId}.");

                if (toUpdate.ClassId != classRegistration.ClassId)
                    throw new SecurityException($"Current id of Class {toUpdate.ClassId} is inconsistent with new id {classRegistration.ClassId}.");
                if (toUpdate.RegistrationTypeId != classRegistration.RegistrationTypeId)
                    throw new SecurityException($"Current id of Registration type {toUpdate.RegistrationTypeId} is inconsistent with new id {classRegistration.RegistrationTypeId}.");               

                toUpdate.StudentEmail = classRegistration.StudentEmail;
                toUpdate.StudentPhone = classRegistration.StudentPhone;
                toUpdate.RegistrantEmail = classRegistration.RegistrantEmail;
                toUpdate.RegistrantPhone = classRegistration.RegistrantPhone;
                toUpdate.LanguageId = classRegistration.LanguageId;

                addressConvertor.UpdateAddresss(toUpdate.StudentAddress, classRegistration.StudentAddress);
                if (toUpdate.RegistrantAddress == null && classRegistration.RegistrantAddress != null)
                    toUpdate.RegistrantAddress = classRegistration.RegistrantAddress;
                if (toUpdate.RegistrantAddress != null && classRegistration.RegistrantAddress == null)
                {
                    toUpdate.RegistrantAddressId = null;
                    context.Addresses.Remove(toUpdate.RegistrantAddress);
                }
                if (toUpdate.RegistrantAddress != null && classRegistration.RegistrantAddress != null) 
                    addressConvertor.UpdateAddresss(toUpdate.RegistrantAddress, classRegistration.RegistrantAddress);

                context.SaveChanges();
            }
        }
        
        public void UpdateClassRegistrationPayment(long registrationId, ClassRegistrationPayment classRegistrationPayment,
            RegistrationOperationOption option = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var registration = context.ClassRegistrations.AddIncludes(ClassRegistrationIncludes.ClassRegistrationPayment).Active()
                    .FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                if (registration == null)
                    throw new ArgumentException($"There is no Class registration with id {registrationId}.");

                var toUpdate = registration.ClassRegistrationPayment;
                toUpdate.CheckNumber = classRegistrationPayment.CheckNumber;
                toUpdate.TransactionNumber = classRegistrationPayment.TransactionNumber;
                toUpdate.PayPalFee = classRegistrationPayment.PayPalFee;
                toUpdate.TotalPayPal = classRegistrationPayment.TotalPayPal;
                toUpdate.TotalCheck = classRegistrationPayment.TotalCheck;
                toUpdate.TotalCash = classRegistrationPayment.TotalCash;
                toUpdate.TotalCreditCard = classRegistrationPayment.TotalCreditCard;
                toUpdate.IsPaidPmi = classRegistrationPayment.IsPaidPmi;
                toUpdate.IsAbsentee = classRegistrationPayment.IsAbsentee;

                context.SaveChanges();
            }
        }

        
        public ClassRegistration ApproveClassRegistration(long registrationId)
        {
            using (var context = new MainEntities())
            {
                var toApprove = context.ClassRegistrations.AddIncludes(ClassRegistrationIncludes.Class).Active().FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                if (toApprove == null)
                    throw new ArgumentException($"There is no Class registration with id {registrationId}.");

                toApprove.IsApproved = true;
                toApprove.Approved = DateTime.Now;
                context.SaveChanges();

                return toApprove;
            }
        }
        
        public void SetRegistrationToReviewed(long registrationId)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassRegistrations.Active().FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no Class registration with id {registrationId}.");
                }

                toUpdate.IsReviewed = true;

                context.SaveChanges();
            }
        }

        
        public void CancelClassRegistration(long registrationId, RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var includes = options.HasFlag(RegistrationOperationOption.CheckOperation)
                    ? ClassRegistrationIncludes.Class
                    : ClassRegistrationIncludes.None;
                var toCancel = context.ClassRegistrations.AddIncludes(includes).Active().FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                if (toCancel == null)
                    throw new ArgumentException($"There is no Class registration with id {registrationId}.");

                if (options.HasFlag(RegistrationOperationOption.CheckOperation))
                    registrationOperationChecker.CheckOperation(RegistrationOperation.CancelRegistration, toCancel);

                toCancel.IsCanceled = true;
                toCancel.Canceled = DateTime.Now;
                context.SaveChanges();
            }
        }

        
        public long? DeleteClassRegistration(long registrationId, RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var includes = options.HasFlag(RegistrationOperationOption.CheckOperation)
                    ? ClassRegistrationIncludes.Class 
                    : ClassRegistrationIncludes.None;

                var toDelete = context.ClassRegistrations.AddIncludes(includes).Active().FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                if (toDelete == null) return null;                    

                if (options.HasFlag(RegistrationOperationOption.CheckOperation))
                    registrationOperationChecker.CheckOperation(RegistrationOperation.DeleteRegistration, toDelete);
                
                context.ClassRegistrations.Remove(toDelete);                
                context.SaveChanges();
                return toDelete.ClassId;
            }

        }


        #region registration operations
        
        public void SetRegistrationAgreementAcceptationState(long registrationId, bool areAgreementsAccepted,
            RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {               
                var toUpdate = context.ClassRegistrations.Active().FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Class registration with id {registrationId}.");            

                toUpdate.AreAgreementsAccepted = areAgreementsAccepted;
                context.SaveChanges();                
            }
        }

        
        public void UpdateClassRegistrationPaymentAndApprove(long registrationId, ClassRegistrationPayment classRegistrationPayment,
            RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassRegistrations.AddIncludes(ClassRegistrationIncludes.ClassRegistrationPayment)
                    .Active().First(x => x.ClassRegistrationId == registrationId);
                
                toUpdate.IsTemporary = false;
                toUpdate.IsApproved = true;
                toUpdate.Approved = DateTime.Now;
                
                toUpdate.ClassRegistrationPayment.TransactionNumber = classRegistrationPayment.TransactionNumber;
                toUpdate.ClassRegistrationPayment.PayPalFee = classRegistrationPayment.PayPalFee;
                toUpdate.ClassRegistrationPayment.TotalPayPal = classRegistrationPayment.TotalPayPal;
                toUpdate.ClassRegistrationPayment.PayPalRecordId = classRegistrationPayment.PayPalRecordId;
                context.SaveChanges();               
            }
        }

        
        public void SetFormerStudentToRegistration(long registrationId, long? formerStudentId, bool updateIsReviewed)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassRegistrations.Active().FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no Class registration with id {registrationId}.");
                }

                toUpdate.FormerStudentId = formerStudentId;
                if (updateIsReviewed)
                {
                    toUpdate.IsReviewed = formerStudentId != null;
                }

                context.SaveChanges();
            }           
        }

        
        public void SetTemporaryRegistrationForApprovement(long registrationId, ApprovementTypeEnum? approvementTypeId, RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var includes = options.HasFlag(RegistrationOperationOption.CheckOperation)
                    ? ClassRegistrationIncludes.Class
                    : ClassRegistrationIncludes.None;

                var toUpdate = context.ClassRegistrations.AddIncludes(includes).Active().FirstOrDefault(x => x.ClassRegistrationId == registrationId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Class registration with id {registrationId}.");

                if (options.HasFlag(RegistrationOperationOption.CheckOperation))
                {
                    classOperationChecker.CheckOperation(ClassOperation.PublicRegistration, toUpdate.Class);
                }
                if (options.HasFlag(RegistrationOperationOption.OnlyForTemporary) && !toUpdate.IsTemporary)
                    throw new InvalidOperationException($"Class registration with id {registrationId} is not temporary.");

                toUpdate.IsTemporary = false;
                toUpdate.ApprovementTypeId = approvementTypeId ?? toUpdate.ApprovementTypeId;
                context.SaveChanges();
            }
        }

        
        public long InsertUpdateRegistrationLastClass(long registrationId,
            ClassRegistrationLastClass registrationLastClass,
            RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var registrationToUpdate = context.ClassRegistrations
                    .AddIncludes(ClassRegistrationIncludes.ClassRegistrationLastClass).Active()
                    .FirstOrDefault(x => x.ClassRegistrationId == registrationId);

                if (registrationToUpdate == null)
                    throw new ArgumentException($"There is no Class registration with id {registrationId}");

                // resets former student
                registrationToUpdate.FormerStudentId = null;

                // inserts or updates last class
                if (registrationToUpdate.ClassRegistrationLastClass == null)
                    registrationToUpdate.ClassRegistrationLastClass = registrationLastClass;
                else
                {
                    var toUpdate = registrationToUpdate.ClassRegistrationLastClass;
                    toUpdate.Location = registrationLastClass.Location;
                    toUpdate.Year = registrationLastClass.Year;
                    toUpdate.Month = registrationLastClass.Month;
                }
                
                context.SaveChanges();
                return registrationToUpdate.ClassRegistrationLastClass.ClassRegistrationLastClassId;
            }
        }

        #endregion


        #region invitations
        
        public List<ClassRegistrationInvitation> GetClassRegistrationInvitations(long classId,
            ClassRegistrationInvitationIncludes includes = ClassRegistrationInvitationIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrationInvitations
                    .AddIncludes(includes).Active().Where(x => x.ClassId == classId).ToList();
                return result;
            }
        }
        
        public ClassRegistrationInvitation GetClassRegistrationInvitationById(long classRegistrationInvitationId,
            ClassRegistrationInvitationIncludes includes = ClassRegistrationInvitationIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrationInvitations
                    .AddIncludes(includes).Active().FirstOrDefault(x => x.ClassRegistrationInvitationId == classRegistrationInvitationId);
                return result;
            }
        }

        
        public ClassRegistrationInvitation GetClassRegistrationInvitationByRequestCode(string requestCode,
            ClassRegistrationInvitationIncludes includes = ClassRegistrationInvitationIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrationInvitations
                    .AddIncludes(includes).Active().SingleOrDefault(x => x.RequestCode == requestCode);
                return result;
            }
        }

        
        public ClassRegistrationInvitation GetClassRegistrationInvitationByRegistrationId(long registrationId,
            ClassRegistrationInvitationIncludes includes = ClassRegistrationInvitationIncludes.None)
        {
            {
                using (var context = new MainEntities())
                {
                    var result = context.ClassRegistrationInvitations.AddIncludes(includes).Active().SingleOrDefault(x => x.ClassRegistrationId == registrationId);
                    return result;
                }
            }
        }

        
        public long InsertClassRegistrationInvitation(ClassRegistrationInvitation classRegistrationInvitation)
        {
            using (var context = new MainEntities())
            {
                context.ClassRegistrationInvitations.Add(classRegistrationInvitation);
                context.SaveChanges();
                return classRegistrationInvitation.ClassRegistrationInvitationId;
            }
        }

        
        public void SetClassRegistrationIdToClassRegistrationInvitation(long invitationId, long? registrationId,
            RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassRegistrationInvitations.Active()
                    .FirstOrDefault(x => x.ClassRegistrationInvitationId == invitationId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Class registration invitation with id {invitationId}.");

                toUpdate.ClassRegistrationId = registrationId;
                context.SaveChanges();                
            }
        }

        
        public long? DeleteClassRegistrationInvitation(long classRegistrationInvitationId, 
            RegistrationOperationOption options = RegistrationOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.ClassRegistrationInvitations.AddIncludes(ClassRegistrationInvitationIncludes.ClassRegistration).Active()
                    .FirstOrDefault(x => x.ClassRegistrationInvitationId == classRegistrationInvitationId);
                if (toDelete == null) return null;
                
                if (options.HasFlag(RegistrationOperationOption.CheckOperation) &&
                    RegistrationInvitationConvertor.GetRegistrationInvitationState(toDelete) != ClassInvitationState.New)
                    throw new InvalidOperationException($"Class registration invitation with id {classRegistrationInvitationId} cannot be deleted in the current state.");

                context.ClassRegistrationInvitations.Remove(toDelete);
                context.SaveChanges();
                return toDelete.ClassId;
            }
        }

        #endregion


        #region class registration file
        
        public List<ClassRegistrationFile> GetClassRegistrationFilesByRegistrationId(long registraitonId)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrationFiles.Active().Where(x => x.ClassRegistrationId == registraitonId).ToList();
                return result;
            }
        }

        
        public List<ClassRegistrationFile> GetClassRegistrationFilesByIds(IEnumerable<long> classRegistrationFileIds)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrationFiles.Active().Where(x => classRegistrationFileIds.Contains(x.ClassRegistrationFileId)).ToList();
                return result;
            }
        }

        
        public ClassRegistrationFile GetClassRegistrationFileByCode(long registrationId, string code)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrationFiles.Active()
                    .FirstOrDefault(x => x.ClassRegistrationId == registrationId && x.Code == code);
                return result;
            }
        }
            
        
        public long InsertClassRegistrationFile(ClassRegistrationFile classRegistrationFile)
        {
            using (var context = new MainEntities())
            {
                context.ClassRegistrationFiles.Add(classRegistrationFile);
                context.SaveChanges();
                return classRegistrationFile.ClassRegistrationFileId;
            }
        }
        
        public void UpdateClassRegistrationFile(ClassRegistrationFile classRegistrationFile)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassRegistrationFiles.Active().FirstOrDefault(x =>
                    x.ClassRegistrationFileId == classRegistrationFile.ClassRegistrationFileId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Class registration file with id {classRegistrationFile.ClassRegistrationId}.");

                toUpdate.DisplayedName = classRegistrationFile.DisplayedName;
                toUpdate.Code = toUpdate.Code;
                toUpdate.Assigned = classRegistrationFile.Assigned;
                toUpdate.FileId = classRegistrationFile.FileId;
                toUpdate.ClassRegistrationId = classRegistrationFile.ClassRegistrationId;

                context.SaveChanges();
            }
        }

        #endregion


        #region reporting
        
        public List<ClassRegistration> GetApprovedDistanceRegistrations(DistanceCrfReportParameters filter,
            ClassRegistrationIncludes include = ClassRegistrationIncludes.None)
        {
            var endDateForFilter = filter.ToDate.AddDays(1);
            using (var context = new MainEntities())
            {
                var query = context.ClassRegistrations.AddIncludes(include)
                    .Active().FilterStateSet(ClassRegistrationStateSet.Approved);                               // active, approved
                
                if (filter.ProfileIds != null)
                    query = query.Where(x => filter.ProfileIds.Contains(x.Class.ProfileId));                    

                var result = query.Where(x => filter.FromDate <= x.Approved && x.Approved < endDateForFilter)                                       // in the time scope
                    .Where(x => !x.Class.Deleted && x.Class.ClassCategoryId == ClassCategoryEnum.DistanceClass && !x.IsCanceled)                    // active, distance and not canceled classes
                    .Where(x => x.Class.CoordinatorId == filter.DistanceCoordinatorId)                                                              // with specified distance coordinator
                    .OrderBy(x => x.Approved).ToList();                                                                                             // sorts registrations
                return result;
            }
        }

        #endregion

    }

}
