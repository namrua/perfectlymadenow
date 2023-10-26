using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data.Extensions;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data.Extensions;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using ClosedXML.Excel;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Main.Core.Classes.Data
{
    /// <summary>
    /// Provides database layer for classes
    /// </summary>
    public class ClassDatabaseLayer : IClassDatabaseLayer
    {
        private readonly IClassOperationChecker classOperationChecker;

        public ClassDatabaseLayer(IClassOperationChecker classOperationChecker)
        {
            this.classOperationChecker = classOperationChecker;
        }
        #region class

        public List<ClassCategory> GetClassCategoriesByIds(IEnumerable<ClassCategoryEnum> classCategoryIds)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassCategories.Where(x => classCategoryIds.Contains(x.ClassCategoryId)).ToList();
                return result;
            }
        }

        // gets classes by filter
        public List<Class> GetClassesByFilter(ClassFilter filter = null, ClassIncludes includes = ClassIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Classes.AddIncludes(includes).Filter(filter).ToList();
                result = result.Select(x => ClassRemoveInactive.RemoveInactiveForClass(x, includes)).ToList();
                return result;
            }
        }

        // gets classes by ids
        public List<Class> GetClassesByIds(IEnumerable<long> classIds, ClassIncludes includes = ClassIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Classes.AddIncludes(includes).Active().Where(x => classIds.Contains(x.ClassId)).ToList();
                result = result.Select(x => ClassRemoveInactive.RemoveInactiveForClass(x, includes)).ToList();
                return result;
            }
        }

        // get class by id
        public Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Classes.AddIncludes(includes).Active().FirstOrDefault(x => x.ClassId == classId);
                result = ClassRemoveInactive.RemoveInactiveForClass(result, includes);
                return result;
            }
        }

        // gets count of approved registrations for class Id
        public int GetApprovedRegistrationCountByClassId(long classId)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassRegistrations.Active()
                    .Where(x => x.ClassId == classId)
                    .FilterStateSet(ClassRegistrationStateSet.Approved).Count();
                return result;
            }
        }

        public bool PayPalKeyOnAnyClass(long payPalKeyId)
        {
            using (var context = new MainEntities())
            {
                var result = context.Classes.Active().Any(x => x.PayPalKeyId == payPalKeyId);
                return result;
            }
        }

        public bool PersonOnAnyClass(long personId)
        {
            using (var context = new MainEntities())
            {
                var anyClass = context.Classes.Active().Any(x => x.GuestInstructorId == personId 
                    || x.CoordinatorId == personId 
                    || x.ClassReportSetting.LocationInfoId == personId);
                if (anyClass)
                {
                    return true;
                }

                var anyClassPerson = context.ClassPersons.ActiveInActiveClass().Any(x => x.PersonId == personId);
                if (anyClassPerson)
                {
                    return true;
                }

                var anyClassPreference = context.Profiles.Active().Any(x => x.ClassPreference.LocationInfoId == personId);
                if (anyClassPreference)
                {
                    return true;
                }

                return false;
            }
        }

        public bool PriceListOnAnyClass(long priceListId)
        {
            using (var context = new MainEntities())
            {
                var result = context.Classes.Active().Any(x => x.PriceListId == priceListId);
                return result;
            }
        }

        // checks if class is link to profile
        public bool AnyClassOnProfile(long profileId)
        {
            using (var context = new MainEntities())
            {
                var result = context.Classes.Active().Any(x => x.ProfileId == profileId);
                return result;
            }
        }
            
        // inserts class
        public long InsertClass(Class cls)
        {
            using (var context = new MainEntities())
            {
                context.Classes.Add(cls);
                context.SaveChanges();
                return cls.ClassId;
            }
        }


        // updates class
        public void UpdateClass(Class cls, bool isFullyEditable)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.Classes.Active().FirstOrDefault(x => x.ClassId == cls.ClassId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no Class with id {cls.ClassId}.");
                }

                // updates fields of entity               
                toUpdate.Location = cls.Location;
                toUpdate.TimeZoneId = cls.TimeZoneId;
                toUpdate.RegistrationEndUtc = cls.RegistrationEndUtc;
                toUpdate.RegistrationEnd = cls.RegistrationEnd;
                toUpdate.EventStartUtc = cls.EventStartUtc;
                toUpdate.EventEndUtc = cls.EventEndUtc;
                toUpdate.EventStart = cls.EventStart;
                toUpdate.EventEnd = cls.EventEnd;
                toUpdate.CoordinatorId = cls.CoordinatorId;
                toUpdate.GuestInstructorId = cls.GuestInstructorId;
                toUpdate.PayPalKeyId = cls.PayPalKeyId;

                // updates restricted fields of entity     
                if (isFullyEditable)
                {
                    toUpdate.ClassTypeId = cls.ClassTypeId;
                    toUpdate.OriginLanguageId = cls.OriginLanguageId;
                    toUpdate.TransLanguageId = cls.TransLanguageId;
                    toUpdate.IsWwaFormAllowed = cls.IsWwaFormAllowed;
                    toUpdate.PriceListId = cls.PriceListId;
                    toUpdate.IntegrationTypeId = cls.IntegrationTypeId;
                    toUpdate.IntegrationEntityId = cls.IntegrationEntityId;
                    toUpdate.RegistrationStartUtc = cls.RegistrationStartUtc;
                    toUpdate.RegistrationStart = cls.RegistrationStart;
                }

                // updates class persons 
                var classPersons = context.ClassPersons.Active().Where(x => x.ClassId == cls.ClassId);
                var updateResolver = new SetUpdateResolver<ClassPerson, string>(GetClassPersonKey, (origItem, newItem) => { });
                var strategy = updateResolver.ResolveStrategy(classPersons, cls.ClassPersons);
                context.ClassPersons.AddRange(strategy.ToAdd);
                context.ClassPersons.RemoveRange(strategy.ToDelete);

                // updates entity
                context.SaveChanges();
            }
        }


        // deletes class
        public void DeleteClass(long classId, ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.Classes.Active().FirstOrDefault(x => x.ClassId == classId);
                if (toDelete == null) return;

                // chacks operation
                if (options.HasFlag(ClassOperationOption.CheckOperation))
                    classOperationChecker.CheckOperation(ClassOperation.DeleteClass, toDelete);

                // deletes class
                context.Classes.Remove(toDelete);
                context.SaveChanges();
            }
        }

        // sets class as canceled
        public void SetClassAsCanceled(long classId, ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var cls = context.Classes.Active().FirstOrDefault(x => x.ClassId == classId);
                if (cls == null)
                    throw new ArgumentException($"There is no Class with id {classId}.");
                if (options.HasFlag(ClassOperationOption.CheckOperation) && (cls.IsFinished || cls.IsCanceled))
                    throw new InvalidOperationException($"Class with id {classId} is already canceled or finished.");

                cls.IsCanceled = true;
                cls.Canceled = DateTime.Now;
                context.SaveChanges();
            }
        }

        // sets class as finished
        public void SetClassAsFinished(long classId, ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var cls = context.Classes.Active().FirstOrDefault(x => x.ClassId == classId);
                if (cls == null)
                    throw new ArgumentException($"There is no Class with id {classId}.");
                if (options.HasFlag(ClassOperationOption.CheckOperation) && (cls.IsFinished || cls.IsCanceled))
                    throw new InvalidOperationException($"Class with id {classId} is already canceled or finished.");

                cls.IsFinished = true;
                cls.Finished = DateTime.Now;
                context.SaveChanges();
            }
        }

        #endregion

        #region class persons

        // gets class persons by classId
        public List<ClassPerson> GetClassPersonsByClassId(long classId)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassPersons.Active().Where(x => x.ClassId == classId).ToList();
                return result;
            }
        }

        // gets class persons by class id and list of roles
        public List<ClassPerson> GetClassPersonsByClassIdAndRoles(long classId, params PersonRoleTypeEnum[] roles)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassPersons.Active().Where(x => x.ClassId == classId && roles.Contains(x.RoleTypeId)).ToList();
                return result;
            }
        }

        #endregion

        #region class actions

        /// gets class action types
        public List<ClassActionType> GetClassActionTypes()
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassActionTypes.ToList();
                return result;
            }
        }


        // gets class actions by class id
        public List<ClassAction> GetClassActions(long classId, ClassActionIncludes includes = ClassActionIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassActions.AddIncludes(includes).Active().Where(x => x.ClassId == classId).ToList();
                return result;
            }
        }


        // get class action by id
        public ClassAction GetClassActionById(long classActionId, ClassActionIncludes includes = ClassActionIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassActions.AddIncludes(includes).Active().FirstOrDefault(x => x.ClassActionId == classActionId);
                return result;
            }
        }


        // insert class action
        public long InsertClassAction(ClassAction classAction)
        {
            using (var context = new MainEntities())
            {
                context.ClassActions.Add(classAction);
                context.SaveChanges();
                return classAction.ClassActionId;
            }
        }

        // set as processed
        // CheckOperation - checks that IsProcessed == false
        public void SetClassActionAsProcessed(long classActionId, ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var classAction = context.ClassActions.Active().FirstOrDefault(x => x.ClassActionId == classActionId);
                if (classAction == null)
                    throw new ArgumentException($"There is no ClassAction with id {classActionId}.");
                if (options.HasFlag(ClassOperationOption.CheckOperation) && classAction.IsProcessed)
                    throw new InvalidOperationException($"ClassAction with id {classActionId} is already processed.");
                classAction.IsProcessed = true;
                classAction.Processed = DateTime.Now;
                context.SaveChanges();
            }

        }

        // deletes class action, returns class id
        // CheckOperation - checks that IsProcessed == false, cannot delete processed operation
        public long? DeleteClassAction(long classActionId, ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var classAction = context.ClassActions.Active().FirstOrDefault(x => x.ClassActionId == classActionId);
                if (classAction == null) return null;

                if (options.HasFlag(ClassOperationOption.CheckOperation) && classAction.IsProcessed)
                    throw new InvalidOperationException($"ClassAction with id {classActionId} is processed and cannot be deleted.");

                context.ClassActions.Remove(classAction);
                context.SaveChanges();
                return classAction.ClassId;
            }

        }

        #endregion

        #region class registration files

        // get list of class files by class id
        public List<ClassFile> GetClassFilesByClassId(long classId)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassFiles.Active().Where(x => x.ClassId == classId).ToList();
                return result;
            }
        }


        // get list of class files by ids
        public List<ClassFile> GetClassFilesByIds(IEnumerable<long> classFileIds)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassFiles.Active().Where(x => classFileIds.Contains(x.ClassFileId)).ToList();
                return result;
            }
        }


        // gets class file by name
        public ClassFile GetClassFileByCode(long classId, string code)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassFiles.Active().FirstOrDefault(x => x.ClassId == classId && x.Code == code);
                return result;
            }
        }

        // insert class file
        public long InsertClassFile(ClassFile classFile)
        {
            using (var context = new MainEntities())
            {
                context.ClassFiles.Add(classFile);
                context.SaveChanges();
                return classFile.ClassFileId;
            }
        }

        // update class file
        public void UpdateClassFile(ClassFile classFile)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassFiles.Active().FirstOrDefault(x => x.ClassFileId == classFile.ClassFileId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Class file with id {classFile.ClassId}.");

                toUpdate.DisplayedName = classFile.DisplayedName;
                toUpdate.Code = classFile.Code;
                toUpdate.Assigned = classFile.Assigned;
                toUpdate.FileId = classFile.FileId;
                toUpdate.ClassId = classFile.ClassId;

                context.SaveChanges();
            }
        }

        #endregion

        #region class report settings

        // updates ClassReportSetting of the class [no options]
        public void UpdateClassReportSettingByClassId(long classId, ClassReportSetting setting, ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var cls = context.Classes.AddIncludes(ClassIncludes.ClassReportSetting).Active()
                    .FirstOrDefault(x => x.ClassId == classId);

                if (cls == null)
                    throw new ArgumentException($"There is no class with id {classId}.");

                var toUpdate = cls.ClassReportSetting;
                toUpdate.LocationCode = setting.LocationCode;
                toUpdate.VenueName = setting.VenueName;
                toUpdate.LocationInfoId = setting.LocationInfoId;

                context.SaveChanges();
            }
        }

        #endregion

        #region class business

        // gets class expense types
        public List<ClassExpenseType> GetClassExpenseTypes()
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassExpenseTypes.ToList();
                return result;
            }
        }


        // updates ClassBusiness of the class [no options]
        public void UpdateClassBusinessByClassId(long classId, ClassBusiness business, bool updateOnlyCustomExpenses,
            ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var cls = context.Classes.AddIncludes(ClassIncludes.ClassBusiness).Active()
                    .FirstOrDefault(x => x.ClassId == classId);
                if (cls == null)
                    throw new ArgumentException($"There is no class with id {classId}.");               

                var toUpdate = cls.ClassBusiness;
                toUpdate.ApprovedBudget = business.ApprovedBudget;
                toUpdate.PrintReimbursement = business.PrintReimbursement;
                toUpdate.AssociatedLectureId = business.AssociatedLectureId;

                // updates class expenses
                business.ClassExpenses.ForEach(x => x.ClassBusinessId = toUpdate.ClassBusinessId);
                var originExpensesQuery = context.ClassExpenses.Active()
                    .Where(x => x.ClassBusinessId == cls.ClassBusinessId);
                if (updateOnlyCustomExpenses)
                    originExpensesQuery = originExpensesQuery.Where(x => x.ClassExpenseTypeId == ClassExpenseTypeEnum.Custom);
                var originExpenses = originExpensesQuery.ToList();

                var updateResolver = new SetUpdateResolver<ClassExpense, int>(x => x.Order, (origItem, newItem) =>
                {
                    origItem.Text = newItem.Text;
                    origItem.ClassExpenseTypeId = newItem.ClassExpenseTypeId;
                    origItem.Value = newItem.Value;
                });
                var strategy = updateResolver.ResolveStrategy(originExpenses, business.ClassExpenses);
                context.ClassExpenses.AddRange(strategy.ToAdd);
                context.ClassExpenses.RemoveRange(strategy.ToDelete);

                // saves data
                context.SaveChanges();
            }
        }


        // updates Class's expenses [no options]
        public void UpdateClassExpenses(long classId, List<ClassExpense> classExpenses,
            ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                // loads class and find out class business id
                var cls = context.Classes.AddIncludes(ClassIncludes.ClassBusiness).Active()
                    .FirstOrDefault(x => x.ClassId == classId);
                if (cls == null)
                    throw new ArgumentException($"There is no class with id {classId}");
                var classBusinessId = cls.ClassBusinessId;

                // set ids for new class expanses
                classExpenses.ForEach(x => x.ClassBusinessId = classBusinessId);
                
                // loads origin class expanses
                var originExpanses = context.ClassExpenses.Active().Where(x => x.ClassBusinessId == classBusinessId);

                // resolves update strategy
                var updateResolver = new SetUpdateResolver<ClassExpense, int>(x => x.Order, (origItem, newItem) =>
                {
                    origItem.Text = newItem.Text;
                    origItem.ClassExpenseTypeId = newItem.ClassExpenseTypeId;
                    origItem.Value = newItem.Value;
                });
                var strategy = updateResolver.ResolveStrategy(originExpanses, classExpenses);
                context.ClassExpenses.AddRange(strategy.ToAdd);
                context.ClassExpenses.RemoveRange(strategy.ToDelete);

                // saves data
                context.SaveChanges();
            }
        }

        #endregion

        #region class style

        // gets registration color scheme types
        public List<RegistrationColorScheme> GetRegistrationColorSchemes()
        {
            using (var context = new MainEntities())
            {
                var result = context.RegistrationColorSchemes.ToList();
                return result;
            }
        }

        // updates ClassStyle of the class [no options]
        public void UpdateClassStyle(long classId, ClassStyle style, bool updateHeaderPictureId,
            ClassOperationOption options = ClassOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var cls = context.Classes.AddIncludes(ClassIncludes.ClassReportSetting).Active()
                    .FirstOrDefault(x => x.ClassId == classId);

                if (cls == null)
                    throw new ArgumentException($"There is no class with id {classId}.");

                var toUpdate = cls.ClassStyle;
                toUpdate.HomepageUrl = style.HomepageUrl;
                toUpdate.RegistrationColorSchemeId = style.RegistrationColorSchemeId;
                if (updateHeaderPictureId)
                    toUpdate.HeaderPictureId = style.HeaderPictureId;
                toUpdate.SendCertificatesByEmail = style.SendCertificatesByEmail;

                context.SaveChanges();
            }
        }

        #endregion
        
        #region private fields

        // gets conversation person key 
        private string GetClassPersonKey(ClassPerson conversationPerson)
        {
            return $"{conversationPerson.PersonId}#{(int) conversationPerson.RoleTypeId}";
        }

        #endregion
    }
}
