using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Model.Queries;
using PerfectlyMadeInc.WebEx.Programs.Data.Models;
using PerfectlyMadeInc.WebEx.Programs.Data.Queries;

namespace PerfectlyMadeInc.WebEx.Programs.Data
{

    /// <summary>
    /// Provides Program database layer
    /// </summary>
    public class ProgramDatabaseLayer : IProgramDatabaseLayer
    {

        // gets active programs
        public List<Program> GetActivePrograms(ProgramFilter filter, long? currentId)
        {
            using (var context = new WebExEntities())
            {
                var result = context.Programs.Filter(filter).ToList();
                if (!currentId.HasValue || result.Any(x => x.ProgramId == currentId)) return result;

                // adds current if not listed - !!! Active() is ignored !!!
                var current = context.Programs.FirstOrDefault(x => x.ProgramId == currentId);
                if (current == null)
                    throw new ArgumentException($"Current Program with id {currentId} does not exist.");

                // adds current to the start of the list
                var newResult = new List<Program> {current};
                newResult.AddRange(result);
                return newResult;
            }
        }


        // gets list of all programs
        public List<Program> GetProgramsByFilter(ProgramFilter filter = null)
        {
            using (var context = new WebExEntities())
            {
                var result = context.Programs.Include("Account").Filter(filter).ToList();
                return result;
            }
        }

        // gets program by id 
        public Program GetProgramById(long programId, ProgramIncludes includes = ProgramIncludes.None)
        {
            using (var context = new WebExEntities())
            {
                var result = context.Programs.AddIncludes(includes)
                    .Active().FirstOrDefault(x => x.ProgramId == programId);
                ProgramRemoveInactive.RemoveInactiveForPrograms(result, includes);
                return result;
            }
        }

        // insert program
        public long InsertProgram(Program program)
        {
            using (var context = new WebExEntities())
            {
                context.Programs.Add(program);
                context.SaveChanges();
                return program.ProgramId;
            }
        }      


        // delete program
        public void DeleteProgram(long programId, bool checkDeletion = false)
        {
            using (var context = new WebExEntities())
            {
                var toDelete = context.Programs.Active().FirstOrDefault(x => x.ProgramId == programId);
                if (toDelete == null) return;

                if (checkDeletion && toDelete.EntityId.HasValue && toDelete.EntityTypeId.HasValue)
                    throw new InvalidOperationException($"Deletion is not allowed for Program with id {programId}.");

                var toDeleteEvents = context.Events.Active().Where(x => x.ProgramId == toDelete.ProgramId);

                context.Events.RemoveRange(toDeleteEvents);
                context.Programs.Remove(toDelete);
                context.SaveChanges();
            }
        }


        // Set programs entity relation
        public void SetProgramEntity(long programId, EntityTypeEnum entityTypeId, long entityId, bool disconnectOther = false)
        {
            using (var context = new WebExEntities())
            {
                var toUpdate = context.Programs.Active().FirstOrDefault(x => x.ProgramId == programId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Program with id {programId}.");
              
                // updates entity
                toUpdate.EntityId = entityId;
                toUpdate.EntityTypeId = entityTypeId;
            
                // disconnects other programs
                if (disconnectOther)
                {
                    var programsToDisconnect = context.Programs.Active()
                        .Where(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId)
                        .Where(x => x.ProgramId != programId);                
                    
                    foreach (var toDisconnect in programsToDisconnect)
                    {
                        toDisconnect.EntityId = null;
                        toDisconnect.EntityTypeId = null;
                    }
                }

                // save changes
                context.SaveChanges();
            }           
        }

        // detaches entity from programs
        public void DetachProgramEntity(EntityTypeEnum entityTypeId, long entityId)
        {
            using (var context = new WebExEntities())
            {
                var programsWithEntity = context.Programs.Active()
                    .Where(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId).ToList();
                foreach (var toDetach in programsWithEntity)
                {
                    toDetach.EntityId = null;
                    toDetach.EntityTypeId = null;
                }
                context.SaveChanges();
            }
        }
                    
    }

}
