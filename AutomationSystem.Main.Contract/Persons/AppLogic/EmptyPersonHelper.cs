using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Persons.AppLogic
{
    public class EmptyPersonHelper : IPersonHelper
    {
        public long? GetDefaultPersonId(PersonRoleTypeEnum role)
        {
            return null;
        }

        public List<long> GetDefaultPersonIds(PersonRoleTypeEnum role)
        {
            return new List<long>();
        }

        public string GetPersonNameById(long? personId)
        {
            return null;
        }

        public List<PickerItem> GetPickerItemsForRole(PersonRoleTypeEnum role)
        {
            return new List<PickerItem>();
        }
    }
}
