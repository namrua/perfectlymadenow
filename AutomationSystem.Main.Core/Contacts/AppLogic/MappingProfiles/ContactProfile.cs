using System.Linq;
using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Routines;
using ContactListItem = AutomationSystem.Main.Contract.Contacts.AppLogic.Models.ContactListItem;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.Contacts.AppLogic.MappingProfiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<FormerStudent, ContactListItem>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.Address, "Address", "FormerStudent"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => MainTextHelper.GetNormalizedEmail(src.Email)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => MainTextHelper.GetFullName(src.Address.FirstName, src.Address.LastName)));

            CreateMap<ContactListDefinition, ContactList>()
                .ForMember(dest => dest.ContactListItems, opt => opt.MapFrom(src => src.ContactListItems));

            CreateMap<ContactListItemDefinition, Model.ContactListItem>();

            CreateMap<Model.ContactListItem, ContactListItemDefinition>();

            CreateMap<ContactList, ContactListSenderForm>();

            CreateMap<ContactList, ContactListDetail>()
                .BeforeMap((src, dest) => EntityHelper.CheckForNull(src.ContactListItems, "ContactListItems", "ContactList"))
                .ForMember(dest => dest.Emails, opt => opt.MapFrom(src => src.ContactListItems.Select(x => x.Email).ToList()))
                .AfterMap((src, dest, context) => context.Mapper.Map(src, dest.Form));
        }
    }
}
