using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Web.Models;
using AutomationSystem.Shared.Contract.Localisation.System;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Proides layout helping methods
    /// todo: refactor integration of identities
    /// </summary>
    public class LayoutHelper : ILayoutHelper
    {

        private readonly ILocalisationService localisationService;
        private readonly IIdentityResolver identityResolver;

        // constructor
        public LayoutHelper(ILocalisationService localisationService, IIdentityResolver identityResolver)
        {
            this.localisationService = localisationService;
            this.identityResolver = identityResolver;
        }


        // gets language selector model
        public LanguageSelectorModel GetLanguageSelectorModel()
        {
            var result = new LanguageSelectorModel();
            var selectedCode = localisationService.CurrentLanguageCode;
            var supportedLanguages = localisationService.GetSupportedLanguages();
            result.SupporteLanguages = supportedLanguages.Select(x => PickerItem.Item(x.Name, x.Description)).ToList();
            result.CurrentLanguage = supportedLanguages.FirstOrDefault(x => x.Name == selectedCode)?.Description ?? "---";          
            return result;
        }

        
        // gets main menu items
        public List<HierarchicalMenuItem> GetMainMenuItems(string activePageId)
        {
            var menuItems = new List<HierarchicalMenuItem>();
     
            // coordinator
            var coordinator = new HierarchicalMenuItem("coordinator", "Coordinator");
            coordinator.AddChild(new HierarchicalMenuItem(MenuItemId.CoordinatorClasses, "Classes", "Index", "Class", activeId: activePageId));
            coordinator.AddChild(new HierarchicalMenuItem(MenuItemId.CoordinatorPrograms, "Programs", "Index", "Program", activeId: activePageId));
            coordinator.AddChild(new HierarchicalMenuItem(MenuItemId.CoordinatorFormerClasses, "Former classes", "Index", "Former", activeId: activePageId));
            coordinator.AddChild(new HierarchicalMenuItem(MenuItemId.CoordinatorFormerStudents, "Former students", "Student", "Former", activeId: activePageId));
            coordinator.AddChild(new HierarchicalMenuItem(MenuItemId.CoordinatorContacts, "Contacts", "Index", "Contact", activeId: activePageId));
            menuItems.Add(coordinator);

            // distance coordinators
            var distanceCoordinator = new HierarchicalMenuItem("distance-coordinator", "Distance coordinator");
            distanceCoordinator.AddChild(new HierarchicalMenuItem(MenuItemId.DistanceCoordinatorDistanceTemplates, "Distance templates", "Index", "DistanceClassTemplate", activeId: activePageId));
            distanceCoordinator.AddChild(new HierarchicalMenuItem(MenuItemId.DistanceCoordinatorDistanceProfiles, "Distance profiles", "Index", "DistanceProfile", activeId: activePageId));

            var distanceCoordinatorReports = new HierarchicalMenuItem(MenuItemId.CoordinatorReports, "Reports", "Wwa", "Report", activeId: activePageId);
            distanceCoordinator.AddChild(AdjustLinkByFirstGrantedTab(distanceCoordinatorReports, TabItemId.Reports));

            menuItems.Add(distanceCoordinator);
                   
            // common
            var common = new HierarchicalMenuItem("common", "Common");
            common.AddChild(new HierarchicalMenuItem(MenuItemId.CommonProfiles, "Profiles", "Index", "Profile", activeId: activePageId));
            common.AddChild(new HierarchicalMenuItem(MenuItemId.CommonPersons, "Persons", "Index", "Person", activeId: activePageId));
            common.AddChild(new HierarchicalMenuItem(MenuItemId.CommonPriceLists, "Price lists", "Index", "PriceList", activeId: activePageId));
            common.AddChild(new HierarchicalMenuItem(MenuItemId.CommonPayPalKeys, "PayPal accounts", "Index", "Payment", activeId: activePageId));
            common.AddChild(new HierarchicalMenuItem(MenuItemId.CommonConferenceAccounts, "WebEx accounts", "Index", "ConferenceAccount", activeId: activePageId));
            menuItems.Add(common);

            // core
            var core = new HierarchicalMenuItem("core", "Core");
            core.AddChild(new HierarchicalMenuItem(MenuItemId.CoreLocalisation, "Localisation", "Index", "Localisation", activeId: activePageId));
            core.AddChild(new HierarchicalMenuItem(MenuItemId.CoreEmailTemplates, "Email templates", "Index", "Email", activeId: activePageId));
            core.AddChild(new HierarchicalMenuItem(MenuItemId.CoreUserAccounts, "User accounts", "Index", "Identity", activeId: activePageId));
            core.AddChild(new HierarchicalMenuItem(MenuItemId.CoreIncidents, "Incidents", "Index", "Incident", activeId: activePageId));

            var corePreferences = new HierarchicalMenuItem(MenuItemId.CorePreferences, "Preferences", "Localisation", "Preference", activeId: activePageId);
            core.AddChild(AdjustLinkByFirstGrantedTab(corePreferences, TabItemId.Preferences));

            menuItems.Add(core);

            // integrates with IDM and returns result
            var result = IntegrateIdentityWithMenuItems(menuItems);
            return result;
        }


        // gets tab menu items
        public List<HierarchicalMenuItem> GetTabMenuItems(string tabId, string activeTabId, object parameters = null)
        {
            var tabItems = new List<HierarchicalMenuItem>();
            switch (tabId)
            {
                // gets preferences tab menu
                case TabItemId.Preferences:
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.PreferencesLocalisation, "Localisation", "Localisation", "Preference", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.PreferencesEmails, "Emails", "Email", "Preference", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.PreferencesIzi, "IZI LLC", "Izi", "Preference", parameters, activeId: activeTabId));
                    break;

                // gets former students tab menu
                case TabItemId.FormerClass:
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.FormerClassDetail, "Detail", "ClassDetail", "Former", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.FormerClassStudents, "Students", "ClassStudents", "Former", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.FormerClassBatchUpload, "Batch upload", "StudentUpload", "Former", parameters, activeId: activeTabId));
                    break;

                // gets classes tab menu
                case TabItemId.Class:
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassDetail, "Detail", "Detail", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassStyle, "Style & Behavior", "Style", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassRegistrations, "Registrations", "Registrations", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassStudents, "Students", "Students", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassInvitations, "Invitations", "Invitations", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassActions, "Actions", "Actions", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassFinance, "Finance", "Business", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassReports, "Reports", "Reports", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassCertificates, "Certificates", "Certificates", "Class", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ClassMaterials, "Materials", "Class", "Materials", parameters, activeId: activeTabId));
                    break;

                //gets distance class templates tab menu
                case TabItemId.DistanceTemplate:
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.DistanceTemplateDetail, "Detail", "Detail", "DistanceClassTemplate", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.DistanceTemplateCompletion, "Completion", "Completion", "DistanceClassTemplate", parameters, activeId: activeTabId));
                    break;

                // gets registrations tab menu
                case TabItemId.Registration:                                    
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.RegistrationDetail, "Detail", "Detail", "Registration", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.RegistrationPersonalData, "Personal data", "PersonalData", "Registration", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.RegistrationPayment, "Payment", "Payment", "Registration", parameters, activeId: activeTabId));
                    // todo: vytunit status review, podle typu
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.RegistrationManualReview, "Manual review", "ManualReview", "Registration", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.RegistrationCommunication, "Communication", "Communication", "Registration", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.RegistrationIntegration, "Integration", "Integration", "Registration", parameters, isDisabled: false, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.RegistrationMaterials, "Materials", "Registration", "Materials", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.RegistrationDocuments, "Documents", "Documents", "Registration", parameters, activeId: activeTabId));
                    break;

                // gets reports tab menu
                case TabItemId.Reports:
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ReportsWwa, "WWA", "Wwa", "Report", parameters, activeId: activeTabId));
                    break;

                // gets profile tab menu
                case TabItemId.Profile:
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ProfileDetail, "Detail", "Detail", "Profile", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ProfileClassPreference, "Class preferences", "ClassPreference", "Profile", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ProfileUsers, "Users", "Users", "Profile", parameters, activeId: activeTabId));
                    tabItems.Add(new HierarchicalMenuItem(TabItemId.ProfileEmailTemplates, "Email templates", "EmailTemplates", "Profile", parameters, activeId: activeTabId));
                    break;


                // default
                default:
                    throw new ArgumentException($"Unknown tab id {tabId}.");
            }

            // integrates with IDM and returns result
            var result = IntegrateIdentityWithTabItems(tabItems);
            return result;
        }



        #region identity integration

        // integrates identity with menu items
        private List<HierarchicalMenuItem> IntegrateIdentityWithMenuItems(List<HierarchicalMenuItem> menuItems)
        {
            var result = new List<HierarchicalMenuItem>();
            foreach (var mainItem in menuItems)
            {
                // resolves children privileges and ignore whether there is no children to add
                var childrenItems = mainItem.Children.Where(x => identityResolver.IsMenuItemGranted(x.Id)).ToList();
                if (!childrenItems.Any())
                    continue;

                // otherwise create new mainMenuItem and adds children with granted privileges
                var newMainItem = new HierarchicalMenuItem(mainItem.Id, mainItem.Text);
                childrenItems.ForEach(x => newMainItem.AddChild(x));
                result.Add(newMainItem);
            }
            return result;
        }


        // integrates identity with tab items
        private List<HierarchicalMenuItem> IntegrateIdentityWithTabItems(List<HierarchicalMenuItem> tabItems)
        {
            var result = tabItems.Where(x => identityResolver.IsTabItemGranted(x.Id)).ToList();
            return result;
        }


        // adjust link by first granted tab assigned to menu item
        private HierarchicalMenuItem AdjustLinkByFirstGrantedTab(HierarchicalMenuItem menuItem, string tabItemId, object parameters = null)
        {
            var firstTabForMenuItem = GetTabMenuItems(tabItemId, tabItemId, parameters).FirstOrDefault();
            if (firstTabForMenuItem == null)
                return menuItem;

            // adjust menuItem URL
            menuItem.Controller = firstTabForMenuItem.Controller;
            menuItem.Activity = firstTabForMenuItem.Activity;
            menuItem.Parameters = firstTabForMenuItem.Parameters;
            return menuItem;
        }

        #endregion


    }

}