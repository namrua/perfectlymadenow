using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace PerfectlyMadeInc.DesignTools.Contract.Events
{
    /// <summary>
    /// Dispatches events to event handlers
    /// </summary>
    public interface IEventDispatcher
    {
        void Dispatch<T>(T evnt) where T : BaseEvent;

        bool Check<T>(T evnt) where T : BaseEvent;
    }
}