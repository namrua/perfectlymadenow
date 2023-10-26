using System;

namespace AutomationSystem.Main.Core.MaterialDistribution.Data.Models
{
    /// <summary>
    /// ClassMaterial includes
    /// </summary>
    [Flags]
    public enum ClassMaterialIncludes
    {
        None = 0,
        Class = 1 << 0,
        ClassClassType = 1 << 1,
        ClassClassPersons = 1 << 2,
        ClassMaterialFiles = 1 << 3,
        ClassMaterialRecipients = 1 << 4,
        ClassMaterialRecipientsClassMaterialDownloadLogs = 1 << 5,
    }
}
