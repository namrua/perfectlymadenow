using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Core.Classes.System.Model
{
    public class ClassFeaturesConfiguration
    {
        public ClassFormConfiguration ClassFormConfiguration = new ClassFormConfiguration();
        public bool AreAutomationNotificationsAllowed { get; set; }
        public bool AreStyleAndBehaviorAllowed { get; set; }
        public bool ShowClassBehaviorSettings { get; set; }
        public bool AreInvitationsAllowed { get; set; }
        public bool AreCertificatesAllowed { get; set; }
        public bool AreCertificatesAllowedForClassPersons { get; set; }
        public bool AreReportsAllowed { get; set; }
        public bool AreFinancialFormsAllowed { get; set; }
        public bool IsSupervisedByMasterCoordinator { get; set; }
        public bool IsPropagationToFormerClassesAllowed { get; set; }
        public bool IsCurrencyAllowed { get; set; }
        public bool AreMaterialsAllowed { get; set; }
    }
}
