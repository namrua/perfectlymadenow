  
using System.Linq;

namespace AutomationSystem.Main.Model.Queries
{
    public static class ActiveExtensions
    {

		// selects active Address entities 
		public static IQueryable<Address> Active(this IQueryable<Address> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Class entities 
		public static IQueryable<Class> Active(this IQueryable<Class> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassAction entities 
		public static IQueryable<ClassAction> Active(this IQueryable<ClassAction> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassBusiness entities 
		public static IQueryable<ClassBusiness> Active(this IQueryable<ClassBusiness> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassExpense entities 
		public static IQueryable<ClassExpense> Active(this IQueryable<ClassExpense> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassFile entities 
		public static IQueryable<ClassFile> Active(this IQueryable<ClassFile> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassMaterial entities 
		public static IQueryable<ClassMaterial> Active(this IQueryable<ClassMaterial> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassMaterialDownloadLog entities 
		public static IQueryable<ClassMaterialDownloadLog> Active(this IQueryable<ClassMaterialDownloadLog> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassMaterialFile entities 
		public static IQueryable<ClassMaterialFile> Active(this IQueryable<ClassMaterialFile> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassMaterialRecipient entities 
		public static IQueryable<ClassMaterialRecipient> Active(this IQueryable<ClassMaterialRecipient> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassPerson entities 
		public static IQueryable<ClassPerson> Active(this IQueryable<ClassPerson> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassPreference entities 
		public static IQueryable<ClassPreference> Active(this IQueryable<ClassPreference> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassPreferenceExpense entities 
		public static IQueryable<ClassPreferenceExpense> Active(this IQueryable<ClassPreferenceExpense> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassRegistration entities 
		public static IQueryable<ClassRegistration> Active(this IQueryable<ClassRegistration> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassRegistrationFile entities 
		public static IQueryable<ClassRegistrationFile> Active(this IQueryable<ClassRegistrationFile> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassRegistrationInvitation entities 
		public static IQueryable<ClassRegistrationInvitation> Active(this IQueryable<ClassRegistrationInvitation> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassRegistrationLastClass entities 
		public static IQueryable<ClassRegistrationLastClass> Active(this IQueryable<ClassRegistrationLastClass> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassRegistrationPayment entities 
		public static IQueryable<ClassRegistrationPayment> Active(this IQueryable<ClassRegistrationPayment> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassReportSetting entities 
		public static IQueryable<ClassReportSetting> Active(this IQueryable<ClassReportSetting> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ClassStyle entities 
		public static IQueryable<ClassStyle> Active(this IQueryable<ClassStyle> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ContactBlackList entities 
		public static IQueryable<ContactBlackList> Active(this IQueryable<ContactBlackList> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ContactList entities 
		public static IQueryable<ContactList> Active(this IQueryable<ContactList> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ContactListItem entities 
		public static IQueryable<ContactListItem> Active(this IQueryable<ContactListItem> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active DistanceClassTemplate entities 
		public static IQueryable<DistanceClassTemplate> Active(this IQueryable<DistanceClassTemplate> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active DistanceClassTemplateClass entities 
		public static IQueryable<DistanceClassTemplateClass> Active(this IQueryable<DistanceClassTemplateClass> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active DistanceClassTemplatePerson entities 
		public static IQueryable<DistanceClassTemplatePerson> Active(this IQueryable<DistanceClassTemplatePerson> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active DistanceProfile entities 
		public static IQueryable<DistanceProfile> Active(this IQueryable<DistanceProfile> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active FormerClass entities 
		public static IQueryable<FormerClass> Active(this IQueryable<FormerClass> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active FormerStudent entities 
		public static IQueryable<FormerStudent> Active(this IQueryable<FormerStudent> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Person entities 
		public static IQueryable<Person> Active(this IQueryable<Person> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active PersonRole entities 
		public static IQueryable<PersonRole> Active(this IQueryable<PersonRole> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active PriceList entities 
		public static IQueryable<PriceList> Active(this IQueryable<PriceList> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active PriceListItem entities 
		public static IQueryable<PriceListItem> Active(this IQueryable<PriceListItem> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active Profile entities 
		public static IQueryable<Profile> Active(this IQueryable<Profile> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active ProfileUser entities 
		public static IQueryable<ProfileUser> Active(this IQueryable<ProfileUser> query) 
		{
			return query.Where(x => !x.Deleted);
		}


		// selects active RoyaltyFeeRate entities 
		public static IQueryable<RoyaltyFeeRate> Active(this IQueryable<RoyaltyFeeRate> query) 
		{
			return query.Where(x => !x.Deleted);
		}

	}
}
