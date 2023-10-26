using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Enums.System;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Shared.Contract.Localisation.System;
using System;

namespace AutomationSystem.Main.Core.Home.AppLogic
{

    /// <summary>
    /// Provides data for previewing purposes
    /// </summary>
    public class PreviewService : IPreviewService
    {

        private readonly ILocalisationService localisationService;
        private readonly IClassDatabaseLayer classDb;
        private readonly IIdentityResolver identityResolver;


        // constructor
        public PreviewService(ILocalisationService localisationService, IClassDatabaseLayer classDb, IIdentityResolver identityResolver)
        {
            this.localisationService = localisationService;
            this.classDb = classDb;
            this.identityResolver = identityResolver;
        }


        // gets preview style page model by back URL and classId
        public PreviewStylePageModel GetPreviewStylePageModelForClass(string backUrl, long classId)
        {
            // checks privileges
            var cls = classDb.GetClassById(classId);
            if (cls == null)
                throw new ArgumentException($"There is no Class with id {classId}.");

            // checks privileges
            identityResolver.CheckEntitleForClass(cls);

            // gets model
            var result = GetPreviewStylePageModel(backUrl);
            return result;
        }

        // gets preview style page model by back URL for profile
        public PreviewStylePageModel GetPreviewStylePageModelForProfile(string backUrl, long profileId)
        {
            // checks privileges
            identityResolver.CheckEntitleForProfileId(Entitle.MainProfiles, profileId);

            // gets model
            var result = GetPreviewStylePageModel(backUrl);
            return result;
        }


        #region private methods

        // gets preview style page model by back URL
        public PreviewStylePageModel GetPreviewStylePageModel(string backUrl)
        {
            var result = new PreviewStylePageModel
            {
                BackUrl = backUrl,
                Countries = localisationService.GetLocalisedEnumItemsByFilter(EnumTypeEnum.Country).SortDefaultCountryFirst()
            };
            return result;
        }

        #endregion

    }

}
