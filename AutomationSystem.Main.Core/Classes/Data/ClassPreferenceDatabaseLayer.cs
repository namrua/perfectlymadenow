using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using PerfectlyMadeInc.Helpers.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.Classes.Data
{
    public class ClassPreferenceDatabaseLayer: IClassPreferenceDatabaseLayer
    {
        // updates ClassPreference of the Profile
        public void UpdateClassPreference(long profileId, ClassPreference classPreference, bool updateHeaderPictureId)
        {
            using (var context = new MainEntities())
            {
                var profile = context.Profiles.Active().FirstOrDefault(x => x.ProfileId == profileId);
                if (profile == null)
                    throw new ArgumentException($"There is no Profile with id {profileId}.");

                var toUpdate = profile.ClassPreference;
                toUpdate.HomepageUrl = classPreference.HomepageUrl;
                toUpdate.RegistrationColorSchemeId = classPreference.RegistrationColorSchemeId;
                toUpdate.VenueName = classPreference.VenueName;
                toUpdate.LocationCode = classPreference.LocationCode;
                toUpdate.SendCertificatesByEmail = classPreference.SendCertificatesByEmail;
                toUpdate.LocationInfoId = classPreference.LocationInfoId;
                toUpdate.CurrencyId = classPreference.CurrencyId;
                if (updateHeaderPictureId)
                    toUpdate.HeaderPictureId = classPreference.HeaderPictureId;

                context.SaveChanges();
            }
        }

        // updates ClassPreference's expenses
        public void UpdateClassPreferenceExpenses(long profileId, List<ClassPreferenceExpense> profileExpenses)
        {
            using (var context = new MainEntities())
            {
                // loads profile and finds out ClassPreferenceId
                var profile = context.Profiles.Active().FirstOrDefault(x => x.ProfileId == profileId);
                if (profile == null)
                    throw new ArgumentException($"There is no Profile with id {profileId}.");
                var classPreferenceId = profile.ClassPreferenceId;

                // set ids for profile expenses
                profileExpenses.ForEach(x => x.ClassPreferenceId = classPreferenceId);

                // loads origin profile expenses
                var originExpenses = context.ClassPreferenceExpenses.Active().Where(x => x.ClassPreferenceId == classPreferenceId);

                // resolves update strategy
                var updateResolver = new SetUpdateResolver<ClassPreferenceExpense, int>(x => x.Order,
                    (originItem, newItem) =>
                    {
                        originItem.Text = newItem.Text;
                        originItem.ClassExpenseTypeId = newItem.ClassExpenseTypeId;
                        originItem.Value = newItem.Value;
                    });
                var strategy = updateResolver.ResolveStrategy(originExpenses, profileExpenses);
                context.ClassPreferenceExpenses.AddRange(strategy.ToAdd);
                context.ClassPreferenceExpenses.RemoveRange(strategy.ToDelete);

                // saves data
                context.SaveChanges();
            }

        }

        // detemrmines whether person can be deleted
        public bool CanDeletePerson(long personId)
        {
            using (var context = new MainEntities())
            {
                if (context.Profiles.Active().Any(x => x.ClassPreference.LocationInfoId == personId))
                    return false;
            }
            return true;
        }
    }
}
