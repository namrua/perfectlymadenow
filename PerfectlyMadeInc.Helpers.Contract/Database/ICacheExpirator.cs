namespace PerfectlyMadeInc.Helpers.Contract.Database
{
    /// <summary>
    /// causes cache expiration
    /// </summary>
    public interface ICacheExpirator
    {

        // sets as expired
        void SetAsExpired();
        
    }

}
