using System;

namespace Framework.Core.Events;

public abstract class Event : IEvent
{
    public Guid EventId { get; protected set; }
    public DateTime PublishDateTime { get; protected set; }

    protected Event()
    {
        this.EventId = Guid.NewGuid();
        this.PublishDateTime = DateTime.UtcNow;
    }
}