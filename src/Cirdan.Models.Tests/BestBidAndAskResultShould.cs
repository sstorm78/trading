using FluentAssertions;
using NUnit.Framework;

namespace Cirdan.Models.Tests
{
    public class BestBidAndAskResultShould
    {
        [Test]
        public void Constructor_ShouldPopulateProperties()
        {
            var result = new BestBidAndAskResult(new Order(0, "b", OrderAction.Add),
                new Order(1, "a", OrderAction.Cancel));

            result.BestAsk.OrderId.Should().Be("a");
            result.BestBid.OrderId.Should().Be("b");
        }
    }
}