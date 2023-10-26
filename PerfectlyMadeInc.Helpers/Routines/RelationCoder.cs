using System;
using PerfectlyMadeInc.Helpers.Contract.Routines;

namespace PerfectlyMadeInc.Helpers.Routines
{
    /// <summary>
    /// Encodes and decodes relation to numeric code
    /// </summary>
    public class RelationCoder<TFirst, TSecond> : IRelationCoder<TFirst, TSecond>
    {

        private readonly long secondOffset;
        private readonly Func<long, TFirst> longToFirst;
        private readonly Func<TFirst, long> firstToLong;
        private readonly Func<long, TSecond> longToSecond;
        private readonly Func<TSecond, long> secondToLong;

        // constructor
        public RelationCoder(long secondOffset, Func<long, TFirst> longToFirst, Func<TFirst, long> firstToLong,
            Func<long, TSecond> longToSecond, Func<TSecond, long> secondToLong)
        {
            this.secondOffset = secondOffset;
            this.longToFirst = longToFirst;
            this.firstToLong = firstToLong;
            this.longToSecond = longToSecond;
            this.secondToLong = secondToLong;
        }


        // gets code from the relation
        public long GetCode(TFirst first, TSecond second)
        {
            var firstLong = firstToLong(first);
            var secondLong = secondToLong(second);
            if (firstLong >= secondOffset)
                throw new ArgumentOutOfRangeException(nameof(first), $"First member {first} of relation exceeds second offset {secondOffset}.");

            var result = firstLong  + secondOffset * secondLong;
            return result;
        }


        // gets first from relation
        public TFirst GetFirst(long code)
        {
            var result = longToFirst(code % secondOffset);
            return result;
        }

        // get second from relation
        public TSecond GetSecond(long code)
        {
            var result = longToSecond(code / secondOffset);
            return result;
        }

    }

}
