using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace PerfectlyMadeInc.DesignTools.Events
{
    /// <summary>
    /// Dispatches events to event handlers
    /// </summary>
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ITracer tracer;

        public EventDispatcher(ITracerFactory tracerFactory, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            tracer = tracerFactory.CreateTracer<EventDispatcher>();
        }

        public void Dispatch<T>(T evnt) where T : BaseEvent
        {
            var handlers = GetHandlersForEvent<T>();
            tracer.Info($"Dispatching event {typeof(T).FullName} [{evnt}]. {handlers.Count} handlers to be called:");

            foreach (var handler in handlers)
            {
                var handlerType = handler.GetType().FullName;
                tracer.Info($"Calling handler {handlerType}");
                Result handlerResult;
                try
                {
                    handlerResult = handler.HandleEvent(evnt);
                }
                catch (Exception e)
                {
                    handlerResult = Result.Error(e, $"Handler {handlerType} throws exception");
                }

                tracer.Info($"Handler {handlerType} result: {handlerResult}");

                if (handlerResult.ResultType == ResultType.Error)
                {
                    throw handlerResult.Exception ?? throw new Exception(handlerResult.Message ?? $"Handler {handlerType} throws exception");
                }
            }
        }

        public bool Check<T>(T evnt) where T : BaseEvent
        {
            var checkers = GetCheckersForEvent<T>();
            tracer.Info($"Check event {typeof(T).FullName} [{evnt}]. {checkers.Count} checkers to be called:");

            foreach (var checker in checkers)
            {
                var checkerType = checker.GetType().FullName;
                var checkerResult = checker.CheckEvent(evnt);
                tracer.Info($"Checker {checkerType} result: {checkerResult}");

                if (!checkerResult)
                {
                    return false;
                }
            }
            return true;
        }

        #region private methods

        private List<IEventHandler<T>> GetHandlersForEvent<T>() where T : BaseEvent
        {
            var result = serviceProvider.GetServices<IEventHandler<T>>().ToList();
            return result;
        }

        private List<IEventChecker<T>> GetCheckersForEvent<T>() where T : BaseEvent
        {
            var result = serviceProvider.GetServices<IEventChecker<T>>().ToList();
            return result;
        }

        #endregion
    }
}
