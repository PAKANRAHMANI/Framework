using System;
using System.Collections.Generic;
using System.Text;
using Framework.Core.Events;

namespace Framework.Domain
{
    public interface IAggregateRoot
    {
        void Publish<T>(T @event) where T : IDomainEvent;
        void PublishDistributed<T>(T @event) where T : IDistributedEvent;

        IReadOnlyCollection<IDomainEvent> GetEvents();
        IReadOnlyCollection<IDistributedEvent> GetDistributedEvents();
        void ClearEvents();
        void ClearDistributedEvents();
    }
}
