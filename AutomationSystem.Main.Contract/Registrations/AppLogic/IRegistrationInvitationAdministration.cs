using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic
{
    public interface IRegistrationInvitationAdministration
    {
        ClassInvitationPageModel GetClassInvitationPageModel(long classId);
        
        ClassInvitationForEdit GetClassInvitationForEdit(RegistrationTypeEnum id, long classId);
        
        ClassInvitationForEdit GetFormInvitationForEdit(ClassInvitationForm form);
        
        long SaveInvitation(ClassInvitationForm form);
        
        long? DeleteInvitation(long invitationId);
    }
}
