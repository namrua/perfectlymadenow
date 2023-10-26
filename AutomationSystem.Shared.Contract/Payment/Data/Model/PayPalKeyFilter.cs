using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Payment.Data.Model
{
    /// <summary>
    /// PaypPalKey filter
    /// </summary>
    public class PayPalKeyFilter
    {
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }
        public long? UserGroupId { get; set; }

        public bool? IsActive { get; set; }

        public CurrencyEnum? CurrencyId { get; set; }

        // restrictions of the set of user groups
        public List<long> UserGroupIds { get; set; }
    }
}
