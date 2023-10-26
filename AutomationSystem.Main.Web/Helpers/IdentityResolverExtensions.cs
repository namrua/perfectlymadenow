using System.Collections.Generic;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Extensions for identity resolver
    /// </summary>
    public static class IdentityResolverExtensions
    {

        private static readonly Dictionary<string, HashSet<Entitle>> menuEntitles;
        private static readonly Dictionary<string, HashSet<Entitle>> tabEntitles;

        // constructor
        static IdentityResolverExtensions()
        {
            menuEntitles = GetManuEntitlesDefinition();
            tabEntitles = GetTabEntitlesDefinition();
        }


        // determines whether the MenuItem is enabled
        public static bool IsMenuItemGranted(this IIdentityResolver identityResolver, string menuItemId)
        {
            if (!menuEntitles.TryGetValue(menuItemId, out var entitles))
                return false;
            var result = identityResolver.IsAnyEntitleGranted(entitles);
            return result;
        }


        // determines whether the MenuItem is enabled
        public static bool IsTabItemGranted(this IIdentityResolver identityResolver, string tabItemId)
        {
            if (!tabEntitles.TryGetValue(tabItemId, out var entitles))
                return false;
            var result = identityResolver.IsAnyEntitleGranted(entitles);
            return result;
        }


        #region definitions

        // gets menu entitles definition
        private static Dictionary<string, HashSet<Entitle>> GetManuEntitlesDefinition()
        {
            var result = new Dictionary<string, HashSet<Entitle>>
            {
                { MenuItemId.CoordinatorClasses, new HashSet<Entitle> { Entitle.MainClasses, Entitle.MainDistanceClasses } },
                { MenuItemId.CoordinatorPrograms, new HashSet<Entitle> { Entitle.WebExPrograms } },
                { MenuItemId.CoordinatorFormerClasses, new HashSet<Entitle> { Entitle.MainFormerClasses, Entitle.MainFormerClassesReadOnly } },
                { MenuItemId.CoordinatorFormerStudents, new HashSet<Entitle> { Entitle.MainFormerClasses, Entitle.MainFormerClassesReadOnly } },
                { MenuItemId.CoordinatorReports, new HashSet<Entitle> { Entitle.MainDistanceClasses } },
                { MenuItemId.CoordinatorContacts, new HashSet<Entitle> { Entitle.MainContacts } },
                { MenuItemId.DistanceCoordinatorDistanceProfiles, new HashSet<Entitle> { Entitle.MainDistanceProfile } },
                { MenuItemId.DistanceCoordinatorDistanceTemplates, new HashSet<Entitle> { Entitle.MainDistanceClassTemplate} },
                { MenuItemId.CommonProfiles, new HashSet<Entitle> { Entitle.MainProfiles } },
                { MenuItemId.CommonPersons, new HashSet<Entitle> { Entitle.MainPersons } },
                { MenuItemId.CommonPriceLists, new HashSet<Entitle> { Entitle.MainPriceLists } },
                { MenuItemId.CommonPayPalKeys, new HashSet<Entitle> { Entitle.CorePayPalKeys } },
                { MenuItemId.CommonConferenceAccounts, new HashSet<Entitle> { Entitle.WebExAccounts } },
                { MenuItemId.CoreLocalisation, new HashSet<Entitle> { Entitle.CoreLocalisation } },
                { MenuItemId.CoreEmailTemplates, new HashSet<Entitle> { Entitle.CoreEmailTemplates } },
                { MenuItemId.CoreUserAccounts, new HashSet<Entitle> { Entitle.CoreUserAccounts, Entitle.CoreUserAccountsRestricted } },
                { MenuItemId.CoreIncidents, new HashSet<Entitle> { Entitle.CoreIncidents } },
                { MenuItemId.CorePreferences, new HashSet<Entitle> { Entitle.CorePreferences, Entitle.MainPreferences } },
                { MenuItemId.CoreJobs, new HashSet<Entitle> { Entitle.CoreJobs } },
            };
            return result;
        }

        // gets menu entitles definition
        private static Dictionary<string, HashSet<Entitle>> GetTabEntitlesDefinition()
        {
            var classesOnly = new HashSet<Entitle> {Entitle.MainClasses};
            var classesDistanceClasses = new HashSet<Entitle> {Entitle.MainClasses, Entitle.MainDistanceClasses};
            var result = new Dictionary<string, HashSet<Entitle>>
            {
                // preference tabs
                {TabItemId.PreferencesLocalisation, new HashSet<Entitle> {Entitle.CorePreferences}},
                {TabItemId.PreferencesEmails, new HashSet<Entitle> {Entitle.CorePreferences}},
                {TabItemId.PreferencesIzi, new HashSet<Entitle> {Entitle.MainPreferences}},

                // former classes tabs
                {TabItemId.FormerClassDetail, new HashSet<Entitle> {Entitle.MainFormerClasses, Entitle.MainFormerClassesReadOnly}},
                {TabItemId.FormerClassStudents, new HashSet<Entitle> {Entitle.MainFormerClasses, Entitle.MainFormerClassesReadOnly}},
                {TabItemId.FormerClassBatchUpload, new HashSet<Entitle> {Entitle.MainFormerClasses, Entitle.MainFormerClasses}},

                // classes tabs
                {TabItemId.ClassDetail, classesDistanceClasses},
                {TabItemId.ClassStyle, classesDistanceClasses},
                {TabItemId.ClassRegistrations, classesDistanceClasses},
                {TabItemId.ClassStudents, classesDistanceClasses},
                {TabItemId.ClassInvitations, classesDistanceClasses},
                {TabItemId.ClassActions, classesDistanceClasses},
                {TabItemId.ClassFinance, classesOnly},
                {TabItemId.ClassReports, classesOnly},
                {TabItemId.ClassCertificates, classesDistanceClasses},
                {TabItemId.ClassMaterials, new HashSet<Entitle> {Entitle.MainFormerClasses, Entitle.MainMaterials}},

                // distance class template tabs
                {TabItemId.DistanceTemplateDetail, new HashSet<Entitle> { Entitle.MainDistanceClassTemplate }},
                {TabItemId.DistanceTemplateCompletion, new HashSet<Entitle> { Entitle.MainDistanceClassTemplate }},

                // registration tabs
                {TabItemId.RegistrationDetail, classesDistanceClasses},
                {TabItemId.RegistrationPersonalData, classesDistanceClasses},
                {TabItemId.RegistrationPayment, classesDistanceClasses},
                {TabItemId.RegistrationManualReview, classesOnly},
                {TabItemId.RegistrationCommunication, classesDistanceClasses},
                {TabItemId.RegistrationIntegration, classesOnly},
                {TabItemId.RegistrationMaterials, new HashSet<Entitle> {Entitle.MainFormerClasses, Entitle.MainMaterials}},
                {TabItemId.RegistrationDocuments, classesDistanceClasses},

                // reports tabs
                {TabItemId.ReportsWwa, new HashSet<Entitle> {Entitle.MainDistanceClasses}},

                // profile tabs
                {TabItemId.ProfileDetail, new HashSet<Entitle> {Entitle.MainProfiles}},
                {TabItemId.ProfileClassPreference, new HashSet<Entitle> {Entitle.MainProfiles}},
                {TabItemId.ProfileUsers, new HashSet<Entitle> {Entitle.MainProfiles}},
                {TabItemId.ProfileEmailTemplates, new HashSet<Entitle> {Entitle.MainProfiles}}
            };
            return result;
        }

        #endregion

    }

}