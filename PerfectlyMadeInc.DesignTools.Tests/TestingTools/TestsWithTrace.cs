using System;
using System.Diagnostics;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.DesignTools.Diagnostics;
using Xunit.Abstractions;

namespace PerfectlyMadeInc.DesignTools.Tests.TestingTools
{
    public class TestsWithTrace : IDisposable
    {
        private readonly XUnitTraceListener listener;
        protected readonly ITracerFactory tracerFactory;

        public TestsWithTrace(ITestOutputHelper testOutputHelper)
        {
            tracerFactory = new ComponentTracerFactory();
            listener = new XUnitTraceListener(testOutputHelper);
            Trace.Listeners.Add(listener);

        }

        public void Dispose()
        {
            Trace.Listeners.Remove(listener);
        }
    }
}
