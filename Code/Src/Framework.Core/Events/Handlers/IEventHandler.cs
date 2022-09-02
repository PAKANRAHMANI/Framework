using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Events.Handlers
{
    public interface IEventHandler<in T> where T : IEvent
    {
        public void Handle(T @event);
    }
}
