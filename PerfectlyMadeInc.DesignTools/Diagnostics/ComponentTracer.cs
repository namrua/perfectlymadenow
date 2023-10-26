using System;
using System.Diagnostics;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace PerfectlyMadeInc.DesignTools.Diagnostics
{
    /// <summary>
    /// Component tracer
    /// </summary>
    public class ComponentTracer : ITracer
    {
        public string ComponentName { get; set; }
        public object ComponentId { get; set; }

        public ComponentTracer(string componentName, object componentId = null)
        {
            ComponentName = componentName;
            ComponentId = componentId;
        }

        public void Error(Exception e)
        {
            Trace.TraceError(AddHeader(e?.ToString()));
        }

        public void Error(string message)
        {
            Trace.TraceError(AddHeader(message));
        }

        public void Error(Exception e, string message)
        {
            Trace.TraceError(AddHeader($"{message}\n{e}"));
        }

        public void Warning(string message)
        {
            Trace.TraceWarning(AddHeader(message));
        }

        public void Warning(Exception e, string message)
        {
            Trace.TraceWarning(AddHeader($"{message}\n{e}"));
        }

        public void Info(string message)
        {
            Trace.TraceInformation(AddHeader(message));
        }

        #region private methods

        private string AddHeader(string message)
        {
            var result = ComponentId == null
                ? $"{ComponentName}: {message}"
                : $"{ComponentName}({ComponentId}): {message}";
            return result;
        }

        #endregion
    }
}
