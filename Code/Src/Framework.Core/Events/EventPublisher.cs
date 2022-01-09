using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IEventAggregator _aggregator;

        public EventPublisher(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
        }
        public void Publish<T>(T @event) where T : IEvent
        {
            _aggregator.Publish(@event);
        }
    }
}
