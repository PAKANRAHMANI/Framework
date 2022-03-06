using System;

namespace Framework.Domain.Events
{
    public class DomainEventStructure
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public string Body { get; set; }
        public string AggregateName { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime PublishDateTime { get; set; }
    }
}
