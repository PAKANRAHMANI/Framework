using System.Collections.Generic;
using System.Linq;
using Framework.Core.Events;
using Newtonsoft.Json;

namespace Framework.Domain.Events
{
    public static class DistributedEventStructureFactory
    {
        public static List<DistributedEventStructure> Create(IEnumerable<IDistributedEvent> distributedEvents)
        {
            return distributedEvents.Select(Create).ToList();
        }
        private static DistributedEventStructure Create(IDistributedEvent domainEvent)
        {
            return new DistributedEventStructure()
            {
                Body = JsonConvert.SerializeObject(domainEvent),
                EventId = domainEvent.EventId.ToString(),
                EventType = domainEvent.GetType().ToString(),
                PublishDateTime = domainEvent.PublishDateTime,
                IsUsed = domainEvent.IsUsed,
                AggregateName = domainEvent.AggregateName

            };
        }
    }
}