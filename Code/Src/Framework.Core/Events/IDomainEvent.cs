using System;

namespace Framework.Core.Events
{
    public interface IDomainEvent : IEvent
    {
        string AggregateType { get; }
    }
}
