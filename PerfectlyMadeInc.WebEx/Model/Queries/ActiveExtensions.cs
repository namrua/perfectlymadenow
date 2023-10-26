  
using System.Linq;

namespace PerfectlyMadeInc.WebEx.Model.Queries
{
    public static class ActiveExtensions
    {

		// selects active Account entities 
		public static IQueryable<Account> Active(this IQueryable<Account> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Event entities 
		public static IQueryable<Event> Active(this IQueryable<Event> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active IntegrationState entities 
		public static IQueryable<IntegrationState> Active(this IQueryable<IntegrationState> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Program entities 
		public static IQueryable<Program> Active(this IQueryable<Program> query) 
		{
			return query.Where(x => !x.Deleted);
		}

	}
}
