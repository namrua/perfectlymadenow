  
using System.Linq;

namespace AutomationSystem.Shared.Model.Queries
{
    public static class ActiveExtensions
    {

		// selects active AppLocalisation entities 
		public static IQueryable<AppLocalisation> Active(this IQueryable<AppLocalisation> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active AsyncRequest entities 
		public static IQueryable<AsyncRequest> Active(this IQueryable<AsyncRequest> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active BatchUpload entities 
		public static IQueryable<BatchUpload> Active(this IQueryable<BatchUpload> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active BatchUploadField entities 
		public static IQueryable<BatchUploadField> Active(this IQueryable<BatchUploadField> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active BatchUploadItem entities 
		public static IQueryable<BatchUploadItem> Active(this IQueryable<BatchUploadItem> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ConferenceAccount entities 
		public static IQueryable<ConferenceAccount> Active(this IQueryable<ConferenceAccount> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active DataLocalisation entities 
		public static IQueryable<DataLocalisation> Active(this IQueryable<DataLocalisation> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Email entities 
		public static IQueryable<Email> Active(this IQueryable<Email> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active EmailAttachment entities 
		public static IQueryable<EmailAttachment> Active(this IQueryable<EmailAttachment> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active EmailParameter entities 
		public static IQueryable<EmailParameter> Active(this IQueryable<EmailParameter> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active EmailTemplate entities 
		public static IQueryable<EmailTemplate> Active(this IQueryable<EmailTemplate> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active EmailTemplateParameter entities 
		public static IQueryable<EmailTemplateParameter> Active(this IQueryable<EmailTemplateParameter> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active EnumLocalisation entities 
		public static IQueryable<EnumLocalisation> Active(this IQueryable<EnumLocalisation> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active File entities 
		public static IQueryable<File> Active(this IQueryable<File> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Incident entities 
		public static IQueryable<Incident> Active(this IQueryable<Incident> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Job entities 
		public static IQueryable<Job> Active(this IQueryable<Job> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active JobRun entities 
		public static IQueryable<JobRun> Active(this IQueryable<JobRun> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active PayPalKey entities 
		public static IQueryable<PayPalKey> Active(this IQueryable<PayPalKey> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active PayPalRecord entities 
		public static IQueryable<PayPalRecord> Active(this IQueryable<PayPalRecord> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Preference entities 
		public static IQueryable<Preference> Active(this IQueryable<Preference> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active User entities 
		public static IQueryable<User> Active(this IQueryable<User> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active UserLogin entities 
		public static IQueryable<UserLogin> Active(this IQueryable<UserLogin> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active UserRoleAssignment entities 
		public static IQueryable<UserRoleAssignment> Active(this IQueryable<UserRoleAssignment> query) 
		{
			return query.Where(x => !x.Deleted);
		}

	}
}
