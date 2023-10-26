using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Base.Contract.Integration
{

    /// <summary>
    /// Data transfer object for Conference Account 
    /// IMPORTANT!!! this BREAKS dependencies on origin ConferenceAccount model object 
    /// </summary>
    public class ConferenceAccountInfo
    {

        public long ConferenceAccountId { get; set; }
        public string Name { get; set; }
        public ConferenceAccountTypeEnum ConferenceAccountTypeId { get; set; }
        public long AccountSettingsId { get; set; }
        public bool Active { get; set; }
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }
        public long? UserGroupId { get; set; }

    }

}
