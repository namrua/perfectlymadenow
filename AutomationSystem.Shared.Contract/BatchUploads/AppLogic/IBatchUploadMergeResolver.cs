using System.Collections.Generic;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.BatchUploads.AppLogic
{
    /// <summary>
    /// Resolves merging of uploaded items on specified entity set
    /// </summary>    
    public interface IBatchUploadMergeResolver<TEntity>
    {

        // binds entity set
        void BindEntities(IEnumerable<TEntity> entitySet);

        // tries to pair batch upload item to existing entity from set
        long? TryPair(BatchUploadItem item);

    }
    
}
