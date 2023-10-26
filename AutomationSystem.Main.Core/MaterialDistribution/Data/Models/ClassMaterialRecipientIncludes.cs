using System;

namespace AutomationSystem.Main.Core.MaterialDistribution.Data.Models
{
    /// <summary>
    /// ClassMaterialRecipient includes
    /// </summary>
    [Flags]
    public enum ClassMaterialRecipientIncludes
    {
        None = 0,
        ClassMaterial = 1 << 0,
        ClassMaterialClass = 1 << 1,
        ClassMaterialClassClassType = 1 << 2,
        ClassMaterialClassClassPersons = 1 << 3,
        ClassMaterialClassMaterialFiles = 1 << 4,
        ClassMaterialDownloadLogs = 1 << 5
    }
}
