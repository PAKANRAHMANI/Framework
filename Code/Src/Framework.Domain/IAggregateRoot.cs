using System;
using System.Collections.Generic;
using System.Text;
using Framework.Core.Events;

namespace Framework.Domain
{
    public interface IAggregateRoot
    {
        void Publish<T>(T @event) where T : IDomainEvent;
        IReadOnlyCollection<IDomainEvent> GetEvents();
        void ClearEvents();
    }
}
