using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Handlers
{
    /// <summary>
    /// Handles automatic distribution materials for registrations
    /// </summary>
    public interface IClassMaterialDistributionHandler
    {
        // handles change of registration personal data
        void HandleRegistrationChange(ClassRegistration oldRegistration, ClassRegistration newRegistration);

        // handles registration aprovement
        void HandleRegistrationAprovement(ClassRegistration registration, bool isApprovedOnPublicPages);
    }
}
