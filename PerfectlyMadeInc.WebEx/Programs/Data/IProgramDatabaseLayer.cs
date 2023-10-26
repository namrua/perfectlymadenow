using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Programs.Data.Models;

namespace PerfectlyMadeInc.WebEx.Programs.Data
{
    /// <summary>
    /// Provides program database layer
    /// </summary>
    public interface IProgramDatabaseLayer
    {
       
        // gets active programs
        List<Program> GetActivePrograms(ProgramFilter filter, long? currentId);

        // gets list of all programs
        List<Program> GetProgramsByFilter(ProgramFilter filter = null);

        // gets program by id
        Program GetProgramById(long programId, ProgramIncludes programIncludes = ProgramIncludes.None);

        // save program
        long InsertProgram(Program program);
     
        // delete program
        void DeleteProgram(long programId, bool checkDeletion = false);
              

        // set programs entity relation
        void SetProgramEntity(long programId, EntityTypeEnum entityTypeId, long entityId, bool disconnectOther = false);

        // detaches entity from programs
        void DetachProgramEntity(EntityTypeEnum entityTypeId, long entityId);      

    }

}
