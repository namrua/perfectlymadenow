using System;

namespace AutomationSystem.Shared.Core.Files.Data.Models
{
    // file includes
    [Flags]
    public enum FileIncludes
    {
        None = 0x00,     
        FileType = 0x01,
        Language = 0x02,      
    } 

}
