using System;
using System.Collections.Generic;
using Framework.Core.Events;

namespace Framework.Testing.Core.Fakes
{
    public class FakePublisher : IEventPublisher
    {
        private readonly List<object> _publishedEvents;

        public FakePublisher()
        {
            _publishedEvents = new List<object>();
        }
        public void Publish<T>(T @event) where T : IEvent
        {
            _publishedEvents.Add(@event);
        }
        public List<object> GetPublishedEvents()
        {
            return _publishedEvents;
        }
        public void Clear()
        {
            _publishedEvents.Clear();
        }
    }
}
