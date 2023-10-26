using System.Diagnostics;
using Xunit.Abstractions;

namespace PerfectlyMadeInc.DesignTools.Tests.TestingTools
{
    public class XUnitTraceListener : TraceListener
    {
        private readonly ITestOutputHelper testOutputHelper;

        public XUnitTraceListener(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public override void Write(string message)
        {
            testOutputHelper.WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            testOutputHelper.WriteLine(message);
        }
    }
}
