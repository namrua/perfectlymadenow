using System;

namespace AutomationSystem.Main.Core.MaterialDistribution.Data.Models
{
    /// <summary>
    /// ClassMaterialFile includes
    /// </summary>
    [Flags]
    public enum ClassMaterialFileIncludes
    {
        None = 0,
        ClassMaterial = 1 << 0,
        ClassMaterialClass = 1 << 1
    }
}
