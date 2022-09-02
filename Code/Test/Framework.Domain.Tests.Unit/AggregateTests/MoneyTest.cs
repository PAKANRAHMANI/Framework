using FluentAssertions;
using Framework.Core.Utilities;
using Xunit;

namespace Framework.Domain.Tests.Unit.AggregateTests
{
    public class MoneyTest
    {
        [Fact]
        public void two_money_with_same_value_must_be_are_equals()
        {
            var money1 = new Money(100,"IRR");
            var money2 = new Money(100,"IRR");

          money1.Should().BeEquivalentTo(money2);
        }
        [Fact]
        public void two_money_with_different_price_must_be_are_not_equals()
        {
            var money1 = new Money(1000,"IRR");
            var money2 = new Money(100,"IRR");

          money1.Should().NotBeEquivalentTo(money2);
        }   
        [Fact]
        public void two_money_with_different_currency_must_be_are_not_equals()
        {
            var money1 = new Money(1000,"IRR");
            var money2 = new Money(1000,"USD");

          money1.Should().NotBeEquivalentTo(money2);
        }
    }
}