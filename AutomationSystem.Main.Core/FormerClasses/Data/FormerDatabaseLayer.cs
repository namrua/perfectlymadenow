using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.FormerClasses.Data.Extensions;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data.Extensions;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.FormerClasses.Data
{

    /// <summary>
    /// Provides former student and class database layer
    /// </summary>
    public class FormerDatabaseLayer : IFormerDatabaseLayer
    {

        // private components
        private readonly IAddressConvertor addressConvertor;

        // constructor
        public FormerDatabaseLayer(IAddressConvertor addressConvertor)
        {
            this.addressConvertor = addressConvertor;
        }

        #region former classes      

        // gets former classes by filter
        public List<FormerClass> GetFormerClassesByFilter(FormerClassFilter filter = null, FormerClassIncludes includes = FormerClassIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.FormerClasses.AddIncludes(includes).Filter(filter).ToList();
                return result;
            }
        }

        // gets former class by id
        public FormerClass GetFormerClassById(long formerClassId, FormerClassIncludes includes = FormerClassIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.FormerClasses.AddIncludes(includes).Active()
                    .FirstOrDefault(x => x.FormerClassId == formerClassId);
                return result;
            }
        }

        // inserts former class
        public long InsertFormerClass(FormerClass formerClass)
        {
            using (var context = new MainEntities())
            {
                context.FormerClasses.Add(formerClass);
                context.SaveChanges();
                return formerClass.FormerClassId;
            }
        }

        // update former class
        public void UpdateFormerClass(FormerClass formerClass, FormerOperationOption options = FormerOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.FormerClasses.Active().FirstOrDefault(x => x.FormerClassId == formerClass.FormerClassId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Former class with id {formerClass.FormerClassId}.");

                // updates former class
                toUpdate.ClassTypeId = formerClass.ClassTypeId;
                toUpdate.Location = formerClass.Location;
                toUpdate.EventStart = formerClass.EventStart;
                toUpdate.EventEnd = formerClass.EventEnd;              
                toUpdate.IsTemporary = formerClass.IsTemporary;
                toUpdate.ProfileId = formerClass.ProfileId;
                if (!options.HasFlag(FormerOperationOption.KeepOwnerId))
                    toUpdate.OwnerId = formerClass.OwnerId;

                // saves changes
                context.SaveChanges();
            }
        }

        // delete former class
        public void DeleteFormerClass(long formerClassId, FormerOperationOption options = FormerOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.FormerClasses.Active().FirstOrDefault(x => x.FormerClassId == formerClassId);
                if (toDelete == null)
                    return;

                // gets students of class
                var studentsToDelete = context.FormerStudents.Active().Where(x => x.FormerClassId == formerClassId).ToList();
                var studentsIds = studentsToDelete.Select(x => x.FormerStudentId).ToList();

                // gets registrations with reviewed students
                var registrationToDisconnect = context.ClassRegistrations.Active()
                    .Where(x => x.FormerStudentId.HasValue && studentsIds.Contains(x.FormerStudentId.Value)).ToList();

                // checks whether former student is used for active class registration as reviewed student               
                if (options.HasFlag(FormerOperationOption.CheckStudentNotUsedForReview)
                    && registrationToDisconnect.AsQueryable().FilterStateSet(ClassRegistrationStateSet.NewApproved).Any())
                    throw new InvalidOperationException($"Former class with id {formerClassId} contains students that are used as reviewed student for class registrations.");

                // disconnect class registrations
                foreach (var toDisconnect in registrationToDisconnect)
                    toDisconnect.FormerStudentId = null;

                // deletes former students and class and saves changes
                context.FormerStudents.RemoveRange(studentsToDelete);                
                context.FormerClasses.Remove(toDelete);
                context.SaveChanges();
            }
        }

        #endregion

        #region former students

        // gets former students by filter
        public List<FormerStudent> GetFormerStudentsByFilter(FormerStudentFilter filter = null, FormerStudentIncludes includes = FormerStudentIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.FormerStudents.AddIncludes(includes).Filter(filter).ToList();
                return result;
            }
        }

        // gets former students by filter
        public List<FormerStudent> GetFormerStudentsForReviewing(
            ClassRegistration registration,
            FormerStudentFilter formerStudentFilter,
            FormerStudentIncludes includes = FormerStudentIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.FormerStudents.AddIncludes(includes).Active()
                    .Filter(formerStudentFilter)
                    .FilterForReview(registration).ToList();
                return result;
            }
        }


        // gets former student by id
        public FormerStudent GetFormerStudentById(long formerStudentId, FormerStudentIncludes includes = FormerStudentIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.FormerStudents.AddIncludes(includes).Active().FirstOrDefault(x => x.FormerStudentId == formerStudentId);
                return result;
            }
        }

        // gets former student from filter scope by id
        public FormerStudent GetFormerStudentByIdAndFilter(
            long formerStudentId,
            FormerStudentFilter formerStudentFilter,
            FormerStudentIncludes includes = FormerStudentIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.FormerStudents.AddIncludes(includes).Filter(formerStudentFilter).FirstOrDefault(x => x.FormerStudentId == formerStudentId);
                return result;
            }
        }

        // inserts former student
        public long InsertFormerStudent(FormerStudent formerStudent)
        {
            using (var context = new MainEntities())
            {
                context.FormerStudents.Add(formerStudent);
                context.SaveChanges();
                return formerStudent.FormerStudentId;
            }
        }

        // updates former student
        public void UpdateFormerStudent(FormerStudent formerStudent, FormerOperationOption options = FormerOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                // updates former student
                var toUpdate = context.FormerStudents
                    .AddIncludes(FormerStudentIncludes.Address).Active()
                    .FirstOrDefault(x => x.FormerStudentId == formerStudent.FormerStudentId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Former student with id {formerStudent.FormerStudentId}");

                // checks whether former class id is consistent
                if (options.HasFlag(FormerOperationOption.CheckStudentClassConsistency) &&
                    toUpdate.FormerClassId != formerStudent.FormerClassId)
                    throw new SecurityException($"FormerStudent with id {toUpdate.FormerStudentId} has inconsistent FormerClassId {toUpdate.FormerClassId} with {formerStudent.FormerClassId}");

                // updates values
                toUpdate.FormerClassId = formerStudent.FormerClassId;
                toUpdate.Email = formerStudent.Email;
                toUpdate.Phone = formerStudent.Phone;
                toUpdate.IsTemporary = formerStudent.IsTemporary;
                addressConvertor.UpdateAddresss(toUpdate.Address, formerStudent.Address);
                if (!options.HasFlag(FormerOperationOption.KeepOwnerId))
                    toUpdate.OwnerId = formerStudent.OwnerId;

                // saves changes
                context.SaveChanges();
            }
        }

        // deletes former student
        public void DeleteFormerStudent(long formerStudentId,
            FormerOperationOption options = FormerOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.FormerStudents.Include("ClassRegistrations").Active().FirstOrDefault(x => x.FormerStudentId == formerStudentId);
                if (toDelete == null)
                    return;

                // checks whether former student is used for active class registration as reviewed student
                var registrations = toDelete.ClassRegistrations.AsQueryable();
                if (options.HasFlag(FormerOperationOption.CheckStudentNotUsedForReview)
                    && registrations.FilterStateSet(ClassRegistrationStateSet.NewApproved).Any())
                    throw new InvalidOperationException($"Former student with id {formerStudentId} is used as reviewed student for class registration.");

                // disconnect class registrations
                foreach (var toDisconnect in registrations.Active())
                    toDisconnect.FormerStudentId = null;

                // deletes former student and save changes
                context.FormerStudents.Remove(toDelete);
                context.SaveChanges();
            }
        }

        #endregion
    }
}
