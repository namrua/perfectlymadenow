using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.AppLogic.Models.Events;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Factories
{
    /// <summary>
    /// Factory for class events
    /// </summary>
    public class ClassEventFactory : IClassEventFactory
    {
        public ClassPersonsChangedEvent CreateClassPersonsChangedEventWhenChanged(Class originClass, Class updatedClass)
        {
            if (originClass.ClassPersons == null)
            {
                throw new InvalidOperationException("ClassPersons is not included into origin Class object.");
            }

            if (updatedClass.ClassPersons == null)
            {
                throw new InvalidOperationException("ClassPersons is not included into updated Class object.");
            }

            var evnt = new ClassPersonsChangedEvent(updatedClass.ClassId);
            UpdateOne(evnt, PersonRoleTypeEnum.Coordinator, originClass.CoordinatorId, updatedClass.CoordinatorId);
            UpdateOne(evnt, PersonRoleTypeEnum.GuestInstructor, originClass.GuestInstructorId, updatedClass.GuestInstructorId);
            UpdateMany(evnt, PersonRoleTypeEnum.Instructor, originClass.ClassPersons, updatedClass.ClassPersons);
            UpdateMany(evnt, PersonRoleTypeEnum.ApprovedStaff, originClass.ClassPersons, updatedClass.ClassPersons);

            if (evnt.AddedPersons.Any() || evnt.RemovedPersons.Any())
            {
                return evnt;
            }

            return null;
        }

        #region private methods

        private void UpdateOne(ClassPersonsChangedEvent evnt, PersonRoleTypeEnum roleTypeId, long? originPersonId, long? updatedPersonId)
        {
            var notSame = originPersonId != updatedPersonId;
            if (updatedPersonId.HasValue)
            {
                evnt.CurrentPersons.Add(new PersonIdAndRole(roleTypeId, updatedPersonId.Value));

                if (notSame)
                {
                    evnt.AddedPersons.Add(new PersonIdAndRole(roleTypeId, updatedPersonId.Value));
                }
            }

            if (originPersonId.HasValue && notSame)
            {
                evnt.RemovedPersons.Add(new PersonIdAndRole(roleTypeId, originPersonId.Value));
            }
        }

        private void UpdateMany(
            ClassPersonsChangedEvent evnt,
            PersonRoleTypeEnum roleTypeId,
            IEnumerable<ClassPerson> originClassPersons,
            IEnumerable<ClassPerson> updatedClassPersons)
        {
            var originIds = new HashSet<long>(originClassPersons.Where(x => x.RoleTypeId == roleTypeId).Select(x => x.PersonId));
            var updatedIds = new HashSet<long>(updatedClassPersons.Where(x => x.RoleTypeId == roleTypeId).Select(x => x.PersonId));

            evnt.CurrentPersons.AddRange(updatedIds.Select(x => new PersonIdAndRole(roleTypeId, x)));
            evnt.AddedPersons.AddRange(updatedIds.Where(x => !originIds.Contains(x)).Select(x => new PersonIdAndRole(roleTypeId, x)));
            evnt.RemovedPersons.AddRange(originIds.Where(x => !updatedIds.Contains(x)).Select(x => new PersonIdAndRole(roleTypeId, x)));
        }

        #endregion
    }
}
