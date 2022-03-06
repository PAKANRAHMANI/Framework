using System;

namespace Framework.Core.Events
{
    public interface IDomainEvent : IEvent
    {
        Guid EventId { get; }
        DateTime PublishDateTime { get; }
        public bool IsUsed { get; }
    }
}
