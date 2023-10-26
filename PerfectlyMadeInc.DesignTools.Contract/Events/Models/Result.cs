using System;

namespace PerfectlyMadeInc.DesignTools.Contract.Events.Models
{
    /// <summary>
    /// Encapsulates result of event handler call
    /// </summary>
    public class Result
    {
        public ResultType ResultType { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public override string ToString()
        {
            var result =  $"ResultType: {ResultType}; Message: {Message}";
            if (Exception != null)
            {
                result += $", Exception: {Exception}";
            }

            return result;
        }

        #region factory methods

        public static Result Success(string message = null)
        {
            return new Result
            {
                ResultType = ResultType.Success,
                Message = message
            };
        }

        public static Result Skipped(string message = null)
        {
            return new Result
            {
                ResultType = ResultType.Skipped,
                Message = message
            };
        }

        public static Result Error(Exception e, string message = null)
        {
            return new Result
            {
                ResultType = ResultType.Error,
                Message = message,
                Exception = e

            };
        }

        #endregion
    }
}
