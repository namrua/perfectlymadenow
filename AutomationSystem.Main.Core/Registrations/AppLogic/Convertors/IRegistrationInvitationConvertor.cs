using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.Convertors
{
    public interface IRegistrationInvitationConvertor
    {
        ClassInvitationItem ConvertToClassInvitationItem(ClassRegistrationInvitation invitation);
        
        ClassRegistrationInvitation ConvertToClassRegistrationInvitation(ClassInvitationForm form, string requestCode);

    }

}
