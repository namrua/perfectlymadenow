using System;
using System.Collections.Generic;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic.Models
{
    /// <summary>
    /// Encapsulates marge result
    /// </summary>
    public class PairingResult<T>
    {

        public List<Tuple<T, T>> Pairs { get; set; }
        public List<T> Unassigned { get; set; }

        // constructor
        public PairingResult()
        {
            Pairs = new List<Tuple<T, T>>();
            Unassigned = new List<T>();
        }

    }
}
