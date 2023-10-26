namespace PerfectlyMadeInc.DesignTools.Contract.Diagnostics
{
    /// <summary>
    /// ITracer factory
    /// </summary>
    public interface ITracerFactory
    {
        ITracer CreateTracer<T>(object componentId = null);
    }
}
