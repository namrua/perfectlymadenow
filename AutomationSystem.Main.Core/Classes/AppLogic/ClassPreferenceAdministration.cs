using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Files.System;
using System;
using System.Collections.Generic;
using System.IO;
using Profile = AutomationSystem.Main.Model.Profile;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class ClassPreferenceAdministration : IClassPreferenceAdministration
    {
        // constants
        public const string HeaderPictureNamePattern = "Profile picture ({0})";


        private readonly IProfileDatabaseLayer profileDb;
        private readonly ICoreFileService coreFileService;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainMapper mainMapper;
        private readonly IClassExpenseFactory classExpenseFactory;
        private readonly IClassPreferenceFactory classPreferenceFactory;
        private readonly IClassPreferenceDatabaseLayer classPreferenceDb;


        // constructor
        public ClassPreferenceAdministration(
            IProfileDatabaseLayer profileDb,
            ICoreFileService coreFileService,
            IIdentityResolver identityResolver,
            IMainMapper mainMapper,
            IClassExpenseFactory classExpenseFactory,
            IClassPreferenceFactory classPreferenceFactory,
            IClassPreferenceDatabaseLayer classPreferenceDb)
        {
            this.profileDb = profileDb;
            this.coreFileService = coreFileService;
            this.identityResolver = identityResolver;
            this.mainMapper = mainMapper;
            this.classExpenseFactory = classExpenseFactory;
            this.classPreferenceFactory = classPreferenceFactory;
            this.classPreferenceDb = classPreferenceDb;
        }

        // gets class preference detail
        public ClassPreferenceDetail GetClassPreferenceDetail(long profileId)
        {
            var profile = GetProfileById(profileId, ProfileIncludes.ClassPreferenceClassPreferenceExpenses | ProfileIncludes.ClassPreferenceLocationInfo | ProfileIncludes.ClassPreferenceCurrency);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);
            var result = mainMapper.Map<ClassPreferenceDetail>(profile.ClassPreference);
            result.ProfileId = profileId;
            return result;
        }


        // gets class preference for edit by profile id
        public ClassPreferenceForEdit GetClassPreferenceForEditByProfileId(long profileId)
        {
            var profile = GetProfileById(profileId, ProfileIncludes.ClassPreference);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);
            var result = classPreferenceFactory.CreateClassPreferenceForEdit(profileId);
            result.Form = mainMapper.Map<ClassPreferenceForm>(profile.ClassPreference);
            result.Form.ProfileId = profileId;
            return result;
        }

        // gets class preference for edit by form
        public ClassPreferenceForEdit GetClassPreferenceForEditByForm(ClassPreferenceForm form)
        {
            identityResolver.CheckEntitle(Entitle.MainProfiles);
            var result = classPreferenceFactory.CreateClassPreferenceForEdit(form.ProfileId);
            result.Form = form;
            return result;
        }

        // saves class preferences
        public void SaveClassPreference(ClassPreferenceForm form, Stream headerPictureContent, string headerPictureName)
        {
            var isPictureUploaded = headerPictureContent != null;
            var updateHeaderPictureId = false;
            long? newHeaderPictureId = null;
            long? headerPictureIdToDelete = null;

            // loads profile and checks access level
            var profile = GetProfileById(form.ProfileId, ProfileIncludes.ClassPreference);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);

            // when origin header picture should be deleted or replaced,
            // origin header picture id is set to be deleted
            if (form.RemoveHeaderPicture || isPictureUploaded)
            {
                headerPictureIdToDelete = profile.ClassPreference.HeaderPictureId;
            }

            // handles removing of picture
            if (form.RemoveHeaderPicture)
            {
                updateHeaderPictureId = true;
            }

            // handles new loaded picture
            if (isPictureUploaded)
            {
                newHeaderPictureId = coreFileService.InsertFile(headerPictureContent,
                    string.Format(HeaderPictureNamePattern, form.ProfileId),
                    headerPictureName, FileTypeEnum.Jpg, isPublic: true);
                updateHeaderPictureId = true;
            }

            // saves class preferences
            var toUpdate = mainMapper.Map<ClassPreference>(form);
            toUpdate.HeaderPictureId = newHeaderPictureId;
            classPreferenceDb.UpdateClassPreference(form.ProfileId, toUpdate, updateHeaderPictureId);

            // deletes origin picture
            if (headerPictureIdToDelete.HasValue)
            {
                coreFileService.DeleteFile(headerPictureIdToDelete.Value);
            }
        }


        // gets expense layout for edit by profile id
        public ExpensesLayoutForEdit GetExpenseLayoutForEditByProfileId(long profileId)
        {
            var profile = GetProfileById(profileId, ProfileIncludes.ClassPreferenceClassPreferenceExpenses | ProfileIncludes.ClassPreferenceCurrency);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);
            var result = classExpenseFactory.CreateExpensesLayoutForEdit(profile.ClassPreference.Currency);
            result.Form = mainMapper.Map<ExpensesLayoutForm>(profile.ClassPreference.ClassPreferenceExpenses);
            result.Form.EntityId = profileId;
            return result;
        }

        // gets expense layout for edit by form
        public ExpensesLayoutForEdit GetExpenseLayoutForEditByForm(ExpensesLayoutForm form)
        {
            var profile = GetProfileById(form.EntityId, ProfileIncludes.ClassPreferenceCurrency);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);

            // returns ExpensesLayoutForEdit
            var result = classExpenseFactory.CreateExpensesLayoutForEdit(profile.ClassPreference.Currency);
            result.Form = form;
            return result;
        }

        // saves expense layout
        public void SaveExpenseLayout(ExpensesLayoutForm form)
        {
            // checks access level
            identityResolver.CheckEntitleForProfileId(Entitle.MainProfiles, form.EntityId);

            // saves expenses
            var expensesToUpdate = mainMapper.Map<List<ClassPreferenceExpense>>(form);
            classPreferenceDb.UpdateClassPreferenceExpenses(form.EntityId, expensesToUpdate);
        }

        #region private methods
        private Profile GetProfileById(long profileId, ProfileIncludes includes = ProfileIncludes.None)
        {
            var result = profileDb.GetProfileById(profileId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Profile with id {profileId}.");
            }

            return result;
        }
        #endregion

    }
}
