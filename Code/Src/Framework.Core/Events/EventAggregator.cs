using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core.Events.Handlers;

namespace Framework.Core.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly List<object> _subscribers = new List<object>();
        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            _subscribers.Add(new ActionEventHandler<T>(action));
        }

        public void Subscribe<T>(IEventHandler<T> @event) where T : IEvent
        {
            _subscribers.Add(@event);
        }

        public void Publish<T>(T @event) where T : IEvent
        {
            var handlers = _subscribers.OfType<IEventHandler<T>>().ToList();
            foreach (var eventHandler in handlers)
                eventHandler.Handle(@event);
        }
    }
}
