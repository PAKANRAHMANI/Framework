using Framework.Core.Events;

namespace Framework.Domain
{
    public class AggregateRootConfigurator : IAggregateRootConfigurator
    {
        private readonly IEventPublisher _publisher;

        public AggregateRootConfigurator(IEventPublisher publisher)
        {
            _publisher = publisher;
        }
        public T Config<T>(T aggregateRoot) where T : IAggregateRoot
        {
            if (aggregateRoot != null)
                aggregateRoot.SetPublisher(_publisher);

            return aggregateRoot;
        }
    }
}