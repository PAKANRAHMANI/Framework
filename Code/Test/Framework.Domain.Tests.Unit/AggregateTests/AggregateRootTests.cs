using System;
using FluentAssertions;
using Xunit;

namespace Framework.Domain.Tests.Unit.AggregateTests
{
    public class AggregateRootTests
    {
        [Fact]
        public void construct_aggregate_from_IAggregateRoot()
        {
            var aggregateRoot = new AggregateRootFake();

            aggregateRoot.Should().BeAssignableTo<IAggregateRoot>();
        }
        [Fact]
        public void when_event_is_published_in_aggregate_add_to_domain_event_collection()
        {
            var aggregateRoot = new AggregateRootFake(new FakePublisher());
            var @event =new TestDomainEvent(Guid.NewGuid(), "Event Published");

            aggregateRoot.Publish(@event);

            aggregateRoot.GetEvents().Should().ContainEquivalentOf(@event);
        }
    }
}
