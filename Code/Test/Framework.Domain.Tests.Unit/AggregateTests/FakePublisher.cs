using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Events;

namespace Framework.Domain.Tests.Unit.AggregateTests
{
    public class FakePublisher : IEventPublisher
    {
        public FakePublisher()
        {
            this._publishedEvents = new List<object>();
        }
        private readonly List<object> _publishedEvents;
        public void Publish<T>(T @event) where T : IEvent
        {
            this._publishedEvents.Add(@event);
        }
    }
}
