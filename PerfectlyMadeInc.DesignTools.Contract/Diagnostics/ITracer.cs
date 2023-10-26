using System;

namespace PerfectlyMadeInc.DesignTools.Contract.Diagnostics
{
    /// <summary>
    /// Provides tracing of diagnostic messages
    /// </summary>
    public interface ITracer
    {
        void Error(Exception e);
        void Error(string message);
        void Error(Exception e, string message);

        void Warning(string message);
        void Warning(Exception e, string message);

        void Info(string message);
    }
}
