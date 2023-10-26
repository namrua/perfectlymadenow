namespace AutomationSystem.Base.Contract.Identities.Models
{

    /// <summary>
    /// Determines entitles of the system
    /// </summary>
    public enum Entitle
    {

        // Core entitles
        CoreLocalisation = 10,
        CoreEmailTemplates = 20,
        CoreEmailTemplatesIntegration = 21,
        CoreUserAccounts = 30,
        CoreUserAccountsRestricted = 31,
        CoreIncidents = 40,
        CorePreferences = 50,
        CorePayPalKeys = 60,
        CoreJobs = 70,

        // Main entitles
        MainProfiles = 2000,
        MainPersons = 2010,
        MainPersonsReadOnly = 2011,
        MainClasses = 2020,
        MainDistanceClasses = 2021,
        MainMaterials = 2022,
        MainPrivateMaterialClasses = 2023,
        MainFormerClasses = 2050,
        MainFormerClassesReadOnly = 2051,
        MainPriceLists = 2100,
        MainPreferences = 2110,
        MainDistanceProfile = 2130,
        MainDistanceClassTemplate = 2150,
        MainContacts = 2170,

        // WebEx entitles
        WebExAccounts = 3000,
        WebExPrograms = 3010,

    }

}
