namespace PerfectlyMadeInc.Helpers.Contract.Routines
{
    /// <summary>
    /// Encodes and decodes relation to numeric code
    /// </summary>
    public interface IRelationCoder<TFirst, TSecond>
    {

        // gets code from the relation
        long GetCode(TFirst first, TSecond second);

        // gets first from relation
        TFirst GetFirst(long code);

        // get second from relation
        TSecond GetSecond(long code);

    }
}
