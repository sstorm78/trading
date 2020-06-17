using FluentAssertions;
using NUnit.Framework;

namespace Cirdan.Models.Tests
{
    public class OrderShould
    {
        [Test]
        public void ConstructorForCancel_ShouldPopulateRightProperties()
        {
            var result = new Order(1,"a", OrderAction.Cancel);

            result.Timestamp.Should().Be(1);
            result.OrderId.Should().Be("a");
            result.Action.Should().Be(OrderAction.Cancel);
        }

        [Test]
        public void ConstructorForUpdate_ShouldPopulateRightProperties()
        {
            var result = new Order(1, "a", OrderAction.Update,4);

            result.Timestamp.Should().Be(1);
            result.OrderId.Should().Be("a");
            result.Action.Should().Be(OrderAction.Update);
            result.Size.Should().Be(4);
        }

        [Test]
        public void ConstructorForAdd_ShouldPopulateRightProperties()
        {
            var result = new Order(1, "a", OrderAction.Add, "t",Side.Ask, 5,10);

            result.Timestamp.Should().Be(1);
            result.OrderId.Should().Be("a");
            result.Action.Should().Be(OrderAction.Add);
            result.Ticker.Should().Be("t");
            result.Side.Should().Be(Side.Ask);
            result.Price.Should().Be(5);
            result.Size.Should().Be(10);
        }
    }
}
