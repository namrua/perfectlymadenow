using System.Data.Entity.Infrastructure;
using AutomationSystem.Shared.Core.AsyncRequests.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.AsyncRequests.Data.Extensions
{
    /// <summary>
    /// Aggregates include extensions
    /// </summary>
    public static class AsyncRequestIncludeExtensions
    {
        // add includes for AsyncRequest
        public static DbQuery<AsyncRequest> AddIncludes(this DbQuery<AsyncRequest> query, AsyncRequestIncludes includes)
        {
            if (includes.HasFlag(AsyncRequestIncludes.AsyncRequestType))
                query = query.Include("AsyncRequestType");
            if (includes.HasFlag(AsyncRequestIncludes.ProcessingState))
                query = query.Include("ProcessingState");
            return query;
        }
    }
}
