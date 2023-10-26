using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace PerfectlyMadeInc.DesignTools.Diagnostics
{
    /// <summary>
    /// ComponentTracer factory
    /// </summary>
    public class ComponentTracerFactory : ITracerFactory
    {
        public ITracer CreateTracer<T>(object componentId = null)
        {
            return new ComponentTracer(typeof(T).FullName, componentId);
        }
    }
}
