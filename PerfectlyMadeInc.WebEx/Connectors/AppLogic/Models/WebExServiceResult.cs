using System;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic.Models
{    
    /// <summary>
    /// Encapsulates result of  WebEx service
    /// </summary>   
    public class WebExServiceResult<T>
    {

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public T Result { get; set; }

    }
}
