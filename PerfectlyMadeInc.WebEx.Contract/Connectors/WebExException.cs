using System;

namespace PerfectlyMadeInc.WebEx.Contract.Connectors
{
    /// <summary>
    /// Result information
    /// </summary>
    public class WebExException : Exception
    {

        // public properties        
        public string ResponseString { get; set; }

        // contructor
        public WebExException(string message, string response = null) : base(message)
        {
            ResponseString = response;
        }

        // constructor
        public WebExException(string message, Exception innerException, string response = null) : base(message, innerException)
        {
            ResponseString = response;
        }

    }
    
}
