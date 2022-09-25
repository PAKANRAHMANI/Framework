using System;

namespace Framework.Core.Events
{
    public interface IDomainEvent : IEvent
    {
        Type AggregateType { get; }
    }
}
