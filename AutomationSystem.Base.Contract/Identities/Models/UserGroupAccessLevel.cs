namespace AutomationSystem.Base.Contract.Identities.Models
{   
    /// <summary>
    /// Determines access level to user groups
    /// TODO: keep access levels respecting linear comparision
    /// TODO: (or redefine comparing algorithms in EntitleMapper.ComputeUserGroupAccessLevelWithDefaultsByPermissions)
    /// </summary>
    public enum UserGroupAccessLevel
    {
        NoAccess,               // no access for any user group access level
        OnlyAssigned,           // access only for assigned
        All                     // access for all
    }

}
