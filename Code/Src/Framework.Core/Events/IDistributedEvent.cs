using System;

namespace Framework.Core.Events
{
    public interface IDistributedEvent : IEvent
    {
        public bool IsUsed { get; }
        string AggregateName { get; }
        Guid AggregateId { get; }
    }
}
