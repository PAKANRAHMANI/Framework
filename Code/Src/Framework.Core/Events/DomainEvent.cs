using System;

namespace Framework.Core.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid EventId { get; protected set; }
        public DateTime PublishDateTime { get; protected set; }
        public string AggregateType { get; protected set; }

        protected DomainEvent()
        {
            this.EventId = Guid.NewGuid();
            this.PublishDateTime = DateTime.UtcNow;
        }
    }
}
