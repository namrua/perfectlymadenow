using PerfectlyMadeInc.DesignTools.Diagnostics;
using Xunit;

namespace PerfectlyMadeInc.DesignTools.Tests.Diagnostics
{
    public class ComponentTracerFactoryTests
    {
        [Fact]
        public void CreateTracer_ReturnsComponentTracerInstance()
        {
            // arrange
            var factory = CreateFactory();

            // act
            var tracer = factory.CreateTracer<ComponentTracerFactoryTests>(123);

            // assert
            Assert.IsType<ComponentTracer>(tracer);
        }

        #region private methods

        private ComponentTracerFactory CreateFactory()
        {
            return new ComponentTracerFactory();
        }

        #endregion
    }
}
