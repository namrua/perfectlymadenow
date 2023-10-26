using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace PerfectlyMadeInc.DesignTools.Contract.Events
{
    /// <summary>
    /// Typed event handler
    /// </summary>
    /// <typeparam name="T">Event type</typeparam>
    public interface IEventHandler<T> where T : BaseEvent
    {
        Result HandleEvent(T evnt);
    }
}
