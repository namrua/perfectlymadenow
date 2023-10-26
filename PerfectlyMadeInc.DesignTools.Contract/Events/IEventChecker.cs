using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace PerfectlyMadeInc.DesignTools.Contract.Events
{
    public interface IEventChecker<T> where T : BaseEvent
    {
        bool CheckEvent(T evnt);
    }
}
