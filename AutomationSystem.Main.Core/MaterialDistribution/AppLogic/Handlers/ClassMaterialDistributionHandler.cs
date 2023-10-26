using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Handlers
{
    /// <summary>
    /// Handles automatic distribution materials for registrations
    /// </summary>
    public class ClassMaterialDistributionHandler : IClassMaterialDistributionHandler
    {

        private readonly IClassMaterialAdministration materialAdministration;
        private readonly IClassMaterialBusinessRules classMaterialBusinessRules;


        // constructors
        public ClassMaterialDistributionHandler(IClassMaterialAdministration materialAdministration, IClassMaterialBusinessRules classMaterialBusinessRules)
        {
            this.materialAdministration = materialAdministration;
            this.classMaterialBusinessRules = classMaterialBusinessRules;
        }


        // handles change of registration personal data
        public void HandleRegistrationChange(ClassRegistration oldRegistration, ClassRegistration newRegistration)
        {
            if (!AreMaterialsSupportedForRegistration(newRegistration))
            {
                return;
            }

            if (GetRegistrationRecipientEmail(oldRegistration) == GetRegistrationRecipientEmail(newRegistration))
            {
                return;
            }

            materialAdministration.DistributeMaterialsToRecipient(
                newRegistration.ClassId,
                new RecipientId(EntityTypeEnum.MainClassRegistration, newRegistration.ClassRegistrationId),
                false);
        }

        // handles registration aprovement
        public void HandleRegistrationAprovement(ClassRegistration registration, bool isApprovedOnPublicPages)
        {
            if (!AreMaterialsSupportedForRegistration(registration))
            {
                return;
            }

            materialAdministration.DistributeMaterialsToRecipient(
                registration.ClassId,
                new RecipientId(EntityTypeEnum.MainClassRegistration, registration.ClassRegistrationId),
                isApprovedOnPublicPages);
        }


        #region private methods

        // gets email recipient (in lowercase)
        // todo: this should be same as MainEmailService - this corrupts encapsulation of recipient email computation and should be unified
        private string GetRegistrationRecipientEmail(ClassRegistration registration)
        {
            var result = registration.RegistrantEmail ?? registration.StudentEmail;
            result = result.Trim().ToLower();
            return result;
        }

        private bool AreMaterialsSupportedForRegistration(ClassRegistration registration)
        {
            if (!classMaterialBusinessRules.AreMaterialsSupported(registration.RegistrationTypeId))
            {
                return false;
            }

            return true;
        }

        #endregion
    }

}