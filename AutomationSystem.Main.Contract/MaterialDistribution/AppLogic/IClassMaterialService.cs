using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic
{
    /// <summary>
    /// Provides class materials and related services
    /// </summary>
    public interface IClassMaterialService
    {
        // returns request info or null when request code was not found
        ClassMaterialRequestInfo GetRequestInfo(string requestCode);

        // gets public language selection page model
        PublicLanguageSelectionPageModel GetPublicLanguageSelectionPageModel(string requestCode, long classId);

        // gets public download page model
        PublicDownloadPageModel GetPublicDownloadPageModel(long classMaterialId, long materialsRecipientId, LanguageEnum languageId);

        // sets language for registration
        void SetClassRegistrationMaterialLanguage(string requestCode, LanguageEnum languageId);

        // get material for download
        FileForDownload GetMaterialForDownload(string requestCode, long classMaterialFileId, WebRequestInfo requestInfo);
    }
}
