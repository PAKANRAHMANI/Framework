using System;
using System.Collections.Generic;
using Framework.Core.Events;

namespace Framework.Testing.Core
{
    public class FakePublisher : IEventPublisher
    {
        private readonly List<object> _publishedEvents;

        public FakePublisher()
        {
            this._publishedEvents = new List<object>();
        }
        public void Publish<T>(T @event) where T : IEvent
        {
            this._publishedEvents.Add(@event);
        }
        public List<object> GetPublishedEvents()
        {
            return this._publishedEvents;
        }
        public void Clear()
        {
            this._publishedEvents.Clear();
        }
    }
}
