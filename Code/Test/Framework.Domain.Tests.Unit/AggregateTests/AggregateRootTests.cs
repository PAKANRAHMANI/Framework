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

    }
}
