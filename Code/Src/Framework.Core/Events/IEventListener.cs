using System;
using System.Collections.Generic;
using System.Text;
using Framework.Core.Events.Handlers;

namespace Framework.Core.Events
{
    public interface IEventListener
    {
        void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IDomainEvent;
        void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : IDomainEvent;
    }
}
