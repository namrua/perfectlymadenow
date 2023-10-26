using System;

namespace PerfectlyMadeInc.WebEx.Programs.Data.Models
{
    /// <summary>
    /// Determines program includes
    /// </summary>
    [Flags]
    public enum ProgramIncludes
    {
        None = 0x00,
        Events = 0x01
    }

}
