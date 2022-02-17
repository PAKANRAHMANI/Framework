using System.Collections.Generic;
using System.Linq;
using Framework.Core.Events;
using Newtonsoft.Json;

namespace Framework.Domain.Events
{
    public static class DomainEventStructureFactory
    {
        public static List<DomainEventStructure> Create(IEnumerable<IDomainEvent> domainEvents)
        {
            return domainEvents.Select(Create).ToList();
        }
        private static DomainEventStructure Create(IDomainEvent domainEvent)
        {
            return new DomainEventStructure()
            {
                Body = JsonConvert.SerializeObject(domainEvent),
                EventId = domainEvent.EventId,
                EventType = domainEvent.GetType().ToString(),
                PublishDateTime = domainEvent.PublishDateTime
            };
        }
    }
}
