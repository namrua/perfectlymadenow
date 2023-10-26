using System;

namespace PerfectlyMadeInc.Helpers.Routines
{
    /// <summary>
    /// Class encapsulates static macros used for object mapping
    /// </summary>
    public static class EntityHelper
    {
        public static void CheckForNull<T>(T entity, string entityPath, string rootEntityName, bool idIsNotNull = true) where T : class
        {
            if (idIsNotNull && entity == null)
            {
                throw new InvalidOperationException($"{entityPath} is not included into {rootEntityName} object.");
            }
        }
    }
}
