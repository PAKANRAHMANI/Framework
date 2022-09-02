using System;
using System.Collections.Generic;
using System.Text;
using Framework.Core.Events.Handlers;

namespace Framework.Core.Events
{
    public interface IEventAggregator
    {
        void Subscribe<T>(Action<T> action) where T : IEvent;
        void Subscribe<T>(IEventHandler<T> @event) where T : IEvent;
        void Publish<T>(T @event) where T : IEvent;
    }
}
