using System.Collections.Generic;

namespace PerfectlyMadeInc.Helpers.Contract.Database.Models
{
    /// <summary>
    /// Result of set update resolver
    /// </summary>    
    public class SetUpdateResolverStrategy<T>
    {
        
        // public properties
        public List<T> ToAdd { get; set; }
        public List<T> ToDelete { get; set; }

        // constructor
        public SetUpdateResolverStrategy()
        {
            ToAdd = new List<T>();
            ToDelete = new List<T>();
        }
    }

}
