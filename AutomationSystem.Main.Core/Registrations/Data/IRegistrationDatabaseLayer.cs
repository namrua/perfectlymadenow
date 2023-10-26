using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.Data
{
    /// <summary>
    /// Provides registration database layer
    /// </summary>
    public interface IRegistrationDatabaseLayer
    {
        
        List<ClassRegistration> GetRegistrationsByStateSet(long classId, ClassRegistrationStateSet stateSet, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None);
        
        List<ClassRegistration> GetRegistrationsByFilter(RegistrationFilter filter, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None);
        
        List<ClassRegistration> GetRegistrationsByIds(IEnumerable<long> registrationIds, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None);
        
        ClassRegistration GetClassRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None);

        
        long InsertClassRegistration(ClassRegistration classRegistration);
        
        void UpdateClassRegistration(ClassRegistration classRegistration, RegistrationOperationOption options = RegistrationOperationOption.None);
        
        void UpdateClassRegistrationPayment(long registrationId, ClassRegistrationPayment classRegistrationPayment, 
            RegistrationOperationOption option = RegistrationOperationOption.None);

        
        ClassRegistration ApproveClassRegistration(long registrationId);
        
        void SetRegistrationToReviewed(long registrationId);
        
        void CancelClassRegistration(long registrationId, RegistrationOperationOption options = RegistrationOperationOption.None);
        
        long? DeleteClassRegistration(long registrationId, RegistrationOperationOption options = RegistrationOperationOption.None);


        #region registration operations
        
        void SetRegistrationAgreementAcceptationState(long registrationId, bool areAgreementsAccepted, RegistrationOperationOption options = RegistrationOperationOption.None);
        
        void UpdateClassRegistrationPaymentAndApprove(long registrationId, ClassRegistrationPayment classRegistrationPayment, RegistrationOperationOption options = RegistrationOperationOption.None);
        
        void SetFormerStudentToRegistration(long registrationId, long? formerStudentId, bool updateIsReviewed);
        
        void SetTemporaryRegistrationForApprovement(long registrationId, ApprovementTypeEnum? approvementTypeId, RegistrationOperationOption options = RegistrationOperationOption.None);
        
        long InsertUpdateRegistrationLastClass(long registrationClassRegistrationId, ClassRegistrationLastClass dbLastClass, RegistrationOperationOption options = RegistrationOperationOption.None);

        #endregion


        #region invitations
        
        List<ClassRegistrationInvitation> GetClassRegistrationInvitations(long classId,
            ClassRegistrationInvitationIncludes includes = ClassRegistrationInvitationIncludes.None);
        
        ClassRegistrationInvitation GetClassRegistrationInvitationById(long classRegistrationInvitationId, 
            ClassRegistrationInvitationIncludes includes = ClassRegistrationInvitationIncludes.None);
        
        ClassRegistrationInvitation GetClassRegistrationInvitationByRequestCode(string requestCode, 
            ClassRegistrationInvitationIncludes includes = ClassRegistrationInvitationIncludes.None);
        
        ClassRegistrationInvitation GetClassRegistrationInvitationByRegistrationId(long registrationId,
            ClassRegistrationInvitationIncludes includes = ClassRegistrationInvitationIncludes.None);
        
        long InsertClassRegistrationInvitation(ClassRegistrationInvitation classRegistrationInvitation);
        
        void SetClassRegistrationIdToClassRegistrationInvitation(long invitationId, long? registrationId, RegistrationOperationOption options = RegistrationOperationOption.None);
        
        long? DeleteClassRegistrationInvitation(long classRegistrationInvitationId, RegistrationOperationOption options = RegistrationOperationOption.None);

        #endregion


        #region registration files
        
        List<ClassRegistrationFile> GetClassRegistrationFilesByRegistrationId(long registraitonId);
        
        List<ClassRegistrationFile> GetClassRegistrationFilesByIds(IEnumerable<long> classRegistrationFileIds);
        
        ClassRegistrationFile GetClassRegistrationFileByCode(long registrationId, string code);

       
        long InsertClassRegistrationFile(ClassRegistrationFile classRegistrationFile);
        
        void UpdateClassRegistrationFile(ClassRegistrationFile classRegistrationFile);

        #endregion


        #region reporting
        
        List<ClassRegistration> GetApprovedDistanceRegistrations(DistanceCrfReportParameters filter, ClassRegistrationIncludes include = ClassRegistrationIncludes.None);

        #endregion
    }

}
