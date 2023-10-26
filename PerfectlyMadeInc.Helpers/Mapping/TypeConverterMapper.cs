using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PerfectlyMadeInc.Helpers.Mapping
{
    public class TypeConverterMapper<TObject, TId, TValue> : ITypeConverter<TId, TValue>
    {
        private readonly Lazy<Dictionary<TId, TValue>> map;

        public TypeConverterMapper(Func<List<TObject>> objectFetcher, Func<TObject, TId> idSelector, Func<TObject, TValue> valueSelector)
        {
            map = new Lazy<Dictionary<TId, TValue>>(() => objectFetcher().ToDictionary(idSelector, valueSelector));
        }

        public TValue Convert(TId source, TValue destination, ResolutionContext context)
        {
            if (!map.Value.TryGetValue(source, out var result))
            {
                return default(TValue);
            }

            return result;
        }
    }
}
