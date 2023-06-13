using System;

namespace Framework.Domain.Events
{
    public class DomainEventStructure
    {
        public string EventId { get; set; }
        public string EventType { get; set; }
        public string Body { get; set; }
        public Type AggregateType { get; set; }
        public DateTime PublishDateTime { get; set; }
    }
}
