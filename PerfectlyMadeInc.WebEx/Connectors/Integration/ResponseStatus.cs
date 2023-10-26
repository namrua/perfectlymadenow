namespace PerfectlyMadeInc.WebEx.Connectors.Integration
{
    /// <summary>
    /// Encapsulates response status informations
    /// </summary>
    public class ResponseStatus
    {

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        // constructor
        public ResponseStatus()
        {
            IsSuccess = true;
        }

    }
}
