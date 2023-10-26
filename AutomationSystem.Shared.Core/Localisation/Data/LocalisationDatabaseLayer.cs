using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Core.Localisation.Data.Extensions;
using AutomationSystem.Shared.Core.Localisation.Data.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Shared.Core.Localisation.Data
{
    /// <summary>
    /// The facade interface for localisation database layer
    /// </summary>
    public class LocalisationDatabaseLayer : ILocalisationDatabaseLayer
    {
        // gets application localisation by module, label and language code
        public AppLocalisation GetAppLocalisationByKeyCached(string module, string label, string languageCode)
        {
            using (var context = new CoreEntities())
            {
                DbQuery<AppLocalisation> query = context.AppLocalisations;
                var result = query.Active().FirstOrDefault(x => x.Module == module && x.Label == label && x.Language.Name == languageCode);
                return result;
            }
        }

        // gets application localisation by id
        public AppLocalisation GetAppLocalisationById(long appLocalisationId, bool includeLanguage = false)
        {
            using (var context = new CoreEntities())
            {
                DbQuery<AppLocalisation> query = context.AppLocalisations;
                if (includeLanguage)
                    query = query.Include("Language");
                var result = query.Active().FirstOrDefault(x => x.AppLocalisationId == appLocalisationId);
                return result;
            }
        }

        // gets application localisation by module, label and language code
        public AppLocalisation GetAppLocalisationByKey(string module, string label, LanguageEnum languageId, bool includeLanguage = false)
        {
            using (var context = new CoreEntities())
            {
                DbQuery<AppLocalisation> query = context.AppLocalisations;
                if (includeLanguage)
                    query = query.Include("Language");
                var result = query.Active().FirstOrDefault(x => x.Module == module && x.Label == label && x.LanguageId == languageId);
                return result;
            }
        }


        // get application localisation values
        public List<AppLocalisation> GetAppLocalisations(AppLocalisationFilter filter, bool includeLanguage = false)
        {
            using (var context = new CoreEntities())
            {
                DbQuery<AppLocalisation> query = context.AppLocalisations;
                if (includeLanguage)
                    query = query.Include("Language");
                var result = query.Filter(filter).ToList();
                return result;
            }
        }

        // inserts or updates application localisation, returns id
        public long InsertUpdateAppLocalisation(long originId, LanguageEnum languageId, string value)
        {
            using (var context = new CoreEntities())
            {
                // gets origin and when languageId is default language saves value
                var origin = context.AppLocalisations.First(x => x.AppLocalisationId == originId);
                if (origin.LanguageId == languageId)
                {
                    origin.Value = value;
                    context.SaveChanges();
                    return origin.AppLocalisationId;
                }

                // gets entity to update
                var toUpdate = context.AppLocalisations
                    .FirstOrDefault(x => x.Module == origin.Module && x.Label == origin.Label && x.LanguageId == languageId);

                // inserts item
                if (toUpdate == null)
                {
                    var toInsert = new AppLocalisation()
                    {
                        Module = origin.Module,
                        Label = origin.Label,
                        LanguageId = languageId,
                        Value = value
                    };
                    context.AppLocalisations.Add(toInsert);
                    context.SaveChanges();
                    return toInsert.AppLocalisationId;
                }

                // updates item
                toUpdate.Value = value;
                context.SaveChanges();
                return toUpdate.AppLocalisationId;
            }
        }

        // inserts or updates application localisation, returns id
        public long InsertUpdateAppLocalisation(AppLocalisation entity)
        {
            using (var context = new CoreEntities())
            {
                var toUpdate = context.AppLocalisations.Active().FirstOrDefault(
                    x => x.LanguageId == entity.LanguageId && x.Module == entity.Module && x.Label == entity.Label);

                // inserts item
                if (toUpdate == null)
                {
                    context.AppLocalisations.Add(entity);
                    context.SaveChanges();
                    return entity.AppLocalisationId;
                }

                // updates item
                toUpdate.Value = entity.Value;
                context.SaveChanges();
                return toUpdate.AppLocalisationId;
            }
        }


        //#region enum localisation

        // get enum localisation by key [Cached]
        public EnumLocalisation GetEnumLocalisationByKeyCached(string languageCode, EnumTypeEnum enumTypeId, int itemId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EnumLocalisations.Include("Language").Active()
                    .FirstOrDefault(x => x.EnumTypeId == enumTypeId && x.ItemId == itemId && x.Language.Name == languageCode);
                return result;
            }
        }

        // gets all enum localisation by language and type [Cached]
        public List<EnumLocalisation> GetEnumLocalisationsByFilterCached(EnumLocalisationFilter filter)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EnumLocalisations.Include("Language").Filter(filter).ToList();
                return result;
            }
        }

        // get enum localisation by key
        public EnumLocalisation GetEnumLocalisationByKey(LanguageEnum languageId, EnumTypeEnum enumTypeId, int itemId,
            bool includeLanguage = false, bool includeEnumType = false)
        {
            using (var context = new CoreEntities())
            {
                DbQuery<EnumLocalisation> query = context.EnumLocalisations;
                if (includeLanguage)
                    query = query.Include("Language");
                if (includeEnumType)
                    query = query.Include("EnumType");
                var result = query.Active().FirstOrDefault(x => x.EnumTypeId == enumTypeId && x.ItemId == itemId && x.LanguageId == languageId);
                return result;
            }
        }

        // gets all enum localisation by language and type
        public List<EnumLocalisation> GetEnumLocalisationsByFilter(EnumLocalisationFilter filter, bool includeLanguage = false, bool includeEnumType = false)
        {
            using (var context = new CoreEntities())
            {
                DbQuery<EnumLocalisation> query = context.EnumLocalisations;
                if (includeLanguage)
                    query = query.Include("Language");
                if (includeEnumType)
                    query = query.Include("EnumType");
                var result = query.Filter(filter).ToList();
                return result;
            }
        }


        // inserts or updates enum localisation
        public long InsertUpdateEnumLocalisation(EnumLocalisation entity)
        {
            using (var context = new CoreEntities())
            {
                var toUpdate = context.EnumLocalisations.Active().FirstOrDefault(
                    x => x.LanguageId == entity.LanguageId && x.EnumTypeId == entity.EnumTypeId && x.ItemId == entity.ItemId);

                // inserts item
                if (toUpdate == null)
                {
                    context.EnumLocalisations.Add(entity);
                    context.SaveChanges();
                    return entity.EnumLocalisationId;
                }

                // updates item
                toUpdate.Name = entity.Name;
                toUpdate.Description = entity.Description;
                context.SaveChanges();
                return toUpdate.EnumLocalisationId;
            }
        }

        //#endregion


        // gets data localisation values
        public List<DataLocalisation> GetDataLocalisationByFilter(DataLocalisationFilter filter)
        {
            using (var context = new CoreEntities())
            {
                var result = context.DataLocalisations.Filter(filter).ToList();
                return result;
            }
        }

        // saves data localisation, returns id
        public void SaveDataLocalisations(DataLocalisationFilter filter, List<DataLocalisation> dataLocalisations)
        {
            using (var context = new CoreEntities())
            {
                var entityDataLocalisation = context.DataLocalisations.Filter(filter).ToList();
                var setUpdateResolver = new SetUpdateResolver<DataLocalisation, string>(x => x.Column, (origItem, newItem) => { origItem.Value = newItem.Value; });
                var strategy = setUpdateResolver.ResolveStrategy(entityDataLocalisation, dataLocalisations);
                context.DataLocalisations.AddRange(strategy.ToAdd);
                context.DataLocalisations.RemoveRange(strategy.ToDelete);
                context.SaveChanges();
            }
        }

        // delete data localisation
        public void DeleteDataLocalisations(EntityTypeEnum entityType, long entityId, IEnumerable<LanguageEnum> exceptLanguage = null)
        {
            using (var context = new CoreEntities())
            {
                var toDeleteQuery = context.DataLocalisations.Active().Where(x => x.EntityTypeId == entityType
                    && x.EntityId == entityId);
                if (exceptLanguage != null)
                    toDeleteQuery = toDeleteQuery.Where(x => !exceptLanguage.Contains(x.LanguageId));
                var toDelete = toDeleteQuery.ToList();
                context.DataLocalisations.RemoveRange(toDelete);
                context.SaveChanges();
            }
        }


    }

}
